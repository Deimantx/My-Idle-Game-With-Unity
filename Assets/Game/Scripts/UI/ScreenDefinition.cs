using IdleGame.Core.Identifiers;
using UnityEngine;

namespace IdleGame.UI
{
    [CreateAssetMenu(menuName = "Idle Game/UI/Screen Definition", fileName = "ScreenDefinition")]
    public sealed class ScreenDefinition : ScriptableObject
    {
        [SerializeField] private string screenId = string.Empty;
        [SerializeField] private string displayName = string.Empty;
        [SerializeField] private string subtitle = string.Empty;
        [SerializeField] private int sortOrder;
        [SerializeField] private bool visibleInNavigation = true;
        [SerializeField] private bool cacheInstance = true;

        public string ScreenId => screenId;
        public string DisplayName => string.IsNullOrWhiteSpace(displayName) ? screenId : displayName;
        public string Subtitle => subtitle;
        public int SortOrder => sortOrder;
        public bool VisibleInNavigation => visibleInNavigation;
        public bool CacheInstance => cacheInstance;

        private void OnValidate()
        {
            if (!string.IsNullOrWhiteSpace(screenId) && !StableId.IsValid(screenId))
            {
                Debug.LogWarning($"{name} uses invalid screen id '{screenId}'. Use lower_snake_case.", this);
            }
        }

        public void ConfigureForEditor(
            string id,
            string title,
            string description,
            int order,
            bool showInNavigation = true,
            bool keepInstanceCached = true)
        {
            screenId = id;
            displayName = title;
            subtitle = description;
            sortOrder = order;
            visibleInNavigation = showInNavigation;
            cacheInstance = keepInstanceCached;
        }
    }
}
