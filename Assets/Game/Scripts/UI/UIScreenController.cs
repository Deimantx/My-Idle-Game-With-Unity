using TMPro;
using UnityEngine;

namespace IdleGame.UI
{
    public class UIScreenController : MonoBehaviour
    {
        [SerializeField] private string screenId = string.Empty;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text subtitleText;

        public string ScreenId => screenId;
        public bool IsOpen { get; private set; }

        public void Initialize(string id)
        {
            screenId = id;
        }

        public void Show()
        {
            IsOpen = true;
            gameObject.SetActive(true);
            OnOpened();
        }

        public void Hide()
        {
            IsOpen = false;
            gameObject.SetActive(false);
            OnClosed();
        }

        protected virtual void OnOpened()
        {
        }

        protected virtual void OnClosed()
        {
        }

        public void ConfigureForEditor(string id, TMP_Text title, TMP_Text subtitle)
        {
            screenId = id;
            titleText = title;
            subtitleText = subtitle;

            if (titleText != null)
            {
                titleText.text = string.IsNullOrWhiteSpace(id) ? "Screen" : id;
            }

            if (subtitleText != null)
            {
                subtitleText.text = "Prototype placeholder";
            }
        }
    }
}
