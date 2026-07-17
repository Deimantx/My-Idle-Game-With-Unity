using IdleGame.UI.Shared;
using IdleGame.UI.Woodcutting;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace IdleGame.Editor
{
    public static class WoodcuttingReferenceScreenRebuilder
    {
        private static readonly Color Deep = new(0.025f, 0.035f, 0.04f, 0.98f);
        private static readonly Color Panel = new(0.045f, 0.065f, 0.075f, 0.98f);
        private static readonly Color Raised = new(0.075f, 0.095f, 0.10f, 0.98f);
        private static readonly Color Border = new(0.34f, 0.20f, 0.09f, 0.95f);
        private static readonly Color Gold = new(0.86f, 0.59f, 0.20f, 1f);
        private static readonly Color Text = new(0.92f, 0.86f, 0.75f, 1f);
        private static readonly Color Muted = new(0.64f, 0.57f, 0.48f, 1f);
        private static readonly Color Green = new(0.12f, 0.43f, 0.18f, 1f);
        private static readonly Color Blue = new(0.06f, 0.42f, 0.65f, 1f);
        private static readonly Color Red = new(0.50f, 0.08f, 0.08f, 1f);
        private const string WoodcuttingIconPath = "Assets/Game/UI/Icons/Profeesions/Woodcutting/WoodcuttingIcon.PNG";

        [MenuItem("Idle Game/Rebuild Woodcutting Reference Screen")]
        public static void Rebuild()
        {
            var screen = FindSceneObject("[SCREEN] WoodcuttingScreen");
            if (screen == null)
            {
                Debug.LogError("Could not find [SCREEN] WoodcuttingScreen.");
                return;
            }

            Undo.RegisterFullObjectHierarchyUndo(screen.gameObject, "Rebuild Woodcutting Screen");
            ClearChildren(screen);
            ConfigureRoot(screen);
            BuildWoodcuttingScreen(screen);
            RebuildActiveActivityBar();
            EditorSceneManager.MarkSceneDirty(screen.gameObject.scene);
        }

        private static void ConfigureRoot(Transform screen)
        {
            var image = screen.GetComponent<Image>() ?? screen.gameObject.AddComponent<Image>();
            image.color = Deep;

            var outline = screen.GetComponent<Outline>() ?? screen.gameObject.AddComponent<Outline>();
            outline.effectColor = Border;
            outline.effectDistance = new Vector2(1f, -1f);

            var layout = screen.GetComponent<VerticalLayoutGroup>() ?? screen.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(12, 12, 10, 10);
            layout.spacing = 10f;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;
        }

        private static void BuildWoodcuttingScreen(Transform screen)
        {
            var controller = screen.GetComponent<WoodcuttingScreenController>();

            var header = PanelObject("[HEADER] WoodcuttingHeader", screen, Raised, 96f);
            var icon = ImageObject("[ICON] WoodcuttingIcon", header.transform, new Vector2(76, 76), new Vector2(50, 0), Text);
            var iconSprite = LoadSprite(WoodcuttingIconPath);
            ApplySprite(icon.GetComponent<Image>(), iconSprite);
            var headerGlyph = TextObject("[TEXT] WoodcuttingIconGlyph", icon.transform, "TREE", 15f, Gold, TextAlignmentOptions.Center, Stretch());
            headerGlyph.gameObject.SetActive(iconSprite == null);
            var title = TextObject("[TEXT] WoodcuttingTitle", header.transform, "Woodcutting", 34f, Text, TextAlignmentOptions.Left, Anchor(0, .5f, 0, .5f, 266, 18, 280, 42));
            var level = TextObject("[TEXT] WoodcuttingLevelText", header.transform, "Level 1", 19f, Gold, TextAlignmentOptions.Left, Anchor(0, .5f, 0, .5f, 206, -18, 160, 28));
            var xpBar = BarObject("[BAR] WoodcuttingXPBar", header.transform, Anchor(0, .5f, 0, .5f, 535, -22, 430, 18), Blue, "[TEXT] WoodcuttingXPText");
            var nextLevel = TextObject("[TEXT] NextLevelText", header.transform, "Next Unlock: Mosswood Tree at Level 5", 16f, Muted, TextAlignmentOptions.Left, Anchor(0, .5f, 0, .5f, 970, -18, 420, 28));
            ButtonObject("[BUTTON] StatisticsButton", header.transform, "Stats", Anchor(1, .5f, 1, .5f, -210, 0, 112, 42), Raised);
            ButtonObject("[BUTTON] HelpButton", header.transform, "Help", Anchor(1, .5f, 1, .5f, -82, 0, 92, 42), Raised);

            var main = LayoutObject("[LAYOUT] WoodcuttingMainContent", screen, 0f, true, true);
            var mainLayout = main.AddComponent<HorizontalLayoutGroup>();
            mainLayout.padding = new RectOffset(0, 0, 0, 0);
            mainLayout.spacing = 12f;
            mainLayout.childControlWidth = true;
            mainLayout.childControlHeight = true;
            mainLayout.childForceExpandWidth = true;
            mainLayout.childForceExpandHeight = true;

            var treePanel = ColumnPanel("[PANEL] TreeSelectionPanel", main.transform, 0.25f, 300f);
            TextObject("[HEADER] TreeSelectionHeader", treePanel.transform, "AVAILABLE TREES", 17f, Gold, TextAlignmentOptions.Left, Anchor(0, 1, 1, 1, 18, -22, 260, 28));
            var treeScroll = PanelObject("[SCROLL] TreeSelectionScroll", treePanel.transform, Deep, 0f, true);
            StretchRect(treeScroll.GetComponent<RectTransform>(), 14, 14, 52, 14);
            var cardContainer = LayoutObject("[DYNAMIC CONTENT] TreeCardContainer", treeScroll.transform, 0f, false, false);
            StretchRect(cardContainer.GetComponent<RectTransform>(), 0, 0, 0, 0);
            var cards = cardContainer.AddComponent<VerticalLayoutGroup>();
            cards.padding = new RectOffset(8, 8, 8, 8);
            cards.spacing = 12f;
            cards.childControlWidth = true;
            cards.childControlHeight = true;
            cards.childForceExpandWidth = true;
            cards.childForceExpandHeight = false;
            for (var i = 0; i < 3; i++)
            {
                CreateTreeCard(cardContainer.transform, i);
            }

            var selectedPanel = ColumnPanel("[PANEL] SelectedTreePanel", main.transform, 0.45f, 520f);
            var selectedName = TextObject("[TEXT] SelectedTreeName", selectedPanel.transform, "Sproutwood Tree", 28f, Gold, TextAlignmentOptions.Center, Anchor(0, 1, 1, 1, 0, -26, 0, 42));
            var description = TextObject("[TEXT] SelectedTreeDescription", selectedPanel.transform, "A young and fragile tree commonly found near Greenvale.", 15f, Muted, TextAlignmentOptions.Center, Anchor(0, 1, 1, 1, 24, -62, -24, 28));
            description.textWrappingMode = TextWrappingModes.Normal;

            var info = PanelObject("[LAYOUT] TreeInformationLayout", selectedPanel.transform, Deep, 300f);
            SetRect(info.GetComponent<RectTransform>(), Anchor(0, 1, 1, 1, 14, -265, -14, 330));
            var treeArt = PanelObject("[IMAGE] SelectedTreeImage", info.transform, Raised, 0f);
            SetRect(treeArt.GetComponent<RectTransform>(), new Vector2(0, .5f), new Vector2(0, .5f), new Vector2(118, 0), new Vector2(204, 238));
            var targetGlyph = TextObject("[TEXT] TargetImagePlaceholder", treeArt.transform, "SPROUTWOOD\nTREE", 20f, Gold, TextAlignmentOptions.Center, Stretch());
            var stats = PanelObject("[PANEL] SelectedTreeStats", info.transform, new Color(0, 0, 0, 0), 0f);
            StretchRect(stats.GetComponent<RectTransform>(), 238, 20, 18, 18);
            var statsLayout = stats.AddComponent<VerticalLayoutGroup>();
            statsLayout.padding = new RectOffset(0, 0, 6, 6);
            statsLayout.spacing = 6f;
            statsLayout.childControlWidth = true;
            statsLayout.childControlHeight = true;
            statsLayout.childForceExpandWidth = true;
            statsLayout.childForceExpandHeight = false;

            TextObject("[TEXT] TreeDurabilityLabel", stats.transform, "Tree Durability", 17f, Text, TextAlignmentOptions.Left, Stretch(), 26f);
            var durability = BarObject("[BAR] TreeDurabilityBar", stats.transform, Stretch(), Green, "[TEXT] TreeDurabilityText", 30f);
            var action = BarObject("[BAR] ActionProgressBar", stats.transform, Stretch(), Blue, "[TEXT] ActionProgressText", 24f);
            TextObject("[TEXT] LevelRequirementText", stats.transform, "Level Requirement     1", 16f, Text, TextAlignmentOptions.Left, Stretch(), 28f);
            TextObject("[TEXT] ActionIntervalText", stats.transform, "Cut Interval          1.50s", 16f, Text, TextAlignmentOptions.Left, Stretch(), 28f);
            TextObject("[TEXT] RewardRateText", stats.transform, "Guaranteed Logs       4", 16f, Text, TextAlignmentOptions.Left, Stretch(), 28f);
            var invText = TextObject("[TEXT] InventorySpaceText", stats.transform, "Inventory Space Available     100 / 100", 16f, Text, TextAlignmentOptions.Left, Stretch(), 28f);
            TextObject("[TEXT] RespawnText", stats.transform, "Respawn               3.0s", 16f, Muted, TextAlignmentOptions.Left, Stretch(), 28f);

            var thresholds = PanelObject("[CONTAINER] ThresholdContainer", selectedPanel.transform, new Color(0, 0, 0, 0), 54f);
            SetRect(thresholds.GetComponent<RectTransform>(), Anchor(0, 1, 1, 1, 14, -466, -14, 58));
            var thresholdLayout = thresholds.AddComponent<HorizontalLayoutGroup>();
            thresholdLayout.spacing = 8f;
            thresholdLayout.padding = new RectOffset(14, 14, 8, 4);
            thresholdLayout.childControlWidth = true;
            thresholdLayout.childControlHeight = true;
            thresholdLayout.childForceExpandWidth = true;
            thresholdLayout.childForceExpandHeight = true;
            for (var i = 0; i < 4; i++)
            {
                var row = PanelObject("[THRESHOLD] Threshold_" + i, thresholds.transform, Raised, 0f);
                row.AddComponent<LayoutElement>().flexibleWidth = 1f;
                TextObject("[TEXT] ThresholdLabel", row.transform, "75%\nPENDING", 12f, Text, TextAlignmentOptions.Center, Stretch());
            }

            var obtainable = PanelObject("[PANEL] ObtainableItemsPanel", selectedPanel.transform, Deep, 126f);
            SetRect(obtainable.GetComponent<RectTransform>(), Anchor(0, 1, 1, 1, 14, -565, -14, 126));
            TextObject("[HEADER] ObtainableItemsHeader", obtainable.transform, "OBTAINABLE ITEMS", 16f, Gold, TextAlignmentOptions.Left, Anchor(0, 1, .5f, 1, 16, -18, 260, 28));
            TextObject("[TEXT] OwnedHeading", obtainable.transform, "OWNED", 16f, Gold, TextAlignmentOptions.Right, Anchor(.5f, 1, 1, 1, 0, -18, -24, 28));
            var obtainableContainer = PanelObject("[DYNAMIC CONTENT] ObtainableItemContainer", obtainable.transform, new Color(0, 0, 0, 0), 0f);
            StretchRect(obtainableContainer.GetComponent<RectTransform>(), 14, 14, 42, 12);
            var lootRow = PanelObject("[ROW] ObtainableItemRow", obtainableContainer.transform, Raised, 0f);
            StretchRect(lootRow.GetComponent<RectTransform>(), 0, 0, 0, 0);
            var lootIcon = ImageObject("[ICON] LootIcon", lootRow.transform, new Vector2(64, 56), new Vector2(42, 0), Text);
            TextObject("[TEXT] LootIconFallback", lootIcon.transform, "S", 18f, Gold, TextAlignmentOptions.Center, Stretch());
            TextObject("[TEXT] LootName", lootRow.transform, "Sproutwood Log", 17f, Text, TextAlignmentOptions.Left, Anchor(0, 0, 1, 1, 84, 0, -150, 0));
            TextObject("[TEXT] OwnedAmount", lootRow.transform, "0", 22f, Green, TextAlignmentOptions.Right, Anchor(1, .5f, 1, .5f, -68, 0, 120, 44));

            var rightColumn = LayoutObject("[LAYOUT] WoodcuttingRightColumn", main.transform, 0f, true, true);
            rightColumn.AddComponent<LayoutElement>().flexibleWidth = 0.30f;
            rightColumn.GetComponent<LayoutElement>().minWidth = 340f;
            var rightLayout = rightColumn.AddComponent<VerticalLayoutGroup>();
            rightLayout.spacing = 10f;
            rightLayout.childControlWidth = true;
            rightLayout.childControlHeight = true;
            rightLayout.childForceExpandWidth = true;
            rightLayout.childForceExpandHeight = false;

            var equipment = PanelObject("[PANEL] ProfessionEquipmentPanel", rightColumn.transform, Deep, 250f);
            TextObject("[HEADER] EquipmentHeader", equipment.transform, "EQUIPMENT", 18f, Gold, TextAlignmentOptions.Center, Anchor(0, 1, 1, 1, 0, -22, 0, 30));
            CreateEquipmentSlot(equipment.transform, "[SLOT] MainHandSlot", "Main-Hand", "[TEXT] MainHandSlotValue", 0.18f, 0.57f);
            CreateEquipmentSlot(equipment.transform, "[SLOT] OffhandSlot", "Offhand", "[TEXT] OffhandSlotValue", 0.50f, 0.57f);
            CreateEquipmentSlot(equipment.transform, "[SLOT] RelicSlot", "Relic", "[TEXT] RelicSlotValue", 0.82f, 0.57f);
            CreateEquipmentSlot(equipment.transform, "[SLOT] RingSlot", "Ring", "[TEXT] RingSlotValue", 0.20f, 0.20f);
            CreateEquipmentSlot(equipment.transform, "[SLOT] AmuletSlot", "Amulet", "[TEXT] AmuletSlotValue", 0.50f, 0.20f);
            CreateEquipmentSlot(equipment.transform, "[SLOT] CapeSlot", "Cape", "[TEXT] CapeSlotValue", 0.80f, 0.20f);

            var companion = PanelObject("[PANEL] CompanionPanel", rightColumn.transform, Deep, 72f);
            TextObject("[TEXT] CompanionTitle", companion.transform, "COMPANION", 16f, Gold, TextAlignmentOptions.Left, Anchor(0, .5f, .45f, .5f, 26, 0, 180, 32));
            var companionSlot = PanelObject("[SLOT] CompanionSlot", companion.transform, Raised, 54f);
            SetRect(companionSlot.GetComponent<RectTransform>(), new Vector2(.55f, .5f), new Vector2(.55f, .5f), Vector2.zero, new Vector2(76, 54));
            TextObject("[TEXT] CompanionSlotValue", companion.transform, "No Companion Assigned", 13f, Muted, TextAlignmentOptions.Left, Anchor(.68f, 0, 1, 1, 0, 0, -12, 0));

            var power = PanelObject("[PANEL] WoodcuttingPowerPanel", rightColumn.transform, Deep, 0f, true);
            TextObject("[HEADER] WoodcuttingPowerHeader", power.transform, "WOODCUTTING POWER", 17f, Gold, TextAlignmentOptions.Center, Anchor(0, 1, 1, 1, 0, -22, 0, 30));
            TextObject("[TEXT] GatheringPowerValue", power.transform, "5", 40f, Green, TextAlignmentOptions.Center, Anchor(0, 1, 1, 1, 0, -72, 0, 52));
            TextObject("[TEXT] GatheringPowerBreakdown", power.transform, "Base 5     Equipment +0     Other +0", 13f, Muted, TextAlignmentOptions.Center, Anchor(0, 1, 1, 1, 14, -116, -14, 28));
            TextObject("[HEADER] ActiveBonusesHeader", power.transform, "ACTIVE BONUS", 15f, Gold, TextAlignmentOptions.Left, Anchor(0, 1, 1, 1, 18, -152, -18, 24));
            TextObject("[TEXT] ActiveBonusesList", power.transform, "Base Woodcutting Power +5", 15f, Text, TextAlignmentOptions.TopLeft, Anchor(0, 0, 1, 1, 18, 14, -18, -180));

            var actions = PanelObject("[ACTIONS] WoodcuttingActionBar", screen, Deep, 96f);
            var start = ButtonObject("[BUTTON] StartWoodcuttingButton", actions.transform, "START", Anchor(.5f, .5f, .5f, .5f, -160, 0, 520, 70), Green, 30f);
            TextObject("[ICON] StartButtonIcon", start.transform, "AXE", 14f, Gold, TextAlignmentOptions.Center, Anchor(0, .5f, 0, .5f, 74, 0, 62, 42));
            var stop = ButtonObject("[BUTTON] StopWoodcuttingButton", actions.transform, "STOP", Anchor(.5f, .5f, .5f, .5f, 260, 0, 180, 60), Red, 24f);

            AssignWoodcuttingReferences(controller, level, selectedName, description, targetGlyph, xpBar, nextLevel, durability, action, invText, start, stop, cardContainer.transform, obtainableContainer.transform, thresholds.transform);
        }

        private static void RebuildActiveActivityBar()
        {
            var activeBar = FindSceneObject("[PERSISTENT] ActiveActivityBar");
            if (activeBar == null)
            {
                return;
            }

            Undo.RegisterFullObjectHierarchyUndo(activeBar.gameObject, "Rebuild Active Activity Bar");
            ClearChildren(activeBar);
            RemoveLayoutDrivers(activeBar);
            var image = activeBar.GetComponent<Image>() ?? activeBar.gameObject.AddComponent<Image>();
            image.color = Deep;
            var outline = activeBar.GetComponent<Outline>() ?? activeBar.gameObject.AddComponent<Outline>();
            outline.effectColor = Border;
            outline.effectDistance = new Vector2(1f, -1f);

            var controller = activeBar.GetComponent<ActiveActivityBarController>();
            var noActivity = PanelObject("[STATE] NoActivityState", activeBar, new Color(0, 0, 0, 0), 0f);
            StretchRect(noActivity.GetComponent<RectTransform>(), 0, 0, 0, 0);
            TextObject("[TEXT] NoActivityText", noActivity.transform, "No Active Activity     Choose Woodcutting or Combat to begin.", 18f, Muted, TextAlignmentOptions.Center, Stretch());

            var profession = PanelObject("[STATE] ProfessionActivityState", activeBar, new Color(0, 0, 0, 0), 0f);
            StretchRect(profession.GetComponent<RectTransform>(), 0, 0, 0, 0);
            var icon = ImageObject("[ICON] ProfessionIcon", profession.transform, new Vector2(76, 76), new Vector2(52, 0), Text);
            var iconSprite = LoadSprite(WoodcuttingIconPath);
            ApplySprite(icon.GetComponent<Image>(), iconSprite);
            var iconGlyph = TextObject("[TEXT] ProfessionIconGlyph", icon.transform, "TREE", 13f, Gold, TextAlignmentOptions.Center, Stretch());
            iconGlyph.gameObject.SetActive(iconSprite == null);
            TextObject("[TEXT] ActiveActivityLabel", profession.transform, "ACTIVE ACTIVITY", 15f, Gold, TextAlignmentOptions.Left, Anchor(0, .5f, 0, .5f, 247, 22, 230, 26));
            TextObject("[TEXT] ProfessionNameText", profession.transform, "Woodcutting", 24f, Text, TextAlignmentOptions.Left, Anchor(0, .5f, 0, .5f, 247, -2, 230, 30));
            TextObject("[TEXT] ProfessionTargetText", profession.transform, "Sproutwood Tree", 16f, Muted, TextAlignmentOptions.Left, Anchor(0, .5f, 0, .5f, 247, -28, 230, 26));
            TextObject("[TEXT] ActiveTimeText", profession.transform, "Active Time\n00:00", 16f, Text, TextAlignmentOptions.Center, Anchor(.27f, .5f, .27f, .5f, 0, 0, 170, 58));
            TextObject("[TEXT] ActivityLevelText", profession.transform, "Level 1", 20f, Text, TextAlignmentOptions.Left, Anchor(.45f, .5f, .45f, .5f, 0, 18, 240, 30));
            var xp = BarObject("[BAR] ProfessionProgressBar", profession.transform, Anchor(.45f, .5f, .45f, .5f, 140, -14, 330, 28), Blue, "[TEXT] ProfessionXPText");
            TextObject("[TEXT] ActivityETAText", profession.transform, "ETA to Level 2\nETA unavailable", 16f, Text, TextAlignmentOptions.Center, Anchor(.74f, .5f, .74f, .5f, 0, 0, 230, 58));
            ButtonObject("[BUTTON] OpenProfessionButton", profession.transform, "OPEN", Anchor(1, .5f, 1, .5f, -224, 0, 112, 52), Raised, 18f);
            ButtonObject("[BUTTON] StopProfessionButton", profession.transform, "STOP", Anchor(1, .5f, 1, .5f, -82, 0, 120, 58), Red, 20f);

            var combat = PanelObject("[STATE] CombatActivityState", activeBar, new Color(0, 0, 0, 0), 0f);
            combat.gameObject.SetActive(false);
            StretchRect(combat.GetComponent<RectTransform>(), 0, 0, 0, 0);

            if (controller != null)
            {
                var so = new SerializedObject(controller);
                SetRef(so, "noActivityState", noActivity.transform);
                SetRef(so, "professionActivityState", profession.transform);
                SetRef(so, "combatActivityState", combat.transform);
                SetRef(so, "professionNameText", FindText(profession.transform, "[TEXT] ProfessionNameText"));
                SetRef(so, "professionTargetText", FindText(profession.transform, "[TEXT] ProfessionTargetText"));
                SetRef(so, "activeTimeText", FindText(profession.transform, "[TEXT] ActiveTimeText"));
                SetRef(so, "professionLevelText", FindText(profession.transform, "[TEXT] ActivityLevelText"));
                SetRef(so, "etaText", FindText(profession.transform, "[TEXT] ActivityETAText"));
                SetRef(so, "professionProgressBar", xp);
                SetRef(so, "openProfessionButton", FindButton(profession.transform, "[BUTTON] OpenProfessionButton"));
                SetRef(so, "stopProfessionButton", FindButton(profession.transform, "[BUTTON] StopProfessionButton"));
                var progression = Object.FindAnyObjectByType<IdleGame.Progression.ProfessionProgressionSystem>(FindObjectsInactive.Include);
                SetRef(so, "progressionSystem", progression);
                so.ApplyModifiedPropertiesWithoutUndo();
            }
        }

        private static void AssignWoodcuttingReferences(
            WoodcuttingScreenController controller,
            TMP_Text level,
            TMP_Text selectedName,
            TMP_Text description,
            TMP_Text targetGlyph,
            RuntimeFillBar xpBar,
            TMP_Text nextLevel,
            RuntimeFillBar durability,
            RuntimeFillBar action,
            TMP_Text inventoryText,
            Button start,
            Button stop,
            Transform treeContainer,
            Transform rewardContainer,
            Transform thresholdContainer)
        {
            if (controller == null)
            {
                return;
            }

            var so = new SerializedObject(controller);
            SetRef(so, "levelText", level);
            SetRef(so, "selectedTreeNameText", selectedName);
            SetRef(so, "selectedTreeDescriptionText", description);
            SetRef(so, "targetPlaceholderText", targetGlyph);
            SetRef(so, "selectedTreeImage", FindImage(controller.transform, "[IMAGE] SelectedTreeImage"));
            SetRef(so, "headerXpBar", xpBar);
            SetRef(so, "headerNextLevelText", nextLevel);
            SetRef(so, "durabilityBar", durability);
            SetRef(so, "actionProgressBar", action);
            SetRef(so, "levelRequirementValueText", FindText(controller.transform, "[TEXT] LevelRequirementText"));
            SetRef(so, "cuttingIntervalValueText", FindText(controller.transform, "[TEXT] ActionIntervalText"));
            SetRef(so, "rewardRateValueText", FindText(controller.transform, "[TEXT] RewardRateText"));
            SetRef(so, "respawnValueText", FindText(controller.transform, "[TEXT] RespawnText"));
            SetRef(so, "inventorySpaceValueText", inventoryText);
            SetRef(so, "gatheringPowerValueText", FindText(controller.transform, "[TEXT] GatheringPowerValue"));
            SetRef(so, "gatheringPowerBreakdownText", FindText(controller.transform, "[TEXT] GatheringPowerBreakdown"));
            SetRef(so, "activeBonusesText", FindText(controller.transform, "[TEXT] ActiveBonusesList"));
            SetRef(so, "startStopButton", start);
            SetRef(so, "startButton", start);
            SetRef(so, "stopButton", stop);
            SetRef(so, "startState", start.transform);
            SetRef(so, "stopState", stop.transform);
            SetRef(so, "treeCardContainer", treeContainer);
            SetRef(so, "rewardPreviewContainer", rewardContainer);
            SetRef(so, "thresholdContainer", thresholdContainer);
            SetRef(so, "mainHandSlotText", FindText(controller.transform, "[TEXT] MainHandSlotValue"));
            SetRef(so, "offhandSlotText", FindText(controller.transform, "[TEXT] OffhandSlotValue"));
            SetRef(so, "ringSlotText", FindText(controller.transform, "[TEXT] RingSlotValue"));
            SetRef(so, "amuletSlotText", FindText(controller.transform, "[TEXT] AmuletSlotValue"));
            SetRef(so, "capeSlotText", FindText(controller.transform, "[TEXT] CapeSlotValue"));
            SetRef(so, "relicSlotText", FindText(controller.transform, "[TEXT] RelicSlotValue"));
            SetRef(so, "companionSlotText", FindText(controller.transform, "[TEXT] CompanionSlotValue"));
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void CreateTreeCard(Transform parent, int index)
        {
            var card = PanelObject("[CARD] TreeCard_" + index, parent, Raised, 104f);
            card.AddComponent<Button>();
            var layout = card.AddComponent<LayoutElement>();
            layout.preferredHeight = 104f;
            var icon = ImageObject("[ICON] TreeIcon", card.transform, new Vector2(72, 72), new Vector2(50, 0), Text);
            TextObject("[TEXT] TreeGlyph", icon.transform, "TREE", 11f, Gold, TextAlignmentOptions.Center, Stretch());
            var name = TextObject("[TEXT] TreeName", card.transform, "Tree", 21f, Text, TextAlignmentOptions.Left, Anchor(0, .5f, 1, .5f, 96, 16, -16, 32));
            var req = TextObject("[TEXT] RequiredLevel", card.transform, "Level 1", 15f, Muted, TextAlignmentOptions.Left, Anchor(0, .5f, 1, .5f, 96, -12, -16, 26));
            var state = TextObject("[STATE] TreeCardState", card.transform, "READY", 12f, Gold, TextAlignmentOptions.Right, Anchor(.55f, 0, 1, 0, 0, 8, -16, 22));
            var binding = card.AddComponent<TreeActivityCard>();
            var so = new SerializedObject(binding);
            SetRef(so, "treeNameText", name);
            SetRef(so, "requiredLevelText", req);
            SetRef(so, "stateText", state);
            SetRef(so, "glyphText", FindText(card.transform, "[TEXT] TreeGlyph"));
            SetRef(so, "iconImage", icon.GetComponent<Image>());
            SetRef(so, "button", card.GetComponent<Button>());
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void CreateEquipmentSlot(Transform parent, string objectName, string title, string valueName, float centerX, float centerY)
        {
            var slot = PanelObject(objectName, parent, Raised, 0f);
            SetRect(slot.GetComponent<RectTransform>(), new Vector2(centerX, centerY), new Vector2(centerX, centerY), Vector2.zero, new Vector2(132, 94));
            TextObject("[TEXT] SlotTitle", slot.transform, title, 13f, Gold, TextAlignmentOptions.Center, Anchor(0, 1, 1, 1, 4, -14, -4, 22));
            TextObject(valueName, slot.transform, "Empty\nNo Woodcutting bonus", 11f, Text, TextAlignmentOptions.Center, Anchor(0, 0, 1, 1, 6, 8, -6, -34));
        }

        private static GameObject ColumnPanel(string name, Transform parent, float flex, float minWidth)
        {
            var panel = PanelObject(name, parent, Deep, 0f, true);
            var element = panel.AddComponent<LayoutElement>();
            element.flexibleWidth = flex;
            element.minWidth = minWidth;
            return panel;
        }

        private static GameObject LayoutObject(string name, Transform parent, float preferredHeight, bool flexibleHeight, bool addRectOnly)
        {
            var go = new GameObject(name, typeof(RectTransform));
            Undo.RegisterCreatedObjectUndo(go, "Create " + name);
            go.transform.SetParent(parent, false);
            var element = go.AddComponent<LayoutElement>();
            if (preferredHeight > 0f)
            {
                element.preferredHeight = preferredHeight;
            }
            if (flexibleHeight)
            {
                element.flexibleHeight = 1f;
            }
            return go;
        }

        private static GameObject PanelObject(string name, Transform parent, Color color, float preferredHeight, bool flexibleHeight = false)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Outline));
            Undo.RegisterCreatedObjectUndo(go, "Create " + name);
            go.transform.SetParent(parent, false);
            var image = go.GetComponent<Image>();
            image.color = color;
            var outline = go.GetComponent<Outline>();
            outline.effectColor = Border;
            outline.effectDistance = new Vector2(1f, -1f);
            if (preferredHeight > 0f || flexibleHeight)
            {
                var element = go.AddComponent<LayoutElement>();
                if (preferredHeight > 0f)
                {
                    element.preferredHeight = preferredHeight;
                }
                if (flexibleHeight)
                {
                    element.flexibleHeight = 1f;
                }
            }
            return go;
        }

        private static RuntimeFillBar BarObject(string name, Transform parent, RectSpec rect, Color fillColor, string textName, float preferredHeight = 0f)
        {
            var bar = PanelObject(name, parent, new Color(0.025f, 0.02f, 0.015f, 1f), 0f);
            SetRect(bar.GetComponent<RectTransform>(), rect);
            if (preferredHeight > 0f)
            {
                var layout = bar.GetComponent<LayoutElement>() ?? bar.AddComponent<LayoutElement>();
                layout.preferredHeight = preferredHeight;
            }
            var fill = new GameObject("Fill", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            Undo.RegisterCreatedObjectUndo(fill, "Create Fill");
            fill.transform.SetParent(bar.transform, false);
            var fillRect = fill.GetComponent<RectTransform>();
            fillRect.anchorMin = new Vector2(0f, 0f);
            fillRect.anchorMax = new Vector2(0f, 1f);
            fillRect.offsetMin = Vector2.zero;
            fillRect.offsetMax = Vector2.zero;
            fill.GetComponent<Image>().color = fillColor;
            var value = TextObject(textName, bar.transform, "0 / 0", 14f, Text, TextAlignmentOptions.Center, Stretch());
            var runtime = bar.AddComponent<RuntimeFillBar>();
            runtime.ConfigureForEditor(fillRect, value);
            return runtime;
        }

        private static Button ButtonObject(string name, Transform parent, string label, RectSpec rect, Color color, float fontSize = 18f)
        {
            var go = PanelObject(name, parent, color, 0f);
            SetRect(go.GetComponent<RectTransform>(), rect);
            var button = go.AddComponent<Button>();
            var text = TextObject("[TEXT] StartButtonText", go.transform, label, fontSize, Text, TextAlignmentOptions.Center, Stretch());
            if (!label.Equals("START"))
            {
                text.name = "[TEXT] Label";
            }
            return button;
        }

        private static GameObject ImageObject(string name, Transform parent, Vector2 size, Vector2 anchoredPosition, Color color)
        {
            var go = PanelObject(name, parent, new Color(0.02f, 0.04f, 0.025f, 1f), 0f);
            SetRect(go.GetComponent<RectTransform>(), new Vector2(0, .5f), new Vector2(0, .5f), anchoredPosition, size);
            go.GetComponent<Image>().color = new Color(color.r * 0.15f, color.g * 0.20f, color.b * 0.12f, 1f);
            return go;
        }

        private static Sprite LoadSprite(string path)
        {
            var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (sprite != null)
            {
                return sprite;
            }

            foreach (var asset in AssetDatabase.LoadAllAssetsAtPath(path))
            {
                if (asset is Sprite nestedSprite)
                {
                    return nestedSprite;
                }
            }

            return null;
        }

        private static void ApplySprite(Image image, Sprite sprite)
        {
            if (image == null || sprite == null)
            {
                return;
            }

            image.sprite = sprite;
            image.preserveAspect = true;
            image.color = Color.white;
        }

        private static TMP_Text TextObject(string name, Transform parent, string text, float size, Color color, TextAlignmentOptions alignment, RectSpec rect, float preferredHeight = 0f)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
            Undo.RegisterCreatedObjectUndo(go, "Create " + name);
            go.transform.SetParent(parent, false);
            SetRect(go.GetComponent<RectTransform>(), rect);
            if (preferredHeight > 0f)
            {
                var layout = go.AddComponent<LayoutElement>();
                layout.preferredHeight = preferredHeight;
            }
            var tmp = go.GetComponent<TextMeshProUGUI>();
            tmp.text = text;
            tmp.fontSize = size;
            tmp.color = color;
            tmp.alignment = alignment;
            tmp.textWrappingMode = TextWrappingModes.NoWrap;
            tmp.overflowMode = TextOverflowModes.Ellipsis;
            tmp.raycastTarget = false;
            return tmp;
        }

        private static Transform FindSceneObject(string name)
        {
            foreach (var transform in Object.FindObjectsByType<Transform>(FindObjectsInactive.Include))
            {
                if (transform.name == name && transform.gameObject.scene.IsValid())
                {
                    return transform;
                }
            }
            return null;
        }

        private static TMP_Text FindText(Transform root, string name)
        {
            if (root == null)
            {
                return null;
            }
            foreach (var text in root.GetComponentsInChildren<TMP_Text>(true))
            {
                if (text.name == name)
                {
                    return text;
                }
            }
            return null;
        }

        private static Image FindImage(Transform root, string name)
        {
            if (root == null)
            {
                return null;
            }

            foreach (var image in root.GetComponentsInChildren<Image>(true))
            {
                if (image.name == name)
                {
                    return image;
                }
            }
            return null;
        }

        private static Button FindButton(Transform root, string name)
        {
            if (root == null)
            {
                return null;
            }
            foreach (var button in root.GetComponentsInChildren<Button>(true))
            {
                if (button.name == name)
                {
                    return button;
                }
            }
            return null;
        }

        private static void ClearChildren(Transform parent)
        {
            for (var i = parent.childCount - 1; i >= 0; i--)
            {
                Object.DestroyImmediate(parent.GetChild(i).gameObject);
            }
        }

        private static void RemoveLayoutDrivers(Transform target)
        {
            foreach (var layout in target.GetComponents<LayoutGroup>())
            {
                Object.DestroyImmediate(layout);
            }

            foreach (var fitter in target.GetComponents<ContentSizeFitter>())
            {
                Object.DestroyImmediate(fitter);
            }

            foreach (var element in target.GetComponents<LayoutElement>())
            {
                Object.DestroyImmediate(element);
            }
        }

        private static void SetRef(SerializedObject so, string propertyName, Object value)
        {
            var property = so.FindProperty(propertyName);
            if (property != null)
            {
                property.objectReferenceValue = value;
            }
        }

        private static RectSpec Stretch()
        {
            return new RectSpec(Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        }

        private static RectSpec Anchor(float ax, float ay, float bx, float by, float x, float y, float w, float h)
        {
            return new RectSpec(new Vector2(ax, ay), new Vector2(bx, by), new Vector2(x, y), new Vector2(w, h));
        }

        private static void StretchRect(RectTransform rect, float left, float right, float top, float bottom)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = new Vector2(left, bottom);
            rect.offsetMax = new Vector2(-right, -top);
        }

        private static void SetTopStretchRect(RectTransform rect, float left, float right, float top, float height)
        {
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = Vector2.one;
            rect.pivot = new Vector2(0.5f, 1f);
            rect.offsetMin = new Vector2(left, -top - height);
            rect.offsetMax = new Vector2(-right, -top);
        }

        private static void SetRect(RectTransform rect, RectSpec spec)
        {
            rect.anchorMin = spec.anchorMin;
            rect.anchorMax = spec.anchorMax;
            var stretchesX = !Mathf.Approximately(spec.anchorMin.x, spec.anchorMax.x);
            var stretchesY = !Mathf.Approximately(spec.anchorMin.y, spec.anchorMax.y);

            if (stretchesX && stretchesY)
            {
                rect.offsetMin = spec.position;
                rect.offsetMax = spec.size;
            }
            else if (stretchesX)
            {
                rect.offsetMin = new Vector2(spec.position.x, rect.offsetMin.y);
                rect.offsetMax = new Vector2(spec.size.x, rect.offsetMax.y);
                rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, spec.position.y);
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, spec.size.y);
            }
            else if (stretchesY)
            {
                rect.offsetMin = new Vector2(rect.offsetMin.x, spec.position.y);
                rect.offsetMax = new Vector2(rect.offsetMax.x, spec.size.y);
                rect.anchoredPosition = new Vector2(spec.position.x, rect.anchoredPosition.y);
                rect.sizeDelta = new Vector2(spec.size.x, rect.sizeDelta.y);
            }
            else
            {
                rect.anchoredPosition = spec.position;
                rect.sizeDelta = spec.size;
            }
        }

        private static void SetRect(RectTransform rect, Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPosition, Vector2 size)
        {
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = size;
        }

        private readonly struct RectSpec
        {
            public readonly Vector2 anchorMin;
            public readonly Vector2 anchorMax;
            public readonly Vector2 position;
            public readonly Vector2 size;

            public RectSpec(Vector2 anchorMin, Vector2 anchorMax, Vector2 position, Vector2 size)
            {
                this.anchorMin = anchorMin;
                this.anchorMax = anchorMax;
                this.position = position;
                this.size = size;
            }
        }
    }
}
