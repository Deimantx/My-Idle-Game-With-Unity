using System.Collections;
using System.Linq;
using IdleGame.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace IdleGame.Tests.PlayMode
{
    public sealed class NavigationFlowTests
    {
        private static readonly string[] ExpectedScreenIds =
        {
            ScreenIds.Woodcutting,
            ScreenIds.Combat,
            ScreenIds.Inventory,
            ScreenIds.Equipment
        };

        [UnityTest]
        public IEnumerator NavigationButtonsOpenEveryScreenAndKeepOneMainScreenActive()
        {
            yield return LoadPrimaryScene();

            var screenManager = FindSingle<ScreenManager>();
            var navigationManager = FindSingle<NavigationManager>();
            var initialButtonCount = FindObjects<NavigationButtonBinding>().Length;
            var initialScreenCount = FindObjects<UIScreenReference>().Length;

            Assert.AreEqual(ScreenIds.Woodcutting, screenManager.CurrentScreenId);
            Assert.AreEqual(1, screenManager.CountActiveMainScreens());

            foreach (var screenId in ExpectedScreenIds)
            {
                var button = navigationManager.GetButtonForScreen(screenId);
                Assert.NotNull(button, "Missing navigation button for " + screenId);

                button.Button.onClick.Invoke();
                yield return null;

                Assert.AreEqual(screenId, screenManager.CurrentScreenId);
                Assert.AreEqual(1, screenManager.CountActiveMainScreens());
                Assert.IsTrue(button.IsSelected);
                Assert.AreEqual(initialButtonCount, FindObjects<NavigationButtonBinding>().Length, "Navigation buttons should be scene-authored, not duplicated at runtime.");
                Assert.AreEqual(initialScreenCount, FindObjects<UIScreenReference>().Length, "Main screens should be scene-authored, not instantiated at runtime.");
            }
        }

        [UnityTest]
        public IEnumerator BackButtonReturnsToPreviousScreen()
        {
            yield return LoadPrimaryScene();

            var screenManager = FindSingle<ScreenManager>();
            var navigationManager = FindSingle<NavigationManager>();

            navigationManager.GetButtonForScreen(ScreenIds.Combat).Button.onClick.Invoke();
            yield return null;
            navigationManager.GetButtonForScreen(ScreenIds.Inventory).Button.onClick.Invoke();
            yield return null;

            Assert.AreEqual(ScreenIds.Inventory, screenManager.CurrentScreenId);
            Assert.IsTrue(navigationManager.BackButton.interactable);

            navigationManager.BackButton.onClick.Invoke();
            yield return null;

            Assert.AreEqual(ScreenIds.Combat, screenManager.CurrentScreenId);
            Assert.AreEqual(1, screenManager.CountActiveMainScreens());
            Assert.IsTrue(navigationManager.GetButtonForScreen(ScreenIds.Combat).IsSelected);
        }

        private static IEnumerator LoadPrimaryScene()
        {
            yield return SceneManager.LoadSceneAsync("SampleScene", LoadSceneMode.Single);
            yield return null;
            yield return new WaitUntil(() => FindObjects<ScreenManager>().Any(manager => !string.IsNullOrEmpty(manager.CurrentScreenId)));
        }

        private static T FindSingle<T>() where T : Object
        {
            var results = FindObjects<T>();
            Assert.AreEqual(1, results.Length, "Expected exactly one " + typeof(T).Name + ".");
            return results[0];
        }

        private static T[] FindObjects<T>() where T : Object
        {
            return Object.FindObjectsByType<T>(FindObjectsInactive.Include);
        }
    }
}
