using System.Linq;
using IdleGame.Equipment;
using IdleGame.Inventory;
using IdleGame.Items;
using IdleGame.Professions.Woodcutting;
using IdleGame.Progression;
using IdleGame.UI.Shared;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IdleGame.UI.Woodcutting
{
    public sealed class WoodcuttingScreenController : MonoBehaviour
    {
        [SerializeField] private WoodcuttingSystem woodcuttingSystem;
        [SerializeField] private ProfessionProgressionSystem progressionSystem;
        [SerializeField] private InventorySystem inventorySystem;
        [SerializeField] private EquipmentSystem equipmentSystem;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private TMP_Text selectedTreeNameText;
        [SerializeField] private TMP_Text selectedTreeDescriptionText;
        [SerializeField] private TMP_Text activeTargetHeaderText;
        [SerializeField] private TMP_Text targetPlaceholderText;
        [SerializeField] private Image selectedTreeImage;
        [SerializeField] private RuntimeFillBar headerXpBar;
        [SerializeField] private TMP_Text headerNextLevelText;
        [SerializeField] private RuntimeFillBar durabilityBar;
        [SerializeField] private RuntimeFillBar actionProgressBar;
        [SerializeField] private TMP_Text cuttingIntervalValueText;
        [SerializeField] private TMP_Text levelRequirementValueText;
        [SerializeField] private TMP_Text rewardRateValueText;
        [SerializeField] private TMP_Text respawnValueText;
        [SerializeField] private TMP_Text selectedRewardValueText;
        [SerializeField] private TMP_Text inventorySpaceValueText;
        [SerializeField] private TMP_Text gatheringPowerValueText;
        [SerializeField] private TMP_Text gatheringPowerBreakdownText;
        [SerializeField] private TMP_Text activeBonusesText;
        [SerializeField] private Button startStopButton;
        [SerializeField] private Button startButton;
        [SerializeField] private Button stopButton;
        [SerializeField] private Transform startState;
        [SerializeField] private Transform stopState;
        [SerializeField] private Transform treeCardContainer;
        [SerializeField] private Transform rewardPreviewContainer;
        [SerializeField] private Transform thresholdContainer;
        [SerializeField] private TMP_Text mainHandSlotText;
        [SerializeField] private TMP_Text offhandSlotText;
        [SerializeField] private TMP_Text ringSlotText;
        [SerializeField] private TMP_Text amuletSlotText;
        [SerializeField] private TMP_Text capeSlotText;
        [SerializeField] private TMP_Text relicSlotText;
        [SerializeField] private TMP_Text companionSlotText;

        private void Awake()
        {
            AutoBind();
        }

        private void OnEnable()
        {
            Subscribe();
            Refresh();
        }

        private void OnDisable()
        {
            if (woodcuttingSystem != null)
            {
                woodcuttingSystem.StateChanged -= Refresh;
                woodcuttingSystem.RewardFeedback -= OnRewardFeedback;
                woodcuttingSystem.Notification -= OnNotification;
            }

            if (progressionSystem != null)
            {
                progressionSystem.ProgressChanged -= OnProgressChanged;
            }

            if (inventorySystem != null)
            {
                inventorySystem.InventoryChanged -= Refresh;
            }

            if (equipmentSystem != null)
            {
                equipmentSystem.EquipmentChanged -= Refresh;
            }
        }

        public void SelectTree(string treeId)
        {
            woodcuttingSystem?.SelectTree(treeId);
        }

        public void ConfigureForEditor(
            WoodcuttingSystem woodcutting,
            ProfessionProgressionSystem progression,
            InventorySystem inventory,
            EquipmentSystem equipment)
        {
            woodcuttingSystem = woodcutting;
            progressionSystem = progression;
            inventorySystem = inventory;
            equipmentSystem = equipment;
            AutoBind();
        }

        public void AutoBind()
        {
            levelText ??= HierarchySearch.FindText(transform, "[TEXT] ProfessionLevel");
            selectedTreeNameText ??= HierarchySearch.FindText(transform, "[TEXT] SelectedTreeName");
            selectedTreeDescriptionText ??= HierarchySearch.FindText(transform, "[TEXT] SelectedTreeDescription");
            activeTargetHeaderText ??= HierarchySearch.FindText(transform, "[HEADER] ActiveTargetHeader");
            targetPlaceholderText ??= HierarchySearch.FindText(transform, "[TEXT] TargetImagePlaceholder");
            selectedTreeImage ??= HierarchySearch.FindImage(transform, "[IMAGE] SelectedTreeImage");
            var woodcuttingHeader = HierarchySearch.FindDeep(transform, "[HEADER] WoodcuttingHeader");
            headerXpBar ??= HierarchySearch.FindOrAddFillBar(woodcuttingHeader, "[BAR] WoodcuttingXPBar");
            headerNextLevelText ??= HierarchySearch.FindText(woodcuttingHeader, "[TEXT] NextLevelText");
            durabilityBar ??= HierarchySearch.FindOrAddFillBar(transform, "[BAR] TargetDurabilityBar");
            actionProgressBar ??= HierarchySearch.FindOrAddFillBar(transform, "[BAR] ActionProgressBar");
            gatheringPowerValueText ??= HierarchySearch.FindText(transform, "[TEXT] GatheringPowerValue");
            gatheringPowerBreakdownText ??= HierarchySearch.FindText(transform, "[TEXT] GatheringPowerBreakdown");
            activeBonusesText ??= HierarchySearch.FindText(transform, "[TEXT] ActiveBonusesList");
            startStopButton ??= HierarchySearch.FindButton(transform, "[BUTTON] WoodcuttingToggleButton");
            startButton ??= HierarchySearch.FindButton(transform, "[BUTTON] StartWoodcuttingButton");
            stopButton ??= HierarchySearch.FindButton(transform, "[BUTTON] StopWoodcuttingButton");
            startState ??= HierarchySearch.FindDeep(transform, "[STATE] StartWoodcuttingState");
            stopState ??= HierarchySearch.FindDeep(transform, "[STATE] StopWoodcuttingState");
            treeCardContainer ??= HierarchySearch.FindDeep(transform, "[DYNAMIC CONTENT] TreeCardContainer");
            rewardPreviewContainer ??= HierarchySearch.FindDeep(transform, "[CONTAINER] SelectedTreeRewardsPreview");
            rewardPreviewContainer ??= HierarchySearch.FindDeep(transform, "[DYNAMIC CONTENT] ObtainableItemContainer");
            mainHandSlotText ??= HierarchySearch.FindText(transform, "[TEXT] MainHandSlotValue");
            offhandSlotText ??= HierarchySearch.FindText(transform, "[TEXT] OffhandSlotValue");
            ringSlotText ??= HierarchySearch.FindText(transform, "[TEXT] RingSlotValue");
            amuletSlotText ??= HierarchySearch.FindText(transform, "[TEXT] AmuletSlotValue");
            capeSlotText ??= HierarchySearch.FindText(transform, "[TEXT] CapeSlotValue");
            relicSlotText ??= HierarchySearch.FindText(transform, "[TEXT] RelicSlotValue");
            companionSlotText ??= HierarchySearch.FindText(transform, "[TEXT] CompanionSlotValue");
            if (thresholdContainer == null)
            {
                thresholdContainer = HierarchySearch.FindDeep(transform, "[CONTAINER] ThresholdContainer");
            }

            var selectedStats = HierarchySearch.FindDeep(transform, "[CONTAINER] SelectedTreeStats");
            if (selectedStats != null)
            {
                cuttingIntervalValueText ??= FindStatValue(selectedStats, "Cutting Interval");
                cuttingIntervalValueText ??= FindStatValue(selectedStats, "Cut Interval");
                levelRequirementValueText ??= FindStatValue(selectedStats, "Level Requirement");
                rewardRateValueText ??= FindStatValue(selectedStats, "Logs per Action");
                selectedRewardValueText ??= FindStatValue(selectedStats, "Selected Reward");
            }

            levelRequirementValueText ??= HierarchySearch.FindText(transform, "[TEXT] LevelRequirementText");
            cuttingIntervalValueText ??= HierarchySearch.FindText(transform, "[TEXT] ActionIntervalText");
            rewardRateValueText ??= HierarchySearch.FindText(transform, "[TEXT] RewardRateText");
            respawnValueText ??= HierarchySearch.FindText(transform, "[TEXT] RespawnText");

            var inventoryPanel = HierarchySearch.FindDeep(transform, "[PANEL] InventoryCapacityPanel");
            inventorySpaceValueText ??= HierarchySearch.FindText(inventoryPanel, "[TEXT] InventorySpaceValue");
            inventorySpaceValueText ??= HierarchySearch.FindText(transform, "[TEXT] InventorySpaceText");

            if (startStopButton != null)
            {
                startStopButton.onClick.RemoveListener(OnStartStopClicked);
                startStopButton.onClick.AddListener(OnStartStopClicked);
            }

            if (startButton != null)
            {
                startButton.onClick.RemoveListener(StartWoodcutting);
                startButton.onClick.AddListener(StartWoodcutting);
            }

            if (stopButton != null)
            {
                stopButton.onClick.RemoveListener(StopWoodcutting);
                stopButton.onClick.AddListener(StopWoodcutting);
            }
        }

        private void Subscribe()
        {
            if (woodcuttingSystem != null)
            {
                woodcuttingSystem.StateChanged -= Refresh;
                woodcuttingSystem.RewardFeedback -= OnRewardFeedback;
                woodcuttingSystem.Notification -= OnNotification;
                woodcuttingSystem.StateChanged += Refresh;
                woodcuttingSystem.RewardFeedback += OnRewardFeedback;
                woodcuttingSystem.Notification += OnNotification;
            }

            if (progressionSystem != null)
            {
                progressionSystem.ProgressChanged -= OnProgressChanged;
                progressionSystem.ProgressChanged += OnProgressChanged;
            }

            if (inventorySystem != null)
            {
                inventorySystem.InventoryChanged -= Refresh;
                inventorySystem.InventoryChanged += Refresh;
            }

            if (equipmentSystem != null)
            {
                equipmentSystem.EquipmentChanged -= Refresh;
                equipmentSystem.EquipmentChanged += Refresh;
            }
        }

        private void Refresh()
        {
            if (woodcuttingSystem == null || progressionSystem == null)
            {
                return;
            }

            var tree = woodcuttingSystem.SelectedTree;
            var level = progressionSystem.GetLevel(WoodcuttingConstants.ProfessionId);
            var xpIntoLevel = progressionSystem.GetXpIntoLevel(WoodcuttingConstants.ProfessionId);
            var xpNeeded = progressionSystem.GetXpNeededForCurrentLevel(WoodcuttingConstants.ProfessionId);

            if (levelText != null)
            {
                levelText.text = $"Level {level}";
            }

            headerXpBar?.SetValue(
                progressionSystem.GetLevelProgress01(WoodcuttingConstants.ProfessionId),
                xpNeeded > 0 ? $"{xpIntoLevel} / {xpNeeded} XP" : "Max Level");

            if (headerNextLevelText != null)
            {
                headerNextLevelText.text = GetNextUnlockText(level);
            }

            if (selectedTreeNameText != null && tree != null)
            {
                selectedTreeNameText.text = tree.DisplayName;
            }

            if (selectedTreeDescriptionText != null && tree != null)
            {
                selectedTreeDescriptionText.text = tree.Description;
            }

            if (activeTargetHeaderText != null && tree != null)
            {
                activeTargetHeaderText.text = woodcuttingSystem.IsRespawning
                    ? $"RESPAWNING  -  {tree.DisplayName.ToUpperInvariant()}"
                    : $"ACTIVE TARGET  -  {tree.DisplayName.ToUpperInvariant()}";
            }

            if (targetPlaceholderText != null && tree != null)
            {
                targetPlaceholderText.text = tree.DisplayName.Replace(" ", "\n").ToUpperInvariant();
                targetPlaceholderText.gameObject.SetActive(tree.Icon == null);
            }

            if (selectedTreeImage != null && tree != null)
            {
                selectedTreeImage.sprite = tree.Icon;
                selectedTreeImage.preserveAspect = true;
                selectedTreeImage.color = tree.Icon == null ? new Color(0.075f, 0.095f, 0.10f, 0.98f) : Color.white;
            }

            RefreshBars(tree);
            RefreshStats(tree);
            RefreshThresholds(tree);
            RefreshCards(tree);
            RefreshRewards(tree);
            RefreshEquipmentSummary();
            RefreshActionButtons(tree);

            if (startState != null)
            {
                startState.gameObject.SetActive(!woodcuttingSystem.IsActive);
            }

            if (stopState != null)
            {
                stopState.gameObject.SetActive(woodcuttingSystem.IsActive);
            }
        }

        private void RefreshBars(WoodcuttingTreeDefinition tree)
        {
            if (tree == null)
            {
                return;
            }

            var durability01 = tree.MaximumDurability <= 0f ? 0f : woodcuttingSystem.CurrentDurability / tree.MaximumDurability;
            durabilityBar?.SetValue(durability01, $"{Mathf.CeilToInt(woodcuttingSystem.CurrentDurability)} / {Mathf.CeilToInt(tree.MaximumDurability)}");

            if (woodcuttingSystem.IsRespawning)
            {
                var respawn01 = tree.RespawnSeconds <= 0f ? 1f : 1f - (woodcuttingSystem.RespawnRemainingSeconds / tree.RespawnSeconds);
                actionProgressBar?.SetValue(respawn01, $"Respawn {Mathf.Max(0f, woodcuttingSystem.RespawnRemainingSeconds):0.0}s");
            }
            else
            {
                var interval = Mathf.Max(0.05f, woodcuttingSystem.CurrentModifiers.finalActionInterval);
                actionProgressBar?.SetValue(woodcuttingSystem.ActionProgressSeconds / interval, $"Next action {Mathf.Max(0f, interval - woodcuttingSystem.ActionProgressSeconds):0.0}s");
            }
        }

        private void RefreshStats(WoodcuttingTreeDefinition tree)
        {
            var modifiers = woodcuttingSystem.CurrentModifiers;

            if (cuttingIntervalValueText != null)
            {
                cuttingIntervalValueText.text = $"{modifiers.finalActionInterval:0.00}s";
            }

            if (levelRequirementValueText != null && tree != null)
            {
                levelRequirementValueText.text = $"Level Requirement     {tree.RequiredLevel}";
            }

            if (rewardRateValueText != null && tree != null)
            {
                var totalLogs = tree.ThresholdRewards
                    .SelectMany(threshold => threshold.rewards)
                    .Where(reward => reward != null && reward.quantity > 0)
                    .Sum(reward => reward.quantity);
                rewardRateValueText.text = $"Guaranteed Logs       {totalLogs}";
            }

            if (respawnValueText != null && tree != null)
            {
                respawnValueText.text = woodcuttingSystem.IsRespawning
                    ? $"Respawning            {Mathf.Max(0f, woodcuttingSystem.RespawnRemainingSeconds):0.0}s"
                    : $"Respawn               {tree.RespawnSeconds:0.0}s";
            }

            if (gatheringPowerValueText != null)
            {
                gatheringPowerValueText.text = $"{modifiers.finalPower:0}";
            }

            if (gatheringPowerBreakdownText != null)
            {
                gatheringPowerBreakdownText.text = $"Base {modifiers.basePower:0}     Equipment +{modifiers.flatPowerFromEquipment:0}     Other +0";
            }

            if (activeBonusesText != null)
            {
                activeBonusesText.text = BuildActiveBonusText(modifiers);
            }

            if (inventorySpaceValueText != null && inventorySystem != null)
            {
                inventorySpaceValueText.text = $"Inventory Space Available     {inventorySystem.FreeSlots} / {inventorySystem.Capacity}";
            }

            if (selectedRewardValueText != null && tree != null)
            {
                selectedRewardValueText.text = string.Join(", ", tree.ThresholdRewards
                    .SelectMany(threshold => threshold.rewards)
                    .Select(reward => GetItemName(reward.itemId))
                    .Distinct());
            }
        }

        private void RefreshCards(WoodcuttingTreeDefinition selectedTree)
        {
            if (treeCardContainer == null)
            {
                return;
            }

            var trees = woodcuttingSystem.Trees.ToList();
            for (var i = 0; i < treeCardContainer.childCount; i++)
            {
                var child = treeCardContainer.GetChild(i);
                if (i >= trees.Count)
                {
                    child.gameObject.SetActive(false);
                    continue;
                }

                child.gameObject.SetActive(true);
                var card = child.GetComponent<TreeActivityCard>() ?? child.gameObject.AddComponent<TreeActivityCard>();
                var tree = trees[i];
                card.Bind(tree, this, woodcuttingSystem.IsTreeUnlocked(tree), selectedTree == tree);
            }
        }

        private void RefreshThresholds(WoodcuttingTreeDefinition tree)
        {
            if (thresholdContainer == null)
            {
                thresholdContainer = HierarchySearch.FindDeep(transform, "[CONTAINER] ThresholdContainer");
            }

            if (thresholdContainer == null || tree == null)
            {
                return;
            }

            var thresholds = tree.ThresholdRewards
                .Where(threshold => threshold != null)
                .OrderByDescending(threshold => threshold.remainingDurabilityPercent)
                .ToList();
            var treeUnlocked = woodcuttingSystem.IsTreeUnlocked(tree);
            var currentPercent = woodcuttingSystem.CurrentDurabilityPercent;
            var currentThreshold = thresholds.FirstOrDefault(threshold =>
                treeUnlocked &&
                !woodcuttingSystem.IsThresholdCompleted(threshold) &&
                currentPercent > threshold.remainingDurabilityPercent);

            for (var i = 0; i < thresholdContainer.childCount; i++)
            {
                var child = thresholdContainer.GetChild(i);
                if (i >= thresholds.Count)
                {
                    child.gameObject.SetActive(false);
                    continue;
                }

                child.gameObject.SetActive(true);
                var threshold = thresholds[i];
                var label = HierarchySearch.FindText(child, "[TEXT] ThresholdLabel");
                if (label == null)
                {
                    continue;
                }

                var thresholdPercent = Mathf.RoundToInt(threshold.remainingDurabilityPercent * 100f);
                var rewardText = string.Join(", ", threshold.rewards
                    .Where(reward => reward != null && reward.quantity > 0)
                    .Select(reward => FormatQuantityAndItem(reward.quantity, reward.itemId)));
                var status = GetThresholdStatus(treeUnlocked, threshold, currentThreshold, currentPercent);

                label.text = $"{thresholdPercent}%  {status}\n{rewardText}";
                ApplyThresholdHighlight(child, status);
            }
        }

        private static void ApplyThresholdHighlight(Transform thresholdCard, string status)
        {
            var image = thresholdCard.GetComponent<Image>();
            var outline = thresholdCard.GetComponent<Outline>() ?? thresholdCard.gameObject.AddComponent<Outline>();
            var isCurrent = status == "CURRENT";
            var isNext = status == "NEXT";

            outline.effectColor = isCurrent
                ? new Color(1f, 0.78f, 0.18f, 1f)
                : isNext
                    ? new Color(0.86f, 0.59f, 0.20f, 0.95f)
                    : new Color(0.34f, 0.20f, 0.09f, 0.95f);
            outline.effectDistance = isCurrent
                ? new Vector2(3f, -3f)
                : isNext
                    ? new Vector2(2f, -2f)
                    : new Vector2(1f, -1f);

            if (image != null)
            {
                image.color = isCurrent
                    ? new Color(0.18f, 0.14f, 0.05f, 0.98f)
                    : new Color(0.075f, 0.095f, 0.10f, 0.98f);
            }
        }

        private string GetThresholdStatus(
            bool treeUnlocked,
            WoodcuttingThresholdReward threshold,
            WoodcuttingThresholdReward currentThreshold,
            float currentDurabilityPercent)
        {
            if (!treeUnlocked)
            {
                return "LOCKED";
            }

            if (woodcuttingSystem.IsThresholdCompleted(threshold))
            {
                return "DONE";
            }

            if (threshold == currentThreshold)
            {
                return woodcuttingSystem.IsActive ? "CURRENT" : "NEXT";
            }

            return currentDurabilityPercent <= threshold.remainingDurabilityPercent ? "READY" : "PENDING";
        }

        private void RefreshRewards(WoodcuttingTreeDefinition tree)
        {
            if (rewardPreviewContainer == null || tree == null)
            {
                return;
            }

            var rewardNames = tree.ThresholdRewards
                .SelectMany(threshold => threshold.rewards)
                .Where(reward => reward != null)
                .GroupBy(reward => reward.itemId)
                .Select(group => (itemId: group.Key, quantity: group.Sum(reward => reward.quantity)))
                .ToList();

            var cardIndex = 0;
            for (var i = 0; i < rewardPreviewContainer.childCount; i++)
            {
                var child = rewardPreviewContainer.GetChild(i);
                if (cardIndex >= rewardNames.Count)
                {
                    child.gameObject.SetActive(false);
                    continue;
                }

                child.gameObject.SetActive(true);
                var reward = rewardNames[cardIndex++];
                var iconImage = HierarchySearch.FindImage(child, "[ICON] LootIcon");
                var iconText = HierarchySearch.FindText(child, "[ICON] LootIcon");
                var nameText = HierarchySearch.FindText(child, "[TEXT] LootName");
                var ownedText = HierarchySearch.FindText(child, "[TEXT] OwnedAmount");
                var owned = inventorySystem != null ? inventorySystem.GetQuantity(reward.itemId) : 0;
                var item = GetItem(reward.itemId);

                if (iconImage != null)
                {
                    iconImage.sprite = item != null ? item.Icon : null;
                    iconImage.preserveAspect = true;
                    iconImage.color = item != null && item.Icon != null ? Color.white : new Color(0.18f, 0.22f, 0.16f, 1f);
                }

                if (iconText != null)
                {
                    iconText.gameObject.SetActive(item == null || item.Icon == null);
                    iconText.text = MakeRewardIcon(reward.itemId);
                }

                if (nameText != null)
                {
                    nameText.text = GetItemName(reward.itemId);
                }

                if (ownedText != null)
                {
                    ownedText.text = owned.ToString();
                }
                else if (nameText != null)
                {
                    nameText.text = $"{GetItemName(reward.itemId)}  Owned: {owned}";
                }
            }
        }

        private void RefreshActionButtons(WoodcuttingTreeDefinition tree)
        {
            var canStart = tree != null && !woodcuttingSystem.IsActive && woodcuttingSystem.IsTreeUnlocked(tree);
            if (startButton != null)
            {
                startButton.interactable = canStart;
            }

            if (stopButton != null)
            {
                stopButton.gameObject.SetActive(woodcuttingSystem.IsActive);
                stopButton.interactable = woodcuttingSystem.IsActive;
            }

            if (startStopButton != null)
            {
                startStopButton.interactable = woodcuttingSystem.IsActive || canStart;
            }
        }

        private void RefreshEquipmentSummary()
        {
            var mainHand = equipmentSystem?.GetEquippedItems()
                .FirstOrDefault(item => item != null && item.Slot == EquipmentSlot.MainHand);
            var offhand = equipmentSystem?.GetEquippedItems()
                .FirstOrDefault(item => item != null && item.Slot == EquipmentSlot.Offhand);
            var relic = equipmentSystem?.GetEquippedItems()
                .FirstOrDefault(item => item != null && item.Slot == EquipmentSlot.Relic);

            if (mainHandSlotText != null)
            {
                mainHandSlotText.text = mainHand == null
                    ? "Empty\nBase Woodcutting Power"
                    : $"{mainHand.DisplayName}\n{GetEquipmentBonusText(mainHand)}";
            }

            if (offhandSlotText != null)
            {
                offhandSlotText.text = mainHand != null && mainHand.TwoHanded
                    ? "Disabled\nTwo-handed Main-Hand"
                    : offhand == null
                        ? "Empty\nNo Offhand bonus"
                        : $"{offhand.DisplayName}\n{GetEquipmentBonusText(offhand)}";
            }

            if (ringSlotText != null)
            {
                ringSlotText.text = "Empty\nNo Woodcutting bonus";
            }

            if (amuletSlotText != null)
            {
                amuletSlotText.text = "Empty\nNo Woodcutting bonus";
            }

            if (capeSlotText != null)
            {
                capeSlotText.text = "Empty\nNo Woodcutting bonus";
            }

            if (relicSlotText != null)
            {
                relicSlotText.text = relic == null
                    ? "Empty\nNo Woodcutting bonus"
                    : $"{relic.DisplayName}\n{GetEquipmentBonusText(relic)}";
            }

            if (companionSlotText != null)
            {
                companionSlotText.text = "No Companion Assigned";
            }
        }

        private string BuildActiveBonusText(ProfessionModifierResult modifiers)
        {
            var lines = new System.Collections.Generic.List<string>();

            lines.Add($"Base Woodcutting Power +{modifiers.basePower:0}");

            if (modifiers.flatPowerFromEquipment != 0f)
            {
                lines.Add($"Equipment Power +{modifiers.flatPowerFromEquipment:0}");
            }

            if (modifiers.actionSpeedPercent != 0f)
            {
                lines.Add($"Equipment Action Speed +{modifiers.actionSpeedPercent:0.#}%");
            }

            if (modifiers.xpPercent != 0f)
            {
                lines.Add($"Equipment XP +{modifiers.xpPercent:0.#}%");
            }

            if (modifiers.outputPercent != 0f)
            {
                lines.Add($"Equipment Output +{modifiers.outputPercent:0.#}%");
            }

            if (lines.Count == 1)
            {
                lines.Add("No active equipment bonuses");
            }

            return string.Join("\n", lines);
        }

        private string GetEquipmentBonusText(EquipmentDefinition equipment)
        {
            var modifier = equipment.ProfessionModifiers
                .FirstOrDefault(entry => entry != null && entry.professionId == WoodcuttingConstants.ProfessionId);

            if (modifier == null)
            {
                return "No Woodcutting bonus";
            }

            var parts = new System.Collections.Generic.List<string>();
            if (modifier.flatPower != 0f)
            {
                parts.Add($"+{modifier.flatPower:0} Power");
            }

            if (modifier.actionSpeedPercent != 0f)
            {
                parts.Add($"+{modifier.actionSpeedPercent:0.#}% Speed");
            }

            if (modifier.xpPercent != 0f)
            {
                parts.Add($"+{modifier.xpPercent:0.#}% XP");
            }

            if (modifier.outputPercent != 0f)
            {
                parts.Add($"+{modifier.outputPercent:0.#}% Output");
            }

            return parts.Count == 0 ? "Woodcutting capable" : string.Join(", ", parts);
        }

        private string GetNextUnlockText(int currentLevel)
        {
            var nextTree = woodcuttingSystem.Trees
                .Where(tree => tree != null && tree.RequiredLevel > currentLevel)
                .OrderBy(tree => tree.RequiredLevel)
                .FirstOrDefault();

            return nextTree == null
                ? "All prototype trees unlocked"
                : $"Next Unlock: {nextTree.DisplayName} at Level {nextTree.RequiredLevel}";
        }

        private TMP_Text FindStatValue(Transform selectedStats, string label)
        {
            foreach (var text in selectedStats.GetComponentsInChildren<TMP_Text>(true))
            {
                if (text.text == label)
                {
                    return HierarchySearch.FindText(text.transform.parent, "[TEXT] StatValue");
                }
            }

            return null;
        }

        private string GetItemName(string itemId)
        {
            var item = GetItem(itemId);
            if (item != null)
            {
                return item.DisplayName;
            }

            return itemId;
        }

        private ItemDefinition GetItem(string itemId)
        {
            return inventorySystem != null &&
                   inventorySystem.ItemDatabase != null &&
                   inventorySystem.ItemDatabase.TryGetItem(itemId, out var item)
                ? item
                : null;
        }

        private string FormatQuantityAndItem(long quantity, string itemId)
        {
            var itemName = GetItemName(itemId);
            if (quantity != 1 && !itemName.EndsWith("s", System.StringComparison.Ordinal))
            {
                itemName += "s";
            }

            return $"{quantity} {itemName}";
        }

        private static string MakeRewardIcon(string itemId)
        {
            return itemId switch
            {
                "item_log_sproutwood" => "S",
                "item_log_mosswood" => "M",
                "item_log_ironbark" => "I",
                _ => "*"
            };
        }

        private void OnStartStopClicked()
        {
            if (woodcuttingSystem == null)
            {
                return;
            }

            if (woodcuttingSystem.IsActive)
            {
                woodcuttingSystem.StopWoodcutting();
            }
            else
            {
                woodcuttingSystem.StartWoodcutting();
            }
        }

        private void StartWoodcutting()
        {
            woodcuttingSystem?.StartWoodcutting();
        }

        private void StopWoodcutting()
        {
            woodcuttingSystem?.StopWoodcutting();
        }

        private void OnRewardFeedback(string message)
        {
            Refresh();
        }

        private void OnNotification(string message)
        {
            Refresh();
        }

        private void OnProgressChanged(string professionId)
        {
            if (professionId == WoodcuttingConstants.ProfessionId)
            {
                Refresh();
            }
        }
    }
}
