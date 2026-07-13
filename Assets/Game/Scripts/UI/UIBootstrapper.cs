using System;
using IdleGame.Core.Bootstrap;
using UnityEngine;

namespace IdleGame.UI
{
    public sealed class UIBootstrapper : MonoBehaviour, IGameService
    {
        [SerializeField] private PersistentUIShell shell;
        [SerializeField] private ScreenManager screenManager;
        [SerializeField] private string defaultScreenId = ScreenIds.Woodcutting;

        public int InitializationOrder => 300;

        public void InitializeService()
        {
            if (shell == null)
            {
                throw new InvalidOperationException($"{nameof(UIBootstrapper)} needs a persistent UI shell.");
            }

            if (!shell.ValidateShell(out var shellError))
            {
                throw new InvalidOperationException(shellError);
            }

            if (screenManager == null)
            {
                throw new InvalidOperationException($"{nameof(UIBootstrapper)} needs a {nameof(ScreenManager)}.");
            }

            if (string.IsNullOrWhiteSpace(screenManager.CurrentScreenId))
            {
                screenManager.OpenScreen(defaultScreenId, false);
            }
        }

        public void ConfigureForEditor(PersistentUIShell shellReference, ScreenManager manager, string defaultScreen)
        {
            shell = shellReference;
            screenManager = manager;
            defaultScreenId = defaultScreen;
        }
    }
}
