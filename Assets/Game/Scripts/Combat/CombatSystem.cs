using System;
using System.Collections.Generic;
using System.Linq;
using IdleGame.Activities;
using IdleGame.Core.Bootstrap;
using IdleGame.Core.Identifiers;
using IdleGame.Equipment;
using IdleGame.Inventory;
using IdleGame.Professions.Woodcutting;
using IdleGame.Progression;
using IdleGame.Save;
using UnityEngine;

namespace IdleGame.Combat
{
    public sealed class CombatSystem : MonoBehaviour, IGameService
    {
        private const float DevotionRegenPerSecond = 3f;
        private const float HeavyStrikeDamageMultiplier = 1.5f;
        private const float HeavyStrikeCooldownSeconds = 8f;
        private const float HeavyStrikeActionTimeMultiplier = 1.5f;
        private const string PrototypeDevotionEffectId = "devotion_effect_warrior_focus";
        private const float PrototypeDevotionDrainPerSecond = 12f;
        private const float PrototypeDevotionDamageMultiplier = 1.1f;
        private const float PotionCooldownSeconds = 10f;
        private const float PotionThreshold01 = 0.35f;
        private const int PotionHealingAmount = 30;

        private static readonly CombatAbilityDefinition HeavyStrikeDefinition = new()
        {
            abilityId = CombatConstants.HeavyStrikeAbilityId,
            displayName = "Heavy Strike",
            actionTimeRule = CombatAbilityActionTimeRule.WeaponAttackIntervalMultiplier,
            weaponIntervalMultiplier = HeavyStrikeActionTimeMultiplier,
            cooldownSeconds = HeavyStrikeCooldownSeconds,
            damageMultiplier = HeavyStrikeDamageMultiplier,
            autoUsePriority = 100,
            requiresWarriorWeapon = true,
            resourceInteraction = CombatAbilityResourceInteraction.None
        };

        private static readonly DevotionEffectDefinition PrototypeDevotionEffect = new()
        {
            devotionEffectId = PrototypeDevotionEffectId,
            displayName = "Warrior Focus",
            devotionDrainPerSecond = PrototypeDevotionDrainPerSecond,
            damageMultiplier = PrototypeDevotionDamageMultiplier,
            requiredWarriorLevel = 1,
            deactivateAtZero = true
        };

        [SerializeField] private CombatCatalog catalog;
        [SerializeField] private InventorySystem inventorySystem;
        [SerializeField] private EquipmentSystem equipmentSystem;
        [SerializeField] private ProfessionProgressionSystem progressionSystem;
        [SerializeField] private ActiveActivityService activeActivityService;
        [SerializeField] private WoodcuttingSystem woodcuttingSystem;

        private readonly HashSet<string> firstClearEnemyIds = new(StringComparer.Ordinal);
        private readonly Dictionary<string, int> killCounts = new(StringComparer.Ordinal);
        private readonly List<string> combatLog = new();

        private CombatEnemyDefinition selectedEnemy;
        private PlayerCombatStats playerStats;
        private PlayerActionKind currentPlayerAction;
        private string currentPlayerAbilityId = string.Empty;
        private float playerActionTimer;
        private float playerActionDuration;
        private float enemyAttackTimer;
        private float respawnRemainingSeconds;
        private float heavyStrikeCooldownRemaining;
        private float potionCooldownRemaining;
        private float warriorXpRemainder;
        private float sessionElapsedSeconds;
        private int sessionDamageDealt;
        private int sessionDamageTaken;
        private int sessionHealingReceived;
        private string queuedManualAbilityId = string.Empty;
        private bool defeatProcessed;
        private bool pendingWoodcuttingConfirmation;
        private bool prototypeDevotionEffectActive;
        private int sessionKillCount;

        public event Action StateChanged;
        public event Action<string> Notification;
        public event Action<string> LogAdded;

