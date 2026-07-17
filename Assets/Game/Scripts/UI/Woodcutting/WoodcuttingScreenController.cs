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
        [SerializeField] private RuntimeFillBar headerXpBar;
        [SerializeField] private RuntimeFillBar durabilityBar;
        [SerializeField] private RuntimeFillBar actionProgressBar;
        [SerializeField] private TMP_Text cuttingIntervalValueText;
        [SerializeField] private TMP_Text selectedRewardValueText;
        [SerializeField] private TMP_Text inventorySpaceValueText;
        [SerializeField] private TMP_Text gatheringPowerValueText;
        [SerializeField] private TMP_Text gatheringPowerBreakdownText;
        [SerializeField] private TMP_Text activeBonusesText;
        [SerializeField] private Button startStopButton;
        [SerializeField] private Transform startState;
        [SerializeField] private Transform stopState;
        [SerializeField] private Transform treeCardContainer;
        [SerializeField] private Transform rewardPreviewContainer;
        [SerializeField] private Transform thresholdContainer;

        private string latestFeedback = string.Empty;

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
            var woodcuttingHeader = HierarchySearch.FindDeep(transform, "[HEADER] WoodcuttingHeader");
            headerXpBar ??= HierarchySearch.FindOrAddFillBar(woodcuttingHeader, "[BAR] WoodcuttingXPBar");
            durabilityBar ??= HierarchySearch.FindOrAddFillBar(transform, "[BAR] TargetDurabilityBar");
            actionProgressBar ??= HierarchySearch.FindOrAddFillBar(transform, "[BAR] ActionProgressBar");
            gatheringPowerValueText ??= HierarchySearch.FindText(transform, "[TEXT] GatheringPowerValue");
            gatheringPowerBreakdownText ??= HierarchySearch.FindText(transform, "[TEXT] GatheringPowerBreakdown");
            activeBonusesText ??= HierarchySearch.FindText(transform, "[TEXT] ActiveBonusesList");
            startStopButton ??= HierarchySearch.FindButton(transform, "[BUTTON] WoodcuttingToggleButton");
            startState ??= HierarchySearch.FindDeep(transform, "[STATE] StartWoodcuttingState");
            stopState ??= HierarchySearch.FindDeep(transform, "[STATE] StopWoodcuttingState");
            treeCardContainer ??= HierarchySearch.FindDeep(transform, "[DYNAMIC CONTENT] TreeCardContainer");
            rewardPreviewContainer ??= HierarchySearch.FindDeep(transform, "[CONTAINER] SelectedTreeRewardsPreview");
            if (thresholdContainer == null)
            {
                thresholdContainer = HierarchySearch.FindDeep(transform, "[CONTAINER] ThresholdContainer");
            }

            var selectedStats = HierarchySearch.FindDeep(transform, "[CONTAINER] SelectedTreeStats");
            if (selectedStats != null)
            {
                cuttingIntervalValueText ??= FindStatValue(selectedStats, "Cutting Interval");
                selectedRewardValueText ??= FindStatValue(selectedStats, "Selected Reward");
            }

            var inventoryPanel = HierarchySearch.FindDeep(transform, "[PANEL] InventoryCapacityPanel");
            inventorySpaceValueText ??= HierarchySearch.FindText(inventoryPanel, "[TEXT] InventorySpaceValue");

            if (startStopButton != null)
            {
                startStopButton.onClick.RemoveListener(OnStartStopClicked);
                startStopButton.onClick.AddListener(OnStartStopClicked);
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
                levelText.text = xpNeeded > 0 ? $"Level {level}  XP {xpIntoLevel} / {xpNeeded}" : $"Level {level}";
            }

            headerXpBar?.SetValue(
                progressionSystem.GetLevelProgress01(WoodcuttingConstants.ProfessionId),
                xpNeeded > 0 ? $"{xpIntoLevel} / {xpNeeded} XP" : "Max Level");

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
            }

            RefreshBars(tree);
            RefreshStats(tree);
            RefreshThresholds(tree);
            RefreshCards(tree);
            RefreshRewards(tree);

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
                var bonusText = equipmentSystem == null
                    ? "No active equipment bonuses"
                    : "Main-Hand and Offhand Woodcutting modifiers resolved from equipment";

                activeBonusesText.text = string.IsNullOrWhiteSpace(latestFeedback)
                    ? bonusText
                    : $"{latestFeedback}\n{bonusText}";
            }

            if (inventorySpaceValueText != null && inventorySystem != null)
            {
                inventorySpaceValueText.text = $"{inventorySystem.FreeSlots} / {inventorySystem.Capacity}";
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
                var iconText = HierarchySearch.FindText(child, "[ICON] LootIcon");
                var nameText = HierarchySearch.FindText(child, "[TEXT] LootName");
                var ownedText = HierarchySearch.FindText(child, "[TEXT] OwnedAmount");
                var owned = inventorySystem != null ? inventorySystem.GetQuantity(reward.itemId) : 0;

                if (iconText != null)
                {
                    iconText.text = MakeRewardIcon(reward.itemId);
                }

                if (nameText != null)
                {
                    nameText.text = $"{FormatQuantityAndItem(reward.quantity, reward.itemId)}  Owned: {owned}";
                }

                if (ownedText != null)
                {
                    ownedText.text = string.Empty;
                }
            }
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
            if (inventorySystem != null && inventorySystem.ItemDatabase != null && inventorySystem.ItemDatabase.TryGetItem(itemId, out var item) && item != null)
            {
                return item.DisplayName;
            }

            return itemId;
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

        private void OnRewardFeedback(string message)
        {
            latestFeedback = message;
            Refresh();
        }

        private void OnNotification(string message)
        {
            latestFeedback = message;
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
