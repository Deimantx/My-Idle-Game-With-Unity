using IdleGame.Core.Identifiers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IdleGame.UI
{
    [RequireComponent(typeof(Button))]
    public sealed class NavigationButtonBinding : MonoBehaviour
    {
        [SerializeField] private string screenId = string.Empty;
        [SerializeField] private Button button;
        [SerializeField] private Image background;
        [SerializeField] private Image selectedMarker;
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text label;
        [SerializeField] private Image lockIcon;
        [SerializeField] private GameObject notificationBadge;
        [SerializeField] private TMP_Text badgeText;
        [SerializeField] private Color normalBackground = new(0.09f, 0.13f, 0.17f, 0.94f);
        [SerializeField] private Color selectedBackground = new(0.28f, 0.19f, 0.08f, 0.98f);
        [SerializeField] private Color disabledBackground = new(0.05f, 0.07f, 0.09f, 0.78f);
        [SerializeField] private Color normalText = new(0.78f, 0.70f, 0.58f, 1f);
        [SerializeField] private Color selectedText = new(0.96f, 0.77f, 0.38f, 1f);
        [SerializeField] private Color disabledText = new(0.32f, 0.34f, 0.36f, 1f);

        private NavigationManager navigationManager;

        public string ScreenId => screenId;
        public Button Button => button;
        public TMP_Text Label => label;
        public Image SelectedMarker => selectedMarker;
        public bool IsSelected { get; private set; }

        private void Reset()
        {
            ResolveReferences();
        }

        private void Awake()
        {
            ResolveReferences();
        }

        private void OnDestroy()
        {
            if (button != null)
            {
                button.onClick.RemoveListener(HandleClick);
            }
        }

        private void OnValidate()
        {
            if (!string.IsNullOrWhiteSpace(screenId) && !StableId.IsValid(screenId))
            {
                Debug.LogWarning($"{name} uses invalid target screen id '{screenId}'. Use lower_snake_case.", this);
            }
        }

        public void Bind(NavigationManager manager)
        {
            ResolveReferences();
            navigationManager = manager;

            if (button != null)
            {
                button.onClick.RemoveListener(HandleClick);
                button.onClick.AddListener(HandleClick);
            }
        }

        public void SetSelected(bool selected)
        {
            IsSelected = selected;

            if (selectedMarker != null)
            {
                selectedMarker.gameObject.SetActive(selected);
            }

            RefreshVisuals();
        }

        public void ConfigureForEditor(
            string targetScreenId,
            Button buttonReference,
            Image backgroundReference,
            Image selectedMarkerReference,
            Image iconReference,
            TMP_Text labelReference,
            Image lockIconReference,
            GameObject notificationBadgeReference,
            TMP_Text badgeTextReference)
        {
            screenId = targetScreenId;
            button = buttonReference;
            background = backgroundReference;
            selectedMarker = selectedMarkerReference;
            icon = iconReference;
            label = labelReference;
            lockIcon = lockIconReference;
            notificationBadge = notificationBadgeReference;
            badgeText = badgeTextReference;
            SetSelected(false);
        }

        private void HandleClick()
        {
            if (navigationManager != null && button != null && button.interactable)
            {
                navigationManager.OpenScreen(screenId);
            }
        }

        private void ResolveReferences()
        {
            if (button == null)
            {
                button = GetComponent<Button>();
            }

            if (background == null)
            {
                background = GetComponent<Image>();
            }

            if (label == null)
            {
                label = GetComponentInChildren<TMP_Text>(true);
            }
        }

        private void RefreshVisuals()
        {
            var isInteractable = button == null || button.interactable;

            if (background != null)
            {
                background.color = !isInteractable ? disabledBackground : IsSelected ? selectedBackground : normalBackground;
            }

            if (label != null)
            {
                label.color = !isInteractable ? disabledText : IsSelected ? selectedText : normalText;
            }

            if (lockIcon != null)
            {
                lockIcon.gameObject.SetActive(!isInteractable);
            }
        }
    }
}
