using System.Collections.Generic;
using System.Linq;
using IdleGame.Core.Bootstrap;
using UnityEngine;
using UnityEngine.UI;

namespace IdleGame.UI
{
    public sealed class NavigationManager : MonoBehaviour, IGameService
    {
        [SerializeField] private ScreenManager screenManager;
        [SerializeField] private List<NavigationButtonBinding> navigationButtons = new();
        [SerializeField] private Button backButton;

        private bool initialized;

        public int InitializationOrder => 200;
        public IReadOnlyList<NavigationButtonBinding> Buttons => navigationButtons;
        public Button BackButton => backButton;

        public void InitializeService()
        {
            if (initialized)
            {
                return;
            }

            if (screenManager == null)
            {
                throw new System.InvalidOperationException($"{nameof(NavigationManager)} needs a {nameof(ScreenManager)}.");
            }

            navigationButtons = navigationButtons.Where(button => button != null).ToList();

            foreach (var button in navigationButtons)
            {
                button.Bind(this);
            }

            screenManager.ScreenChanged += HandleScreenChanged;

            if (backButton != null)
            {
                backButton.onClick.RemoveListener(HandleBackClicked);
                backButton.onClick.AddListener(HandleBackClicked);
            }

            RefreshSelectedState(screenManager.CurrentScreenId);
            initialized = true;
        }

        private void OnDestroy()
        {
            if (screenManager != null)
            {
                screenManager.ScreenChanged -= HandleScreenChanged;
            }

            if (backButton != null)
            {
                backButton.onClick.RemoveListener(HandleBackClicked);
            }
        }

        public void ConfigureForEditor(ScreenManager manager, IEnumerable<NavigationButtonBinding> buttons, Button back)
        {
            screenManager = manager;
            navigationButtons = buttons?.Where(button => button != null).ToList() ?? new List<NavigationButtonBinding>();
            backButton = back;
            initialized = false;
        }

        public bool OpenScreen(string screenId)
        {
            return screenManager != null && screenManager.OpenScreen(screenId);
        }

        public NavigationButtonBinding GetButtonForScreen(string screenId)
        {
            return navigationButtons.Find(button => button != null && button.ScreenId == screenId);
        }

        private void HandleScreenChanged(string screenId)
        {
            RefreshSelectedState(screenId);
        }

        private void RefreshSelectedState(string screenId)
        {
            foreach (var button in navigationButtons)
            {
                button.SetSelected(button.ScreenId == screenId);
            }

            if (backButton != null)
            {
                backButton.interactable = screenManager != null && screenManager.CanGoBack;
            }
        }

        private void HandleBackClicked()
        {
            screenManager?.GoBack();
        }
    }
}
