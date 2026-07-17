using System;
using System.Collections.Generic;
using System.Linq;
using IdleGame.Activities;
using IdleGame.Core.Bootstrap;
using IdleGame.Equipment;
using IdleGame.Inventory;
using IdleGame.Items;
using IdleGame.Progression;
using IdleGame.Save;
using UnityEngine;

namespace IdleGame.Professions.Woodcutting
{
    public sealed class WoodcuttingSystem : MonoBehaviour, IGameService
    {
        [SerializeField] private WoodcuttingCatalog catalog;
        [SerializeField] private InventorySystem inventorySystem;
        [SerializeField] private ProfessionProgressionSystem progressionSystem;
        [SerializeField] private EquipmentSystem equipmentSystem;
        [SerializeField] private ActiveActivityService activeActivityService;

        private readonly HashSet<string> completedThresholds = new(StringComparer.Ordinal);
        private ProfessionModifierResolver modifierResolver;
        private WoodcuttingTreeDefinition selectedTree;

        public event Action StateChanged;
        public event Action<string> RewardFeedback;
        public event Action<string> Notification;

        public int InitializationOrder => 50;
        public bool IsActive { get; private set; }
        public bool IsRespawning { get; private set; }
        public float CurrentDurability { get; private set; }
        public float ActionProgressSeconds { get; private set; }
        public float RespawnRemainingSeconds { get; private set; }
        public WoodcuttingTreeDefinition SelectedTree => selectedTree;
        public IReadOnlyList<WoodcuttingTreeDefinition> Trees => catalog != null ? catalog.Trees : Array.Empty<WoodcuttingTreeDefinition>();
        public float CurrentDurabilityPercent => selectedTree == null || selectedTree.MaximumDurability <= 0f
            ? 0f
            : CurrentDurability / selectedTree.MaximumDurability;

        public ProfessionModifierResult CurrentModifiers
        {
            get
            {
                modifierResolver ??= new ProfessionModifierResolver(equipmentSystem);
                return modifierResolver.Resolve(
                    WoodcuttingConstants.ProfessionId,
                    WoodcuttingConstants.BasePower,
                    WoodcuttingConstants.BaseActionInterval);
            }
        }

        public void InitializeService()
        {
            if (catalog == null)
            {
                throw new InvalidOperationException($"{nameof(WoodcuttingSystem)} needs a tree catalog.");
            }

            if (inventorySystem == null)
            {
                throw new InvalidOperationException($"{nameof(WoodcuttingSystem)} needs an inventory system.");
            }

            if (progressionSystem == null)
            {
                throw new InvalidOperationException($"{nameof(WoodcuttingSystem)} needs a progression system.");
            }

            modifierResolver = new ProfessionModifierResolver(equipmentSystem);
            LoadFromSave(SaveManager.Instance?.Data.woodcutting);
            progressionSystem.LevelChanged += OnProfessionLevelChanged;
        }

        private void Update()
        {
            if (!IsActive || selectedTree == null)
            {
                return;
            }

            if (IsRespawning)
            {
                RespawnRemainingSeconds -= Time.deltaTime;
                if (RespawnRemainingSeconds <= 0f)
                {
                    Respawn();
                }

                StateChanged?.Invoke();
                return;
            }

            var interval = Mathf.Max(0.05f, CurrentModifiers.finalActionInterval);
            ActionProgressSeconds += Time.deltaTime;
            while (ActionProgressSeconds >= interval && IsActive && !IsRespawning)
            {
                ActionProgressSeconds -= interval;
                ApplyAction();
            }

            StateChanged?.Invoke();
        }

        public bool SelectTree(string treeId)
        {
            if (IsActive)
            {
                return false;
            }

            if (!catalog.TryGetTree(treeId, out var tree) || tree == null)
            {
                return false;
            }

            selectedTree = tree;
            CurrentDurability = tree.MaximumDurability;
            ActionProgressSeconds = 0f;
            IsRespawning = false;
            RespawnRemainingSeconds = 0f;
            completedThresholds.Clear();
            StateChanged?.Invoke();
            SaveManager.Instance?.SaveNow();
            return true;
        }

