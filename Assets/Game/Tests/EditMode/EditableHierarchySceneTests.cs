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
    }
}
