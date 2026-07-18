using UnityEngine;

namespace IdleGame.UI.Tooltips
{
    public sealed class StaticTooltipProvider : MonoBehaviour, ITooltipDataProvider
    {
        [SerializeField] private string displayName = string.Empty;
        [SerializeField] private string category = string.Empty;
        [SerializeField, TextArea] private string description = string.Empty;
        [SerializeField] private Sprite icon;

        public void ConfigureForEditor(string tooltipName, string tooltipCategory, string tooltipDescription, Sprite tooltipIcon = null)
        {
            displayName = tooltipName;
            category = tooltipCategory;
            description = tooltipDescription;
            icon = tooltipIcon;
        }

        public TooltipData BuildTooltipData()
        {
            return new TooltipData
            {
                DisplayName = displayName,
                Category = category,
                Description = description,
                Icon = icon,
                SourceId = name
            };
        }
    }
}
