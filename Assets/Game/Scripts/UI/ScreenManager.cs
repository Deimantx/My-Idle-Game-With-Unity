using System;
using System.Collections.Generic;
using System.Linq;
using IdleGame.Core.Bootstrap;
using IdleGame.Core.Identifiers;
using UnityEngine;

namespace IdleGame.UI
{
    public sealed class ScreenManager : MonoBehaviour, IGameService
    {
        [SerializeField] private UIScreenCatalog screenCatalog;
        [SerializeField] private RectTransform screenContainer;
        [SerializeField] private string defaultScreenId = ScreenIds.Woodcutting;
        [SerializeField] private List<UIScreenReference> screens = new();

        private readonly Dictionary<string, UIScreenReference> screenLookup = new(StringComparer.Ordinal);
        private readonly Stack<string> history = new();
        private UIScreenReference currentScreen;
        private bool initialized;

        public event Action<string> ScreenChanged;

        public int InitializationOrder => 100;
        public string CurrentScreenId { get; private set; } = string.Empty;
        public bool CanGoBack => history.Count > 0;
        public UIScreenCatalog ScreenCatalog => screenCatalog;
        public RectTransform ScreenContainer => screenContainer;
        public IReadOnlyList<UIScreenReference> Screens => screens;

        public void InitializeService()
        {
            if (initialized)
            {
                return;
            }

            if (screenContainer == null)
            {
                throw new InvalidOperationException($"{nameof(ScreenManager)} needs a screen container.");
            }

            BuildLookup();
            HideAllScreens();
            initialized = true;
        }

        public bool OpenScreen(string screenId, bool addToHistory = true)
        {
            if (!initialized)
            {
                InitializeService();
            }

            StableId.ThrowIfInvalid(screenId, nameof(screenId));

            if (CurrentScreenId == screenId)
            {
                ScreenChanged?.Invoke(CurrentScreenId);
                return false;
            }

            if (screenLookup.Count == 0 || !screenLookup.ContainsKey(screenId))
            {
                BuildLookup();
            }

            if (!screenLookup.TryGetValue(screenId, out var requestedScreen) || requestedScreen == null)
            {
                throw new KeyNotFoundException($"Screen '{screenId}' is not assigned to {nameof(ScreenManager)}.");
            }

            if (currentScreen != null && addToHistory && requestedScreen.AllowBackNavigation && !string.IsNullOrEmpty(CurrentScreenId))
            {
                history.Push(CurrentScreenId);
            }

            currentScreen?.Hide();

            currentScreen = requestedScreen;
            CurrentScreenId = currentScreen.ScreenId;
            SetOnlyCurrentActive();
            currentScreen.Show();
            ScreenChanged?.Invoke(CurrentScreenId);
            return true;
        }

        public bool OpenDefaultScreen()
        {
            return OpenScreen(defaultScreenId, false);
        }

        public bool GoBack()
        {
            if (!initialized)
            {
                InitializeService();
            }

            while (history.Count > 0)
            {
                var previousId = history.Pop();

                if (previousId != CurrentScreenId && screenLookup.ContainsKey(previousId))
                {
                    return OpenScreen(previousId, false);
                }
            }

            ScreenChanged?.Invoke(CurrentScreenId);
            return false;
        }

        public int CountActiveMainScreens()
        {
            return screens.Count(screen => screen != null && screen.Root.activeSelf);
        }

        public UIScreenReference GetScreenReference(string screenId)
        {
            if (!initialized)
            {
                InitializeService();
            }

            if (screenLookup.Count == 0 || !screenLookup.ContainsKey(screenId))
            {
                BuildLookup();
            }

            return screenLookup.TryGetValue(screenId, out var screenReference) ? screenReference : null;
        }

        public void ConfigureForEditor(UIScreenCatalog catalog, RectTransform container, IEnumerable<UIScreenReference> sceneScreens, string defaultScreen)
        {
            screenCatalog = catalog;
            screenContainer = container;
            screens = sceneScreens?.Where(screen => screen != null).ToList() ?? new List<UIScreenReference>();
            defaultScreenId = defaultScreen;
            screenLookup.Clear();
            initialized = false;
        }

        private void BuildLookup()
        {
            screenLookup.Clear();

            foreach (var screen in screens)
            {
                if (screen == null)
                {
                    throw new InvalidOperationException($"{nameof(ScreenManager)} has an empty screen reference.");
                }

                if (!StableId.IsValid(screen.ScreenId))
                {
                    throw new InvalidOperationException($"{screen.name} has invalid screen id '{screen.ScreenId}'.");
                }

                if (!screenLookup.TryAdd(screen.ScreenId, screen))
                {
                    throw new InvalidOperationException($"{nameof(ScreenManager)} has duplicate screen id '{screen.ScreenId}'.");
                }
            }

            if (!screenLookup.ContainsKey(defaultScreenId))
            {
                throw new InvalidOperationException($"{nameof(ScreenManager)} default screen '{defaultScreenId}' is not assigned.");
            }
        }

        private void HideAllScreens()
        {
            foreach (var screen in screens)
            {
                if (screen != null)
                {
                    screen.Hide();
                }
            }

            currentScreen = null;
            CurrentScreenId = string.Empty;
        }

        private void SetOnlyCurrentActive()
        {
            foreach (var screen in screens)
            {
                if (screen == null)
                {
                    continue;
                }

                if (screen == currentScreen)
                {
                    screen.Root.SetActive(true);
                }
                else
                {
                    screen.Hide();
                }
            }
        }
    }
}