        public int InitializationOrder => 60;
        public CombatCatalog Catalog => catalog;
        public IReadOnlyList<string> CombatLog => combatLog;
        public bool IsActive { get; private set; }
        public bool IsRespawning { get; private set; }
        public bool AutoRepeat { get; private set; } = true;
        public bool HeavyStrikeAutoUse { get; private set; } = true;
        public string SelectedRegionId { get; private set; } = string.Empty;
        public string SelectedActivityTypeId { get; private set; } = string.Empty;
        public string SelectedLocationId { get; private set; } = string.Empty;
        public string SelectedEnemyId => selectedEnemy != null ? selectedEnemy.EnemyId : string.Empty;
        public CombatEnemyDefinition SelectedEnemy => selectedEnemy;
        public PlayerCombatStats PlayerStats => playerStats;
        public int PlayerHealth { get; private set; }
        public float PlayerDevotion { get; private set; }
        public int EnemyHealth { get; private set; }
        public float PlayerAttackProgress01 => playerActionDuration <= 0f ? 0f : Mathf.Clamp01(playerActionTimer / playerActionDuration);
        public float EnemyAttackProgress01 => selectedEnemy == null ? 0f : Mathf.Clamp01(enemyAttackTimer / selectedEnemy.AttackInterval);
        public float HeavyStrikeCooldownRemaining => heavyStrikeCooldownRemaining;
        public float PotionCooldownRemaining => potionCooldownRemaining;
        public float RespawnRemainingSeconds => respawnRemainingSeconds;
        public int SessionKillCount => sessionKillCount;
        public int SessionElapsedSeconds => Mathf.FloorToInt(sessionElapsedSeconds);
        public int SessionDamageDealt => sessionDamageDealt;
        public int SessionDamageTaken => sessionDamageTaken;
        public int SessionHealingReceived => sessionHealingReceived;
        public float SessionDamagePerSecond => sessionElapsedSeconds <= 0f ? 0f : sessionDamageDealt / sessionElapsedSeconds;
        public bool IsStartConfirmationPending => pendingWoodcuttingConfirmation;
        public string QueuedCombatAbilityId => queuedManualAbilityId;
        public bool IsHeavyStrikeQueued => string.Equals(queuedManualAbilityId, CombatConstants.HeavyStrikeAbilityId, StringComparison.Ordinal);
        public bool HasQueuedCombatAction => !string.IsNullOrWhiteSpace(queuedManualAbilityId);
        public bool IsPerformingAutoAttack => currentPlayerAction == PlayerActionKind.AutoAttack;
        public bool IsPerformingHeavyStrike => currentPlayerAction == PlayerActionKind.CombatAbility &&
                                               string.Equals(currentPlayerAbilityId, CombatConstants.HeavyStrikeAbilityId, StringComparison.Ordinal);
        public bool CanQueueHeavyStrike => IsActive &&
                                           selectedEnemy != null &&
                                           EnemyHealth > 0 &&
                                           !IsHeavyStrikeQueued &&
                                           !IsPerformingHeavyStrike &&
                                           CanStartHeavyStrike(out _);
        public float PlayerActionRemainingSeconds => currentPlayerAction == PlayerActionKind.None ? 0f : Mathf.Max(0f, playerActionDuration - playerActionTimer);
        public float PlayerActionDurationSeconds => playerActionDuration;
        public string QueuedActionLabel => IsHeavyStrikeQueued ? HeavyStrikeDefinition.displayName : string.Empty;
        public string HeavyStrikeDevotionLabel => HeavyStrikeDefinition.resourceInteraction == CombatAbilityResourceInteraction.None ? "Free" : "Required";
        public string HeavyStrikeUnavailableReason
        {
            get
            {
                if (!IsActive || selectedEnemy == null || EnemyHealth <= 0)
                {
                    return "Requires active Combat.";
                }

                if (IsHeavyStrikeQueued)
                {
                    return "Already queued.";
                }

                if (IsPerformingHeavyStrike)
                {
                    return "Casting.";
                }

                return CanStartHeavyStrike(out var reason) ? string.Empty : reason;
            }
        }
        public bool IsPrototypeDevotionEffectActive => prototypeDevotionEffectActive;
        public string PlayerActionLabel => currentPlayerAction switch
        {
            PlayerActionKind.AutoAttack => "Auto Attack",
            PlayerActionKind.CombatAbility => HeavyStrikeDefinition.displayName,
            _ => "Ready"
        };