        public bool IsTreeUnlocked(WoodcuttingTreeDefinition tree)
        {
            return tree != null && progressionSystem.GetLevel(WoodcuttingConstants.ProfessionId) >= tree.RequiredLevel;
        }

        public bool IsThresholdCompleted(WoodcuttingThresholdReward threshold)
        {
            return threshold != null && completedThresholds.Contains(threshold.thresholdKey);
        }

        public bool StartWoodcutting()
        {
            if (IsActive)
            {
                return true;
            }

            if (selectedTree == null || !IsTreeUnlocked(selectedTree))
            {
                Notification?.Invoke(selectedTree == null
                    ? "Select a tree."
                    : $"Requires Woodcutting Level {selectedTree.RequiredLevel}.");
                return false;
            }

            if (!inventorySystem.CanAcceptAnyThreshold(selectedTree))
            {
                StopWoodcutting("Inventory Full");
                return false;
            }

            if (activeActivityService != null && !activeActivityService.RequestStartPrimary(WoodcuttingConstants.ActivityId, "Woodcutting", selectedTree.DisplayName))
            {
                Notification?.Invoke($"{activeActivityService.ActiveActivityName} is already active. Stop it before starting Woodcutting.");
                return false;
            }

            IsActive = true;
            StateChanged?.Invoke();
            SaveManager.Instance?.SaveNow();
            return true;
        }

        public void StopWoodcutting(string reason = "Stopped")
        {
            IsActive = false;
            ActionProgressSeconds = 0f;
            activeActivityService?.Stop(WoodcuttingConstants.ActivityId);
            Notification?.Invoke(reason);
            StateChanged?.Invoke();
            SaveManager.Instance?.SaveNow();
        }

        public WoodcuttingSaveData CreateSaveData()
        {
            return new WoodcuttingSaveData
            {
                selectedTreeId = selectedTree != null ? selectedTree.TreeId : "tree_sproutwood",
                isActive = IsActive,
                currentDurability = CurrentDurability,
                actionProgressSeconds = ActionProgressSeconds,
                isRespawning = IsRespawning,
                respawnRemainingSeconds = RespawnRemainingSeconds,
                completedThresholdKeys = completedThresholds.ToList()
            };
        }

        public void ConfigureForEditor(
            WoodcuttingCatalog treeCatalog,
            InventorySystem inventory,
            ProfessionProgressionSystem progression,
            EquipmentSystem equipment,
            ActiveActivityService activeActivity)
        {
            catalog = treeCatalog;
            inventorySystem = inventory;
            progressionSystem = progression;
            equipmentSystem = equipment;
            activeActivityService = activeActivity;
        }

        private void LoadFromSave(WoodcuttingSaveData saveData)
        {
            var currentLevel = progressionSystem.GetLevel(WoodcuttingConstants.ProfessionId);
            var fallback = catalog.GetFirstUnlockedOrFirst(currentLevel);
            var selectedId = saveData?.selectedTreeId;
            if (!string.IsNullOrWhiteSpace(selectedId) && catalog.TryGetTree(selectedId, out var loadedTree))
            {
                selectedTree = loadedTree;
            }
            else
            {
                selectedTree = fallback;
            }

            CurrentDurability = selectedTree != null ? selectedTree.MaximumDurability : 0f;
            if (saveData != null && selectedTree != null)
            {
                CurrentDurability = Mathf.Clamp(saveData.currentDurability, 0f, selectedTree.MaximumDurability);
                ActionProgressSeconds = Mathf.Max(0f, saveData.actionProgressSeconds);
                IsRespawning = saveData.isRespawning;
                RespawnRemainingSeconds = Mathf.Max(0f, saveData.respawnRemainingSeconds);
                IsActive = saveData.isActive;
                completedThresholds.Clear();

                if (saveData.completedThresholdKeys != null)
                {
                    foreach (var key in saveData.completedThresholdKeys.Where(key => !string.IsNullOrWhiteSpace(key)))
                    {
                        completedThresholds.Add(key);
                    }
                }
            }

            if (IsActive && selectedTree != null)
            {
                activeActivityService?.RequestStartPrimary(WoodcuttingConstants.ActivityId, "Woodcutting", selectedTree.DisplayName);
            }
        }

