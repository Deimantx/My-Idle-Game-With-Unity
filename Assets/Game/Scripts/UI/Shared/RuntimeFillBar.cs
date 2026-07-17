using TMPro;
using UnityEngine;

namespace IdleGame.UI.Shared
{
    public sealed class RuntimeFillBar : MonoBehaviour
    {
        [SerializeField] private RectTransform fill;
        [SerializeField] private TMP_Text valueText;

        public TMP_Text ValueText => valueText;

        public void SetValue(float value01, string label)
        {
            if (fill != null)
            {
                fill.anchorMin = new Vector2(0f, fill.anchorMin.y);
                fill.anchorMax = new Vector2(Mathf.Clamp01(value01), fill.anchorMax.y);
                fill.offsetMin = new Vector2(0f, fill.offsetMin.y);
                fill.offsetMax = new Vector2(0f, fill.offsetMax.y);
                fill.localScale = new Vector3(1f, fill.localScale.y, fill.localScale.z);
            }

            if (valueText != null)
            {
                valueText.text = label;
            }
        }

        public void ConfigureForEditor(RectTransform fillReference, TMP_Text textReference)
        {
            fill = fillReference;
            valueText = textReference;
        }
    }
}