        public void InitializeService()
        {
            if (catalog == null)
            {
                throw new InvalidOperationException($"{nameof(CombatSystem)} needs a combat catalog.");
            }

            if (inventorySystem == null)
            {
                throw new InvalidOperationException($"{nameof(CombatSystem)} needs an inventory system.");
            }

            if (equipmentSystem == null)
            {
                throw new InvalidOperationException($"{nameof(CombatSystem)} needs an equipment system.");
            }

            if (progressionSystem == null)
            {
                throw new InvalidOperationException($"{nameof(CombatSystem)} needs a progression system.");
            }

            LoadFromSave(SaveManager.Instance?.Data.combat);
        }

        private void Update()
        {
            TickCombat(Time.deltaTime);
        }

        public void DebugTick(float deltaSeconds)
        {
            TickCombat(deltaSeconds);
        }

        private void TickCombat(float deltaSeconds)
        {
            if (!IsActive || selectedEnemy == null)
            {
                return;
            }

            var delta = Mathf.Max(0f, deltaSeconds);
            sessionElapsedSeconds += delta;
            heavyStrikeCooldownRemaining = Mathf.Max(0f, heavyStrikeCooldownRemaining - delta);
            potionCooldownRemaining = Mathf.Max(0f, potionCooldownRemaining - delta);
            TickDevotion(delta);

            if (IsRespawning)
            {
                respawnRemainingSeconds -= delta;
                if (respawnRemainingSeconds <= 0f)
                {
                    BeginEncounter();
                }

                StateChanged?.Invoke();
                return;
            }

            TryUsePotion();
            TickPlayerAction(delta);

            enemyAttackTimer += delta;

            while (selectedEnemy != null && enemyAttackTimer >= selectedEnemy.AttackInterval && IsActive && !IsRespawning && !defeatProcessed)
            {
                enemyAttackTimer -= selectedEnemy.AttackInterval;
                ResolveEnemyAttack();
            }

            StateChanged?.Invoke();
        }

        public void SelectRegion(string regionId)
        {
            SelectedRegionId = regionId == catalog.RegionId ? regionId : string.Empty;
            SelectedActivityTypeId = string.Empty;
            SelectedLocationId = string.Empty;
            selectedEnemy = null;
            pendingWoodcuttingConfirmation = false;
            SaveManager.Instance?.SaveNow();
            StateChanged?.Invoke();
        }

        public void SelectActivityType(string activityTypeId)
        {
            if (SelectedRegionId != catalog.RegionId || activityTypeId != catalog.ActivityTypeId)
            {
                return;
            }

            SelectedActivityTypeId = activityTypeId;
            SelectedLocationId = string.Empty;
            selectedEnemy = null;
            pendingWoodcuttingConfirmation = false;
            SaveManager.Instance?.SaveNow();
            StateChanged?.Invoke();
        }

        public void SelectLocation(string locationId)
        {
            if (SelectedRegionId != catalog.RegionId || SelectedActivityTypeId != catalog.ActivityTypeId || locationId != catalog.LocationId)
            {
                return;
            }

            SelectedLocationId = locationId;
            selectedEnemy = null;
            pendingWoodcuttingConfirmation = false;
            SaveManager.Instance?.SaveNow();
            StateChanged?.Invoke();
        }

        public void SelectEnemy(string enemyId)
        {
            if (SelectedLocationId != catalog.LocationId || !catalog.TryGetEnemy(enemyId, out var enemy))
            {
                return;
            }

            selectedEnemy = enemy;
            pendingWoodcuttingConfirmation = false;
            SaveManager.Instance?.SaveNow();
            StateChanged?.Invoke();
        }

        public bool IsEnemyUnlocked(CombatEnemyDefinition enemy)
        {
            return enemy != null && progressionSystem.GetLevel(CombatConstants.WarriorProgressionId) >= enemy.RequiredWarriorLevel;
        }

