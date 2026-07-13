using IdleGame.Core.Identifiers;
using UnityEngine;

namespace IdleGame.UI
{
    [CreateAssetMenu(menuName = "Idle Game/UI/Navigation Group", fileName = "NavigationGroup")]
    public sealed class NavigationGroupDefinition : ScriptableObject
    {
        [SerializeField] private string groupId = string.Empty;
        [SerializeField] private string displayName = string.Empty;
        [SerializeField] private int sortOrder;
        [SerializeField] private bool expandedByDefault = true;

        public string GroupId => groupId;
        public string DisplayName => string.IsNullOrWhiteSpace(displayName) ? groupId : displayName;
        public int SortOrder => sortOrder;
        public bool ExpandedByDefault => expandedByDefault;

        private void OnValidate()
        {
            if (!string.IsNullOrWhiteSpace(groupId) && !StableId.IsValid(groupId))
            {
                Debug.LogWarning($"{name} uses invalid navigation group id '{groupId}'. Use lower_snake_case.", this);
            }
        }

        public void ConfigureForEditor(string id, string title, int order, bool startsExpanded = true)
        {
            groupId = id;
            displayName = title;
            sortOrder = order;
            expandedByDefault = startsExpanded;
        }
    }
}
