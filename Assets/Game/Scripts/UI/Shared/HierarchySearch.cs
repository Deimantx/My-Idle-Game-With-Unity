using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IdleGame.UI.Shared
{
    public static class HierarchySearch
    {
        public static Transform FindDeep(Transform root, string name)
        {
            if (root == null)
            {
                return null;
            }

            if (root.name == name)
            {
                return root;
            }

            foreach (Transform child in root)
            {
                var result = FindDeep(child, name);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        public static TMP_Text FindText(Transform root, string name)
        {
            return FindDeep(root, name)?.GetComponent<TMP_Text>();
        }

        public static Button FindButton(Transform root, string name)
        {
            return FindDeep(root, name)?.GetComponent<Button>();
        }

        public static RuntimeFillBar FindOrAddFillBar(Transform root, string barName)
        {
            var bar = FindDeep(root, barName);
            if (bar == null)
            {
                return null;
            }

            var runtimeBar = bar.GetComponent<RuntimeFillBar>();
            if (runtimeBar == null)
            {
                runtimeBar = bar.gameObject.AddComponent<RuntimeFillBar>();
            }

            var fill = FindDeep(bar, "Fill") as RectTransform;
            var value = FindText(bar, "[TEXT] ValueText");
            runtimeBar.ConfigureForEditor(fill, value);
            return runtimeBar;
        }
    }
}
