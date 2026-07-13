using System;
using System.Collections.Generic;
using System.Linq;
using IdleGame.Core.Identifiers;
using UnityEngine;

namespace IdleGame.UI
{
    [CreateAssetMenu(menuName = "Idle Game/UI/Screen Catalog", fileName = "UIScreenCatalog")]
    public sealed class UIScreenCatalog : ScriptableObject
    {
        [SerializeField] private List<ScreenDefinition> screens = new();

        private readonly Dictionary<string, ScreenDefinition> lookup = new(StringComparer.Ordinal);

        public IReadOnlyList<ScreenDefinition> Screens => screens;

        public bool TryGetDefinition(string screenId, out ScreenDefinition definition)
        {
            EnsureLookup();
            return lookup.TryGetValue(screenId, out definition);
        }

        public ScreenDefinition GetDefinition(string screenId)
        {
            if (TryGetDefinition(screenId, out var definition))
            {
                return definition;
            }

            throw new KeyNotFoundException($"Screen '{screenId}' is not registered in {name}.");
        }

        public IReadOnlyList<ScreenDefinition> GetNavigationScreens()
        {
            return screens
                .Where(screen => screen != null && screen.VisibleInNavigation)
                .OrderBy(screen => screen.SortOrder)
                .ThenBy(screen => screen.DisplayName)
                .ToArray();
        }

        public bool ValidateCatalog(out string error)
        {
            var seen = new HashSet<string>(StringComparer.Ordinal);

            foreach (var screen in screens)
            {
                if (screen == null)
                {
                    error = $"{name} contains an empty screen entry.";
                    return false;
                }

                if (!StableId.IsValid(screen.ScreenId))
                {
                    error = $"{screen.name} has invalid screen id '{screen.ScreenId}'.";
                    return false;
                }

                if (!seen.Add(screen.ScreenId))
                {
                    error = $"{name} contains duplicate screen id '{screen.ScreenId}'.";
                    return false;
                }

                if (screen.ScreenPrefab == null)
                {
                    error = $"{screen.name} has no screen prefab.";
                    return false;
                }
            }

            error = string.Empty;
            return true;
        }

        public void ConfigureForEditor(IEnumerable<ScreenDefinition> definitions)
        {
            screens = definitions?.Where(definition => definition != null).ToList() ?? new List<ScreenDefinition>();
            lookup.Clear();
        }

        private void EnsureLookup()
        {
            if (lookup.Count == screens.Count(screen => screen != null))
            {
                return;
            }

            lookup.Clear();

            foreach (var screen in screens.Where(screen => screen != null))
            {
                lookup[screen.ScreenId] = screen;
            }
        }
    }
}
