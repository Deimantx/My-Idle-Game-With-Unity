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
        private const string PrefabRoot = "Assets/Game/UI/Prefabs";
        private const string NavigationPrefabFolder = "Assets/Game/UI/Prefabs/Navigation";
        private const string ScreenPrefabFolder = "Assets/Game/UI/Prefabs/Screens";

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

            foreach (var prefabGuid in AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Game/UI/Prefabs" }))
            {
                var path = AssetDatabase.GUIDToAssetPath(prefabGuid);
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab != null)
                {
                    total += ScanMissingScripts(prefab);
                }
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
            var navigationButtonPrefab = CreateNavigationButtonPrefab();
            var screens = GetScreens();

            var screenPrefabs = screens.ToDictionary(screen => screen.Id, CreateScreenPrefab, StringComparer.Ordinal);

            var screenDefinitions = screens
                .Select(screen => CreateAsset<ScreenDefinition>(
                    $"{ScreenDataFolder}/Screen_{screen.Title}.asset",
                    asset => asset.ConfigureForEditor(screen.Id, screen.Title, screen.Subtitle, screen.ScreenOrder, screenPrefabs[screen.Id])))
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

            return new BuildArtifacts(screenCatalog, navigationCatalog, navigationButtonPrefab, screenPrefabs);
        }

        private static GameObject CreateNavigationButtonPrefab()
        {
            var path = $"{NavigationPrefabFolder}/NavigationButton.prefab";
            DeleteAssetIfExists(path);

            var root = CreateUIObject("[NAV BUTTON] NavigationButton", null);
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

            var prefab = PrefabUtility.SaveAsPrefabAsset(root, path);
            Object.DestroyImmediate(root);
            return prefab;
        }

        private static GameObject CreateScreenPrefab(ScreenSpec screen)
        {
            var path = $"{ScreenPrefabFolder}/{screen.Title}Screen.prefab";
            DeleteAssetIfExists(path);

            var root = CreateUIObject($"[SCREEN] {screen.Title}Screen", null);
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

            var prefab = PrefabUtility.SaveAsPrefabAsset(root, path);
            Object.DestroyImmediate(root);
            return prefab;
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
            var scrollContent = CreateMainScroll("[SCROLL] WoodcuttingMainScroll", parent);
            var activeTarget = CreateSection("[SECTION] ActiveTargetSection", scrollContent.transform);
            CreateSectionHeader(activeTarget.transform, "Active Target");
            CreateInfoGrid(activeTarget.transform, new[] { "TargetImage", "TargetName", "TreeHealthBar", "TargetRewards" });
            CreateDynamicContainer("[DYNAMIC CONTENT] TreeCardContainer", scrollContent.transform, 240f);
            var details = CreateSection("[SECTION] WoodcuttingDetailsSection", scrollContent.transform);
            CreateInfoGrid(details.transform, new[] { "EquipmentPanel", "GatheringPowerPanel", "LootPreviewPanel", "ActiveBonusesPanel" });
            CreateActionRow("[ACTIONS] WoodcuttingActions", parent, new[] { ("[BUTTON] StartWoodcuttingButton", "Start", Success), ("[BUTTON] StopWoodcuttingButton", "Stop", Danger) });
        }

        private static void BuildCombatScreen(Transform parent)
        {
            var scrollContent = CreateMainScroll("[SCROLL] CombatMainScroll", parent);
            var selection = CreateSection("[SECTION] CombatSelectionSection", scrollContent.transform);
            CreateSectionHeader(selection.transform, "Combat Selection");
            CreateDynamicContainer("[DYNAMIC CONTENT] RegionCardContainer", selection.transform, 96f);
            CreateDynamicContainer("[DYNAMIC CONTENT] EnemyCardContainer", selection.transform, 160f);

            var activeCombat = CreateSection("[SECTION] ActiveCombatSection", scrollContent.transform);
            CreateInfoGrid(activeCombat.transform, new[] { "PlayerPanel", "CompanionPanel", "EnemyPanel", "TemporaryLootPanel" });
            CreateDynamicContainer("[DYNAMIC CONTENT] CombatLogRows", scrollContent.transform, 180f);
            CreateActionRow("[ACTIONS] CombatActions", parent, new[] { ("[BUTTON] EnterEncounterButton", "Enter Encounter", Danger), ("[BUTTON] QuitCombatButton", "Quit", PanelBackground) });
        }

        private static void BuildInventoryScreen(Transform parent)
        {
            var scrollContent = CreateMainScroll("[SCROLL] InventoryMainScroll", parent);
            var controls = CreateSection("[SECTION] InventoryControlsSection", scrollContent.transform);
            CreateInfoGrid(controls.transform, new[] { "SearchField", "FilterButtons", "SortDropdown", "CapacityPanel" });
            var body = CreateSection("[SECTION] InventoryBodySection", scrollContent.transform);
            CreateDynamicContainer("[DYNAMIC CONTENT] InventorySlotGrid", body.transform, 360f);
            CreateInfoGrid(body.transform, new[] { "ItemDetailsPanel", "ComparisonPanel" });
            CreateActionRow("[ACTIONS] InventoryActions", parent, new[] { ("[BUTTON] PickAllButton", "Pick All", PanelBackground), ("[BUTTON] DropSelectedButton", "Drop Selected", Danger) });
        }

        private static void BuildEquipmentScreen(Transform parent)
        {
            var scrollContent = CreateMainScroll("[SCROLL] EquipmentMainScroll", parent);
            var slots = CreateSection("[SECTION] EquipmentSlotsSection", scrollContent.transform);
            CreateInfoGrid(slots.transform, new[] { "WeaponSlot", "OffhandSlot", "HelmetSlot", "ChestSlot", "RingSlot", "AmuletSlot", "CapeSlot", "ToolSlot" });
            var details = CreateSection("[SECTION] EquipmentDetailsSection", scrollContent.transform);
            CreateInfoGrid(details.transform, new[] { "CharacterSummaryPanel", "StatPreviewPanel", "LoadoutNotesPanel", "EquipmentInventoryPanel" });
            CreateActionRow("[ACTIONS] EquipmentActions", parent, new[] { ("[BUTTON] EquipSelectedButton", "Equip", PanelBackground), ("[BUTTON] UnequipSelectedButton", "Unequip", Danger) });
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

            var screenReferences = InstantiateScreens(screenContainer.transform, artifacts);

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

            leftNavigation = CreateLeftNavigation(mainBody.transform, artifacts.NavigationButtonPrefab, out navigationButtons, out backButton);
            SetLeftNavigationRect((RectTransform)leftNavigation.transform, 270f);

            var contentColumn = CreateUIObject("[LAYOUT] ContentColumn", mainBody.transform);
            SetStretch((RectTransform)contentColumn.transform, Vector2.zero, Vector2.one, new Vector2(270f, 0f), Vector2.zero);

            activeActivityBar = CreateActiveActivityBar(contentColumn.transform);
            screenContainer = CreatePanel("[SCREENS] ScreenContainer", contentColumn.transform, DeepBackground, Border);
            SetStretch((RectTransform)screenContainer.transform, Vector2.zero, Vector2.one, Vector2.zero, new Vector2(0f, -88f));
            return mainBody;
        }

        private static GameObject CreateLeftNavigation(Transform parent, GameObject buttonPrefab, out List<NavigationButtonBinding> navigationButtons, out Button backButton)
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
            navigationButtons.Add(CreateNavigationButtonInstance(buttonPrefab, mainGroup.transform, ScreenIds.Combat, "Combat"));
            navigationButtons.Add(CreateNavigationButtonInstance(buttonPrefab, mainGroup.transform, ScreenIds.Inventory, "Inventory"));
            navigationButtons.Add(CreateNavigationButtonInstance(buttonPrefab, mainGroup.transform, ScreenIds.Equipment, "Equipment"));

            var professionGroup = CreateNavigationGroup("[NAV GROUP] ProfessionsGroup", content, "Professions");
            navigationButtons.Add(CreateNavigationButtonInstance(buttonPrefab, professionGroup.transform, ScreenIds.Woodcutting, "Woodcutting"));

            var accountGroup = CreateNavigationGroup("[NAV GROUP] AccountGroup", content, "Account");
            navigationButtons.Add(CreateNavigationButtonInstance(buttonPrefab, accountGroup.transform, ScreenIds.Settings, "Settings"));

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

        private static IReadOnlyList<UIScreenReference> InstantiateScreens(Transform screenContainer, BuildArtifacts artifacts)
        {
            var screenReferences = new List<UIScreenReference>();
            foreach (var screen in GetScreens())
            {
                var prefab = artifacts.ScreenPrefabs[screen.Id];
                var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, screenContainer);
                instance.name = $"[SCREEN] {screen.Title}Screen";
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

        private static NavigationButtonBinding CreateNavigationButtonInstance(GameObject prefab, Transform parent, string screenId, string label)
        {
            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, parent);
            instance.name = $"[NAV BUTTON] {label}Button";
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
            CreateText("[TEXT] DynamicContentNote", container.transform, "Runtime entries use editable prefabs here.", 18f, TextSecondary, TextAlignmentOptions.Center);
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
            EnsureFolder(PrefabRoot);
            EnsureFolder(NavigationPrefabFolder);
            EnsureFolder(ScreenPrefabFolder);
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
                new ScreenSpec(ScreenIds.Inventory, "Inventory", "Editable inventory screen shell. Item slots remain future dynamic prefab content.", "main", 20, 20),
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
            public BuildArtifacts(UIScreenCatalog screenCatalog, NavigationCatalog navigationCatalog, GameObject navigationButtonPrefab, IReadOnlyDictionary<string, GameObject> screenPrefabs)
            {
                ScreenCatalog = screenCatalog;
                NavigationCatalog = navigationCatalog;
                NavigationButtonPrefab = navigationButtonPrefab;
                ScreenPrefabs = screenPrefabs;
            }

            public UIScreenCatalog ScreenCatalog { get; }
            public NavigationCatalog NavigationCatalog { get; }
            public GameObject NavigationButtonPrefab { get; }
            public IReadOnlyDictionary<string, GameObject> ScreenPrefabs { get; }
        }
    }
}