        public bool StartCombat()
        {
            if (!ValidateStart(out var reason))
            {
                Notify(reason);
                return false;
            }

            if (woodcuttingSystem != null && woodcuttingSystem.IsActive && !pendingWoodcuttingConfirmation)
            {
                pendingWoodcuttingConfirmation = true;
                Notify("Starting Combat will stop Woodcutting. Press Start Combat again to confirm.");
                StateChanged?.Invoke();
                return false;
            }

            if (woodcuttingSystem != null && woodcuttingSystem.IsActive)
            {
                woodcuttingSystem.StopWoodcutting("Stopped for Combat");
            }

            pendingWoodcuttingConfirmation = false;
            if (activeActivityService != null && !activeActivityService.RequestStartPrimary(CombatConstants.ActivityId, "Combat", selectedEnemy.DisplayName))
            {
                Notify($"{activeActivityService.ActiveActivityName} is already active. Stop it before starting Combat.");
                return false;
            }

            playerStats = PlayerCombatStats.Resolve(equipmentSystem);
            PlayerHealth = playerStats.MaxHealth;
            PlayerDevotion = playerStats.MaxDevotion;
            sessionKillCount = 0;
            sessionElapsedSeconds = 0f;
            sessionDamageDealt = 0;
            sessionDamageTaken = 0;
            sessionHealingReceived = 0;
            IsActive = true;
            equipmentSystem.SetEquipmentLocked(true);
            AddLog($"Combat started: {selectedEnemy.DisplayName}");
            BeginEncounter();
            SaveManager.Instance?.SaveNow();
            return true;
        }

        public void QuitCombat()
        {
            if (!IsActive)
            {
                return;
            }

            StopCombat("Combat quit.");
        }

        public void SetAutoRepeat(bool value)
        {
            AutoRepeat = value;
            SaveManager.Instance?.SaveNow();
            StateChanged?.Invoke();
        }

        public void SetHeavyStrikeAutoUse(bool value)
        {
            HeavyStrikeAutoUse = value;
            SaveManager.Instance?.SaveNow();
            StateChanged?.Invoke();
        }

        public bool QueueHeavyStrike()
        {
            if (!IsActive || selectedEnemy == null || EnemyHealth <= 0)
            {
                Notify("Heavy Strike requires active Combat.");
                return false;
            }

            if (IsHeavyStrikeQueued)
            {
                Notify("Heavy Strike is already queued.");
                return false;
            }

            if (IsPerformingHeavyStrike)
            {
                Notify("Heavy Strike is already casting.");
                return false;
            }

            if (!CanStartHeavyStrike(out var reason))
            {
                Notify(reason);
                return false;
            }

            queuedManualAbilityId = CombatConstants.HeavyStrikeAbilityId;
            AddLog("Heavy Strike queued.");
            StateChanged?.Invoke();
            return true;
        }

        public void SetPrototypeDevotionEffectActive(bool active)
        {
            if (active)
            {
                if (PlayerDevotion <= 0f)
                {
                    Notify($"{PrototypeDevotionEffect.displayName} requires Devotion.");
                    return;
                }

                prototypeDevotionEffectActive = true;
                AddLog($"{PrototypeDevotionEffect.displayName} activated.");
            }
            else if (prototypeDevotionEffectActive)
            {
                prototypeDevotionEffectActive = false;
                AddLog($"{PrototypeDevotionEffect.displayName} deactivated.");
            }

            StateChanged?.Invoke();
        }

        public bool TryUseMinorHealingPotion()
        {
            if (!IsActive || selectedEnemy == null)
            {
                Notify("Minor Potion requires active Combat.");
                return false;
            }

            return TryUsePotion(true);
        }

        public CombatSaveData CreateSaveData()
        {
            return new CombatSaveData
            {
                selectedRegionId = SelectedRegionId,
                selectedActivityTypeId = SelectedActivityTypeId,
                selectedLocationId = SelectedLocationId,
                selectedEnemyId = SelectedEnemyId,
                autoRepeat = AutoRepeat,
                heavyStrikeAutoUse = HeavyStrikeAutoUse,
                firstClearEnemyIds = firstClearEnemyIds.ToList(),
                killCounts = killCounts.Select(pair => new CombatKillCountSaveData { enemyId = pair.Key, count = pair.Value }).ToList()
            };
        }

        public void ConfigureForEditor(
            CombatCatalog combatCatalog,
            InventorySystem inventory,
            EquipmentSystem equipment,
            ProfessionProgressionSystem progression,
            ActiveActivityService activeActivity,
            WoodcuttingSystem woodcutting)
        {
            catalog = combatCatalog;
            inventorySystem = inventory;
            equipmentSystem = equipment;
            progressionSystem = progression;
            activeActivityService = activeActivity;
            woodcuttingSystem = woodcutting;
        }

