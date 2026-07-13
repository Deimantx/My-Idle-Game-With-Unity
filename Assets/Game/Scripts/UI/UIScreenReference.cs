using IdleGame.Core.Identifiers;
using UnityEngine;

namespace IdleGame.UI
{
    public sealed class UIScreenReference : MonoBehaviour
    {
        [SerializeField] private string screenId = string.Empty;
        [SerializeField] private UIScreenController screenController;
        [SerializeField] private GameObject defaultSelectedObject;
        [SerializeField] private bool preserveState = true;
        [SerializeField] private bool allowBackNavigation = true;

        public string ScreenId => screenId;
        public UIScreenController ScreenController => screenController;
        public GameObject Root => gameObject;
        public GameObject DefaultSelectedObject => defaultSelectedObject;
        public bool PreserveState => preserveState;
        public bool AllowBackNavigation => allowBackNavigation;

        private void Reset()
        {
            if (screenController == null)
            {
                screenController = GetComponent<UIScreenController>();
            }
        }

        private void OnValidate()
        {
            if (screenController == null)
            {
                screenController = GetComponent<UIScreenController>();
            }

            if (!string.IsNullOrWhiteSpace(screenId) && !StableId.IsValid(screenId))
            {
                Debug.LogWarning($"{name} uses invalid screen id '{screenId}'. Use lower_snake_case.", this);
            }
        }

        public void Show()
        {
            if (screenController != null)
            {
                screenController.Show();
            }
            else
            {
                gameObject.SetActive(true);
            }
        }

        public void Hide()
        {
            if (screenController != null)
            {
                screenController.Hide();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        public void ConfigureForEditor(string id, UIScreenController controller, GameObject selectedObject = null, bool shouldPreserveState = true, bool canGoBack = true)
        {
            screenId = id;
            screenController = controller;
            defaultSelectedObject = selectedObject;
            preserveState = shouldPreserveState;
            allowBackNavigation = canGoBack;
        }
    }
}
