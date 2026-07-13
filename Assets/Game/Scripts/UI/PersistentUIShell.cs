using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace IdleGame.UI
{
    public sealed class PersistentUIShell : MonoBehaviour
    {
        [SerializeField] private RectTransform topBar;
        [SerializeField] private RectTransform leftNavigation;
        [SerializeField] private RectTransform activeActivityBar;
        [SerializeField] private RectTransform screenContainer;
        [SerializeField] private RectTransform dropdownLayer;
        [SerializeField] private RectTransform tooltipLayer;
        [SerializeField] private RectTransform notificationLayer;
        [SerializeField] private RectTransform popupLayer;
        [SerializeField] private RectTransform modalLayer;
        [SerializeField] private RectTransform loadingLayer;

        public RectTransform TopBar => topBar;
        public RectTransform LeftNavigation => leftNavigation;
        public RectTransform ActiveActivityBar => activeActivityBar;
        public RectTransform ScreenContainer => screenContainer;
        public RectTransform DropdownLayer => dropdownLayer;
        public RectTransform TooltipLayer => tooltipLayer;
        public RectTransform NotificationLayer => notificationLayer;
        public RectTransform PopupLayer => popupLayer;
        public RectTransform ModalLayer => modalLayer;
        public RectTransform LoadingLayer => loadingLayer;

        public bool ValidateShell(out string error)
        {
            var missing = new List<string>();
            AddMissing(missing, topBar, nameof(topBar));
            AddMissing(missing, leftNavigation, nameof(leftNavigation));
            AddMissing(missing, activeActivityBar, nameof(activeActivityBar));
            AddMissing(missing, screenContainer, nameof(screenContainer));
            AddMissing(missing, dropdownLayer, nameof(dropdownLayer));
            AddMissing(missing, tooltipLayer, nameof(tooltipLayer));
            AddMissing(missing, notificationLayer, nameof(notificationLayer));
            AddMissing(missing, popupLayer, nameof(popupLayer));
            AddMissing(missing, modalLayer, nameof(modalLayer));
            AddMissing(missing, loadingLayer, nameof(loadingLayer));

            if (missing.Count == 0)
            {
                error = string.Empty;
                return true;
            }

            var builder = new StringBuilder();
            builder.Append(name).Append(" is missing shell references: ");
            builder.Append(string.Join(", ", missing));
            error = builder.ToString();
            return false;
        }

        public void ConfigureForEditor(
            RectTransform topBarRoot,
            RectTransform leftNavigationRoot,
            RectTransform activeActivityBarRoot,
            RectTransform screenContainerRoot,
            RectTransform dropdownLayerRoot,
            RectTransform tooltipLayerRoot,
            RectTransform notificationLayerRoot,
            RectTransform popupLayerRoot,
            RectTransform modalLayerRoot,
            RectTransform loadingLayerRoot)
        {
            topBar = topBarRoot;
            leftNavigation = leftNavigationRoot;
            activeActivityBar = activeActivityBarRoot;
            screenContainer = screenContainerRoot;
            dropdownLayer = dropdownLayerRoot;
            tooltipLayer = tooltipLayerRoot;
            notificationLayer = notificationLayerRoot;
            popupLayer = popupLayerRoot;
            modalLayer = modalLayerRoot;
            loadingLayer = loadingLayerRoot;
        }

        private static void AddMissing(List<string> missing, Object reference, string fieldName)
        {
            if (reference == null)
            {
                missing.Add(fieldName);
            }
        }
    }
}