        private void BeginEncounter()
        {
            IsRespawning = false;
            respawnRemainingSeconds = 0f;
            defeatProcessed = false;
            ClearPlayerActionChannel();
            enemyAttackTimer = 0f;
            EnemyHealth = selectedEnemy.MaximumHealth;
            activeActivityService?.RequestStartPrimary(CombatConstants.ActivityId, "Combat", selectedEnemy.DisplayName);
            AddLog($"{selectedEnemy.DisplayName} appears.");
            StateChanged?.Invoke();
        }

        private bool ValidateStart(out string reason)
        {
            reason = string.Empty;
            if (SelectedRegionId != catalog.RegionId || SelectedActivityTypeId != catalog.ActivityTypeId || SelectedLocationId != catalog.LocationId || selectedEnemy == null)
            {
                reason = "Complete the Combat selection first.";
                return false;
            }

            if (!IsEnemyUnlocked(selectedEnemy))
            {
                reason = $"Requires Warrior Level {selectedEnemy.RequiredWarriorLevel}.";
                return false;
            }

            var mainHand = equipmentSystem.GetMainHandDefinition();
            if (mainHand == null || !mainHand.IsWarriorCompatible || mainHand.CombatMaxDamage <= 0)
            {
                reason = "Equip a Warrior-compatible Main-Hand weapon.";
                return false;
            }

            return true;
        }

        private void ResolvePlayerAttack(string label, float damageMultiplier)
        {
            if (!IsActive || selectedEnemy == null || EnemyHealth <= 0)
            {
                return;
            }

            var hitChance = CombatFormula.GetHitChance01(playerStats.Accuracy, selectedEnemy.Defense);
            if (UnityEngine.Random.value > hitChance)
            {
                AddLog($"{label}: miss.");
                return;
            }

            var rolled = UnityEngine.Random.Range(playerStats.MinDamage, playerStats.MaxDamage + 1);
            rolled = Mathf.Max(1, Mathf.RoundToInt(rolled * damageMultiplier));
            if (prototypeDevotionEffectActive)
            {
                rolled = Mathf.Max(1, Mathf.RoundToInt(rolled * PrototypeDevotionEffect.damageMultiplier));
            }

            var critical = UnityEngine.Random.value <= playerStats.CriticalChance;
            var damage = CombatFormula.MitigateDamage(rolled, selectedEnemy.Defense, critical, playerStats.CriticalDamageMultiplier);
            var validDamage = Mathf.Min(damage, EnemyHealth);
            EnemyHealth = Mathf.Max(0, EnemyHealth - damage);
            sessionDamageDealt += validDamage;

            AddWarriorXp(validDamage * selectedEnemy.WarriorXpCoefficient);
            AddLog($"{label}: {(critical ? "critical " : string.Empty)}hit for {validDamage}.");

            if (EnemyHealth <= 0)
            {
                ProcessEnemyDefeat();
            }
        }

        private void ResolveEnemyAttack()
        {
            if (!IsActive || selectedEnemy == null || EnemyHealth <= 0)
            {
                return;
            }

            var hitChance = CombatFormula.GetHitChance01(selectedEnemy.Accuracy, playerStats.Defense);
            if (UnityEngine.Random.value > hitChance)
            {
                AddLog($"{selectedEnemy.DisplayName} misses.");
                return;
            }

            var rolled = UnityEngine.Random.Range(selectedEnemy.MinDamage, selectedEnemy.MaxDamage + 1);
            var critical = UnityEngine.Random.value <= selectedEnemy.CriticalChance;
            var damage = CombatFormula.MitigateDamage(rolled, playerStats.Defense, critical, selectedEnemy.CriticalDamageMultiplier);
            PlayerHealth = Mathf.Max(0, PlayerHealth - damage);
            sessionDamageTaken += damage;
            AddLog($"{selectedEnemy.DisplayName} {(critical ? "critically " : string.Empty)}hits for {damage}.");

            if (PlayerHealth <= 0)
            {
                ProcessPlayerDefeat();
            }
        }

        private void TryUsePotion()
        {
            TryUsePotion(false);
        }