        private void ApplyAction()
        {
            var previousDurability = CurrentDurability;
            CurrentDurability = Mathf.Max(0f, CurrentDurability - Mathf.Max(0f, CurrentModifiers.finalPower));
            if (!GrantCrossedThresholds(previousDurability, CurrentDurability))
            {
                return;
            }

            if (CurrentDurability <= 0f)
            {
                progressionSystem.AddExperience(WoodcuttingConstants.ProfessionId, selectedTree.CompletionXp);
                IsRespawning = true;
                RespawnRemainingSeconds = selectedTree.RespawnSeconds;
                ActionProgressSeconds = 0f;
                RewardFeedback?.Invoke($"+{selectedTree.CompletionXp} Woodcutting XP");
                SaveManager.Instance?.SaveNow();
            }
        }

        private bool GrantCrossedThresholds(float previousDurability, float currentDurability)
        {
            var previousPercent = selectedTree.MaximumDurability <= 0f ? 0f : previousDurability / selectedTree.MaximumDurability;
            var currentPercent = selectedTree.MaximumDurability <= 0f ? 0f : currentDurability / selectedTree.MaximumDurability;

            foreach (var threshold in selectedTree.ThresholdRewards
                         .Where(threshold => threshold != null)
                         .OrderByDescending(threshold => threshold.remainingDurabilityPercent))
            {
                if (completedThresholds.Contains(threshold.thresholdKey))
                {
                    continue;
                }

                if (previousPercent > threshold.remainingDurabilityPercent && currentPercent <= threshold.remainingDurabilityPercent)
                {
                    if (!GrantRewards(threshold))
                    {
                        CurrentDurability = currentDurability;
                        StopWoodcutting("Inventory Full");
                        return false;
                    }

                    completedThresholds.Add(threshold.thresholdKey);
                    StateChanged?.Invoke();
                }
            }

            SaveManager.Instance?.SaveNow();
            return true;
        }

        private bool GrantRewards(WoodcuttingThresholdReward threshold)
        {
            foreach (var reward in threshold.rewards.Where(reward => reward != null && reward.quantity > 0))
            {
                if (!inventorySystem.CanAccept(reward.itemId, reward.quantity))
                {
                    return false;
                }
            }

            foreach (var reward in threshold.rewards.Where(reward => reward != null && reward.quantity > 0))
            {
                if (!inventorySystem.TryAddItem(reward.itemId, reward.quantity))
                {
                    return false;
                }

                RewardFeedback?.Invoke(FormatReward(reward.itemId, reward.quantity));
            }

            return true;
        }

        private string FormatReward(string itemId, long quantity)
        {
            var displayName = itemId;
            if (inventorySystem.ItemDatabase != null && inventorySystem.ItemDatabase.TryGetItem(itemId, out var item) && item != null)
            {
                displayName = item.DisplayName;
            }

            return $"+{quantity} {displayName}";
        }

        private void Respawn()
        {
            IsRespawning = false;
            RespawnRemainingSeconds = 0f;
            CurrentDurability = selectedTree.MaximumDurability;
            ActionProgressSeconds = 0f;
            completedThresholds.Clear();
            StateChanged?.Invoke();
            SaveManager.Instance?.SaveNow();
        }

        private void OnProfessionLevelChanged(string professionId, int level)
        {
            if (professionId == WoodcuttingConstants.ProfessionId)
            {
                Notification?.Invoke($"Woodcutting Level {level}");
                StateChanged?.Invoke();
            }
        }
    }

    internal static class WoodcuttingInventoryExtensions
    {
        public static bool CanAcceptAnyThreshold(this InventorySystem inventorySystem, WoodcuttingTreeDefinition tree)
        {
            if (inventorySystem == null || tree == null)
            {
                return false;
            }

            var sawReward = false;
            foreach (var reward in tree.ThresholdRewards
                         .Where(threshold => threshold != null)
                         .SelectMany(threshold => threshold.rewards)
                         .Where(reward => reward != null && reward.quantity > 0))
            {
                sawReward = true;
                if (inventorySystem.CanAccept(reward.itemId, reward.quantity))
                {
                    return true;
                }
            }

            return !sawReward;
        }
    }
}
