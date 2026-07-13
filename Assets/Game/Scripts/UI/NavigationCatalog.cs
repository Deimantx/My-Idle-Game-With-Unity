using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IdleGame.UI
{
    [CreateAssetMenu(menuName = "Idle Game/UI/Navigation Catalog", fileName = "NavigationCatalog")]
    public sealed class NavigationCatalog : ScriptableObject
    {
        [SerializeField] private List<NavigationGroupDefinition> groups = new();
        [SerializeField] private List<NavigationEntryDefinition> entries = new();

        public IReadOnlyList<NavigationGroupDefinition> Groups => groups;
        public IReadOnlyList<NavigationEntryDefinition> Entries => entries;

        public IEnumerable<NavigationGroupDefinition> GetGroups()
        {
            return groups
                .Where(group => group != null)
                .OrderBy(group => group.SortOrder)
                .ThenBy(group => group.DisplayName);
        }

        public IEnumerable<NavigationEntryDefinition> GetEntries(NavigationGroupDefinition group)
        {
            return entries
                .Where(entry => entry != null && entry.Visible && entry.Group == group && entry.Screen != null)
                .OrderBy(entry => entry.SortOrder)
                .ThenBy(entry => entry.DisplayName);
        }

        public bool ValidateCatalog(out string error)
        {
            var groupIds = new HashSet<string>(StringComparer.Ordinal);
            var screenIds = new HashSet<string>(StringComparer.Ordinal);

            foreach (var group in groups)
            {
                if (group == null)
                {
                    error = $"{name} contains an empty navigation group.";
                    return false;
                }

                if (!groupIds.Add(group.GroupId))
                {
                    error = $"{name} contains duplicate navigation group '{group.GroupId}'.";
                    return false;
                }
            }

            foreach (var entry in entries)
            {
                if (entry == null || entry.Group == null || entry.Screen == null)
                {
                    error = $"{name} contains an incomplete navigation entry.";
                    return false;
                }

                if (!screenIds.Add(entry.Screen.ScreenId))
                {
                    error = $"{name} contains duplicate navigation entry for '{entry.Screen.ScreenId}'.";
                    return false;
                }
            }

            error = string.Empty;
            return true;
        }

        public void ConfigureForEditor(IEnumerable<NavigationGroupDefinition> navigationGroups, IEnumerable<NavigationEntryDefinition> navigationEntries)
        {
            groups = navigationGroups?.Where(group => group != null).ToList() ?? new List<NavigationGroupDefinition>();
            entries = navigationEntries?.Where(entry => entry != null).ToList() ?? new List<NavigationEntryDefinition>();
        }
    }
}