        private bool TryUsePotion(bool manual)
        {
            if (potionCooldownRemaining > 0f || PlayerHealth <= 0 || (!manual && PlayerHealth > playerStats.MaxHealth * PotionThreshold01) || (manual && PlayerHealth >= playerStats.MaxHealth))
            {
                if (manual && PlayerHealth >= playerStats.MaxHealth)
                {
                    Notify("Minor Potion is not needed at full Health.");
                }
                else if (manual && potionCooldownRemaining > 0f)
                {
                    Notify($"Minor Potion is on cooldown for {potionCooldownRemaining:0.0}s.");
                }

                return false;
            }

            if (inventorySystem.GetQuantity(CombatConstants.MinorHealingPotionItemId) <= 0)
            {
                if (manual)
                {
                    Notify("No Minor Healing Potions available.");
                }

                return false;
            }

            if (!inventorySystem.TryRemoveItem(CombatConstants.MinorHealingPotionItemId, 1))
            {
                return false;
            }

            var previous = PlayerHealth;
            PlayerHealth = Mathf.Min(playerStats.MaxHealth, PlayerHealth + PotionHealingAmount);
            sessionHealingReceived += PlayerHealth - previous;
            potionCooldownRemaining = PotionCooldownSeconds;
            AddLog($"Minor Healing Potion restores {PlayerHealth - previous} Health.");
            StateChanged?.Invoke();
            return true;
        }

        private void TickPlayerAction(float delta)
        {
            if (!IsActive || selectedEnemy == null || EnemyHealth <= 0 || defeatProcessed)
            {
                return;
            }

            if (currentPlayerAction == PlayerActionKind.None)
            {
                BeginNextPlayerAction();
            }

            if (currentPlayerAction == PlayerActionKind.None)
            {
                return;
            }

            playerActionTimer += delta;
            if (playerActionTimer < playerActionDuration)
            {
                return;
            }

            ResolveCurrentPlayerAction();
            if (IsActive && !IsRespawning && !defeatProcessed && selectedEnemy != null && EnemyHealth > 0)
            {
                BeginNextPlayerAction();
            }
        }

        private void BeginNextPlayerAction()
        {
            if (TryBeginQueuedManualAbility())
            {
                return;
            }

            if (TryBeginEmergencyOrConditionalAutoAbility())
            {
                return;
            }

            if (TryBeginHighestPriorityAutoAbility())
            {
                return;
            }

            BeginAutoAttack();
        }

        private bool TryBeginQueuedManualAbility()
        {
            if (string.IsNullOrWhiteSpace(queuedManualAbilityId))
            {
                return false;
            }

            if (string.Equals(queuedManualAbilityId, CombatConstants.HeavyStrikeAbilityId, StringComparison.Ordinal) &&
                CanStartHeavyStrike(out _))
            {
                queuedManualAbilityId = string.Empty;
                BeginHeavyStrike();
                return true;
            }

            AddLog("Queued Combat Ability cleared because it is no longer valid.");
            queuedManualAbilityId = string.Empty;
            return false;
        }

        private bool TryBeginEmergencyOrConditionalAutoAbility()
        {
            return false;
        }

        private bool TryBeginHighestPriorityAutoAbility()
        {
            if (!HeavyStrikeAutoUse || !CanStartHeavyStrike(out _))
            {
                return false;
            }

            BeginHeavyStrike();
            return true;
        }

        private void BeginAutoAttack()
        {
            currentPlayerAction = PlayerActionKind.AutoAttack;
            currentPlayerAbilityId = string.Empty;
            playerActionTimer = 0f;
            playerActionDuration = Mathf.Max(0.05f, playerStats.AttackInterval);
        }

        private void BeginHeavyStrike()
        {
            currentPlayerAction = PlayerActionKind.CombatAbility;
            currentPlayerAbilityId = HeavyStrikeDefinition.abilityId;
            playerActionTimer = 0f;
            playerActionDuration = HeavyStrikeDefinition.GetActionTime(playerStats);
            heavyStrikeCooldownRemaining = HeavyStrikeDefinition.cooldownSeconds;
            AddLog($"{HeavyStrikeDefinition.displayName} begins.");
        }

        private void ResolveCurrentPlayerAction()
        {
            var action = currentPlayerAction;
            var abilityId = currentPlayerAbilityId;
            ClearCurrentPlayerAction();

            if (action == PlayerActionKind.AutoAttack)
            {
                ResolvePlayerAttack("Player attacks", 1f);
                return;
            }

            if (action == PlayerActionKind.CombatAbility &&
                string.Equals(abilityId, HeavyStrikeDefinition.abilityId, StringComparison.Ordinal))
            {
                ResolvePlayerAttack(HeavyStrikeDefinition.displayName, HeavyStrikeDefinition.damageMultiplier);
            }
        }

