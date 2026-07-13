using UnityEngine;

namespace IdleGame.UI
{
    [CreateAssetMenu(menuName = "Idle Game/UI/Navigation Entry", fileName = "NavigationEntry")]
    public sealed class NavigationEntryDefinition : ScriptableObject
    {
        [SerializeField] private NavigationGroupDefinition group;
        [SerializeField] private ScreenDefinition screen;
        [SerializeField] private string displayNameOverride = string.Empty;
        [SerializeField] private int sortOrder;
        [SerializeField] private bool visible = true;

        public NavigationGroupDefinition Group => group;
        public ScreenDefinition Screen => screen;
        public string DisplayName => string.IsNullOrWhiteSpace(displayNameOverride) && screen != null ? screen.DisplayName : displayNameOverride;
        public int SortOrder => sortOrder;
        public bool Visible => visible;

        public void ConfigureForEditor(
            NavigationGroupDefinition navigationGroup,
            ScreenDefinition screenDefinition,
            int order,
            string labelOverride = "",
            bool isVisible = true)
        {
            group = navigationGroup;
            screen = screenDefinition;
            displayNameOverride = labelOverride;
            sortOrder = order;
            visible = isVisible;
        }
    }
}
