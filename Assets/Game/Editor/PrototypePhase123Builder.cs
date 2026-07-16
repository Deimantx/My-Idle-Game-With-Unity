using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IdleGame.Core.Bootstrap;
using IdleGame.UI;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace IdleGame.Editor
{
    public static class PrototypePhase123Builder
    {
        private const string InputActionsPath = "Assets/InputSystem_Actions.inputactions";
        private const string DataFolder = "Assets/Game/UI/Data";
        private const string ScreenDataFolder = "Assets/Game/UI/Data/Screens";
        private const string NavigationDataFolder = "Assets/Game/UI/Data/Navigation";

        private static readonly Color DeepBackground = HtmlColor("0D1117");
        private static readonly Color MainBackground = HtmlColor("121923");
        private static readonly Color PanelBackground = HtmlColor("1A2430");
        private static readonly Color RaisedPanel = HtmlColor("233140");
        private static readonly Color HoverPanel = HtmlColor("2A3949");
        private static readonly Color Border = HtmlColor("344455");
        private static readonly Color StrongBorder = HtmlColor("53677C");
        private static readonly Color AccentGold = HtmlColor("D0A44B");
        private static readonly Color AccentTeal = HtmlColor("3C9CB5");
        private static readonly Color TextPrimary = HtmlColor("E8D9BE");
        private static readonly Color TextSecondary = HtmlColor("AFA18A");
        private static readonly Color Danger = HtmlColor("7A241F");
        private static readonly Color Success = HtmlColor("1E6B34");

        [MenuItem("Tools/Idle Game/Build Editable UI Hierarchy")]
        public static void BuildEditableHierarchy()
        {
            ValidatePrimaryScene();
            var eventSystem = ConfirmSingleEventSystem();
            ConfigureInputSystem(eventSystem);

            EnsureFolders();
            var artifacts = CreateAssets();
            BuildScene(artifacts);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorSceneManager.SaveOpenScenes();
            Debug.Log("Editable UI hierarchy generated successfully.");
        }

        [MenuItem("Tools/Idle Game/Build Prototype Phases 1-3")]
        public static void Build()
        {
            BuildEditableHierarchy();
        }

        [MenuItem("Tools/Idle Game/Scan Missing Scripts")]
        public static void ScanMissingScripts()
        {
            var total = 0;
            var scene = SceneManager.GetActiveScene();

            foreach (var root in scene.GetRootGameObjects())
            {
                total += ScanMissingScripts(root);
            }

            if (total == 0)
            {
                Debug.Log("Missing script scan complete: no missing scripts found.");
            }
            else
            {
                Debug.LogError($"Missing script scan found {total} missing script component(s).");
            }
        }

        private static void ValidatePrimaryScene()
        {
            var scene = EditorSceneManager.GetActiveScene();
            if (!scene.IsValid() || scene.path != "Assets/Scenes/SampleScene.unity")
            {
                throw new InvalidOperationException("Expected Assets/Scenes/SampleScene.unity to be the active primary gameplay scene.");
            }
        }

        private static EventSystem ConfirmSingleEventSystem()
        {
            var eventSystems = Object.FindObjectsByType<EventSystem>(FindObjectsInactive.Include);
            if (eventSystems.Length != 1)
            {
                throw new InvalidOperationException($"Expected exactly one EventSystem. Found {eventSystems.Length}.");
            }

            var eventSystem = eventSystems[0];
            eventSystem.gameObject.name = "[EVENTSYSTEM] EventSystem";

            var inputModule = eventSystem.GetComponent<InputSystemUIInputModule>();
            if (inputModule == null)
            {
                inputModule = eventSystem.gameObject.AddComponent<InputSystemUIInputModule>();
            }

            foreach (var module in eventSystem.GetComponents<BaseInputModule>())
            {
                if (module != inputModule)
                {
                    Object.DestroyImmediate(module);
                }
            }

            EditorUtility.SetDirty(eventSystem.gameObject);
            return eventSystem;
        }

        private static void ConfigureInputSystem(EventSystem eventSystem)
        {
            var module = eventSystem.GetComponent<InputSystemUIInputModule>();
            var actionsAsset = AssetDatabase.LoadAssetAtPath<InputActionAsset>(InputActionsPath);
            if (actionsAsset == null)
            {
                throw new InvalidOperationException($"Input actions asset not found at {InputActionsPath}.");
            }

            var uiMap = actionsAsset.FindActionMap("UI", true);
            var references = AssetDatabase.LoadAllAssetsAtPath(InputActionsPath)
                .OfType<InputActionReference>()
                .Where(reference => reference != null && reference.action != null)
                .ToList();

            InputActionReference FindReference(string actionName)
            {
                var action = uiMap.FindAction(actionName, true);
                var existing = references.FirstOrDefault(reference => reference.action != null && reference.action.id == action.id);
                if (existing != null)
                {
                    return existing;
                }

                var created = InputActionReference.Create(action);
                created.name = "UI/" + actionName;
                AssetDatabase.AddObjectToAsset(created, actionsAsset);
                references.Add(created);
                return created;
            }

            module.actionsAsset = actionsAsset;
            module.move = FindReference("Navigate");
            module.submit = FindReference("Submit");
            module.cancel = FindReference("Cancel");
            module.point = FindReference("Point");
            module.leftClick = FindReference("Click");
            module.rightClick = FindReference("RightClick");
            module.middleClick = FindReference("MiddleClick");
            module.scrollWheel = FindReference("ScrollWheel");
            module.trackedDevicePosition = FindReference("TrackedDevicePosition");
            module.trackedDeviceOrientation = FindReference("TrackedDeviceOrientation");
            EditorUtility.SetDirty(module);
        }

        private static BuildArtifacts CreateAssets()
        {
            var screens = GetScreens();

            var screenDefinitions = screens
                .Select(screen => CreateAsset<ScreenDefinition>(
                    $"{ScreenDataFolder}/Screen_{screen.Title}.asset",
                    asset => asset.ConfigureForEditor(screen.Id, screen.Title, screen.Subtitle, screen.ScreenOrder)))
                .ToList();

            var screenCatalog = CreateAsset<UIScreenCatalog>(
                $"{DataFolder}/UIScreenCatalog.asset",
                asset => asset.ConfigureForEditor(screenDefinitions));

            var mainGroup = CreateAsset<NavigationGroupDefinition>(
                $"{NavigationDataFolder}/NavGroup_Main.asset",
                asset => asset.ConfigureForEditor("main", "Main", 10));
            var professionsGroup = CreateAsset<NavigationGroupDefinition>(
                $"{NavigationDataFolder}/NavGroup_Professions.asset",
                asset => asset.ConfigureForEditor("professions", "Professions", 20));
            var accountGroup = CreateAsset<NavigationGroupDefinition>(
                $"{NavigationDataFolder}/NavGroup_Account.asset",
                asset => asset.ConfigureForEditor("account", "Account", 30));

            NavigationGroupDefinition GroupFor(ScreenSpec screen)
            {
                return screen.GroupId switch
                {
                    "professions" => professionsGroup,
                    "account" => accountGroup,
                    _ => mainGroup
                };
            }

            var entries = screens
                .Select(screen =>
                {
                    var screenDefinition = screenDefinitions.First(definition => definition.ScreenId == screen.Id);
                    return CreateAsset<NavigationEntryDefinition>(
                        $"{NavigationDataFolder}/NavEntry_{screen.Title}.asset",
                        asset => asset.ConfigureForEditor(GroupFor(screen), screenDefinition, screen.NavigationOrder));
                })
                .ToList();

            var navigationCatalog = CreateAsset<NavigationCatalog>(
                $"{DataFolder}/NavigationCatalog.asset",
                asset => asset.ConfigureForEditor(new[] { mainGroup, professionsGroup, accountGroup }, entries));

            return new BuildArtifacts(screenCatalog, navigationCatalog);
        }

        private static GameObject CreateNavigationButtonObject(string name, Transform parent)
        {
            var root = CreateUIObject(name, parent);
            var rect = (RectTransform)root.transform;
            rect.sizeDelta = new Vector2(238f, 52f);

            var button = root.AddComponent<Button>();
            var layout = root.AddComponent<LayoutElement>();
            layout.minHeight = 52f;
            layout.preferredHeight = 52f;

            var background = CreateImage("[IMAGE] Background", root.transform, PanelBackground, Border);
            SetStretch((RectTransform)background.transform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            button.targetGraphic = background.GetComponent<Image>();
            button.transition = Selectable.Transition.ColorTint;
            button.colors = ButtonColors(Color.white, HoverPanel, DeepBackground, RaisedPanel, new Color(0.35f, 0.35f, 0.35f, 1f));

            var selectedMarker = CreateImage("[IMAGE] SelectedMarker", root.transform, AccentGold, AccentGold);
            SetStretch((RectTransform)selectedMarker.transform, new Vector2(0f, 0f), new Vector2(0f, 1f), new Vector2(0f, 5f), new Vector2(5f, -5f));
            selectedMarker.SetActive(false);

            var icon = CreateImage("[ICON] Icon", root.transform, AccentGold, Border);
            var iconRect = (RectTransform)icon.transform;
            iconRect.anchorMin = new Vector2(0f, 0.5f);
            iconRect.anchorMax = new Vector2(0f, 0.5f);
            iconRect.anchoredPosition = new Vector2(28f, 0f);
            iconRect.sizeDelta = new Vector2(24f, 24f);

            var labelObject = CreateText("[TEXT] Label", root.transform, "Navigation", 21f, TextPrimary, TextAlignmentOptions.Left | TextAlignmentOptions.Midline);
            SetStretch((RectTransform)labelObject.transform, Vector2.zero, Vector2.one, new Vector2(56f, 0f), new Vector2(-38f, 0f));

            var lockIcon = CreateImage("[ICON] LockIcon", root.transform, TextSecondary, Border);
            var lockRect = (RectTransform)lockIcon.transform;
            lockRect.anchorMin = new Vector2(1f, 0.5f);
            lockRect.anchorMax = new Vector2(1f, 0.5f);
            lockRect.anchoredPosition = new Vector2(-22f, 0f);
            lockRect.sizeDelta = new Vector2(18f, 18f);
            lockIcon.SetActive(false);

            var badge = CreateImage("[BADGE] NotificationBadge", root.transform, Danger, AccentGold);
            var badgeRect = (RectTransform)badge.transform;
            badgeRect.anchorMin = new Vector2(1f, 1f);
            badgeRect.anchorMax = new Vector2(1f, 1f);
            badgeRect.anchoredPosition = new Vector2(-12f, -10f);
            badgeRect.sizeDelta = new Vector2(26f, 20f);
            var badgeTextObject = CreateText("[TEXT] BadgeText", badge.transform, "0", 14f, TextPrimary, TextAlignmentOptions.Center);
            SetStretch((RectTransform)badgeTextObject.transform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            badge.SetActive(false);

            var binding = root.AddComponent<NavigationButtonBinding>();
            binding.ConfigureForEditor(string.Empty, button, background.GetComponent<Image>(), selectedMarker.GetComponent<Image>(), icon.GetComponent<Image>(), labelObject.GetComponent<TMP_Text>(), lockIcon.GetComponent<Image>(), badge, badgeTextObject.GetComponent<TMP_Text>());

            return root;
        }

        private static GameObject CreateScreenObject(ScreenSpec screen, Transform parent)
        {
            var root = CreateUIObject($"[SCREEN] {screen.Title}Screen", parent);
            SetStretch((RectTransform)root.transform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            AddPanel(root, MainBackground, Border);
            var controller = root.AddComponent<UIScreenController>();
            var screenReference = root.AddComponent<UIScreenReference>();

            var layout = root.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(16, 16, 16, 16);
            layout.spacing = 12f;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            var header = CreateScreenHeader(screen, root.transform, out var titleText, out var subtitleText);
            controller.ConfigureForEditor(screen.Id, titleText, subtitleText);
            titleText.text = screen.Title;
            subtitleText.text = screen.Subtitle;
            screenReference.ConfigureForEditor(screen.Id, controller);

            switch (screen.Id)
            {
                case ScreenIds.Woodcutting:
                    BuildWoodcuttingScreen(root.transform);
                    break;
                case ScreenIds.Combat:
                    BuildCombatScreen(root.transform);
                    break;
                case ScreenIds.Inventory:
                    BuildInventoryScreen(root.transform);
                    break;
                case ScreenIds.Equipment:
                    BuildEquipmentScreen(root.transform);
                    break;
                default:
                    BuildSettingsScreen(root.transform);
                    break;
            }

            return root;
        }

        private static GameObject CreateScreenHeader(ScreenSpec screen, Transform parent, out TMP_Text titleText, out TMP_Text subtitleText)
        {
            var header = CreatePanel($"[HEADER] {screen.Title}Header", parent, RaisedPanel, StrongBorder);
            AddLayout(header, 96f);

            var icon = CreateImage("[ICON] ScreenIcon", header.transform, AccentGold, Border);
            var iconRect = (RectTransform)icon.transform;
            iconRect.anchorMin = new Vector2(0f, 0.5f);
            iconRect.anchorMax = new Vector2(0f, 0.5f);
            iconRect.anchoredPosition = new Vector2(46f, 0f);
            iconRect.sizeDelta = new Vector2(58f, 58f);

            var titleObject = CreateText("[TEXT] ScreenTitle", header.transform, screen.Title, 36f, AccentGold, TextAlignmentOptions.Left | TextAlignmentOptions.Top);
            SetStretch((RectTransform)titleObject.transform, Vector2.zero, Vector2.one, new Vector2(92f, 42f), new Vector2(-320f, -12f));
            titleText = titleObject.GetComponent<TMP_Text>();

            var subtitleObject = CreateText("[TEXT] ScreenSubtitle", header.transform, screen.Subtitle, 18f, TextSecondary, TextAlignmentOptions.Left | TextAlignmentOptions.Bottom);
            SetStretch((RectTransform)subtitleObject.transform, Vector2.zero, Vector2.one, new Vector2(94f, 12f), new Vector2(-320f, -48f));
            subtitleText = subtitleObject.GetComponent<TMP_Text>();

            CreateAnchoredButton("[BUTTON] StatisticsButton", header.transform, "Stats", new Vector2(1f, 0.5f), new Vector2(-210f, 0f), new Vector2(116f, 42f), PanelBackground);
            CreateAnchoredButton("[BUTTON] HelpButton", header.transform, "Help", new Vector2(1f, 0.5f), new Vector2(-82f, 0f), new Vector2(96f, 42f), PanelBackground);
            return header;
        }

        private static void BuildWoodcuttingScreen(Transform parent)
        {
            ConfigureWoodcuttingHeader(parent);

            var authoringLayout = CreatePanel("[LAYOUT] WoodcuttingAuthoringLayout", parent, new Color(0f, 0f, 0f, 0.04f), Border);
            var authoringElement = authoringLayout.AddComponent<LayoutElement>();
            authoringElement.minHeight = 610f;
            authoringElement.flexibleHeight = 1f;
            var authoringGroup = authoringLayout.AddComponent<VerticalLayoutGroup>();
            authoringGroup.padding = new RectOffset(0, 0, 0, 0);
            authoringGroup.spacing = 10f;
            authoringGroup.childControlWidth = true;
            authoringGroup.childControlHeight = true;
            authoringGroup.childForceExpandWidth = true;
            authoringGroup.childForceExpandHeight = false;

            CreateWoodcuttingMainArea(authoringLayout.transform);
            CreateWoodcuttingPotentialLoot(authoringLayout.transform);
        }

        private static void ConfigureWoodcuttingHeader(Transform parent)
        {
            var header = FindChildTransform(parent, "[HEADER] WoodcuttingHeader");
            if (header == null)
            {
                return;
            }

            var title = FindChild<TMP_Text>(header, "[TEXT] ScreenTitle");
            var subtitle = FindChild<TMP_Text>(header, "[TEXT] ScreenSubtitle");
            title.text = "Woodcutting";
            subtitle.text = "Choose a tree, review its yield, and prepare your gathering loadout.";

            var titleRect = (RectTransform)title.transform;
            titleRect.anchorMin = new Vector2(0f, 0.5f);
            titleRect.anchorMax = new Vector2(0f, 0.5f);
            titleRect.pivot = new Vector2(0f, 0.5f);
            titleRect.anchoredPosition = new Vector2(94f, 13f);
            titleRect.sizeDelta = new Vector2(220f, 42f);

            var subtitleRect = (RectTransform)subtitle.transform;
            subtitleRect.anchorMin = new Vector2(0f, 0.5f);
            subtitleRect.anchorMax = new Vector2(0f, 0.5f);
            subtitleRect.pivot = new Vector2(0f, 0.5f);
            subtitleRect.anchoredPosition = new Vector2(96f, -25f);
            subtitleRect.sizeDelta = new Vector2(560f, 24f);

            CreateAnchoredText("[TEXT] ProfessionLevel", header, "Level 76", 24f, AccentTeal, TextAlignmentOptions.Left | TextAlignmentOptions.Midline,
                new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(320f, -4f), new Vector2(450f, 30f));
            CreateAnchoredText("[TEXT] MasteryLabel", header, "MASTERY", 13f, AccentGold, TextAlignmentOptions.Right | TextAlignmentOptions.Midline,
                new Vector2(1f, 0.5f), new Vector2(1f, 0.5f), new Vector2(-430f, 4f), new Vector2(-340f, 26f));
            CreateAnchoredText("[TEXT] MasteryValue", header, "32", 27f, AccentGold, TextAlignmentOptions.Right | TextAlignmentOptions.Midline,
                new Vector2(1f, 0.5f), new Vector2(1f, 0.5f), new Vector2(-430f, -28f), new Vector2(-340f, 4f));
        }

        private static void CreateWoodcuttingMainArea(Transform parent)
        {
            var main = CreateUIObject("[LAYOUT] WoodcuttingMainArea", parent);
            var element = main.AddComponent<LayoutElement>();
            element.minHeight = 470f;
            element.flexibleHeight = 1f;
            var layout = main.AddComponent<HorizontalLayoutGroup>();
            layout.padding = new RectOffset(0, 0, 0, 0);
            layout.spacing = 10f;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = false;
            layout.childForceExpandHeight = true;

            CreateWoodcuttingTreeSelection(main.transform);
            CreateWoodcuttingSelectedTreeDetails(main.transform);
            CreateWoodcuttingLoadout(main.transform);
        }

        private static void CreateWoodcuttingTreeSelection(Transform parent)
        {
            var selection = CreatePanel("[SECTION] ActivitySelectionSection", parent, PanelBackground, Border);
            var element = selection.AddComponent<LayoutElement>();
            element.preferredWidth = 330f;
            element.minWidth = 290f;
            var layout = selection.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(10, 10, 10, 10);
            layout.spacing = 8f;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            CreateSectionHeader(selection.transform, "Select Tree");
            CreateWoodcuttingTreeScroll(selection.transform);
        }

        private static void CreateWoodcuttingTreeScroll(Transform parent)
        {
            var scroll = CreatePanel("[SCROLL] TreeSelectionScroll", parent, DeepBackground, Border);
            var element = scroll.AddComponent<LayoutElement>();
            element.minHeight = 380f;
            element.flexibleHeight = 1f;
            var scrollRect = scroll.AddComponent<ScrollRect>();
            scrollRect.horizontal = false;
            scrollRect.vertical = true;
            scrollRect.movementType = ScrollRect.MovementType.Clamped;
            scrollRect.scrollSensitivity = 26f;

            var viewport = CreatePanel("Viewport", scroll.transform, new Color(0f, 0f, 0f, 0.04f), Border);
            SetStretch((RectTransform)viewport.transform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            viewport.AddComponent<Mask>().showMaskGraphic = false;
            scrollRect.viewport = (RectTransform)viewport.transform;

            var content = CreateUIObject("[DYNAMIC CONTENT] TreeCardContainer", viewport.transform);
            var contentRect = (RectTransform)content.transform;
            SetStretch(contentRect, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(6f, 0f), new Vector2(-12f, 0f));
            contentRect.pivot = new Vector2(0.5f, 1f);
            var contentLayout = content.AddComponent<VerticalLayoutGroup>();
            contentLayout.padding = new RectOffset(0, 0, 6, 6);
            contentLayout.spacing = 6f;
            contentLayout.childControlWidth = true;
            contentLayout.childControlHeight = true;
            contentLayout.childForceExpandWidth = true;
            contentLayout.childForceExpandHeight = false;
            content.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            scrollRect.content = contentRect;

            var trees = new[]
            {
                ("Normal Tree", "Level 1", "READY", false, false),
                ("Oak Tree", "Level 15", "READY", false, false),
                ("Willow Tree", "Level 30", "READY", false, false),
                ("Teak Tree", "Level 45", "READY", false, false),
                ("Maple Tree", "Level 60", "READY", false, false),
                ("Yew Tree", "Level 75", "SELECTED", true, false),
                ("Magic Tree", "Level 90", "LOCKED", false, true),
                ("Elder Tree", "Level 105", "LOCKED", false, true)
            };

            for (var i = 0; i < trees.Length; i++)
            {
                CreateWoodcuttingTreeCard(content.transform, i + 1, trees[i].Item1, trees[i].Item2, trees[i].Item3, trees[i].Item4, trees[i].Item5);
            }
        }

        private static void CreateWoodcuttingTreeCard(Transform parent, int index, string treeName, string level, string state, bool selected, bool locked)
        {
            var card = CreatePanel("[CARD] TreeCard_" + index.ToString("00"), parent, selected ? HoverPanel : RaisedPanel, selected ? AccentGold : Border);
            AddLayout(card, 58f);
            var image = card.GetComponent<Image>();
            var button = card.AddComponent<Button>();
            button.targetGraphic = image;
            button.colors = ButtonColors(Color.white, HoverPanel, DeepBackground, selected ? AccentGold : RaisedPanel, new Color(0.35f, 0.35f, 0.35f, 1f));

            var icon = CreatePanel("[ICON] TreeIcon", card.transform, locked ? DeepBackground : Success, locked ? Border : AccentGold);
            var iconRect = (RectTransform)icon.transform;
            iconRect.anchorMin = new Vector2(0f, 0.5f);
            iconRect.anchorMax = new Vector2(0f, 0.5f);
            iconRect.anchoredPosition = new Vector2(31f, 0f);
            iconRect.sizeDelta = new Vector2(44f, 44f);
            CreateAnchoredText("[TEXT] TreeGlyph", icon.transform, locked ? "X" : "TREE", locked ? 18f : 11f, locked ? TextSecondary : TextPrimary, TextAlignmentOptions.Center, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);

            CreateAnchoredText("[TEXT] TreeName", card.transform, treeName, 17f, locked ? TextSecondary : TextPrimary, TextAlignmentOptions.Left | TextAlignmentOptions.Top,
                new Vector2(0f, 1f), Vector2.one, new Vector2(64f, -30f), new Vector2(-72f, -5f));
            CreateAnchoredText("[TEXT] RequiredLevel", card.transform, level, 13f, locked ? Danger : TextSecondary, TextAlignmentOptions.Left | TextAlignmentOptions.Bottom,
                Vector2.zero, new Vector2(1f, 0f), new Vector2(64f, 5f), new Vector2(-72f, 25f));
            CreateAnchoredText("[STATE] TreeCardState", card.transform, state, 11f, selected ? AccentGold : locked ? Danger : Success, TextAlignmentOptions.Right | TextAlignmentOptions.Midline,
                new Vector2(1f, 0f), Vector2.one, new Vector2(-74f, 0f), new Vector2(-8f, 0f));
        }

        private static void CreateWoodcuttingSelectedTreeDetails(Transform parent)
        {
            var details = CreatePanel("[SECTION] SelectedTreeDetailsSection", parent, PanelBackground, Border);
            var element = details.AddComponent<LayoutElement>();
            element.minWidth = 620f;
            element.flexibleWidth = 1f;
            var layout = details.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(12, 12, 10, 10);
            layout.spacing = 8f;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            var selectedHeader = CreatePanel("[HEADER] SelectedTreeHeader", details.transform, DeepBackground, Border);
            AddLayout(selectedHeader, 42f);
            CreateAnchoredText("[TEXT] SelectedTreeName", selectedHeader.transform, "Yew Tree", 24f, AccentGold, TextAlignmentOptions.Center, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);

            CreateWoodcuttingActiveTarget(details.transform);
            CreateWoodcuttingSelectedTreeStats(details.transform);
            CreateWoodcuttingCapacity(details.transform);
            CreateWoodcuttingActionBar(details.transform);
        }

        private static void CreateWoodcuttingActiveTarget(Transform parent)
        {
            var target = CreatePanel("[SECTION] ActiveTargetSection", parent, DeepBackground, Border);
            var element = target.AddComponent<LayoutElement>();
            element.minHeight = 250f;
            element.flexibleHeight = 1f;
            CreateAnchoredText("[HEADER] ActiveTargetHeader", target.transform, "ACTIVE TARGET  •  YEW TREE", 15f, AccentGold, TextAlignmentOptions.Left | TextAlignmentOptions.Top,
                Vector2.zero, Vector2.one, new Vector2(12f, -10f), new Vector2(-12f, -8f));

            var treeImage = CreatePanel("[IMAGE] TargetImage", target.transform, MainBackground, Success);
            var imageRect = (RectTransform)treeImage.transform;
            imageRect.anchorMin = new Vector2(0f, 0f);
            imageRect.anchorMax = new Vector2(0f, 1f);
            imageRect.offsetMin = new Vector2(14f, 14f);
            imageRect.offsetMax = new Vector2(210f, -42f);
            CreateAnchoredText("[TEXT] TargetImagePlaceholder", treeImage.transform, "YEW\nTREE", 30f, Success, TextAlignmentOptions.Center, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);

            CreateAnchoredText("[TEXT] TreeHealthLabel", target.transform, "Tree Health", 18f, TextPrimary, TextAlignmentOptions.Center,
                new Vector2(0f, 1f), Vector2.one, new Vector2(230f, -70f), new Vector2(-20f, -40f));
            CreateWoodcuttingProgressBar("[BAR] TargetDurabilityBar", target.transform, new Vector2(230f, -102f), new Vector2(-20f, -76f), Success, 0.425f, "4,250 / 10,000  (42.5%)");
            CreateWoodcuttingProgressBar("[BAR] ActionProgressBar", target.transform, new Vector2(230f, -134f), new Vector2(-20f, -112f), AccentTeal, 0.68f, "Next action  1.5s");
            CreateWoodcuttingThresholds(target.transform);
        }

        private static void CreateWoodcuttingProgressBar(string name, Transform parent, Vector2 offsetMin, Vector2 offsetMax, Color fillColor, float fillAmount, string value)
        {
            var bar = CreatePanel(name, parent, MainBackground, Border);
            var rect = (RectTransform)bar.transform;
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = Vector2.one;
            rect.offsetMin = offsetMin;
            rect.offsetMax = offsetMax;
            var fill = CreateImage("Fill", bar.transform, fillColor, fillColor);
            SetStretch((RectTransform)fill.transform, Vector2.zero, new Vector2(fillAmount, 1f), Vector2.zero, Vector2.zero);
            CreateAnchoredText("[TEXT] ValueText", bar.transform, value, 13f, TextPrimary, TextAlignmentOptions.Center, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        }

        private static void CreateWoodcuttingThresholds(Transform parent)
        {
            var thresholds = CreateUIObject("[CONTAINER] ThresholdContainer", parent);
            var rect = (RectTransform)thresholds.transform;
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = Vector2.one;
            rect.offsetMin = new Vector2(230f, -220f);
            rect.offsetMax = new Vector2(-20f, -148f);
            var layout = thresholds.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = 8f;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = true;

            var labels = new[] { "75%\nCOMPLETED", "50%\nCOMPLETED", "25%\nCURRENT", "0%\nLOCKED" };
            for (var i = 0; i < labels.Length; i++)
            {
                var marker = CreatePanel("[THRESHOLD] Threshold_" + i, thresholds.transform, i < 2 ? Success : i == 2 ? RaisedPanel : DeepBackground, i == 2 ? AccentGold : Border);
                CreateAnchoredText("[TEXT] ThresholdLabel", marker.transform, labels[i], 12f, i == 2 ? AccentGold : i < 2 ? TextPrimary : TextSecondary, TextAlignmentOptions.Center,
                    Vector2.zero, Vector2.one, new Vector2(2f, 2f), new Vector2(-2f, -2f));
            }
        }

        private static void CreateWoodcuttingSelectedTreeStats(Transform parent)
        {
            var row = CreateUIObject("[CONTAINER] SelectedTreeStats", parent);
            AddLayout(row, 62f);
            var layout = row.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = 8f;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = true;

            CreateWoodcuttingStatCard(row.transform, "Cutting Interval", "2.25s");
            CreateWoodcuttingStatCard(row.transform, "Success Rate", "95%");
            CreateWoodcuttingStatCard(row.transform, "Logs / Action", "1");
            CreateWoodcuttingStatCard(row.transform, "Selected Reward", "Yew Logs");

            var rewards = CreateUIObject("[CONTAINER] SelectedTreeRewards", row.transform);
            rewards.AddComponent<LayoutElement>().flexibleWidth = 0.001f;
            rewards.SetActive(false);
            var requirements = CreateUIObject("[CONTAINER] SelectedTreeRequirements", row.transform);
            requirements.AddComponent<LayoutElement>().flexibleWidth = 0.001f;
            requirements.SetActive(false);
        }

        private static void CreateWoodcuttingStatCard(Transform parent, string label, string value)
        {
            var card = CreatePanel("[STAT] " + label.Replace(" ", string.Empty), parent, RaisedPanel, Border);
            CreateAnchoredText("[TEXT] StatLabel", card.transform, label, 12f, TextSecondary, TextAlignmentOptions.Center,
                Vector2.zero, Vector2.one, new Vector2(4f, 20f), new Vector2(-4f, -2f));
            CreateAnchoredText("[TEXT] StatValue", card.transform, value, 17f, value == "95%" ? Success : TextPrimary, TextAlignmentOptions.Center,
                Vector2.zero, Vector2.one, new Vector2(4f, 2f), new Vector2(-4f, -20f));
        }

        private static void CreateWoodcuttingCapacity(Transform parent)
        {
            var capacity = CreatePanel("[PANEL] InventoryCapacityPanel", parent, DeepBackground, Border);
            AddLayout(capacity, 42f);
            CreateAnchoredText("[TEXT] InventorySpaceLabel", capacity.transform, "Inventory Space Available", 15f, TextPrimary, TextAlignmentOptions.Left | TextAlignmentOptions.Midline,
                Vector2.zero, Vector2.one, new Vector2(14f, 0f), new Vector2(-160f, 0f));
            CreateAnchoredText("[TEXT] InventorySpaceValue", capacity.transform, "640 / 900", 18f, Success, TextAlignmentOptions.Right | TextAlignmentOptions.Midline,
                Vector2.zero, Vector2.one, new Vector2(200f, 0f), new Vector2(-14f, 0f));
        }

        private static void CreateWoodcuttingActionBar(Transform parent)
        {
            var actions = CreatePanel("[ACTIONS] WoodcuttingActionBar", parent, RaisedPanel, Border);
            AddLayout(actions, 58f);
            var layout = actions.AddComponent<HorizontalLayoutGroup>();
            layout.padding = new RectOffset(8, 8, 8, 8);
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = true;

            var toggle = CreatePanel("[BUTTON] WoodcuttingToggleButton", actions.transform, Success, AccentGold);
            toggle.AddComponent<LayoutElement>().flexibleWidth = 1f;
            var image = toggle.GetComponent<Image>();
            var button = toggle.AddComponent<Button>();
            button.targetGraphic = image;
            button.colors = ButtonColors(Color.white, HoverPanel, DeepBackground, Success, new Color(0.35f, 0.35f, 0.35f, 1f));

            var startState = CreateUIObject("[STATE] StartWoodcuttingState", toggle.transform);
            SetStretch((RectTransform)startState.transform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            CreateAnchoredText("[TEXT] StartWoodcuttingLabel", startState.transform, "START WOODCUTTING", 20f, TextPrimary, TextAlignmentOptions.Center,
                Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);

            var stopState = CreateUIObject("[STATE] StopWoodcuttingState", toggle.transform);
            SetStretch((RectTransform)stopState.transform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            CreateAnchoredText("[TEXT] StopWoodcuttingLabel", stopState.transform, "STOP WOODCUTTING", 20f, TextPrimary, TextAlignmentOptions.Center,
                Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            stopState.SetActive(false);
        }

        private static void CreateWoodcuttingLoadout(Transform parent)
        {
            var loadout = CreateUIObject("[SECTION] WoodcuttingLoadoutSection", parent);
            var element = loadout.AddComponent<LayoutElement>();
            element.preferredWidth = 440f;
            element.minWidth = 380f;
            var layout = loadout.AddComponent<VerticalLayoutGroup>();
            layout.spacing = 8f;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            CreateWoodcuttingEquipmentPanel(loadout.transform);
            CreateWoodcuttingGatheringPower(loadout.transform);
            CreateWoodcuttingActiveBonuses(loadout.transform);
        }

        private static void CreateWoodcuttingEquipmentPanel(Transform parent)
        {
            var panel = CreatePanel("[PANEL] EquipmentPanel", parent, PanelBackground, Border);
            AddLayout(panel, 224f);
            CreateAnchoredText("[HEADER] EquipmentHeader", panel.transform, "EQUIPMENT", 18f, AccentGold, TextAlignmentOptions.Center,
                new Vector2(0f, 1f), Vector2.one, new Vector2(8f, -38f), new Vector2(-8f, -7f));

            var grid = CreateUIObject("[LAYOUT] WoodcuttingEquipmentGrid", panel.transform);
            var rect = (RectTransform)grid.transform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = new Vector2(10f, 10f);
            rect.offsetMax = new Vector2(-10f, -42f);
            var layout = grid.AddComponent<GridLayoutGroup>();
            layout.padding = new RectOffset(4, 4, 4, 4);
            layout.spacing = new Vector2(8f, 8f);
            layout.cellSize = new Vector2(126f, 76f);
            layout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            layout.constraintCount = 3;

            CreateWoodcuttingLoadoutSlot(grid.transform, "ToolSlot", "TOOL", "Dragon Axe");
            CreateWoodcuttingLoadoutSlot(grid.transform, "CompanionSlot", "OFF-HAND", "Woodcutter Shield");
            CreateWoodcuttingLoadoutSlot(grid.transform, "FoodSlot", "RING", "Gold Ring");
            CreateWoodcuttingLoadoutSlot(grid.transform, "RelicSlot", "AMULET", "Forest Amulet");
            CreateWoodcuttingLoadoutSlot(grid.transform, "CapeSlot", "CAPE", "Woodcutter Cape");
            CreateWoodcuttingLoadoutSlot(grid.transform, "EmptySlot", "BONUS", "Empty");
        }

        private static void CreateWoodcuttingLoadoutSlot(Transform parent, string objectName, string label, string value)
        {
            var slot = CreatePanel("[SLOT] " + objectName, parent, DeepBackground, Border);
            CreateAnchoredText("[TEXT] SlotType", slot.transform, label, 10f, AccentGold, TextAlignmentOptions.Center,
                Vector2.zero, Vector2.one, new Vector2(3f, 28f), new Vector2(-3f, -3f));
            CreateAnchoredText("[TEXT] SlotValue", slot.transform, value, 12f, value == "Empty" ? TextSecondary : TextPrimary, TextAlignmentOptions.Center,
                Vector2.zero, Vector2.one, new Vector2(3f, 3f), new Vector2(-3f, -24f));
        }

        private static void CreateWoodcuttingGatheringPower(Transform parent)
        {
            var power = CreatePanel("[PANEL] GatheringPowerPanel", parent, PanelBackground, Border);
            AddLayout(power, 138f);
            CreateAnchoredText("[HEADER] GatheringPowerHeader", power.transform, "GATHERING POWER", 17f, AccentGold, TextAlignmentOptions.Center,
                new Vector2(0f, 1f), Vector2.one, new Vector2(8f, -34f), new Vector2(-8f, -6f));
            CreateAnchoredText("[TEXT] GatheringPowerValue", power.transform, "1,950", 29f, Success, TextAlignmentOptions.Center,
                new Vector2(0f, 1f), Vector2.one, new Vector2(8f, -76f), new Vector2(-8f, -36f));
            CreateAnchoredText("[TEXT] GatheringPowerBreakdown", power.transform, "Tool Power  1,260     Equipment  +420     Other  +270", 13f, TextPrimary, TextAlignmentOptions.Center,
                new Vector2(0f, 1f), Vector2.one, new Vector2(8f, -126f), new Vector2(-8f, -82f));
        }

        private static void CreateWoodcuttingActiveBonuses(Transform parent)
        {
            var bonuses = CreatePanel("[PANEL] ActiveBonusesPanel", parent, PanelBackground, Border);
            var element = bonuses.AddComponent<LayoutElement>();
            element.minHeight = 130f;
            element.flexibleHeight = 1f;
            CreateAnchoredText("[HEADER] ActiveBonusesHeader", bonuses.transform, "ACTIVE BONUSES", 17f, AccentGold, TextAlignmentOptions.Left | TextAlignmentOptions.Top,
                Vector2.zero, Vector2.one, new Vector2(12f, -10f), new Vector2(-12f, -8f));
            CreateAnchoredText("[TEXT] ActiveBonusesList", bonuses.transform,
                "Woodcutting Potion        +15%       23:45\nCompanion: Timberpaw     +8%       47:12\nGuild Bonus                       +5%\nEvent Bonus                      +10%       1d 6h",
                14f, TextPrimary, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(16f, -42f), new Vector2(-12f, -8f));
        }

        private static void CreateWoodcuttingPotentialLoot(Transform parent)
        {
            var loot = CreatePanel("[SECTION] PotentialLootSection", parent, PanelBackground, Border);
            AddLayout(loot, 126f);
            CreateAnchoredText("[HEADER] PotentialLootHeader", loot.transform, "POTENTIAL LOOT  (FROM CURRENT TREE)", 16f, AccentGold, TextAlignmentOptions.Center,
                new Vector2(0f, 1f), Vector2.one, new Vector2(8f, -32f), new Vector2(-8f, -4f));

            var row = CreateUIObject("[CONTAINER] SelectedTreeRewardsPreview", loot.transform);
            var rect = (RectTransform)row.transform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = new Vector2(10f, 8f);
            rect.offsetMax = new Vector2(-10f, -34f);
            var layout = row.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = 10f;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = true;

            CreateWoodcuttingLootCard(row.transform, "Yew Logs", "3,580");
            CreateWoodcuttingLootCard(row.transform, "Yew Longbow", "120");
            CreateWoodcuttingLootCard(row.transform, "Yew Shield", "85");
            CreateWoodcuttingLootCard(row.transform, "Yew Shortbow", "210");
            CreateWoodcuttingLootCard(row.transform, "Yew Arrows", "2,450");
        }

        private static void CreateWoodcuttingLootCard(Transform parent, string itemName, string owned)
        {
            var card = CreatePanel("[CARD] " + itemName.Replace(" ", string.Empty) + "Loot", parent, DeepBackground, Border);
            CreateAnchoredText("[ICON] LootIcon", card.transform, itemName.Contains("Logs") ? "LOGS" : "ITEM", 15f, AccentGold, TextAlignmentOptions.Center,
                Vector2.zero, new Vector2(0.28f, 1f), new Vector2(4f, 4f), new Vector2(-2f, -4f));
            CreateAnchoredText("[TEXT] LootName", card.transform, itemName, 14f, TextPrimary, TextAlignmentOptions.Left | TextAlignmentOptions.Top,
                new Vector2(0.28f, 0f), Vector2.one, new Vector2(6f, 12f), new Vector2(-6f, -8f));
            CreateAnchoredText("[TEXT] OwnedAmount", card.transform, "Owned: " + owned, 14f, Success, TextAlignmentOptions.Left | TextAlignmentOptions.Bottom,
                new Vector2(0.28f, 0f), Vector2.one, new Vector2(6f, 8f), new Vector2(-6f, -10f));
        }

        private static void BuildCombatScreen(Transform parent)
        {
            var authoringLayout = CreatePanel("[LAYOUT] CombatAuthoringLayout", parent, new Color(0f, 0f, 0f, 0.04f), Border);
            var authoringElement = authoringLayout.AddComponent<LayoutElement>();
            authoringElement.preferredHeight = 0f;
            authoringElement.flexibleHeight = 1f;
            var layout = authoringLayout.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(0, 0, 0, 0);
            layout.spacing = 10f;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            CreateCombatSetupArea(authoringLayout.transform);
            CreateActiveCombatArea(authoringLayout.transform);
            CreateCombatActionBarSection(authoringLayout.transform);
            CreateCombatLogSection(authoringLayout.transform);
        }

        private static void CreateCombatSetupArea(Transform parent)
        {
            var area = CreatePanel("[LAYOUT] CombatSetupArea", parent, new Color(0f, 0f, 0f, 0.08f), Border);
            AddLayout(area, 292f);
            var layout = area.AddComponent<HorizontalLayoutGroup>();
            layout.padding = new RectOffset(0, 0, 0, 0);
            layout.spacing = 12f;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = false;
            layout.childForceExpandHeight = true;

            CreateCombatSelectionSection(area.transform);
            CreateSelectedEncounterSection(area.transform);
        }

        private static void CreateCombatSelectionSection(Transform parent)
        {
            var section = CreatePanel("[SECTION] CombatSelectionSection", parent, PanelBackground, Border);
            var element = section.AddComponent<LayoutElement>();
            element.flexibleWidth = 1f;
            element.minWidth = 650f;
            var layout = section.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(14, 14, 10, 12);
            layout.spacing = 8f;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            var header = CreatePanel("[HEADER] CombatSelectionHeader", section.transform, DeepBackground, Border);
            AddLayout(header, 36f);
            CreateAnchoredText("[TEXT] CombatSelectionTitle", header.transform, "+ COMBAT SELECTION", 18f, AccentGold, TextAlignmentOptions.Left | TextAlignmentOptions.Midline, Vector2.zero, Vector2.one, new Vector2(8f, 0f), new Vector2(-150f, 0f));
            CreateAnchoredButton("[BUTTON] CollapseCombatSelectionButton", header.transform, "Collapse", new Vector2(1f, 0.5f), new Vector2(-58f, 0f), new Vector2(112f, 28f), PanelBackground);

            var expanded = CreateUIObject("[STATE] CombatSelectionExpandedState", section.transform);
            var expandedLayout = expanded.AddComponent<VerticalLayoutGroup>();
            expandedLayout.spacing = 6f;
            expandedLayout.childControlWidth = true;
            expandedLayout.childControlHeight = true;
            expandedLayout.childForceExpandWidth = true;
            expandedLayout.childForceExpandHeight = false;
            expanded.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            CreateCombatChoiceRow(expanded.transform, "[ROW] RegionSelectionRow", "REGION", new[]
            {
                ("Greenvale", "Forest", false),
                ("Grimwood", "Pine", true),
                ("Ashen Peaks", "Mount", false)
            }, 46f);

            CreateCombatChoiceRow(expanded.transform, "[ROW] ActivityTypeSelectionRow", "ACTIVITY TYPE", new[]
            {
                ("Areas", "Area", false),
                ("Elite Areas", "Elite", false),
                ("Dungeons", "Dungeon", true),
                ("Bosses", "Boss", false),
                ("Tower", "Tower", false)
            }, 44f);

            CreateCombatChoiceRow(expanded.transform, "[ROW] DungeonSelectionRow", "DUNGEONS", new[]
            {
                ("Fallen Catacombs", "Lv. 60", false),
                ("Rotting Sewers", "Lv. 62", false),
                ("Overgrown Crypt", "Lv. 64", true),
                ("Sunken Mausoleum", "Lv. 66", false),
                ("Bone Warrens", "Lv. 68", false)
            }, 44f);

            CreateCombatChoiceRow(expanded.transform, "[ROW] EncounterSelectionRow", "ENCOUNTERS", new[]
            {
                ("Skeletal Warrior", "Lv. 64", false),
                ("Crypt Stalker", "Lv. 64", false),
                ("Bandit Scout", "Lv. 68", true),
                ("Crypt Witch", "Lv. 68", false),
                ("Grave Warden", "Lv. 70", false)
            }, 46f);

            var collapsed = CreatePanel("[STATE] CombatSelectionCollapsedState", section.transform, DeepBackground, AccentGold);
            AddLayout(collapsed, 58f);
            CreateAnchoredText("[TEXT] CollapsedCombatSelectionBreadcrumb", collapsed.transform, "Grimwood > Dungeons > Overgrown Crypt > Bandit Scout", 17f, TextPrimary, TextAlignmentOptions.Left | TextAlignmentOptions.Midline, Vector2.zero, Vector2.one, new Vector2(12f, 0f), new Vector2(-150f, 0f));
            CreateAnchoredButton("[BUTTON] ExpandCombatSelectionButton", collapsed.transform, "Expand", new Vector2(1f, 0.5f), new Vector2(-58f, 0f), new Vector2(112f, 32f), PanelBackground);
            collapsed.SetActive(false);
        }

        private static void CreateSelectedEncounterSection(Transform parent)
        {
            var section = CreatePanel("[SECTION] SelectedEncounterSection", parent, PanelBackground, Border);
            var element = section.AddComponent<LayoutElement>();
            element.preferredWidth = 520f;
            element.minWidth = 450f;

            CreateAnchoredText("[HEADER] SelectedEncounterHeader", section.transform, "SELECTED ENCOUNTER", 18f, AccentGold, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(14f, -12f), new Vector2(-14f, -8f));

            var portrait = CreatePanel("[PORTRAIT] SelectedEncounterPortrait", section.transform, DeepBackground, Border);
            var portraitRect = (RectTransform)portrait.transform;
            portraitRect.anchorMin = new Vector2(0f, 1f);
            portraitRect.anchorMax = new Vector2(0f, 1f);
            portraitRect.pivot = new Vector2(0f, 1f);
            portraitRect.anchoredPosition = new Vector2(16f, -48f);
            portraitRect.sizeDelta = new Vector2(120f, 118f);
            CreateAnchoredText("[TEXT] PortraitPlaceholder", portrait.transform, "ENEMY\nPORTRAIT", 18f, TextSecondary, TextAlignmentOptions.Center, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);

            CreateAnchoredText("[TEXT] SelectedEncounterName", section.transform, "Bandit Scout", 23f, AccentGold, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(154f, -52f), new Vector2(-14f, -8f));
            CreateAnchoredText("[TEXT] SelectedEncounterLevel", section.transform, "Level 68", 16f, AccentGold, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(154f, -82f), new Vector2(-14f, -8f));
            CreateAnchoredText("[TEXT] SelectedEncounterDamageType", section.transform, "Damage Type: Slash", 15f, TextPrimary, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(154f, -116f), new Vector2(-14f, -8f));
            CreateAnchoredText("[HEADER] PossibleLootHeader", section.transform, "POSSIBLE LOOT", 15f, AccentGold, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(154f, -148f), new Vector2(-14f, -8f));
            CreateLootSlots("[LOOT] PossibleLootSlots", section.transform, new Vector2(154f, -198f), 5, 44f);

            var enter = CreateAnchoredButton("[BUTTON] EnterEncounterButton", section.transform, "Enter Encounter", new Vector2(0.5f, 0f), new Vector2(0f, 28f), new Vector2(400f, 46f), Danger);
            enter.GetComponentInChildren<TMP_Text>(true).fontSize = 22f;
        }

        private static void CreateCombatChoiceRow(Transform parent, string name, string headerText, IReadOnlyList<(string Title, string Subtitle, bool Selected)> choices, float height)
        {
            var row = CreatePanel(name, parent, new Color(0f, 0f, 0f, 0.04f), Border);
            AddLayout(row, height);
            CreateAnchoredText("[LABEL] " + headerText.Replace(" ", string.Empty) + "Label", row.transform, headerText, 13f, AccentGold, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(8f, -4f), new Vector2(-8f, -4f));

            var cardsRoot = CreateUIObject("[LAYOUT] " + headerText.Replace(" ", string.Empty) + "Cards", row.transform);
            SetStretch((RectTransform)cardsRoot.transform, Vector2.zero, Vector2.one, new Vector2(0f, 18f), Vector2.zero);
            var layout = cardsRoot.AddComponent<HorizontalLayoutGroup>();
            layout.padding = new RectOffset(0, 0, 0, 0);
            layout.spacing = 8f;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = true;

            foreach (var choice in choices)
            {
                CreateCombatChoiceCard(cardsRoot.transform, choice.Title, choice.Subtitle, choice.Selected);
            }
        }

        private static void CreateCombatChoiceCard(Transform parent, string title, string subtitle, bool selected)
        {
            var card = CreatePanel("[CARD] " + title.Replace(" ", string.Empty) + "Choice", parent, selected ? HoverPanel : DeepBackground, selected ? AccentGold : Border);
            card.AddComponent<LayoutElement>().flexibleWidth = 1f;
            CreateAnchoredText("[TEXT] ChoiceTitle", card.transform, title, 16f, selected ? AccentGold : TextPrimary, TextAlignmentOptions.Center, Vector2.zero, Vector2.one, new Vector2(4f, 12f), new Vector2(-4f, -4f));
            CreateAnchoredText("[TEXT] ChoiceSubtitle", card.transform, subtitle, 12f, TextSecondary, TextAlignmentOptions.Center, Vector2.zero, Vector2.one, new Vector2(4f, 0f), new Vector2(-4f, -30f));
        }

        private static void CreateActiveCombatArea(Transform parent)
        {
            var area = CreatePanel("[LAYOUT] ActiveCombatArea", parent, new Color(0f, 0f, 0f, 0.08f), Border);
            AddLayout(area, 268f);
            var layout = area.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = 12f;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = false;
            layout.childForceExpandHeight = true;

            CreateActiveCombatPlayerSection(area.transform);
            CreateActiveCombatEnemySection(area.transform);
            CreateCombatSideColumn(area.transform);
        }

        private static void CreateActiveCombatPlayerSection(Transform parent)
        {
            var section = CreatePanel("[SECTION] ActiveCombatPlayerSection", parent, PanelBackground, Border);
            var element = section.AddComponent<LayoutElement>();
            element.preferredWidth = 520f;
            element.minWidth = 440f;
            CreateAnchoredText("[HEADER] ActiveCombatHeader", section.transform, "ACTIVE COMBAT", 18f, AccentGold, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(14f, -12f), new Vector2(-14f, -8f));
            CreateCombatPortrait(section.transform, "[PORTRAIT] PlayerPortrait", "PLAYER", new Vector2(16f, -44f), new Vector2(116f, 118f));
            CreateAnchoredText("[TEXT] PlayerName", section.transform, "Aldaron", 19f, AccentGold, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(150f, -46f), new Vector2(-178f, -8f));
            CreateAnchoredText("[TEXT] PlayerLevel", section.transform, "Level 76", 14f, AccentGold, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(150f, -70f), new Vector2(-178f, -8f));
            CreateCombatBar(section.transform, "[BAR] PlayerHealthBar", new Vector2(150f, -98f), new Vector2(165f, 16f), Danger, "4,320 / 4,580");
            CreateCombatBar(section.transform, "[BAR] PlayerResourceBar", new Vector2(150f, -122f), new Vector2(165f, 16f), AccentTeal, "76 / 100");
            CreateAnchoredText("[TEXT] PlayerDamageType", section.transform, "Damage Type: Slash", 13f, TextPrimary, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(150f, -150f), new Vector2(-178f, -8f));
            CreateCombatBar(section.transform, "[BAR] PlayerAttackTimer", new Vector2(16f, -184f), new Vector2(250f, 12f), AccentGold, "Attack Timer 1.2s");
            CreateCombatStatusSlots(section.transform, "[SLOTS] PlayerStatusSlots", new Vector2(16f, -220f), new[] { "DEF 2", "ICE 3", "FIRE 1" });

            var companion = CreatePanel("[PANEL] CompanionPanel", section.transform, DeepBackground, Border);
            var companionRect = (RectTransform)companion.transform;
            companionRect.anchorMin = new Vector2(1f, 1f);
            companionRect.anchorMax = new Vector2(1f, 1f);
            companionRect.pivot = new Vector2(1f, 1f);
            companionRect.anchoredPosition = new Vector2(-16f, -44f);
            companionRect.sizeDelta = new Vector2(150f, 150f);
            CreateAnchoredText("[HEADER] CompanionHeader", companion.transform, "COMPANION", 14f, AccentGold, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(8f, -8f), new Vector2(-8f, -8f));
            CreateAnchoredText("[TEXT] CompanionName", companion.transform, "Lena\nRank 15", 16f, TextPrimary, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(8f, -42f), new Vector2(-8f, -8f));
            CreateCombatBar(companion.transform, "[BAR] CompanionAttackTimer", new Vector2(8f, -110f), new Vector2(128f, 12f), AccentGold, "0.8s");
        }

        private static void CreateActiveCombatEnemySection(Transform parent)
        {
            var section = CreatePanel("[SECTION] ActiveCombatEnemySection", parent, PanelBackground, Border);
            var element = section.AddComponent<LayoutElement>();
            element.flexibleWidth = 1f;
            element.minWidth = 430f;
            CreateAnchoredText("[HEADER] EnemyHeader", section.transform, "ENEMY", 18f, Danger, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(14f, -12f), new Vector2(-14f, -8f));
            CreateCombatPortrait(section.transform, "[PORTRAIT] EnemyPortrait", "BANDIT", new Vector2(16f, -44f), new Vector2(116f, 118f));
            CreateAnchoredText("[TEXT] EnemyName", section.transform, "Bandit Scout", 19f, AccentGold, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(148f, -46f), new Vector2(-14f, -8f));
            CreateAnchoredText("[TEXT] EnemyLevel", section.transform, "Level 68", 14f, AccentGold, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(148f, -70f), new Vector2(-14f, -8f));
            CreateCombatBar(section.transform, "[BAR] EnemyHealthBar", new Vector2(148f, -98f), new Vector2(190f, 16f), Danger, "2,185 / 3,200");
            CreateAnchoredText("[TEXT] EnemyDamageType", section.transform, "Damage Type: Slash", 13f, TextPrimary, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(148f, -126f), new Vector2(-14f, -8f));
            CreateAnchoredText("[TEXT] EnemyCastingLabel", section.transform, "Casting: Quick Strike", 13f, Danger, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(148f, -152f), new Vector2(-14f, -8f));
            CreateCombatBar(section.transform, "[BAR] EnemyAttackTimer", new Vector2(16f, -184f), new Vector2(330f, 12f), AccentGold, "Attack Timer 1.6s");
            CreateAnchoredText("[HEADER] EnemyAbilitiesHeader", section.transform, "ENEMY ABILITIES", 13f, AccentGold, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(16f, -206f), new Vector2(-16f, -8f));
            CreateCombatAbilitySlots(section.transform, "[SLOTS] EnemyAbilitySlots", new Vector2(16f, -226f), new[] { "Slash\n1.6s", "Smoke\n6.1s", "Cross\n4.8s", "Rage\n12.3s", "Hide\n7.6s" }, 42f);
        }

        private static void CreateCombatSideColumn(Transform parent)
        {
            var column = CreateUIObject("[LAYOUT] CombatSideColumn", parent);
            var element = column.AddComponent<LayoutElement>();
            element.preferredWidth = 360f;
            element.minWidth = 320f;
            var layout = column.AddComponent<VerticalLayoutGroup>();
            layout.spacing = 12f;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            var loot = CreatePanel("[SECTION] TemporaryLootSection", column.transform, PanelBackground, Border);
            AddLayout(loot, 124f);
            CreateAnchoredText("[HEADER] TemporaryLootHeader", loot.transform, "TEMPORARY LOOT", 15f, AccentGold, TextAlignmentOptions.Center, Vector2.zero, Vector2.one, new Vector2(8f, -10f), new Vector2(-8f, -8f));
            CreateLootSlots("[LOOT] TemporaryLootGrid", loot.transform, new Vector2(18f, -48f), 10, 34f);

            var progress = CreatePanel("[SECTION] DungeonProgressSection", column.transform, PanelBackground, Border);
            AddLayout(progress, 124f);
            CreateAnchoredText("[HEADER] DungeonProgressHeader", progress.transform, "DUNGEON PROGRESS", 15f, AccentGold, TextAlignmentOptions.Center, Vector2.zero, Vector2.one, new Vector2(8f, -10f), new Vector2(-8f, -8f));
            CreateAnchoredText("[TEXT] DungeonWaveText", progress.transform, "Wave 3 / 5\n2 Waves Left", 23f, AccentGold, TextAlignmentOptions.Center, Vector2.zero, Vector2.one, new Vector2(8f, -66f), new Vector2(-8f, -8f));
            CreateDungeonProgressDots(progress.transform);
        }

        private static void CreateCombatActionBarSection(Transform parent)
        {
            var section = CreatePanel("[SECTION] CombatActionBarSection", parent, PanelBackground, Border);
            AddLayout(section, 86f);
            var layout = section.AddComponent<HorizontalLayoutGroup>();
            layout.padding = new RectOffset(8, 8, 8, 8);
            layout.spacing = 10f;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = false;
            layout.childForceExpandHeight = true;

            var player = CreatePanel("[PANEL] PlayerAbilitiesPanel", section.transform, DeepBackground, Border);
            player.AddComponent<LayoutElement>().preferredWidth = 315f;
            CreateAnchoredText("[HEADER] PlayerAbilitiesHeader", player.transform, "PLAYER", 13f, AccentGold, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(10f, -6f), new Vector2(-10f, -8f));
            CreateCombatAbilitySlots(player.transform, "[SLOTS] PlayerAbilitySlots", new Vector2(10f, -32f), new[] { "Slash\n1.2s", "Pierce\n4.8s", "Guard\n7.6s", "Shadow\n12.3s" }, 42f);

            var consumables = CreatePanel("[PANEL] CombatConsumablesPanel", section.transform, DeepBackground, Border);
            consumables.AddComponent<LayoutElement>().flexibleWidth = 1f;
            CreateAnchoredText("[HEADER] CombatConsumablesHeader", consumables.transform, "CONSUMABLES / ELIXIRS / FOOD", 13f, AccentGold, TextAlignmentOptions.Center, Vector2.zero, Vector2.one, new Vector2(10f, -6f), new Vector2(-10f, -8f));
            CreateCombatAbilitySlots(consumables.transform, "[SLOTS] ConsumableSlots", new Vector2(12f, -32f), new[] { "Potion\n12", "Elixir A\n8", "Elixir B\n5", "Elixir C\n3", "Elixir D\n6", "Food\n21" }, 42f);
        }

        private static void CreateCombatLogSection(Transform parent)
        {
            var log = CreatePanel("[SECTION] CombatLogSection", parent, PanelBackground, Border);
            AddLayout(log, 72f);
            CreateAnchoredText("[HEADER] CombatLogHeader", log.transform, "COMBAT LOG", 14f, AccentGold, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(12f, -7f), new Vector2(-110f, -8f));
            CreateAnchoredText("[TEXT] CombatLogRows", log.transform, "[00:12] Aldaron used Slash.  [00:10] Bandit Scout used Quick Strike.  [00:08] Lena used Bite.\n[00:10] Bandit Scout took 532 damage.  [00:08] Bandit Scout took 312 damage.", 12f, TextPrimary, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(12f, -28f), new Vector2(-112f, -6f));
            CreateAnchoredButton("[BUTTON] ClearCombatLogButton", log.transform, "Clear Log", new Vector2(1f, 0f), new Vector2(-54f, 22f), new Vector2(88f, 26f), PanelBackground);
        }

        private static void CreateCombatPortrait(Transform parent, string name, string label, Vector2 position, Vector2 size)
        {
            var portrait = CreatePanel(name, parent, DeepBackground, Border);
            var rect = (RectTransform)portrait.transform;
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.pivot = new Vector2(0f, 1f);
            rect.anchoredPosition = position;
            rect.sizeDelta = size;
            CreateAnchoredText("[TEXT] PortraitLabel", portrait.transform, label, 18f, TextSecondary, TextAlignmentOptions.Center, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        }

        private static void CreateCombatBar(Transform parent, string name, Vector2 position, Vector2 size, Color fillColor, string label)
        {
            var bar = CreatePanel(name, parent, DeepBackground, Border);
            var rect = (RectTransform)bar.transform;
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.pivot = new Vector2(0f, 1f);
            rect.anchoredPosition = position;
            rect.sizeDelta = size;
            var fill = CreateImage("[FILL] BarFill", bar.transform, fillColor, fillColor);
            SetStretch((RectTransform)fill.transform, Vector2.zero, new Vector2(0.72f, 1f), Vector2.zero, Vector2.zero);
            CreateAnchoredText("[TEXT] BarLabel", bar.transform, label, 11f, TextPrimary, TextAlignmentOptions.Center, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        }

        private static void CreateCombatStatusSlots(Transform parent, string name, Vector2 position, IReadOnlyList<string> labels)
        {
            CreateCombatAbilitySlots(parent, name, position, labels, 44f);
        }

        private static void CreateCombatAbilitySlots(Transform parent, string name, Vector2 position, IReadOnlyList<string> labels, float slotSize)
        {
            var root = CreateUIObject(name, parent);
            var rect = (RectTransform)root.transform;
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.pivot = new Vector2(0f, 1f);
            rect.anchoredPosition = position;
            rect.sizeDelta = new Vector2(labels.Count * (slotSize + 8f), slotSize);
            var layout = root.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = 8f;
            layout.childControlWidth = false;
            layout.childControlHeight = true;

            foreach (var label in labels)
            {
                var slot = CreatePanel("[SLOT] " + label.Split('\n')[0].Replace(" ", string.Empty) + "Slot", root.transform, RaisedPanel, Border);
                var element = slot.AddComponent<LayoutElement>();
                element.preferredWidth = slotSize;
                element.preferredHeight = slotSize;
                CreateAnchoredText("[TEXT] SlotLabel", slot.transform, label, 12f, TextPrimary, TextAlignmentOptions.Center, Vector2.zero, Vector2.one, new Vector2(2f, 2f), new Vector2(-2f, -2f));
            }
        }

        private static void CreateLootSlots(string name, Transform parent, Vector2 position, int count, float slotSize)
        {
            var root = CreateUIObject(name, parent);
            var rect = (RectTransform)root.transform;
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.pivot = new Vector2(0f, 1f);
            rect.anchoredPosition = position;
            var columns = Mathf.Min(5, count);
            var rows = Mathf.CeilToInt(count / (float)columns);
            rect.sizeDelta = new Vector2(columns * slotSize + Mathf.Max(0, columns - 1) * 8f, rows * slotSize + Mathf.Max(0, rows - 1) * 8f);
            var grid = root.AddComponent<GridLayoutGroup>();
            grid.cellSize = new Vector2(slotSize, slotSize);
            grid.spacing = new Vector2(8f, 8f);
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = columns;

            for (var i = 0; i < count; i++)
            {
                var slot = CreatePanel("[SLOT] LootSlot" + (i + 1), root.transform, DeepBackground, Border);
                CreateAnchoredText("[TEXT] LootLabel", slot.transform, i < 5 ? "Loot" : string.Empty, 11f, TextSecondary, TextAlignmentOptions.Center, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            }
        }

        private static void CreateDungeonProgressDots(Transform parent)
        {
            var dots = CreateUIObject("[PROGRESS] DungeonWaveDots", parent);
            var rect = (RectTransform)dots.transform;
            rect.anchorMin = new Vector2(0.5f, 0f);
            rect.anchorMax = new Vector2(0.5f, 0f);
            rect.pivot = new Vector2(0.5f, 0f);
            rect.anchoredPosition = new Vector2(0f, 16f);
            rect.sizeDelta = new Vector2(260f, 34f);
            var layout = dots.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = 12f;
            layout.childControlWidth = false;
            layout.childControlHeight = true;
            layout.childAlignment = TextAnchor.MiddleCenter;

            for (var i = 1; i <= 5; i++)
            {
                var dot = CreatePanel("[STEP] Wave" + i, dots.transform, i <= 2 ? Success : DeepBackground, i == 3 ? AccentGold : Border);
                var element = dot.AddComponent<LayoutElement>();
                element.preferredWidth = 34f;
                element.preferredHeight = 34f;
                CreateAnchoredText("[TEXT] WaveNumber", dot.transform, i.ToString(), 14f, TextPrimary, TextAlignmentOptions.Center, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            }
        }

        private static void BuildInventoryScreen(Transform parent)
        {
            AddInventoryHeaderCapacity(parent);

            var authoringLayout = CreatePanel("[LAYOUT] InventoryAuthoringLayout", parent, new Color(0f, 0f, 0f, 0.04f), Border);
            var authoringElement = authoringLayout.AddComponent<LayoutElement>();
            authoringElement.preferredHeight = 0f;
            authoringElement.flexibleHeight = 1f;
            var layout = authoringLayout.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(0, 0, 0, 0);
            layout.spacing = 10f;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            CreateInventoryControls(authoringLayout.transform);
            CreateInventoryMainLayout(authoringLayout.transform);

            var empty = CreatePanel("[PANEL] InventoryEmptyState", authoringLayout.transform, DeepBackground, Border);
            AddLayout(empty, 96f);
            CreateAnchoredText("[TEXT] InventoryEmptyTitle", empty.transform, "No Items Found", 22f, AccentGold, TextAlignmentOptions.Center, Vector2.zero, Vector2.one, Vector2.zero, new Vector2(0f, -20f));
            CreateAnchoredText("[TEXT] InventoryEmptyBody", empty.transform, "Try adjusting filters or check back later.", 15f, TextSecondary, TextAlignmentOptions.Center, Vector2.zero, Vector2.one, new Vector2(0f, 18f), Vector2.zero);
            empty.SetActive(false);
        }

        private static void AddInventoryHeaderCapacity(Transform screenRoot)
        {
            var header = FindChildTransform(screenRoot, "[HEADER] InventoryHeader");
            if (header == null || FindChildTransform(header, "[DISPLAY] InventoryCapacityDisplay") != null)
            {
                return;
            }

            var capacity = CreatePanel("[DISPLAY] InventoryCapacityDisplay", header, new Color(0f, 0f, 0f, 0.04f), Border);
            var rect = (RectTransform)capacity.transform;
            rect.anchorMin = new Vector2(1f, 0.5f);
            rect.anchorMax = new Vector2(1f, 0.5f);
            rect.pivot = new Vector2(1f, 0.5f);
            rect.anchoredPosition = new Vector2(-330f, 0f);
            rect.sizeDelta = new Vector2(260f, 56f);
            CreateAnchoredText("[TEXT] InventoryCapacityLabel", capacity.transform, "Inventory Capacity", 14f, TextPrimary, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(48f, -8f), new Vector2(-10f, -8f));
            CreateAnchoredText("[TEXT] InventoryCapacityValue", capacity.transform, "640 / 900", 25f, TextPrimary, TextAlignmentOptions.Left | TextAlignmentOptions.Bottom, Vector2.zero, Vector2.one, new Vector2(48f, 6f), new Vector2(-10f, 0f));
            var icon = CreateImage("[ICON] InventoryCapacityIcon", capacity.transform, AccentGold, Border);
            var iconRect = (RectTransform)icon.transform;
            iconRect.anchorMin = new Vector2(0f, 0.5f);
            iconRect.anchorMax = new Vector2(0f, 0.5f);
            iconRect.pivot = new Vector2(0f, 0.5f);
            iconRect.anchoredPosition = new Vector2(10f, 0f);
            iconRect.sizeDelta = new Vector2(30f, 30f);
        }

        private static void CreateInventoryControls(Transform parent)
        {
            var controls = CreatePanel("[CONTROLS] InventoryControls", parent, RaisedPanel, Border);
            AddLayout(controls, 54f);
            var layout = controls.AddComponent<HorizontalLayoutGroup>();
            layout.padding = new RectOffset(10, 10, 8, 8);
            layout.spacing = 8f;
            layout.childControlWidth = false;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = false;
            layout.childForceExpandHeight = true;

            CreateInventorySearchInput(controls.transform);

            foreach (var filter in new[] { "All", "Equipment", "Consumables", "Resources", "Materials", "Quest / Special" })
            {
                var button = CreateButton("[BUTTON] " + filter.Replace(" ", string.Empty).Replace("/", string.Empty) + "Filter", controls.transform, filter, filter == "Quest / Special" ? 138f : 112f, filter == "All" ? AccentGold : PanelBackground);
                button.GetComponentInChildren<TMP_Text>(true).fontSize = 15f;
            }

            var spacer = CreateUIObject("[SPACER] InventoryControlsSpacer", controls.transform);
            spacer.AddComponent<LayoutElement>().flexibleWidth = 1f;

            var clear = CreateButton("[BUTTON] ClearInventoryFiltersButton", controls.transform, "Clear Filters", 132f, PanelBackground);
            clear.GetComponentInChildren<TMP_Text>(true).fontSize = 15f;

            CreateInventorySortDropdown(controls.transform);
        }

        private static void CreateInventorySearchInput(Transform parent)
        {
            var search = CreatePanel("[INPUT] InventorySearchInput", parent, DeepBackground, Border);
            var element = search.AddComponent<LayoutElement>();
            element.preferredWidth = 300f;
            element.preferredHeight = 38f;
            CreateAnchoredText("[TEXT] SearchPlaceholder", search.transform, "Search items...", 15f, TextSecondary, TextAlignmentOptions.Left | TextAlignmentOptions.Midline, Vector2.zero, Vector2.one, new Vector2(12f, 0f), new Vector2(-42f, 0f));
            CreateAnchoredText("[ICON] SearchIcon", search.transform, "?", 18f, AccentGold, TextAlignmentOptions.Center, new Vector2(1f, 0f), Vector2.one, new Vector2(-38f, 0f), new Vector2(-6f, 0f));
        }

        private static void CreateInventorySortDropdown(Transform parent)
        {
            var sort = CreatePanel("[DROPDOWN] InventorySortDropdown", parent, DeepBackground, Border);
            var element = sort.AddComponent<LayoutElement>();
            element.preferredWidth = 250f;
            element.preferredHeight = 38f;
            CreateAnchoredText("[TEXT] SortLabel", sort.transform, "Sort: Recently Obtained", 15f, TextPrimary, TextAlignmentOptions.Left | TextAlignmentOptions.Midline, Vector2.zero, Vector2.one, new Vector2(12f, 0f), new Vector2(-36f, 0f));
            CreateAnchoredText("[ICON] DropdownArrow", sort.transform, "v", 16f, AccentGold, TextAlignmentOptions.Center, new Vector2(1f, 0f), Vector2.one, new Vector2(-34f, 0f), new Vector2(-6f, 0f));
        }

        private static void CreateInventoryMainLayout(Transform parent)
        {
            var main = CreatePanel("[LAYOUT] InventoryMainLayout", parent, new Color(0f, 0f, 0f, 0.08f), Border);
            var element = main.AddComponent<LayoutElement>();
            element.preferredHeight = 0f;
            element.flexibleHeight = 1f;
            var layout = main.AddComponent<HorizontalLayoutGroup>();
            layout.padding = new RectOffset(0, 0, 0, 0);
            layout.spacing = 10f;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = false;
            layout.childForceExpandHeight = true;

            CreateInventoryGridPanel(main.transform);
            CreateInventoryItemDetailsPanel(main.transform);
        }

        private static void CreateInventoryGridPanel(Transform parent)
        {
            var panel = CreatePanel("[PANEL] InventoryGridPanel", parent, PanelBackground, Border);
            var element = panel.AddComponent<LayoutElement>();
            element.flexibleWidth = 1f;
            element.minWidth = 820f;
            var layout = panel.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(10, 10, 10, 10);
            layout.spacing = 8f;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            CreateInventoryGridScroll(panel.transform);
            CreateInventoryLegend(panel.transform);
        }

        private static void CreateInventoryGridScroll(Transform parent)
        {
            var scroll = CreatePanel("[SCROLL] InventoryGridScroll", parent, DeepBackground, Border);
            var scrollElement = scroll.AddComponent<LayoutElement>();
            scrollElement.minHeight = 560f;
            scrollElement.preferredHeight = 600f;
            scrollElement.flexibleHeight = 1f;
            var scrollRect = scroll.AddComponent<ScrollRect>();
            scrollRect.horizontal = false;
            scrollRect.vertical = true;
            scrollRect.movementType = ScrollRect.MovementType.Clamped;
            scrollRect.scrollSensitivity = 28f;

            var viewport = CreatePanel("Viewport", scroll.transform, new Color(0f, 0f, 0f, 0.05f), Border);
            SetStretch((RectTransform)viewport.transform, Vector2.zero, Vector2.one, new Vector2(0f, 0f), new Vector2(-16f, 0f));
            viewport.AddComponent<Mask>().showMaskGraphic = false;
            scrollRect.viewport = (RectTransform)viewport.transform;

            var contentObject = CreateUIObject("[DYNAMIC CONTENT] InventoryItemContainer", viewport.transform);
            var content = (RectTransform)contentObject.transform;
            SetStretch(content, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(8f, 0f), new Vector2(-8f, -8f));
            content.pivot = new Vector2(0.5f, 1f);
            var grid = contentObject.AddComponent<GridLayoutGroup>();
            grid.cellSize = new Vector2(86f, 86f);
            grid.spacing = new Vector2(8f, 8f);
            grid.padding = new RectOffset(0, 0, 8, 8);
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = 8;
            contentObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            scrollRect.content = content;

            CreateInventorySlotPlaceholders(contentObject.transform);
            CreateInventoryVisualScrollbar(scroll.transform);
        }

        private static void CreateInventoryVisualScrollbar(Transform parent)
        {
            var bar = CreatePanel("[SCROLLBAR] InventoryGridScrollbar", parent, new Color(0f, 0f, 0f, 0.35f), Border);
            var rect = (RectTransform)bar.transform;
            rect.anchorMin = new Vector2(1f, 0f);
            rect.anchorMax = new Vector2(1f, 1f);
            rect.pivot = new Vector2(1f, 0.5f);
            rect.offsetMin = new Vector2(-12f, 10f);
            rect.offsetMax = new Vector2(-4f, -10f);

            var handle = CreatePanel("[HANDLE] InventoryGridScrollbarHandle", bar.transform, AccentGold, AccentGold);
            var handleRect = (RectTransform)handle.transform;
            handleRect.anchorMin = new Vector2(0f, 0.68f);
            handleRect.anchorMax = new Vector2(1f, 0.98f);
            handleRect.offsetMin = Vector2.zero;
            handleRect.offsetMax = Vector2.zero;
        }

        private static void CreateInventorySlotPlaceholders(Transform parent)
        {
            var entries = new (string Glyph, string Quantity, string Marker, bool Selected)[]
            {
                ("AXE", "1", "", true),
                ("BOOT", "1", "F", false),
                ("LOG", "3,580", "", false),
                ("ORE", "1,260", "", false),
                ("FISH", "480", "", false),
                ("HERB", "215", "", false),
                ("POT", "38", "N", false),
                ("MEAT", "96", "", false),
                ("SHD", "1", "E", false),
                ("RING", "1", "", false),
                ("AMLT", "1", "", false),
                ("CAPE", "1", "L", false),
                ("PICK", "1", "", false),
                ("COAL", "640", "", false),
                ("MUSH", "120", "", false),
                ("BONE", "210", "", false),
                ("ARR", "2,450", "", false),
                ("RUNE", "350", "", false),
                ("RUNE", "275", "N", false),
                ("RUNE", "180", "", false),
                ("GEM", "75", "", false),
                ("FEA", "340", "", false),
                ("ELIX", "62", "", false),
                ("BREAD", "80", "", false),
                ("BAR", "140", "", false),
                ("HIDE", "310", "", false),
                ("ROPE", "90", "", false),
                ("NAIL", "220", "", false),
                ("LEA", "175", "", false),
                ("POT", "210", "", false),
                ("KEY", "12", "", false),
                ("MAP", "45", "", false),
                ("FIRE", "180", "", false),
                ("BUCK", "9", "", false),
                ("ING", "290", "", false),
                ("TOOL", "40", "", false),
                ("COIN", "1,050", "", false),
                ("BAG", "15", "", false),
                ("FERN", "310", "", false),
                ("FLWR", "120", "", false),
                ("CAR", "65", "", false),
                ("WOOL", "55", "", false),
                ("APPLE", "75", "", false),
                ("LOAF", "90", "", false),
                ("FOOD", "30", "", false),
                ("POT", "28", "", false),
                ("", "", "", false),
                ("", "", "", false)
            };

            for (var i = 0; i < entries.Length; i++)
            {
                CreateInventorySlot(parent, i + 1, entries[i].Glyph, entries[i].Quantity, entries[i].Marker, entries[i].Selected);
            }
        }

        private static void CreateInventorySlot(Transform parent, int index, string glyph, string quantity, string marker, bool selected)
        {
            var slot = CreatePanel("[SLOT] InventorySlot" + index.ToString("00"), parent, selected ? HoverPanel : DeepBackground, selected ? AccentGold : Border);
            var element = slot.AddComponent<LayoutElement>();
            element.preferredWidth = 86f;
            element.preferredHeight = 86f;
            CreateAnchoredText("[TEXT] ItemGlyph", slot.transform, glyph, glyph.Length > 4 ? 16f : 18f, selected ? AccentGold : TextPrimary, TextAlignmentOptions.Center, Vector2.zero, Vector2.one, new Vector2(4f, 16f), new Vector2(-4f, -10f));
            CreateAnchoredText("[TEXT] Quantity", slot.transform, quantity, 16f, TextPrimary, TextAlignmentOptions.Right | TextAlignmentOptions.Bottom, Vector2.zero, Vector2.one, new Vector2(4f, 0f), new Vector2(-8f, 4f));
            if (!string.IsNullOrEmpty(marker))
            {
                var markerColor = marker == "L" ? Danger : marker == "E" ? Success : AccentGold;
                CreateAnchoredText("[BADGE] ItemStateBadge", slot.transform, marker, 15f, markerColor, TextAlignmentOptions.Center, new Vector2(1f, 1f), Vector2.one, new Vector2(-28f, -24f), new Vector2(-4f, -2f));
            }
        }

        private static void CreateInventoryLegend(Transform parent)
        {
            var legend = CreatePanel("[LEGEND] InventoryStateLegend", parent, DeepBackground, Border);
            AddLayout(legend, 38f);
            var layout = legend.AddComponent<HorizontalLayoutGroup>();
            layout.padding = new RectOffset(12, 12, 6, 6);
            layout.spacing = 24f;
            layout.childControlWidth = false;
            layout.childControlHeight = true;
            layout.childAlignment = TextAnchor.MiddleLeft;

            CreateInventoryLegendItem(legend.transform, "F", "Favorite", AccentGold);
            CreateInventoryLegendItem(legend.transform, "E", "Equipped", Success);
            CreateInventoryLegendItem(legend.transform, "L", "Locked", Danger);
            CreateInventoryLegendItem(legend.transform, "R", "Reserved", TextPrimary);
            CreateInventoryLegendItem(legend.transform, "N", "New", AccentGold);
        }

        private static void CreateInventoryLegendItem(Transform parent, string badge, string label, Color color)
        {
            var item = CreateUIObject("[ITEM] " + label + "Legend", parent);
            var element = item.AddComponent<LayoutElement>();
            element.preferredWidth = 118f;
            var layout = item.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = 6f;
            layout.childControlWidth = false;
            layout.childControlHeight = true;
            layout.childAlignment = TextAnchor.MiddleLeft;
            var badgeObject = CreateText("[BADGE] " + label + "Badge", item.transform, badge, 15f, color, TextAlignmentOptions.Center);
            badgeObject.AddComponent<LayoutElement>().preferredWidth = 22f;
            CreateText("[TEXT] " + label + "Label", item.transform, label, 14f, TextPrimary, TextAlignmentOptions.Left | TextAlignmentOptions.Midline);
        }

        private static void CreateInventoryItemDetailsPanel(Transform parent)
        {
            var panel = CreatePanel("[PANEL] ItemDetailsPanel", parent, PanelBackground, Border);
            var element = panel.AddComponent<LayoutElement>();
            element.preferredWidth = 610f;
            element.minWidth = 540f;
            var layout = panel.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(10, 10, 10, 10);
            layout.spacing = 8f;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            CreateInventorySelectedItemSummary(panel.transform);
            CreateInventoryItemInfoLayout(panel.transform);
            CreateInventoryItemActions(panel.transform);
        }

        private static void CreateInventorySelectedItemSummary(Transform parent)
        {
            var summary = CreatePanel("[SECTION] SelectedInventoryItemSection", parent, DeepBackground, Border);
            AddLayout(summary, 220f);
            CreateAnchoredText("[TEXT] ItemName", summary.transform, "Iron Axe", 27f, AccentGold, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(16f, -14f), new Vector2(-180f, -8f));
            CreateAnchoredText("[TEXT] ItemCategory", summary.transform, "One-Handed Axe", 17f, AccentGold, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(16f, -50f), new Vector2(-180f, -8f));
            CreateAnchoredText("[TEXT] ItemQuantity", summary.transform, "Quantity Owned\n1\n\nLifetime Obtained\n1", 15f, TextPrimary, TextAlignmentOptions.Right | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(360f, -16f), new Vector2(-16f, -8f));

            var icon = CreatePanel("[IMAGE] ItemIcon", summary.transform, RaisedPanel, AccentGold);
            var iconRect = (RectTransform)icon.transform;
            iconRect.anchorMin = new Vector2(0f, 1f);
            iconRect.anchorMax = new Vector2(0f, 1f);
            iconRect.pivot = new Vector2(0f, 1f);
            iconRect.anchoredPosition = new Vector2(18f, -86f);
            iconRect.sizeDelta = new Vector2(130f, 116f);
            CreateAnchoredText("[TEXT] ItemGlyph", icon.transform, "AXE", 34f, AccentGold, TextAlignmentOptions.Center, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);

            CreateAnchoredText("[TEXT] ItemDescription", summary.transform, "A sturdy iron axe with a balanced blade,\neffective for felling trees.", 15f, TextPrimary, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(166f, -96f), new Vector2(-16f, -8f));
            CreateAnchoredText("[TEXT] RequiredLevelLabel", summary.transform, "Required Level", 14f, TextPrimary, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(166f, -164f), new Vector2(-170f, -8f));
            CreateAnchoredText("[TEXT] RequiredLevelValue", summary.transform, "1", 15f, TextPrimary, TextAlignmentOptions.Right | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(350f, -164f), new Vector2(-16f, -8f));
            CreateAnchoredText("[TEXT] SellValueLabel", summary.transform, "Sell Value", 14f, TextPrimary, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(166f, -190f), new Vector2(-170f, -8f));
            CreateAnchoredText("[TEXT] SellValue", summary.transform, "120", 21f, AccentGold, TextAlignmentOptions.Right | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(350f, -190f), new Vector2(-16f, -8f));
        }

        private static void CreateInventoryItemInfoLayout(Transform parent)
        {
            var row = CreateUIObject("[LAYOUT] ItemDetailsInfoLayout", parent);
            AddLayout(row, 316f);
            var layout = row.AddComponent<HorizontalLayoutGroup>();
            layout.padding = new RectOffset(0, 0, 0, 0);
            layout.spacing = 12f;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = false;
            layout.childForceExpandHeight = true;

            var leftColumn = CreateUIObject("[LAYOUT] ItemDetailsLeftColumn", row.transform);
            leftColumn.AddComponent<LayoutElement>().flexibleWidth = 1f;
            var leftLayout = leftColumn.AddComponent<VerticalLayoutGroup>();
            leftLayout.spacing = 14f;
            leftLayout.childControlWidth = true;
            leftLayout.childControlHeight = true;
            leftLayout.childForceExpandWidth = true;
            leftLayout.childForceExpandHeight = false;

            CreateInventoryItemStats(leftColumn.transform);
            CreateInventoryItemSources(leftColumn.transform);
            CreateInventoryUsedInPanel(row.transform);
        }

        private static void CreateInventoryItemStats(Transform parent)
        {
            var stats = CreatePanel("[CONTAINER] ItemStatsContainer", parent, DeepBackground, Border);
            AddLayout(stats, 146f);
            CreateAnchoredText("[HEADER] ToolStatsHeader", stats.transform, "TOOL STATS", 17f, AccentGold, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(12f, -10f), new Vector2(-12f, -8f));
            CreateStatLine(stats.transform, "Gathering Power", "1,260", 0);
            CreateStatLine(stats.transform, "Durability", "210 / 210", 1, Success);
            CreateStatLine(stats.transform, "Speed", "+6%", 2, Success);
            CreateStatLine(stats.transform, "Bonus vs Trees", "+10%", 3, Success);
            CreateInventoryDurabilityBar(stats.transform);
        }

        private static void CreateInventoryDurabilityBar(Transform parent)
        {
            var bar = CreatePanel("[BAR] DurabilityBar", parent, DeepBackground, Border);
            var rect = (RectTransform)bar.transform;
            rect.anchorMin = new Vector2(0.48f, 1f);
            rect.anchorMax = new Vector2(1f, 1f);
            rect.pivot = new Vector2(1f, 1f);
            rect.anchoredPosition = new Vector2(-12f, -70f);
            rect.sizeDelta = new Vector2(0f, 18f);
            var fill = CreateImage("[FILL] DurabilityFill", bar.transform, Success, Success);
            SetStretch((RectTransform)fill.transform, Vector2.zero, new Vector2(1f, 1f), Vector2.zero, Vector2.zero);
        }

        private static void CreateInventoryItemSources(Transform parent)
        {
            var sources = CreatePanel("[CONTAINER] ItemSourcesContainer", parent, DeepBackground, Border);
            AddLayout(sources, 156f);
            CreateAnchoredText("[HEADER] SourcesHeader", sources.transform, "SOURCES", 17f, AccentGold, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(12f, -10f), new Vector2(-12f, -8f));
            CreateAnchoredText("[TEXT] SourceList", sources.transform, "Smithing (Level 1)\nShop (General Goods)\nWoodcutting Progression Rewards", 15f, TextPrimary, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(18f, -42f), new Vector2(-12f, -8f));
        }

        private static void CreateInventoryUsedInPanel(Transform parent)
        {
            var usedIn = CreatePanel("[CONTAINER] UsedInUpgradeContainer", parent, DeepBackground, Border);
            var element = usedIn.AddComponent<LayoutElement>();
            element.preferredWidth = 260f;
            element.minWidth = 230f;
            CreateAnchoredText("[HEADER] UsedInUpgradeHeader", usedIn.transform, "USED IN / UPGRADE", 17f, AccentGold, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(12f, -10f), new Vector2(-12f, -8f));
            var icon = CreatePanel("[ICON] UpgradeItemIcon", usedIn.transform, RaisedPanel, Border);
            var rect = (RectTransform)icon.transform;
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.pivot = new Vector2(0f, 1f);
            rect.anchoredPosition = new Vector2(16f, -56f);
            rect.sizeDelta = new Vector2(60f, 60f);
            CreateAnchoredText("[TEXT] UpgradeGlyph", icon.transform, "AXE", 18f, AccentGold, TextAlignmentOptions.Center, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            CreateAnchoredText("[TEXT] UsedInUpgradeBody", usedIn.transform, "Steel Axe\nLevel 20 Smithing", 16f, TextPrimary, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(90f, -58f), new Vector2(-12f, -8f));
        }

        private static void CreateInventoryItemActions(Transform parent)
        {
            var actions = CreatePanel("[ACTIONS] InventoryItemActions", parent, RaisedPanel, Border);
            AddLayout(actions, 96f);
            var grid = actions.AddComponent<GridLayoutGroup>();
            grid.cellSize = new Vector2(174f, 34f);
            grid.spacing = new Vector2(10f, 8f);
            grid.padding = new RectOffset(10, 10, 10, 10);
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = 3;

            CreateButton("[BUTTON] EquipItemButton", actions.transform, "Equip", 0f, Success);
            CreateButton("[BUTTON] FavoriteItemButton", actions.transform, "Favorite", 0f, PanelBackground);
            CreateButton("[BUTTON] LockItemButton", actions.transform, "Lock", 0f, PanelBackground);
            CreateButton("[BUTTON] ViewSourcesButton", actions.transform, "View Sources", 0f, RaisedPanel);
            CreateButton("[BUTTON] SellItemButton", actions.transform, "Sell", 0f, AccentGold);
            CreateButton("[BUTTON] DestroyItemButton", actions.transform, "Destroy", 0f, Danger);
        }

        private static void BuildEquipmentScreen(Transform parent)
        {
            var scrollContent = CreateMainScroll("[SCROLL] EquipmentMainScroll", parent);
            var upperBody = CreatePanel("[LAYOUT] EquipmentUpperBody", scrollContent.transform, new Color(0f, 0f, 0f, 0.08f), Border);
            AddLayout(upperBody, 560f);
            var upperLayout = upperBody.AddComponent<HorizontalLayoutGroup>();
            upperLayout.padding = new RectOffset(0, 0, 0, 0);
            upperLayout.spacing = 12f;
            upperLayout.childControlWidth = true;
            upperLayout.childControlHeight = true;
            upperLayout.childForceExpandWidth = false;
            upperLayout.childForceExpandHeight = true;

            CreateEquipmentPaperDollSection(upperBody.transform);
            CreateSelectedEquipmentItemSection(upperBody.transform);
            CreateEquipmentRightColumn(upperBody.transform);
            CreateEquipmentBottomSection(scrollContent.transform);
        }

        private static void CreateEquipmentPaperDollSection(Transform parent)
        {
            var section = CreatePanel("[SECTION] EquipmentPaperDollSection", parent, PanelBackground, Border);
            var element = section.AddComponent<LayoutElement>();
            element.preferredWidth = 480f;
            element.minWidth = 420f;

            CreateAnchoredText("[HEADER] EquipmentPanelTitle", section.transform, "EQUIPMENT", 20f, AccentGold, TextAlignmentOptions.Left | TextAlignmentOptions.Midline, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(14f, -40f), new Vector2(-14f, -10f));
            CreateCharacterSilhouette(section.transform);

            CreateEquipmentSlot(section.transform, "Helmet", "Helmet", "Helm", new Vector2(0f, 170f), false);
            CreateEquipmentSlot(section.transform, "Chest", "Chest", "Armor", new Vector2(0f, 82f), false);
            CreateEquipmentSlot(section.transform, "Legs", "Legs", "Greaves", new Vector2(0f, -6f), false);
            CreateEquipmentSlot(section.transform, "Boots", "Boots", "Boots", new Vector2(0f, -94f), false);

            CreateEquipmentSlot(section.transform, "Weapon", "Weapon", "Axe", new Vector2(-166f, 82f), true);
            CreateEquipmentSlot(section.transform, "Shield", "Shield", "Shield", new Vector2(166f, 82f), false);
            CreateEquipmentSlot(section.transform, "Gloves", "Gloves", "Gloves", new Vector2(-166f, -6f), false);
            CreateEquipmentSlot(section.transform, "Cape", "Cape", "Cape", new Vector2(166f, -6f), false);
            CreateEquipmentSlot(section.transform, "Ring", "Ring", "Ring", new Vector2(-166f, -94f), false);
            CreateEquipmentSlot(section.transform, "Amulet", "Amulet", "Amulet", new Vector2(166f, -94f), false);
            CreateEquipmentSlot(section.transform, "Relic", "Relic", "Relic", new Vector2(-166f, -182f), false);
            CreateEquipmentSlot(section.transform, "Tool", "Tool", "Tool", new Vector2(166f, -182f), true);

            var power = CreatePanel("[PANEL] EquipmentPowerPanel", section.transform, DeepBackground, Border);
            var powerRect = (RectTransform)power.transform;
            powerRect.anchorMin = new Vector2(0.5f, 0f);
            powerRect.anchorMax = new Vector2(0.5f, 0f);
            powerRect.pivot = new Vector2(0.5f, 0f);
            powerRect.anchoredPosition = new Vector2(0f, 16f);
            powerRect.sizeDelta = new Vector2(240f, 56f);
            CreateAnchoredText("[TEXT] EquipmentPowerLabel", power.transform, "Equipment Power", 16f, AccentGold, TextAlignmentOptions.Center, Vector2.zero, Vector2.one, new Vector2(0f, -24f), Vector2.zero);
            CreateAnchoredText("[TEXT] EquipmentPowerValue", power.transform, "PWR 1,260", 25f, TextPrimary, TextAlignmentOptions.Center, Vector2.zero, Vector2.one, Vector2.zero, new Vector2(0f, -18f));
        }

        private static void CreateSelectedEquipmentItemSection(Transform parent)
        {
            var section = CreatePanel("[SECTION] SelectedItemSection", parent, PanelBackground, Border);
            var element = section.AddComponent<LayoutElement>();
            element.preferredWidth = 455f;
            element.minWidth = 390f;
            var layout = section.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(14, 14, 12, 12);
            layout.spacing = 10f;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            CreateSectionHeader(section.transform, "Selected Item");

            var hero = CreatePanel("[PANEL] SelectedItemHero", section.transform, DeepBackground, Border);
            AddLayout(hero, 150f);
            var heroLayout = hero.AddComponent<HorizontalLayoutGroup>();
            heroLayout.padding = new RectOffset(10, 10, 10, 10);
            heroLayout.spacing = 12f;
            heroLayout.childControlWidth = false;
            heroLayout.childControlHeight = true;
            var icon = CreatePanel("[ICON] SelectedItemIcon", hero.transform, RaisedPanel, AccentGold);
            var iconElement = icon.AddComponent<LayoutElement>();
            iconElement.preferredWidth = 130f;
            CreateAnchoredText("[TEXT] SelectedItemGlyph", icon.transform, "AXE", 34f, AccentGold, TextAlignmentOptions.Center, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            var description = CreatePanel("[PANEL] SelectedItemDescription", hero.transform, new Color(0f, 0f, 0f, 0.04f), Border);
            description.AddComponent<LayoutElement>().flexibleWidth = 1f;
            CreateAnchoredText("[TEXT] SelectedItemName", description.transform, "Ironwood Ranger Axe", 23f, AccentGold, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(10f, 94f), new Vector2(-10f, -8f));
            CreateAnchoredText("[TEXT] SelectedItemType", description.transform, "Two-Handed Axe (Tool)", 16f, AccentGold, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(10f, 68f), new Vector2(-10f, -36f));
            CreateAnchoredText("[TEXT] SelectedItemFlavor", description.transform, "A finely balanced axe made from ironwood, perfect for felling trees with efficiency.", 15f, TextPrimary, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(10f, 18f), new Vector2(-10f, -64f));

            var stats = CreatePanel("[PANEL] ToolStatsPanel", section.transform, DeepBackground, Border);
            AddLayout(stats, 104f);
            CreateAnchoredText("[HEADER] ToolStatsHeader", stats.transform, "TOOL STATS", 17f, AccentGold, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(10f, -12f), new Vector2(-10f, -8f));
            CreateStatLine(stats.transform, "Gathering Power", "1,260", 0);
            CreateStatLine(stats.transform, "Durability", "210 / 210", 1);
            CreateStatLine(stats.transform, "Speed", "+12%", 2, Success);

            var usage = CreatePanel("[LAYOUT] SourcesAndUpgradeRow", section.transform, DeepBackground, Border);
            AddLayout(usage, 92f);
            var usageLayout = usage.AddComponent<HorizontalLayoutGroup>();
            usageLayout.padding = new RectOffset(10, 10, 8, 8);
            usageLayout.spacing = 10f;
            usageLayout.childControlHeight = true;
            usageLayout.childControlWidth = true;
            usageLayout.childForceExpandWidth = true;
            CreateSmallInfoPanel("[PANEL] SourcesPanel", usage.transform, "SOURCES", "Woodcutting Lv. 40\nShop: Forest Goods\nProgression Rewards");
            CreateSmallInfoPanel("[PANEL] UsedInUpgradePanel", usage.transform, "USED IN / UPGRADE", "Steelwood Axe\nLevel 60 Woodcutting");

            var actions = CreatePanel("[ACTIONS] EquipmentItemActions", section.transform, RaisedPanel, Border);
            AddLayout(actions, 82f);
            var actionsLayout = actions.AddComponent<GridLayoutGroup>();
            actionsLayout.cellSize = new Vector2(132f, 34f);
            actionsLayout.spacing = new Vector2(8f, 8f);
            actionsLayout.padding = new RectOffset(8, 8, 8, 8);
            actionsLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            actionsLayout.constraintCount = 3;
            CreateButton("[BUTTON] UnequipSelectedButton", actions.transform, "Unequip", 0f, Success);
            CreateButton("[BUTTON] FavoriteEquipmentButton", actions.transform, "Favorite", 0f, PanelBackground);
            CreateButton("[BUTTON] LockEquipmentButton", actions.transform, "Lock", 0f, PanelBackground);
            CreateButton("[BUTTON] ViewSourcesButton", actions.transform, "View Sources", 0f, RaisedPanel);
            CreateButton("[BUTTON] SellEquipmentButton", actions.transform, "Sell", 0f, AccentGold);
        }

        private static void CreateEquipmentRightColumn(Transform parent)
        {
            var column = CreateUIObject("[LAYOUT] EquipmentRightColumn", parent);
            var element = column.AddComponent<LayoutElement>();
            element.flexibleWidth = 1f;
            element.minWidth = 520f;
            var layout = column.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(0, 0, 0, 0);
            layout.spacing = 10f;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            var filters = CreatePanel("[CONTROLS] EquipmentFilterBar", column.transform, RaisedPanel, Border);
            AddLayout(filters, 44f);
            var filterContent = CreateHorizontalScrollContent(filters, "[CONTENT] EquipmentFilterContent", new RectOffset(8, 8, 7, 7), 8f);
            foreach (var filter in new[] { "All", "Weapon", "Armor", "Jewelry", "Tool", "Combat", "Profession" })
            {
                CreateButton("[BUTTON] Filter" + filter + "Button", filterContent, filter, 82f, PanelBackground);
            }

            var search = CreatePanel("[CONTROLS] EquipmentSearchSortRow", column.transform, DeepBackground, Border);
            AddLayout(search, 44f);
            CreateAnchoredText("[TEXT] SearchPlaceholder", search.transform, "Search equipment...", 16f, TextSecondary, TextAlignmentOptions.Left | TextAlignmentOptions.Midline, Vector2.zero, new Vector2(0.58f, 1f), new Vector2(12f, 0f), new Vector2(-8f, 0f));
            CreateAnchoredText("[TEXT] SortLabel", search.transform, "Sort: Power v", 16f, AccentGold, TextAlignmentOptions.Center, new Vector2(0.62f, 0f), Vector2.one, new Vector2(0f, 0f), new Vector2(-12f, 0f));

            var inventory = CreatePanel("[DYNAMIC CONTENT] EquipmentInventoryList", column.transform, DeepBackground, Border);
            AddLayout(inventory, 160f);
            var inventoryContent = CreateVerticalScrollContent(inventory, "[CONTENT] EquipmentInventoryContent", new RectOffset(8, 8, 8, 8), 5f);
            CreateEquipmentListRow(inventoryContent, "Ironwood Ranger Axe", "1,260", "Lv. 40", true);
            CreateEquipmentListRow(inventoryContent, "Steel Lumber Axe", "1,050", "Lv. 30", false);
            CreateEquipmentListRow(inventoryContent, "Oak Splitter Axe", "820", "Lv. 20", false);
            CreateEquipmentListRow(inventoryContent, "Bronze Hatchet", "610", "Lv. 10", false);

            var comparison = CreatePanel("[SECTION] EquipmentComparisonSection", column.transform, PanelBackground, Border);
            AddLayout(comparison, 158f);
            CreateAnchoredText("[HEADER] ComparisonHeader", comparison.transform, "EQUIPMENT COMPARISON", 17f, AccentGold, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(12f, -12f), new Vector2(-12f, -8f));
            CreateComparisonItemPanel(comparison.transform, "[PANEL] CurrentComparisonItemPanel", "CURRENT ITEM", "Ironwood Ranger Axe", "PWR 1,260", true);
            CreateComparisonItemPanel(comparison.transform, "[PANEL] SelectedComparisonItemPanel", "SELECTED ITEM", "Steel Lumber Axe", "PWR 1,050", false);
            CreateComparisonStatRow(comparison.transform, "Gathering Power", "1,260", "1,050", "-210", 0, false);
            CreateComparisonStatRow(comparison.transform, "Durability", "210 / 210", "180 / 180", "-30", 1, false);
            CreateComparisonStatRow(comparison.transform, "Speed", "+12%", "+10%", "-2%", 2, false);

            var currentStats = CreatePanel("[SECTION] CurrentStatsSection", column.transform, PanelBackground, Border);
            AddLayout(currentStats, 104f);
            CreateAnchoredText("[HEADER] CurrentStatsHeader", currentStats.transform, "CURRENT STATS", 17f, AccentGold, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(12f, -12f), new Vector2(-12f, -8f));
            CreateStatPair(currentStats.transform, "Attack Damage", "145", "Gathering Power", "1,260", 0);
            CreateStatPair(currentStats.transform, "Defense", "132", "Action Speed", "1.12s", 1);
            CreateStatPair(currentStats.transform, "Accuracy", "92%", "Profession Bonus", "+18%", 2);
        }

        private static void CreateEquipmentBottomSection(Transform parent)
        {
            var section = CreatePanel("[SECTION] ConsumablesAndRunesSection", parent, PanelBackground, Border);
            AddLayout(section, 158f);
            var layout = section.AddComponent<HorizontalLayoutGroup>();
            layout.padding = new RectOffset(14, 14, 14, 14);
            layout.spacing = 12f;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = false;
            layout.childForceExpandHeight = true;

            foreach (var item in new[] { "Food\nHearty Stew", "Potion\nHealing Potion x28", "Elixir 1\nPrecision Elixir x15", "Elixir 2\nFortitude Elixir x12", "Elixir 3\nGathering Elixir x10", "Elixir 4\nSwiftness Elixir x8" })
            {
                var slot = CreatePanel("[SLOT] " + item.Split('\n')[0].Replace(" ", string.Empty) + "ConsumableSlot", section.transform, DeepBackground, Border);
                slot.AddComponent<LayoutElement>().preferredWidth = 150f;
                CreateAnchoredText("[TEXT] ConsumableLabel", slot.transform, item, 16f, TextPrimary, TextAlignmentOptions.Center, Vector2.zero, Vector2.one, new Vector2(6f, 6f), new Vector2(-6f, -6f));
            }

            var runes = CreatePanel("[PANEL] RunesPanel", section.transform, DeepBackground, AccentTeal);
            runes.AddComponent<LayoutElement>().flexibleWidth = 1f;
            CreateAnchoredText("[HEADER] RunesHeader", runes.transform, "RUNES", 18f, AccentGold, TextAlignmentOptions.Center, new Vector2(0f, 1f), Vector2.one, new Vector2(8f, -36f), new Vector2(-8f, -8f));
            CreateAnchoredText("[TEXT] RunesGlyph", runes.transform, "*  *  *", 38f, AccentTeal, TextAlignmentOptions.Center, Vector2.zero, Vector2.one, new Vector2(0f, 8f), new Vector2(0f, -40f));
            CreateAnchoredButton("[BUTTON] OpenRunesButton", runes.transform, "Open Runes", new Vector2(0.5f, 0f), new Vector2(0f, 28f), new Vector2(160f, 34f), PanelBackground);
        }

        private static void CreateCharacterSilhouette(Transform parent)
        {
            var silhouette = CreatePanel("[MANNEQUIN] CharacterSilhouette", parent, new Color(0.03f, 0.05f, 0.06f, 0.72f), Border);
            var rect = (RectTransform)silhouette.transform;
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(0f, -20f);
            rect.sizeDelta = new Vector2(210f, 380f);

            CreateSilhouettePiece(silhouette.transform, "[SILHOUETTE] Head", new Vector2(0f, 132f), new Vector2(58f, 62f));
            CreateSilhouettePiece(silhouette.transform, "[SILHOUETTE] Torso", new Vector2(0f, 48f), new Vector2(104f, 118f));
            CreateSilhouettePiece(silhouette.transform, "[SILHOUETTE] LeftArm", new Vector2(-76f, 34f), new Vector2(38f, 142f));
            CreateSilhouettePiece(silhouette.transform, "[SILHOUETTE] RightArm", new Vector2(76f, 34f), new Vector2(38f, 142f));
            CreateSilhouettePiece(silhouette.transform, "[SILHOUETTE] LeftLeg", new Vector2(-30f, -96f), new Vector2(46f, 150f));
            CreateSilhouettePiece(silhouette.transform, "[SILHOUETTE] RightLeg", new Vector2(30f, -96f), new Vector2(46f, 150f));
        }

        private static void CreateSilhouettePiece(Transform parent, string name, Vector2 position, Vector2 size)
        {
            var piece = CreatePanel(name, parent, new Color(0.18f, 0.21f, 0.22f, 0.42f), new Color(0.35f, 0.28f, 0.18f, 0.65f));
            var rect = (RectTransform)piece.transform;
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = position;
            rect.sizeDelta = size;
        }

        private static void CreateEquipmentSlot(Transform parent, string id, string label, string itemText, Vector2 position, bool highlighted)
        {
            CreateAnchoredText("[LABEL] " + id + "SlotLabel", parent, label, 15f, TextPrimary, TextAlignmentOptions.Center, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), position + new Vector2(-48f, 42f), position + new Vector2(48f, 64f));

            var slot = CreatePanel("[SLOT] " + id + "Slot", parent, DeepBackground, highlighted ? AccentGold : Border);
            var rect = (RectTransform)slot.transform;
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = position;
            rect.sizeDelta = new Vector2(76f, 72f);

            var glyph = id switch
            {
                "Weapon" => "AXE",
                "Shield" => "SHD",
                "Helmet" => "HELM",
                "Chest" => "CHEST",
                "Legs" => "LEGS",
                "Boots" => "BOOT",
                "Gloves" => "GLV",
                "Cape" => "CAPE",
                "Ring" => "RING",
                "Amulet" => "AMLT",
                "Relic" => "RELC",
                "Tool" => "TOOL",
                _ => "ITEM"
            };

            CreateAnchoredText("[TEXT] " + id + "SlotGlyph", slot.transform, glyph, 19f, highlighted ? AccentGold : TextSecondary, TextAlignmentOptions.Center, Vector2.zero, Vector2.one, new Vector2(0f, 8f), new Vector2(0f, -8f));
            CreateAnchoredText("[TEXT] " + id + "ItemName", slot.transform, itemText, 11f, TextSecondary, TextAlignmentOptions.Center, Vector2.zero, Vector2.one, new Vector2(3f, 2f), new Vector2(-3f, -50f));
        }

        private static void CreateStatLine(Transform parent, string label, string value, int row, Color? valueColor = null)
        {
            var top = -42f - row * 24f;
            CreateAnchoredText("[TEXT] " + label.Replace(" ", string.Empty) + "Label", parent, label, 15f, TextPrimary, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(12f, top), new Vector2(-130f, top - 22f));
            CreateAnchoredText("[TEXT] " + label.Replace(" ", string.Empty) + "Value", parent, value, 15f, valueColor ?? TextPrimary, TextAlignmentOptions.Right | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(120f, top), new Vector2(-12f, top - 22f));
        }

        private static void CreateSmallInfoPanel(string name, Transform parent, string header, string body)
        {
            var panel = CreatePanel(name, parent, new Color(0f, 0f, 0f, 0.05f), Border);
            panel.AddComponent<LayoutElement>().flexibleWidth = 1f;
            CreateAnchoredText("[HEADER] " + header.Replace(" ", string.Empty) + "Header", panel.transform, header, 14f, AccentGold, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(8f, -8f), new Vector2(-8f, -8f));
            CreateAnchoredText("[TEXT] " + header.Replace(" ", string.Empty) + "Body", panel.transform, body, 13f, TextPrimary, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(8f, -32f), new Vector2(-8f, -8f));
        }

        private static void CreateEquipmentListRow(Transform parent, string itemName, string power, string level, bool selected)
        {
            var row = CreatePanel("[ROW] " + itemName.Replace(" ", string.Empty) + "Row", parent, selected ? HoverPanel : RaisedPanel, selected ? AccentGold : Border);
            AddLayout(row, 34f);
            CreateAnchoredText("[TEXT] ItemIcon", row.transform, "AX", 17f, AccentGold, TextAlignmentOptions.Center, Vector2.zero, new Vector2(0f, 1f), new Vector2(10f, 0f), new Vector2(42f, 0f));
            CreateAnchoredText("[TEXT] ItemName", row.transform, itemName, 16f, selected ? AccentGold : TextPrimary, TextAlignmentOptions.Left | TextAlignmentOptions.Midline, Vector2.zero, Vector2.one, new Vector2(50f, 0f), new Vector2(-210f, 0f));
            CreateAnchoredText("[TEXT] ItemPower", row.transform, "PWR " + power, 15f, AccentGold, TextAlignmentOptions.Right | TextAlignmentOptions.Midline, new Vector2(1f, 0f), Vector2.one, new Vector2(-200f, 0f), new Vector2(-74f, 0f));
            CreateAnchoredText("[TEXT] ItemLevel", row.transform, level, 14f, AccentGold, TextAlignmentOptions.Right | TextAlignmentOptions.Midline, new Vector2(1f, 0f), Vector2.one, new Vector2(-70f, 0f), new Vector2(-10f, 0f));
        }

        private static void CreateComparisonItemPanel(Transform parent, string name, string label, string itemName, string power, bool leftSide)
        {
            var panel = CreatePanel(name, parent, DeepBackground, Border);
            var rect = (RectTransform)panel.transform;
            var minX = leftSide ? 0f : 0.5f;
            var maxX = leftSide ? 0.5f : 1f;
            SetStretch(rect, new Vector2(minX, 1f), new Vector2(maxX, 1f), new Vector2(leftSide ? 12f : 6f, -88f), new Vector2(leftSide ? -6f : -12f, -34f));

            var icon = CreatePanel("[ICON] ComparisonItemIcon", panel.transform, RaisedPanel, leftSide ? AccentGold : Border);
            var iconRect = (RectTransform)icon.transform;
            iconRect.anchorMin = new Vector2(0f, 0.5f);
            iconRect.anchorMax = new Vector2(0f, 0.5f);
            iconRect.pivot = new Vector2(0f, 0.5f);
            iconRect.anchoredPosition = new Vector2(8f, 0f);
            iconRect.sizeDelta = new Vector2(38f, 38f);
            CreateAnchoredText("[TEXT] ComparisonItemIconGlyph", icon.transform, "AX", 15f, AccentGold, TextAlignmentOptions.Center, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);

            CreateAnchoredText("[LABEL] ComparisonItemLabel", panel.transform, label, 12f, AccentGold, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(54f, -6f), new Vector2(-8f, -20f));
            CreateAnchoredText("[TEXT] ComparisonItemName", panel.transform, itemName, 15f, leftSide ? AccentGold : TextPrimary, TextAlignmentOptions.Left | TextAlignmentOptions.Midline, Vector2.zero, Vector2.one, new Vector2(54f, -22f), new Vector2(-8f, -37f));
            CreateAnchoredText("[TEXT] ComparisonItemPower", panel.transform, power, 13f, TextSecondary, TextAlignmentOptions.Left | TextAlignmentOptions.Bottom, Vector2.zero, Vector2.one, new Vector2(54f, -44f), new Vector2(-8f, -6f));
        }

        private static void CreateComparisonStatRow(Transform parent, string stat, string current, string selected, string delta, int row, bool deltaPositive)
        {
            var top = -96f - row * 19f;
            var statRow = CreatePanel("[ROW] Comparison" + stat.Replace(" ", string.Empty).Replace("/", string.Empty) + "Row", parent, new Color(0f, 0f, 0f, 0.05f), Border);
            SetStretch((RectTransform)statRow.transform, Vector2.zero, Vector2.one, new Vector2(12f, top - 18f), new Vector2(-12f, top));
            CreateAnchoredText("[TEXT] ComparisonStat", statRow.transform, stat, 12f, TextPrimary, TextAlignmentOptions.Left | TextAlignmentOptions.Midline, Vector2.zero, Vector2.one, new Vector2(8f, 0f), new Vector2(-350f, 0f));
            CreateAnchoredText("[TEXT] ComparisonCurrent", statRow.transform, current, 12f, TextPrimary, TextAlignmentOptions.Right | TextAlignmentOptions.Midline, Vector2.zero, Vector2.one, new Vector2(190f, 0f), new Vector2(-260f, 0f));
            CreateAnchoredText("[TEXT] ComparisonArrow", statRow.transform, ">", 12f, AccentGold, TextAlignmentOptions.Center, new Vector2(0.5f, 0f), new Vector2(0.5f, 1f), new Vector2(-12f, 0f), new Vector2(12f, 0f));
            CreateAnchoredText("[TEXT] ComparisonSelected", statRow.transform, selected, 12f, TextPrimary, TextAlignmentOptions.Right | TextAlignmentOptions.Midline, Vector2.zero, Vector2.one, new Vector2(330f, 0f), new Vector2(-118f, 0f));
            CreateAnchoredText("[TEXT] ComparisonDelta", statRow.transform, delta, 12f, deltaPositive ? Success : Danger, TextAlignmentOptions.Right | TextAlignmentOptions.Midline, Vector2.zero, Vector2.one, new Vector2(450f, 0f), new Vector2(-8f, 0f));
        }

        private static void CreateComparisonLine(Transform parent, string stat, string current, string selected, string delta, int row)
        {
            var top = -46f - row * 28f;
            CreateAnchoredText("[TEXT] Comparison" + row + "Stat", parent, stat, 14f, TextPrimary, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(14f, top), new Vector2(-410f, top - 24f));
            CreateAnchoredText("[TEXT] Comparison" + row + "Current", parent, current, 14f, TextPrimary, TextAlignmentOptions.Right | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(190f, top), new Vector2(-260f, top - 24f));
            CreateAnchoredText("[TEXT] Comparison" + row + "Selected", parent, selected, 14f, TextPrimary, TextAlignmentOptions.Right | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(330f, top), new Vector2(-120f, top - 24f));
            CreateAnchoredText("[TEXT] Comparison" + row + "Delta", parent, delta, 14f, Danger, TextAlignmentOptions.Right | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(450f, top), new Vector2(-14f, top - 24f));
        }

        private static void CreateStatPair(Transform parent, string leftLabel, string leftValue, string rightLabel, string rightValue, int row)
        {
            var top = -44f - row * 24f;
            CreateAnchoredText("[TEXT] CurrentStat" + row + "Left", parent, leftLabel + "    " + leftValue, 14f, TextPrimary, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(14f, top), new Vector2(-300f, top - 22f));
            CreateAnchoredText("[TEXT] CurrentStat" + row + "Right", parent, rightLabel + "    " + rightValue, 14f, TextPrimary, TextAlignmentOptions.Left | TextAlignmentOptions.Top, Vector2.zero, Vector2.one, new Vector2(300f, top), new Vector2(-14f, top - 22f));
        }

        private static TMP_Text CreateAnchoredText(string name, Transform parent, string text, float size, Color color, TextAlignmentOptions alignment, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax)
        {
            var textObject = CreateText(name, parent, text, size, color, alignment);
            SetStretch((RectTransform)textObject.transform, anchorMin, anchorMax, offsetMin, offsetMax);
            return textObject.GetComponent<TMP_Text>();
        }

        private static void BuildSettingsScreen(Transform parent)
        {
            var scrollContent = CreateMainScroll("[SCROLL] SettingsMainScroll", parent);
            var settings = CreateSection("[SECTION] SettingsOptionsSection", scrollContent.transform);
            CreateInfoGrid(settings.transform, new[] { "AudioPanel", "GraphicsPanel", "GameplayPanel", "SavePanel" });
            CreateActionRow("[ACTIONS] SettingsActions", parent, new[] { ("[BUTTON] ApplySettingsButton", "Apply", Success), ("[BUTTON] CloseSettingsButton", "Close", PanelBackground) });
        }

        private static void BuildScene(BuildArtifacts artifacts)
        {
            DeleteGeneratedRoot("[BOOTSTRAP] GameBootstrap");
            DeleteGeneratedRoot("[SYSTEMS] GameSystems");
            DeleteGeneratedRoot("[UI] UI System");

            var systemsRoot = new GameObject("[SYSTEMS] GameSystems");
            var uiRoot = new GameObject("[UI] UI System");
            var bootstrapRoot = new GameObject("[BOOTSTRAP] GameBootstrap");

            var mainCanvas = CreateMainCanvas(uiRoot.transform);
            var topBar = CreateTopBar(mainCanvas.transform);
            var mainBody = CreateMainBody(mainCanvas.transform, out var leftNavigation, out var activeActivityBar, out var screenContainer, out var navigationButtons, out var backButton, artifacts);

            var dropdownLayer = CreateOverlayLayer("[OVERLAY] DropdownLayer", mainCanvas.transform);
            var tooltipLayer = CreateOverlayLayer("[OVERLAY] TooltipLayer", mainCanvas.transform);
            CreatePanel("[TOOLTIP] SharedTooltip", tooltipLayer.transform, RaisedPanel, AccentTeal).SetActive(false);
            var notificationLayer = CreateOverlayLayer("[OVERLAY] NotificationLayer", mainCanvas.transform);
            CreateContainer("[CONTAINER] NotificationContainer", notificationLayer.transform, 360f);
            var popupLayer = CreateOverlayLayer("[OVERLAY] PopupLayer", mainCanvas.transform);
            CreatePanel("[POPUP] SharedPopupRoot", popupLayer.transform, RaisedPanel, AccentGold).SetActive(false);
            var modalLayer = CreateOverlayLayer("[OVERLAY] ModalLayer", mainCanvas.transform);
            CreateImage("[IMAGE] ModalBlocker", modalLayer.transform, new Color(0f, 0f, 0f, 0.55f), Border).SetActive(false);
            CreateContainer("[CONTAINER] ModalContainer", modalLayer.transform, 520f);
            var loadingLayer = CreateOverlayLayer("[OVERLAY] LoadingLayer", mainCanvas.transform);
            CreateImage("[IMAGE] LoadingBlocker", loadingLayer.transform, new Color(0f, 0f, 0f, 0.65f), Border).SetActive(false);
            CreateText("[TEXT] LoadingText", loadingLayer.transform, "Loading...", 24f, TextPrimary, TextAlignmentOptions.Center).SetActive(false);
            CreateProgressBar("[BAR] LoadingProgress", loadingLayer.transform, 300f).SetActive(false);

            var shell = uiRoot.AddComponent<PersistentUIShell>();
            shell.ConfigureForEditor((RectTransform)topBar.transform, (RectTransform)leftNavigation.transform, (RectTransform)activeActivityBar.transform, (RectTransform)screenContainer.transform, (RectTransform)dropdownLayer.transform, (RectTransform)tooltipLayer.transform, (RectTransform)notificationLayer.transform, (RectTransform)popupLayer.transform, (RectTransform)modalLayer.transform, (RectTransform)loadingLayer.transform);

            var screenReferences = InstantiateScreens(screenContainer.transform);

            var managers = new GameObject("[UI MANAGERS] UIManagers");
            managers.transform.SetParent(uiRoot.transform, false);
            var screenManager = managers.AddComponent<ScreenManager>();
            screenManager.ConfigureForEditor(artifacts.ScreenCatalog, (RectTransform)screenContainer.transform, screenReferences, ScreenIds.Woodcutting);
            var navigationManager = managers.AddComponent<NavigationManager>();
            navigationManager.ConfigureForEditor(screenManager, navigationButtons, backButton);
            var uiBootstrapper = managers.AddComponent<UIBootstrapper>();
            uiBootstrapper.ConfigureForEditor(shell, screenManager, ScreenIds.Woodcutting);

            var bootstrap = bootstrapRoot.AddComponent<GameBootstrap>();
            bootstrap.ConfigureForEditor(new MonoBehaviour[] { screenManager, navigationManager, uiBootstrapper });

            systemsRoot.transform.SetSiblingIndex(2);
            uiRoot.transform.SetSiblingIndex(3);
            bootstrapRoot.transform.SetSiblingIndex(4);

            EditorUtility.SetDirty(systemsRoot);
            EditorUtility.SetDirty(uiRoot);
            EditorUtility.SetDirty(bootstrapRoot);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            Selection.activeGameObject = mainCanvas;
        }

        private static GameObject CreateMainCanvas(Transform parent)
        {
            var canvasObject = CreateUIObject("[UI] MainCanvas", parent);
            SetStretch((RectTransform)canvasObject.transform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);

            var canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;
            canvasObject.AddComponent<GraphicRaycaster>();

            var background = CreateImage("[IMAGE] Background", canvasObject.transform, DeepBackground, DeepBackground);
            SetStretch((RectTransform)background.transform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            background.transform.SetAsFirstSibling();
            return canvasObject;
        }

        private static GameObject CreateTopBar(Transform parent)
        {
            var topBar = CreatePanel("[PERSISTENT] TopBar", parent, PanelBackground, Border);
            SetStretch((RectTransform)topBar.transform, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0f, -64f), Vector2.zero);

            var layout = topBar.AddComponent<HorizontalLayoutGroup>();
            layout.padding = new RectOffset(18, 18, 8, 8);
            layout.spacing = 18f;
            layout.childAlignment = TextAnchor.MiddleLeft;
            layout.childControlWidth = false;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = false;
            layout.childForceExpandHeight = true;

            var left = CreateLayoutSection("[LAYOUT] LeftSection", topBar.transform, 230f);
            CreateText("[TEXT] GameTitle", left.transform, "Aldaron\nLevel 76", 22f, AccentGold, TextAlignmentOptions.Left | TextAlignmentOptions.Midline);

            var center = CreateLayoutSection("[LAYOUT] CenterSection", topBar.transform, 1f, true);
            CreateText("[TEXT] EmptyCenterLabel", center.transform, string.Empty, 18f, TextSecondary, TextAlignmentOptions.Center);

            var right = CreateLayoutSection("[LAYOUT] RightSection", topBar.transform, 780f);
            var rightLayout = right.AddComponent<HorizontalLayoutGroup>();
            rightLayout.spacing = 10f;
            rightLayout.childControlWidth = false;
            rightLayout.childControlHeight = true;
            rightLayout.childAlignment = TextAnchor.MiddleRight;

            CreateDisplay("[DISPLAY] GoldDisplay", right.transform, "[ICON] GoldIcon", "[TEXT] GoldValue", "12.45M");
            CreateDisplay("[DISPLAY] InventoryCapacityDisplay", right.transform, "[ICON] InventoryIcon", "[TEXT] InventoryCapacityValue", "640 / 900");
            CreateDisplay("[DISPLAY] SaveStatusDisplay", right.transform, "[ICON] SaveStatusIcon", "[TEXT] SaveStatusText", "Saved");
            CreateButton("[BUTTON] NotificationsButton", right.transform, "!", 52f, PanelBackground);
            CreateButton("[BUTTON] SettingsButton", right.transform, "Settings", 100f, PanelBackground);
            return topBar;
        }

        private static GameObject CreateMainBody(Transform parent, out GameObject leftNavigation, out GameObject activeActivityBar, out GameObject screenContainer, out List<NavigationButtonBinding> navigationButtons, out Button backButton, BuildArtifacts artifacts)
        {
            var mainBody = CreateUIObject("[LAYOUT] MainBody", parent);
            SetStretch((RectTransform)mainBody.transform, Vector2.zero, Vector2.one, Vector2.zero, new Vector2(0f, -64f));

            leftNavigation = CreateLeftNavigation(mainBody.transform, out navigationButtons, out backButton);
            SetLeftNavigationRect((RectTransform)leftNavigation.transform, 270f);

            var contentColumn = CreateUIObject("[LAYOUT] ContentColumn", mainBody.transform);
            SetStretch((RectTransform)contentColumn.transform, Vector2.zero, Vector2.one, new Vector2(270f, 0f), Vector2.zero);

            activeActivityBar = CreateActiveActivityBar(contentColumn.transform);
            screenContainer = CreatePanel("[SCREENS] ScreenContainer", contentColumn.transform, DeepBackground, Border);
            SetStretch((RectTransform)screenContainer.transform, Vector2.zero, Vector2.one, Vector2.zero, new Vector2(0f, -88f));
            return mainBody;
        }

        private static GameObject CreateLeftNavigation(Transform parent, out List<NavigationButtonBinding> navigationButtons, out Button backButton)
        {
            navigationButtons = new List<NavigationButtonBinding>();
            var nav = CreatePanel("[PERSISTENT] LeftNavigation", parent, MainBackground, Border);

            var layout = nav.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(12, 12, 10, 12);
            layout.spacing = 8f;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            var header = CreatePanel("[HEADER] NavigationHeader", nav.transform, RaisedPanel, Border);
            AddLayout(header, 52f);
            CreateText("[TEXT] NavigationTitle", header.transform, "Navigation", 22f, AccentGold, TextAlignmentOptions.Left | TextAlignmentOptions.Midline);
            CreateAnchoredButton("[BUTTON] CollapseNavigationButton", header.transform, "<", new Vector2(1f, 0.5f), new Vector2(-24f, 0f), new Vector2(34f, 34f), PanelBackground);

            var scroll = CreateScrollView("[SCROLL] NavigationScrollView", nav.transform, out var content);
            scroll.GetComponent<LayoutElement>().flexibleHeight = 1f;

            var mainGroup = CreateNavigationGroup("[NAV GROUP] MainGroup", content, "Main");
            navigationButtons.Add(CreateNavigationButtonInstance(mainGroup.transform, ScreenIds.Combat, "Combat"));
            navigationButtons.Add(CreateNavigationButtonInstance(mainGroup.transform, ScreenIds.Inventory, "Inventory"));
            navigationButtons.Add(CreateNavigationButtonInstance(mainGroup.transform, ScreenIds.Equipment, "Equipment"));

            var professionGroup = CreateNavigationGroup("[NAV GROUP] ProfessionsGroup", content, "Professions");
            navigationButtons.Add(CreateNavigationButtonInstance(professionGroup.transform, ScreenIds.Woodcutting, "Woodcutting"));

            var accountGroup = CreateNavigationGroup("[NAV GROUP] AccountGroup", content, "Account");
            navigationButtons.Add(CreateNavigationButtonInstance(accountGroup.transform, ScreenIds.Settings, "Settings"));

            var back = CreateButton("[BUTTON] BackNavigationButton", nav.transform, "Back", 0f, PanelBackground);
            AddLayout(back, 48f);
            backButton = back.GetComponent<Button>();
            backButton.interactable = false;
            return nav;
        }

        private static GameObject CreateActiveActivityBar(Transform parent)
        {
            var bar = CreatePanel("[PERSISTENT] ActiveActivityBar", parent, PanelBackground, StrongBorder);
            SetTopStretch((RectTransform)bar.transform, 88f);

            var layout = bar.AddComponent<HorizontalLayoutGroup>();
            layout.padding = new RectOffset(16, 16, 10, 10);
            layout.spacing = 16f;
            layout.childAlignment = TextAnchor.MiddleLeft;
            layout.childControlWidth = false;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = false;
            layout.childForceExpandHeight = true;

            var noActivity = CreateState("[STATE] NoActivityState", bar.transform, 320f);
            CreateText("[TEXT] NoActivityText", noActivity.transform, "No active activity", 22f, TextSecondary, TextAlignmentOptions.Center);
            var profession = CreateState("[STATE] ProfessionActivityState", bar.transform, 560f);
            CreateText("[TEXT] ProfessionNameText", profession.transform, "Woodcutting", 22f, AccentGold, TextAlignmentOptions.Left | TextAlignmentOptions.Midline);
            CreateProgressBar("[BAR] ProfessionProgressBar", profession.transform, 230f);
            CreateButton("[BUTTON] OpenProfessionButton", profession.transform, "Open", 92f, PanelBackground);
            CreateButton("[BUTTON] StopProfessionButton", profession.transform, "Stop", 92f, Danger);
            profession.SetActive(false);

            var combat = CreateState("[STATE] CombatActivityState", bar.transform, 620f);
            CreateText("[TEXT] CombatDisciplineText", combat.transform, "Combat", 22f, AccentGold, TextAlignmentOptions.Left | TextAlignmentOptions.Midline);
            CreateProgressBar("[BAR] CompactPlayerHealthBar", combat.transform, 160f);
            CreateProgressBar("[BAR] CompactEnemyHealthBar", combat.transform, 160f);
            CreateButton("[BUTTON] OpenCombatButton", combat.transform, "Open", 92f, PanelBackground);
            CreateButton("[BUTTON] QuitCombatButton", combat.transform, "Quit", 92f, Danger);
            combat.SetActive(false);

            return bar;
        }

        private static void SetLeftNavigationRect(RectTransform rectTransform, float width)
        {
            rectTransform.anchorMin = new Vector2(0f, 0f);
            rectTransform.anchorMax = new Vector2(0f, 1f);
            rectTransform.pivot = new Vector2(0f, 0.5f);
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = new Vector2(width, 0f);
            rectTransform.localScale = Vector3.one;
        }

        private static void SetTopStretch(RectTransform rectTransform, float height)
        {
            rectTransform.anchorMin = new Vector2(0f, 1f);
            rectTransform.anchorMax = new Vector2(1f, 1f);
            rectTransform.pivot = new Vector2(0.5f, 1f);
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = new Vector2(0f, height);
            rectTransform.localScale = Vector3.one;
        }

        private static IReadOnlyList<UIScreenReference> InstantiateScreens(Transform screenContainer)
        {
            var screenReferences = new List<UIScreenReference>();
            foreach (var screen in GetScreens())
            {
                var instance = CreateScreenObject(screen, screenContainer);
                SetStretch((RectTransform)instance.transform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
                instance.SetActive(screen.Id == ScreenIds.Woodcutting);
                var reference = instance.GetComponent<UIScreenReference>();
                if (reference == null)
                {
                    throw new InvalidOperationException($"{instance.name} is missing {nameof(UIScreenReference)}.");
                }

                screenReferences.Add(reference);
            }

            return screenReferences;
        }

        private static NavigationButtonBinding CreateNavigationButtonInstance(Transform parent, string screenId, string label)
        {
            var instance = CreateNavigationButtonObject($"[NAV BUTTON] {label}Button", parent);
            var binding = instance.GetComponent<NavigationButtonBinding>();
            var button = instance.GetComponent<Button>();
            var background = FindChild<Image>(instance.transform, "[IMAGE] Background");
            var marker = FindChild<Image>(instance.transform, "[IMAGE] SelectedMarker");
            var icon = FindChild<Image>(instance.transform, "[ICON] Icon");
            var labelText = FindChild<TMP_Text>(instance.transform, "[TEXT] Label");
            var lockIcon = FindChild<Image>(instance.transform, "[ICON] LockIcon");
            var badge = FindChildTransform(instance.transform, "[BADGE] NotificationBadge")?.gameObject;
            var badgeText = FindChild<TMP_Text>(instance.transform, "[TEXT] BadgeText");
            labelText.text = label;
            binding.ConfigureForEditor(screenId, button, background, marker, icon, labelText, lockIcon, badge, badgeText);
            return binding;
        }

        private static GameObject CreateNavigationGroup(string name, Transform parent, string label)
        {
            var group = CreateUIObject(name, parent);
            var layout = group.AddComponent<VerticalLayoutGroup>();
            layout.spacing = 7f;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;
            var fitter = group.AddComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            var header = CreateText("[HEADER] " + label + "GroupHeader", group.transform, label.ToUpperInvariant(), 18f, AccentGold, TextAlignmentOptions.Left | TextAlignmentOptions.Midline);
            AddLayout(header, 28f);
            return group;
        }

        private static GameObject CreateMainScroll(string name, Transform parent)
        {
            var scroll = CreateScrollView(name, parent, out var content);
            var layout = scroll.GetComponent<LayoutElement>();
            layout.flexibleHeight = 1f;
            layout.minHeight = 440f;
            return content.gameObject;
        }

        private static GameObject CreateScrollView(string name, Transform parent, out RectTransform content)
        {
            var scroll = CreatePanel(name, parent, DeepBackground, Border);
            var layout = scroll.AddComponent<LayoutElement>();
            layout.minHeight = 120f;
            var scrollRect = scroll.AddComponent<ScrollRect>();
            scrollRect.horizontal = false;
            scrollRect.vertical = true;
            scrollRect.movementType = ScrollRect.MovementType.Clamped;
            scrollRect.scrollSensitivity = 28f;

            var viewport = CreatePanel("Viewport", scroll.transform, new Color(0f, 0f, 0f, 0.05f), Border);
            SetStretch((RectTransform)viewport.transform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            viewport.AddComponent<Mask>().showMaskGraphic = false;
            scrollRect.viewport = (RectTransform)viewport.transform;

            var contentObject = CreateUIObject("Content", viewport.transform);
            content = (RectTransform)contentObject.transform;
            SetStretch(content, new Vector2(0f, 1f), new Vector2(1f, 1f), Vector2.zero, Vector2.zero);
            content.pivot = new Vector2(0.5f, 1f);
            var contentLayout = contentObject.AddComponent<VerticalLayoutGroup>();
            contentLayout.padding = new RectOffset(10, 10, 10, 10);
            contentLayout.spacing = 12f;
            contentLayout.childControlWidth = true;
            contentLayout.childControlHeight = true;
            contentLayout.childForceExpandWidth = true;
            contentLayout.childForceExpandHeight = false;
            var fitter = contentObject.AddComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            scrollRect.content = content;
            return scroll;
        }

        private static Transform CreateHorizontalScrollContent(GameObject scroll, string contentName, RectOffset padding, float spacing)
        {
            var scrollRect = scroll.AddComponent<ScrollRect>();
            scrollRect.horizontal = true;
            scrollRect.vertical = false;
            scrollRect.movementType = ScrollRect.MovementType.Clamped;
            scrollRect.scrollSensitivity = 24f;

            var viewport = CreatePanel("Viewport", scroll.transform, new Color(0f, 0f, 0f, 0.04f), Border);
            SetStretch((RectTransform)viewport.transform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            viewport.AddComponent<Mask>().showMaskGraphic = false;
            scrollRect.viewport = (RectTransform)viewport.transform;

            var contentObject = CreateUIObject(contentName, viewport.transform);
            var content = (RectTransform)contentObject.transform;
            SetStretch(content, new Vector2(0f, 0f), new Vector2(0f, 1f), Vector2.zero, Vector2.zero);
            content.pivot = new Vector2(0f, 0.5f);
            content.anchoredPosition = Vector2.zero;
            content.sizeDelta = Vector2.zero;

            var layout = contentObject.AddComponent<HorizontalLayoutGroup>();
            layout.padding = padding;
            layout.spacing = spacing;
            layout.childControlWidth = false;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = false;
            layout.childForceExpandHeight = true;

            var fitter = contentObject.AddComponent<ContentSizeFitter>();
            fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            scrollRect.content = content;
            return contentObject.transform;
        }

        private static Transform CreateVerticalScrollContent(GameObject scroll, string contentName, RectOffset padding, float spacing)
        {
            var scrollRect = scroll.AddComponent<ScrollRect>();
            scrollRect.horizontal = false;
            scrollRect.vertical = true;
            scrollRect.movementType = ScrollRect.MovementType.Clamped;
            scrollRect.scrollSensitivity = 24f;

            var viewport = CreatePanel("Viewport", scroll.transform, new Color(0f, 0f, 0f, 0.04f), Border);
            SetStretch((RectTransform)viewport.transform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            viewport.AddComponent<Mask>().showMaskGraphic = false;
            scrollRect.viewport = (RectTransform)viewport.transform;

            var contentObject = CreateUIObject(contentName, viewport.transform);
            var content = (RectTransform)contentObject.transform;
            SetStretch(content, new Vector2(0f, 1f), new Vector2(1f, 1f), Vector2.zero, Vector2.zero);
            content.pivot = new Vector2(0.5f, 1f);
            content.anchoredPosition = Vector2.zero;
            content.sizeDelta = Vector2.zero;

            var layout = contentObject.AddComponent<VerticalLayoutGroup>();
            layout.padding = padding;
            layout.spacing = spacing;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            var fitter = contentObject.AddComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            scrollRect.content = content;
            return contentObject.transform;
        }

        private static GameObject CreateSection(string name, Transform parent)
        {
            var section = CreatePanel(name, parent, PanelBackground, Border);
            var layout = section.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(14, 14, 12, 12);
            layout.spacing = 10f;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;
            var fitter = section.AddComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            return section;
        }

        private static void CreateSectionHeader(Transform parent, string text)
        {
            var header = CreateText("[HEADER] " + text.Replace(" ", string.Empty) + "Header", parent, text, 22f, AccentGold, TextAlignmentOptions.Left | TextAlignmentOptions.Midline);
            AddLayout(header, 34f);
        }

        private static void CreateInfoGrid(Transform parent, IReadOnlyList<string> panelNames)
        {
            var grid = CreateUIObject("[LAYOUT] InfoGrid", parent);
            var gridLayout = grid.AddComponent<GridLayoutGroup>();
            gridLayout.cellSize = new Vector2(280f, 118f);
            gridLayout.spacing = new Vector2(12f, 12f);
            gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayout.constraintCount = 4;
            var layout = grid.AddComponent<LayoutElement>();
            layout.preferredHeight = Mathf.Ceil(panelNames.Count / 4f) * 130f;
            foreach (var panelName in panelNames)
            {
                var panel = CreatePanel("[PANEL] " + panelName, grid.transform, RaisedPanel, Border);
                CreateText("[TEXT] " + panelName + "Label", panel.transform, panelName, 18f, TextSecondary, TextAlignmentOptions.Center);
            }
        }

        private static void CreateDynamicContainer(string name, Transform parent, float height)
        {
            var container = CreatePanel(name, parent, DeepBackground, Border);
            AddLayout(container, height);
            CreateText("[TEXT] DynamicContentNote", container.transform, "Runtime entries use scene-authored templates here.", 18f, TextSecondary, TextAlignmentOptions.Center);
        }

        private static void CreateActionRow(string name, Transform parent, IReadOnlyList<(string Name, string Label, Color Color)> buttons)
        {
            var row = CreatePanel(name, parent, RaisedPanel, Border);
            AddLayout(row, 64f);
            var layout = row.AddComponent<HorizontalLayoutGroup>();
            layout.padding = new RectOffset(12, 12, 10, 10);
            layout.spacing = 12f;
            layout.childControlWidth = false;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = false;
            layout.childForceExpandHeight = true;
            foreach (var button in buttons)
            {
                CreateButton(button.Name, row.transform, button.Label, 180f, button.Color);
            }
        }

        private static GameObject CreateState(string name, Transform parent, float width)
        {
            var state = CreatePanel(name, parent, RaisedPanel, Border);
            var layoutElement = state.AddComponent<LayoutElement>();
            layoutElement.preferredWidth = width;
            var layout = state.AddComponent<HorizontalLayoutGroup>();
            layout.padding = new RectOffset(10, 10, 8, 8);
            layout.spacing = 10f;
            layout.childControlWidth = false;
            layout.childControlHeight = true;
            layout.childAlignment = TextAnchor.MiddleLeft;
            return state;
        }

        private static GameObject CreateDisplay(string name, Transform parent, string iconName, string textName, string value)
        {
            var display = CreatePanel(name, parent, RaisedPanel, Border);
            var element = display.AddComponent<LayoutElement>();
            element.preferredWidth = 165f;
            var layout = display.AddComponent<HorizontalLayoutGroup>();
            layout.padding = new RectOffset(8, 8, 6, 6);
            layout.spacing = 8f;
            layout.childControlWidth = false;
            layout.childControlHeight = true;
            var icon = CreateImage(iconName, display.transform, AccentGold, Border);
            icon.GetComponent<LayoutElement>().preferredWidth = 26f;
            CreateText(textName, display.transform, value, 20f, TextPrimary, TextAlignmentOptions.Left | TextAlignmentOptions.Midline);
            return display;
        }

        private static GameObject CreateContainer(string name, Transform parent, float width)
        {
            var container = CreatePanel(name, parent, RaisedPanel, Border);
            var rect = (RectTransform)container.transform;
            rect.anchorMin = new Vector2(1f, 0.5f);
            rect.anchorMax = new Vector2(1f, 0.5f);
            rect.anchoredPosition = new Vector2(-width * 0.5f - 24f, 0f);
            rect.sizeDelta = new Vector2(width, 420f);
            return container;
        }

        private static GameObject CreateLayoutSection(string name, Transform parent, float widthOrFlex, bool flexible = false)
        {
            var section = CreateUIObject(name, parent);
            var element = section.AddComponent<LayoutElement>();
            if (flexible)
            {
                element.flexibleWidth = widthOrFlex;
            }
            else
            {
                element.preferredWidth = widthOrFlex;
            }

            return section;
        }

        private static GameObject CreateOverlayLayer(string name, Transform parent)
        {
            var layer = CreateUIObject(name, parent);
            SetStretch((RectTransform)layer.transform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            var canvasGroup = layer.AddComponent<CanvasGroup>();
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
            return layer;
        }

        private static GameObject CreateProgressBar(string name, Transform parent, float width)
        {
            var bar = CreatePanel(name, parent, DeepBackground, Border);
            var element = bar.AddComponent<LayoutElement>();
            element.preferredWidth = width;
            element.preferredHeight = 24f;
            var fill = CreateImage("Fill", bar.transform, AccentTeal, AccentTeal);
            SetStretch((RectTransform)fill.transform, Vector2.zero, new Vector2(0.55f, 1f), Vector2.zero, Vector2.zero);
            var text = CreateText("[TEXT] ValueText", bar.transform, "-- / --", 14f, TextPrimary, TextAlignmentOptions.Center);
            SetStretch((RectTransform)text.transform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            return bar;
        }

        private static GameObject CreateAnchoredButton(string name, Transform parent, string label, Vector2 anchor, Vector2 position, Vector2 size, Color color)
        {
            var button = CreatePanel(name, parent, color, Border);
            var rect = (RectTransform)button.transform;
            rect.anchorMin = anchor;
            rect.anchorMax = anchor;
            rect.anchoredPosition = position;
            rect.sizeDelta = size;
            var image = button.GetComponent<Image>();
            var buttonComponent = button.AddComponent<Button>();
            buttonComponent.targetGraphic = image;
            buttonComponent.colors = ButtonColors(Color.white, HoverPanel, DeepBackground, color, new Color(0.35f, 0.35f, 0.35f, 1f));
            var labelObject = CreateText("[TEXT] Label", button.transform, label, 18f, TextPrimary, TextAlignmentOptions.Center);
            SetStretch((RectTransform)labelObject.transform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            return button;
        }

        private static GameObject CreateButton(string name, Transform parent, string label, float width, Color color)
        {
            var button = CreatePanel(name, parent, color, Border);
            var element = button.AddComponent<LayoutElement>();
            if (width > 0f)
            {
                element.preferredWidth = width;
            }

            element.preferredHeight = 42f;
            var image = button.GetComponent<Image>();
            var buttonComponent = button.AddComponent<Button>();
            buttonComponent.targetGraphic = image;
            buttonComponent.colors = ButtonColors(Color.white, HoverPanel, DeepBackground, color, new Color(0.35f, 0.35f, 0.35f, 1f));
            var labelObject = CreateText("[TEXT] Label", button.transform, label, 18f, TextPrimary, TextAlignmentOptions.Center);
            SetStretch((RectTransform)labelObject.transform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            return button;
        }

        private static GameObject CreateImage(string name, Transform parent, Color background, Color border)
        {
            var imageObject = CreateUIObject(name, parent);
            AddPanel(imageObject, background, border);
            var layout = imageObject.AddComponent<LayoutElement>();
            layout.preferredWidth = 32f;
            layout.preferredHeight = 32f;
            return imageObject;
        }

        private static GameObject CreatePanel(string name, Transform parent, Color background, Color border)
        {
            var panel = CreateUIObject(name, parent);
            AddPanel(panel, background, border);
            return panel;
        }

        private static void AddPanel(GameObject target, Color background, Color border)
        {
            var image = target.AddComponent<Image>();
            image.color = background;
            image.raycastTarget = true;
            var outline = target.AddComponent<Outline>();
            outline.effectColor = border;
            outline.effectDistance = new Vector2(1f, -1f);
            outline.useGraphicAlpha = false;
        }

        private static GameObject CreateText(string name, Transform parent, string text, float size, Color color, TextAlignmentOptions alignment)
        {
            var textObject = CreateUIObject(name, parent);
            var label = textObject.AddComponent<TextMeshProUGUI>();
            label.text = text;
            label.fontSize = size;
            label.color = color;
            label.alignment = alignment;
            label.textWrappingMode = TextWrappingModes.Normal;
            label.raycastTarget = false;
            return textObject;
        }

        private static void AddLayout(GameObject target, float preferredHeight)
        {
            var layout = target.AddComponent<LayoutElement>();
            layout.minHeight = preferredHeight;
            layout.preferredHeight = preferredHeight;
        }

        private static GameObject CreateUIObject(string name, Transform parent)
        {
            var gameObject = new GameObject(name, typeof(RectTransform));
            gameObject.layer = LayerMask.NameToLayer("UI");
            if (parent != null)
            {
                gameObject.transform.SetParent(parent, false);
            }

            return gameObject;
        }

        private static void SetStretch(RectTransform rectTransform, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax)
        {
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
            rectTransform.offsetMin = offsetMin;
            rectTransform.offsetMax = offsetMax;
            rectTransform.localScale = Vector3.one;
        }

        private static T FindChild<T>(Transform root, string childName) where T : Component
        {
            var child = FindChildTransform(root, childName);
            if (child == null)
            {
                throw new InvalidOperationException($"Could not find child '{childName}' under '{root.name}'.");
            }

            return child.GetComponent<T>();
        }

        private static Transform FindChildTransform(Transform root, string childName)
        {
            foreach (Transform child in root)
            {
                if (child.name == childName)
                {
                    return child;
                }

                var nested = FindChildTransform(child, childName);
                if (nested != null)
                {
                    return nested;
                }
            }

            return null;
        }

        private static ColorBlock ButtonColors(Color normal, Color highlighted, Color pressed, Color selected, Color disabled)
        {
            return new ColorBlock
            {
                normalColor = normal,
                highlightedColor = highlighted,
                pressedColor = pressed,
                selectedColor = selected,
                disabledColor = disabled,
                colorMultiplier = 1f,
                fadeDuration = 0.08f
            };
        }

        private static void EnsureFolders()
        {
            EnsureFolder("Assets/Game");
            EnsureFolder("Assets/Game/UI");
            EnsureFolder(DataFolder);
            EnsureFolder(ScreenDataFolder);
            EnsureFolder(NavigationDataFolder);
        }

        private static void EnsureFolder(string folderPath)
        {
            if (AssetDatabase.IsValidFolder(folderPath))
            {
                return;
            }

            var parent = Path.GetDirectoryName(folderPath)?.Replace("\\", "/");
            var name = Path.GetFileName(folderPath);
            if (string.IsNullOrWhiteSpace(parent))
            {
                throw new InvalidOperationException("Invalid folder path: " + folderPath);
            }

            EnsureFolder(parent);
            AssetDatabase.CreateFolder(parent, name);
        }

        private static T CreateAsset<T>(string path, Action<T> configure) where T : ScriptableObject
        {
            DeleteAssetIfExists(path);
            var asset = ScriptableObject.CreateInstance<T>();
            configure(asset);
            AssetDatabase.CreateAsset(asset, path);
            EditorUtility.SetDirty(asset);
            return asset;
        }

        private static void DeleteAssetIfExists(string path)
        {
            if (AssetDatabase.LoadAssetAtPath<Object>(path) != null)
            {
                AssetDatabase.DeleteAsset(path);
            }
        }

        private static void DeleteGeneratedRoot(string name)
        {
            var root = GameObject.Find(name);
            if (root != null)
            {
                Object.DestroyImmediate(root);
            }
        }

        private static int ScanMissingScripts(GameObject root)
        {
            var count = 0;
            var missingOnRoot = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(root);
            if (missingOnRoot > 0)
            {
                Debug.LogError($"Missing script on '{GetPath(root.transform)}': {missingOnRoot}", root);
                count += missingOnRoot;
            }

            foreach (Transform child in root.transform)
            {
                count += ScanMissingScripts(child.gameObject);
            }

            return count;
        }

        private static string GetPath(Transform transform)
        {
            var names = new Stack<string>();
            var current = transform;
            while (current != null)
            {
                names.Push(current.name);
                current = current.parent;
            }

            return string.Join("/", names);
        }

        private static Color HtmlColor(string hex)
        {
            if (ColorUtility.TryParseHtmlString("#" + hex, out var color))
            {
                return color;
            }

            throw new InvalidOperationException("Invalid color " + hex);
        }

        private static IReadOnlyList<ScreenSpec> GetScreens()
        {
            return new[]
            {
                new ScreenSpec(ScreenIds.Combat, "Combat", "Editable combat screen shell. Combat simulation remains out of scope for this UI refactor.", "main", 10, 10),
                new ScreenSpec(ScreenIds.Inventory, "Inventory", "Editable inventory screen shell. Item slots remain future dynamic scene content.", "main", 20, 20),
                new ScreenSpec(ScreenIds.Equipment, "Equipment", "Editable equipment screen shell with visible slot layout placeholders.", "main", 30, 30),
                new ScreenSpec(ScreenIds.Woodcutting, "Woodcutting", "Editable profession screen shell. Woodcutting gameplay is intentionally not implemented yet.", "professions", 40, 10),
                new ScreenSpec(ScreenIds.Settings, "Settings", "Editable settings screen shell for future options.", "account", 50, 10)
            };
        }

        private readonly struct ScreenSpec
        {
            public ScreenSpec(string id, string title, string subtitle, string groupId, int screenOrder, int navigationOrder)
            {
                Id = id;
                Title = title;
                Subtitle = subtitle;
                GroupId = groupId;
                ScreenOrder = screenOrder;
                NavigationOrder = navigationOrder;
            }

            public string Id { get; }
            public string Title { get; }
            public string Subtitle { get; }
            public string GroupId { get; }
            public int ScreenOrder { get; }
            public int NavigationOrder { get; }
        }

        private readonly struct BuildArtifacts
        {
            public BuildArtifacts(UIScreenCatalog screenCatalog, NavigationCatalog navigationCatalog)
            {
                ScreenCatalog = screenCatalog;
                NavigationCatalog = navigationCatalog;
            }

            public UIScreenCatalog ScreenCatalog { get; }
            public NavigationCatalog NavigationCatalog { get; }
        }
    }
}