        private bool CanStartHeavyStrike(out string reason)
        {
            reason = string.Empty;
            if (selectedEnemy == null || EnemyHealth <= 0)
            {
                reason = "Heavy Strike requires a target.";
                return false;
            }

            if (heavyStrikeCooldownRemaining > 0f)
            {
                reason = $"Heavy Strike is on cooldown for {heavyStrikeCooldownRemaining:0.0}s.";
                return false;
            }

            if (HeavyStrikeDefinition.requiresWarriorWeapon)
            {
                var mainHand = equipmentSystem.GetMainHandDefinition();
                if (mainHand == null || !mainHand.IsWarriorCompatible || mainHand.CombatMaxDamage <= 0)
                {
                    reason = "Equip a Warrior-compatible Main-Hand weapon.";
                    return false;
                }
            }

            return true;
        }

        private void TickDevotion(float delta)
        {
            var devotionDelta = DevotionRegenPerSecond * delta;
            if (prototypeDevotionEffectActive)
            {
                devotionDelta -= PrototypeDevotionEffect.devotionDrainPerSecond * delta;
            }

            PlayerDevotion = Mathf.Clamp(PlayerDevotion + devotionDelta, 0f, playerStats.MaxDevotion);
            if (prototypeDevotionEffectActive && PrototypeDevotionEffect.deactivateAtZero && PlayerDevotion <= 0f)
            {
                prototypeDevotionEffectActive = false;
                AddLog($"{PrototypeDevotionEffect.displayName} deactivated: Devotion depleted.");
            }
        }

        private void ClearPlayerActionChannel()
        {
            queuedManualAbilityId = string.Empty;
            ClearCurrentPlayerAction();
        }

        private void ClearCurrentPlayerAction()
        {
            currentPlayerAction = PlayerActionKind.None;
            currentPlayerAbilityId = string.Empty;
            playerActionTimer = 0f;
            playerActionDuration = 0f;
        }

        private void ProcessEnemyDefeat()
        {
            if (defeatProcessed || selectedEnemy == null)
            {
                return;
            }

            defeatProcessed = true;
            sessionKillCount++;
            var enemyId = selectedEnemy.EnemyId;
            killCounts[enemyId] = killCounts.TryGetValue(enemyId, out var count) ? count + 1 : 1;
            AddLog($"{selectedEnemy.DisplayName} defeated.");

            var gold = UnityEngine.Random.Range(selectedEnemy.MinGold, selectedEnemy.MaxGold + 1);
            if (gold > 0)
            {
                inventorySystem.AddGold(gold);
                AddLog($"+{gold} Gold.");
            }

            foreach (var loot in selectedEnemy.Loot.Where(loot => loot != null && loot.IsValid))
            {
                if (UnityEngine.Random.value <= loot.chance)
                {
                    AutoLootItem(loot.itemId, UnityEngine.Random.Range(loot.minQuantity, loot.maxQuantity + 1));
                }
            }

            if (!firstClearEnemyIds.Contains(enemyId))
            {
                firstClearEnemyIds.Add(enemyId);
                foreach (var reward in selectedEnemy.FirstClearRewards.Where(reward => reward != null && reward.IsValid))
                {
                    AutoLootItem(reward.itemId, UnityEngine.Random.Range(reward.minQuantity, reward.maxQuantity + 1));
                    AddLog($"First-clear reward: {FormatItem(reward.itemId, reward.minQuantity)}.");
                }
            }

            SaveManager.Instance?.SaveNow();

            if (AutoRepeat && IsActive)
            {
                IsRespawning = true;
                respawnRemainingSeconds = selectedEnemy.RespawnSeconds;
                ClearPlayerActionChannel();
                enemyAttackTimer = 0f;
            }
            else
            {
                StopCombat("Enemy defeated.");
            }
        }

        private void ProcessPlayerDefeat()
        {
            if (defeatProcessed)
            {
                return;
            }

            defeatProcessed = true;
            AddLog("Player defeated. XP and loot already earned are preserved.");
            StopCombat("Player defeated.");
            PlayerHealth = playerStats.MaxHealth;
            PlayerDevotion = playerStats.MaxDevotion;
        }

