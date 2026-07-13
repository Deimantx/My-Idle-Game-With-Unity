using System.Linq;
using IdleGame.UI;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace IdleGame.Tests.EditMode
{
    public sealed class EditableHierarchySceneTests
    {
        private static readonly string[] RequiredScreenIds =
        {
            ScreenIds.Woodcutting,
            ScreenIds.Combat,
            ScreenIds.Inventory,
            ScreenIds.Equipment
        };

        [Test]
        public void PrimarySceneContainsEditableNavigationButtonsAndScreenRoots()
        {
            EditorSceneManager.OpenScene("Assets/Scenes/SampleScene.unity", OpenSceneMode.Single);

            Assert.NotNull(GameObject.Find("[UI] MainCanvas"));
            Assert.NotNull(GameObject.Find("[PERSISTENT] TopBar"));
            Assert.NotNull(GameObject.Find("[PERSISTENT] LeftNavigation"));
            Assert.NotNull(GameObject.Find("[PERSISTENT] ActiveActivityBar"));
            Assert.NotNull(GameObject.Find("[SCREENS] ScreenContainer"));

            var navigationButtons = Object.FindObjectsByType<NavigationButtonBinding>(FindObjectsInactive.Include);
            foreach (var screenId in RequiredScreenIds)
            {
                Assert.IsTrue(navigationButtons.Any(button => button.ScreenId == screenId), "Missing visible navigation button for " + screenId);
            }

            var screenReferences = Object.FindObjectsByType<UIScreenReference>(FindObjectsInactive.Include);
            foreach (var screenId in RequiredScreenIds)
            {
                var screenReference = screenReferences.FirstOrDefault(reference => reference.ScreenId == screenId);
                Assert.NotNull(screenReference, "Missing visible screen root for " + screenId);
                Assert.AreEqual("[SCREENS] ScreenContainer", screenReference.transform.parent.name);
            }
        }

        [Test]
        public void PrimaryShellMajorPanelsExposeEditableRectTransforms()
        {
            EditorSceneManager.OpenScene("Assets/Scenes/SampleScene.unity", OpenSceneMode.Single);

            var mainBody = GameObject.Find("[LAYOUT] MainBody");
            var contentColumn = GameObject.Find("[LAYOUT] ContentColumn");
            var leftNavigation = GameObject.Find("[PERSISTENT] LeftNavigation");
            var activeActivityBar = GameObject.Find("[PERSISTENT] ActiveActivityBar");

            Assert.NotNull(mainBody);
            Assert.NotNull(contentColumn);
            Assert.NotNull(leftNavigation);
            Assert.NotNull(activeActivityBar);

            Assert.IsNull(mainBody.GetComponent<HorizontalLayoutGroup>(), "MainBody must not drive LeftNavigation width through a HorizontalLayoutGroup.");
            Assert.IsNull(contentColumn.GetComponent<VerticalLayoutGroup>(), "ContentColumn must not drive ActiveActivityBar height through a VerticalLayoutGroup.");

            var leftRect = (RectTransform)leftNavigation.transform;
            Assert.AreEqual(leftRect.anchorMin.x, leftRect.anchorMax.x, "LeftNavigation width should be directly editable through RectTransform sizeDelta.x.");
            Assert.AreEqual(270f, leftRect.sizeDelta.x, 0.01f);

            var activityRect = (RectTransform)activeActivityBar.transform;
            Assert.AreEqual(activityRect.anchorMin.y, activityRect.anchorMax.y, "ActiveActivityBar height should be directly editable through RectTransform sizeDelta.y.");
            Assert.AreEqual(88f, activityRect.sizeDelta.y, 0.01f);
        }

        [Test]
        public void CombatScreenContainsFutureReadyCollapsibleSelectionLayout()
        {
            EditorSceneManager.OpenScene("Assets/Scenes/SampleScene.unity", OpenSceneMode.Single);

            var combatScreen = Object.FindObjectsByType<UIScreenReference>(FindObjectsInactive.Include)
                .FirstOrDefault(reference => reference.ScreenId == ScreenIds.Combat);

            Assert.NotNull(combatScreen);
            Assert.NotNull(FindDeepChild(combatScreen.transform, "[LAYOUT] CombatAuthoringLayout"));
            Assert.IsNull(FindDeepChild(combatScreen.transform, "[SCROLL] CombatMainScroll"), "Combat's permanent dashboard panels must stay visible in Edit Mode, not hidden inside a whole-screen ScrollRect.");
            Assert.NotNull(FindDeepChild(combatScreen.transform, "[SECTION] CombatSelectionSection"));
            Assert.NotNull(FindDeepChild(combatScreen.transform, "[STATE] CombatSelectionExpandedState"));
            Assert.NotNull(FindDeepChild(combatScreen.transform, "[STATE] CombatSelectionCollapsedState"));
            Assert.NotNull(FindDeepChild(combatScreen.transform, "[BUTTON] CollapseCombatSelectionButton"));
            Assert.NotNull(FindDeepChild(combatScreen.transform, "[BUTTON] ExpandCombatSelectionButton"));
            Assert.NotNull(FindDeepChild(combatScreen.transform, "[SECTION] SelectedEncounterSection"));
            Assert.NotNull(FindDeepChild(combatScreen.transform, "[SECTION] ActiveCombatPlayerSection"));
            Assert.NotNull(FindDeepChild(combatScreen.transform, "[SECTION] ActiveCombatEnemySection"));
            Assert.NotNull(FindDeepChild(combatScreen.transform, "[SECTION] TemporaryLootSection"));
            Assert.NotNull(FindDeepChild(combatScreen.transform, "[SECTION] DungeonProgressSection"));
            var combatLog = FindDeepChild(combatScreen.transform, "[SECTION] CombatLogSection");
            Assert.NotNull(combatLog);
            Assert.IsFalse(HasAncestor(combatLog, "[SCROLL] CombatMainScroll"), "CombatLogSection should be directly visible/editable in Edit Mode.");
        }

        [Test]
        public void InventoryScreenContainsEditableReferenceLayout()
        {
            EditorSceneManager.OpenScene("Assets/Scenes/SampleScene.unity", OpenSceneMode.Single);

            var inventoryScreen = Object.FindObjectsByType<UIScreenReference>(FindObjectsInactive.Include)
                .FirstOrDefault(reference => reference.ScreenId == ScreenIds.Inventory);

            Assert.NotNull(inventoryScreen);
            Assert.NotNull(FindDeepChild(inventoryScreen.transform, "[LAYOUT] InventoryAuthoringLayout"));
            Assert.IsNull(FindDeepChild(inventoryScreen.transform, "[SCROLL] InventoryMainScroll"), "Inventory's permanent dashboard layout must stay visible in Edit Mode; only the item grid should scroll.");
            Assert.NotNull(FindDeepChild(inventoryScreen.transform, "[CONTROLS] InventoryControls"));
            Assert.NotNull(FindDeepChild(inventoryScreen.transform, "[INPUT] InventorySearchInput"));
            Assert.NotNull(FindDeepChild(inventoryScreen.transform, "[BUTTON] AllFilter"));
            Assert.NotNull(FindDeepChild(inventoryScreen.transform, "[DROPDOWN] InventorySortDropdown"));
            Assert.NotNull(FindDeepChild(inventoryScreen.transform, "[LAYOUT] InventoryMainLayout"));
            Assert.NotNull(FindDeepChild(inventoryScreen.transform, "[PANEL] InventoryGridPanel"));
            Assert.NotNull(FindDeepChild(inventoryScreen.transform, "[SCROLL] InventoryGridScroll"));
            Assert.NotNull(FindDeepChild(inventoryScreen.transform, "[DYNAMIC CONTENT] InventoryItemContainer"));
            Assert.NotNull(FindDeepChild(inventoryScreen.transform, "[PANEL] ItemDetailsPanel"));
            Assert.NotNull(FindDeepChild(inventoryScreen.transform, "[CONTAINER] ItemStatsContainer"));
            Assert.NotNull(FindDeepChild(inventoryScreen.transform, "[CONTAINER] ItemSourcesContainer"));
            Assert.NotNull(FindDeepChild(inventoryScreen.transform, "[CONTAINER] UsedInUpgradeContainer"));
            Assert.NotNull(FindDeepChild(inventoryScreen.transform, "[ACTIONS] InventoryItemActions"));
            Assert.NotNull(FindDeepChild(inventoryScreen.transform, "[BUTTON] EquipItemButton"));
            Assert.NotNull(FindDeepChild(inventoryScreen.transform, "[BUTTON] DestroyItemButton"));
            Assert.NotNull(FindDeepChild(inventoryScreen.transform, "[PANEL] InventoryEmptyState"));
        }

        [Test]
        public void WoodcuttingScreenContainsEditableReferenceLayout()
        {
            EditorSceneManager.OpenScene("Assets/Scenes/SampleScene.unity", OpenSceneMode.Single);

            var woodcuttingScreen = Object.FindObjectsByType<UIScreenReference>(FindObjectsInactive.Include)
                .FirstOrDefault(reference => reference.ScreenId == ScreenIds.Woodcutting);

            Assert.NotNull(woodcuttingScreen);
            Assert.NotNull(FindDeepChild(woodcuttingScreen.transform, "[LAYOUT] WoodcuttingAuthoringLayout"));
            Assert.IsNull(FindDeepChild(woodcuttingScreen.transform, "[SCROLL] WoodcuttingMainScroll"), "Woodcutting's permanent dashboard panels must remain visible and editable before Play Mode; only the tree list should scroll.");
            Assert.NotNull(FindDeepChild(woodcuttingScreen.transform, "[LAYOUT] WoodcuttingMainArea"));
            Assert.NotNull(FindDeepChild(woodcuttingScreen.transform, "[SECTION] ActivitySelectionSection"));
            Assert.NotNull(FindDeepChild(woodcuttingScreen.transform, "[SCROLL] TreeSelectionScroll"));
            Assert.NotNull(FindDeepChild(woodcuttingScreen.transform, "[DYNAMIC CONTENT] TreeCardContainer"));
            Assert.NotNull(FindDeepChild(woodcuttingScreen.transform, "[SECTION] SelectedTreeDetailsSection"));
            Assert.NotNull(FindDeepChild(woodcuttingScreen.transform, "[SECTION] ActiveTargetSection"));
            Assert.NotNull(FindDeepChild(woodcuttingScreen.transform, "[BAR] TargetDurabilityBar"));
            Assert.NotNull(FindDeepChild(woodcuttingScreen.transform, "[BAR] ActionProgressBar"));
            Assert.NotNull(FindDeepChild(woodcuttingScreen.transform, "[CONTAINER] ThresholdContainer"));
            Assert.NotNull(FindDeepChild(woodcuttingScreen.transform, "[SECTION] WoodcuttingLoadoutSection"));
            Assert.NotNull(FindDeepChild(woodcuttingScreen.transform, "[PANEL] EquipmentPanel"));
            Assert.NotNull(FindDeepChild(woodcuttingScreen.transform, "[PANEL] GatheringPowerPanel"));
            Assert.NotNull(FindDeepChild(woodcuttingScreen.transform, "[PANEL] ActiveBonusesPanel"));
            Assert.NotNull(FindDeepChild(woodcuttingScreen.transform, "[SECTION] PotentialLootSection"));
            Assert.NotNull(FindDeepChild(woodcuttingScreen.transform, "[ACTIONS] WoodcuttingActionBar"));
            Assert.NotNull(FindDeepChild(woodcuttingScreen.transform, "[BUTTON] WoodcuttingToggleButton"));
            Assert.NotNull(FindDeepChild(woodcuttingScreen.transform, "[STATE] StartWoodcuttingState"));
            Assert.NotNull(FindDeepChild(woodcuttingScreen.transform, "[STATE] StopWoodcuttingState"));
            Assert.IsNull(FindDeepChild(woodcuttingScreen.transform, "[BUTTON] StartWoodcuttingButton"), "Start and Stop must share one future-ready toggle button.");
            Assert.IsNull(FindDeepChild(woodcuttingScreen.transform, "[BUTTON] StopWoodcuttingButton"), "Start and Stop must share one future-ready toggle button.");
        }

        private static Transform FindDeepChild(Transform root, string childName)
        {
            foreach (Transform child in root)
            {
                if (child.name == childName)
                {
                    return child;
                }

                var nested = FindDeepChild(child, childName);
                if (nested != null)
                {
                    return nested;
                }
            }

            return null;
        }

        private static bool HasAncestor(Transform transform, string ancestorName)
        {
            var current = transform.parent;
            while (current != null)
            {
                if (current.name == ancestorName)
                {
                    return true;
                }

                current = current.parent;
            }

            return false;
        }
    }
}