        private void AutoLootItem(string itemId, long quantity)
        {
            if (quantity <= 0 || !StableId.IsValid(itemId))
            {
                return;
            }

            if (inventorySystem.TryAddItem(itemId, quantity))
            {
                AddLog($"Looted {FormatItem(itemId, quantity)} to Inventory.");
                return;
            }

            AutoRepeat = false;
            AddLog($"Inventory full. Could not loot {FormatItem(itemId, quantity)}.");
            Notify("Inventory is full. Auto Repeat stopped.");
        }

        private void AddWarriorXp(float amount)
        {
            if (amount <= 0f)
            {
                return;
            }

            warriorXpRemainder += amount;
            var wholeXp = Mathf.FloorToInt(warriorXpRemainder);
            if (wholeXp <= 0)
            {
                return;
            }

            warriorXpRemainder -= wholeXp;
            progressionSystem.AddExperience(CombatConstants.WarriorProgressionId, wholeXp);
        }

        private void StopCombat(string message)
        {
            IsActive = false;
            IsRespawning = false;
            sessionKillCount = 0;
            sessionElapsedSeconds = 0f;
            sessionDamageDealt = 0;
            sessionDamageTaken = 0;
            sessionHealingReceived = 0;
            ClearPlayerActionChannel();
            enemyAttackTimer = 0f;
            respawnRemainingSeconds = 0f;
            pendingWoodcuttingConfirmation = false;
            prototypeDevotionEffectActive = false;
            equipmentSystem.SetEquipmentLocked(false);
            activeActivityService?.Stop(CombatConstants.ActivityId);
            Notify(message);
            SaveManager.Instance?.SaveNow();
            StateChanged?.Invoke();
        }

        private void LoadFromSave(CombatSaveData saveData)
        {
            SelectedRegionId = !string.IsNullOrWhiteSpace(saveData?.selectedRegionId) ? saveData.selectedRegionId : catalog.RegionId;
            SelectedActivityTypeId = !string.IsNullOrWhiteSpace(saveData?.selectedActivityTypeId) ? saveData.selectedActivityTypeId : catalog.ActivityTypeId;
            SelectedLocationId = !string.IsNullOrWhiteSpace(saveData?.selectedLocationId) ? saveData.selectedLocationId : catalog.LocationId;
            if (saveData != null && !string.IsNullOrWhiteSpace(saveData.selectedEnemyId) && catalog.TryGetEnemy(saveData.selectedEnemyId, out var loadedEnemy))
            {
                selectedEnemy = loadedEnemy;
            }
            else
            {
                selectedEnemy = catalog.GetFirstEnemy();
            }

            AutoRepeat = saveData == null || saveData.autoRepeat;
            HeavyStrikeAutoUse = saveData == null || saveData.heavyStrikeAutoUse;
            firstClearEnemyIds.Clear();
            killCounts.Clear();

            if (saveData?.firstClearEnemyIds != null)
            {
                foreach (var id in saveData.firstClearEnemyIds.Where(StableId.IsValid))
                {
                    firstClearEnemyIds.Add(id);
                }
            }

            if (saveData?.killCounts != null)
            {
                foreach (var saved in saveData.killCounts.Where(saved => saved != null && StableId.IsValid(saved.enemyId)))
                {
                    killCounts[saved.enemyId] = Mathf.Max(0, saved.count);
                }
            }

            // Temporary loot was removed. Old saved temporary/protected entries are intentionally ignored.
        }

        private void AddLog(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            combatLog.Add(message);
            while (combatLog.Count > 200)
            {
                combatLog.RemoveAt(0);
            }

            LogAdded?.Invoke(message);
        }

        private void Notify(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                Notification?.Invoke(message);
                AddLog(message);
            }
        }

        private string FormatItem(string itemId, long quantity)
        {
            var displayName = itemId;
            if (inventorySystem.ItemDatabase != null && inventorySystem.ItemDatabase.TryGetItem(itemId, out var item) && item != null)
            {
                displayName = item.DisplayName;
            }

            return $"{quantity} {displayName}";
        }

        private enum PlayerActionKind
        {
            None = 0,
            AutoAttack = 1,
            CombatAbility = 2
        }
    }
}
