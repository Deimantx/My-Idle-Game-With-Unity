using IdleGame.UI.Tooltips;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace IdleGame.Editor
{
    public static class TooltipSceneBuilder
    {
        private static readonly Color PanelBackground = new(0.045f, 0.055f, 0.075f, 0.98f);
        private static readonly Color Border = new(0.75f, 0.58f, 0.25f, 0.95f);
        private static readonly Color Text = new(0.90f, 0.84f, 0.74f);
        private static readonly Color Muted = new(0.62f, 0.66f, 0.72f);
        private static readonly Color Gold = new(0.94f, 0.78f, 0.40f);

        [MenuItem("Tools/Idle Game/Configure Global Tooltip")]
        public static void ConfigureGlobalTooltip()
        {
            var canvasObject = GameObject.Find("[UI] MainCanvas");
            var tooltipLayer = GameObject.Find("[OVERLAY] TooltipLayer");
            if (canvasObject == null || tooltipLayer == null)
            {
                Debug.LogWarning("TooltipSceneBuilder could not find [UI] MainCanvas or [OVERLAY] TooltipLayer.");
                return;
            }

            Undo.RegisterFullObjectHierarchyUndo(tooltipLayer, "Configure Global Tooltip");
            var canvas = canvasObject.GetComponent<Canvas>();
            var canvasRect = canvasObject.transform as RectTransform;
            var layerRect = tooltipLayer.transform as RectTransform;
            SetStretch(layerRect);
            var layerGroup = tooltipLayer.GetComponent<CanvasGroup>() ?? tooltipLayer.AddComponent<CanvasGroup>();
            layerGroup.alpha = 1f;
            layerGroup.interactable = false;
            layerGroup.blocksRaycasts = false;

            var panel = FindOrCreatePanel(tooltipLayer.transform);
            ConfigurePanel(panel);
            RemoveGeneratedTooltipChildren(panel.transform);
            var header = CreateHeader(panel.transform, out var iconImage, out var nameText, out var categoryText);
            header.transform.SetAsFirstSibling();
            CreateDivider(panel.transform, "[DIVIDER] TooltipHeaderDivider");
            var bodyContent = CreateBodyScroll(panel.transform);
            var descriptionText = CreateText(bodyContent, "[TEXT] TooltipDescription", "Description", 13f, Text, TextAlignmentOptions.TopLeft, 0f);
            var effects = CreateSection(bodyContent, "[GROUP] Effects", "EFFECTS");
            var primary = CreateSection(bodyContent, "[GROUP] PrimaryStats", "STATISTICS");
            var secondary = CreateSection(bodyContent, "[GROUP] SecondaryStats", "DETAILS");
            var requirements = CreateSection(bodyContent, "[GROUP] Requirements", "REQUIREMENTS");
            var comparison = CreateSection(bodyContent, "[GROUP] Comparison", "COMPARISON");
            var footer = CreateFooterSection(bodyContent);

            DisableRaycasts(panel);
            var manager = tooltipLayer.GetComponent<TooltipManager>() ?? tooltipLayer.AddComponent<TooltipManager>();
            manager.ConfigureForEditor(
                canvas,
                canvasRect,
                panel.transform as RectTransform,
                panel.GetComponent<CanvasGroup>(),
                iconImage,
                nameText,
                categoryText,
                descriptionText,
                primary.Group,
                primary.Rows,
                secondary.Group,
                secondary.Rows,
                effects.Group,
                effects.Rows,
                requirements.Group,
                requirements.Rows,
                comparison.Group,
                comparison.Rows,
                footer.Group,
                footer.Body);

            panel.SetActive(false);
            EditorUtility.SetDirty(tooltipLayer);
            EditorUtility.SetDirty(panel);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        private static GameObject FindOrCreatePanel(Transform tooltipLayer)
        {
            var panel = tooltipLayer.Find("[PANEL] GlobalTooltip")?.gameObject;
            if (panel != null)
            {
                return panel;
            }

            panel = tooltipLayer.Find("[TOOLTIP] SharedTooltip")?.gameObject;
            if (panel == null)
            {
                panel = new GameObject("[PANEL] GlobalTooltip", typeof(RectTransform));
                panel.transform.SetParent(tooltipLayer, false);
            }

            panel.name = "[PANEL] GlobalTooltip";
            return panel;
        }

        private static void ConfigurePanel(GameObject panel)
        {
            var rect = panel.transform as RectTransform;
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0f, 1f);
            rect.sizeDelta = new Vector2(340f, 0f);
            rect.anchoredPosition = Vector2.zero;

            var image = panel.GetComponent<Image>();
            if (image == null)
            {
                image = panel.AddComponent<Image>();
            }

            image.color = PanelBackground;
            image.raycastTarget = false;
            var outline = panel.GetComponent<Outline>();
            if (outline == null)
            {
                outline = panel.AddComponent<Outline>();
            }

            outline.effectColor = Border;
            outline.effectDistance = new Vector2(1f, -1f);
            outline.useGraphicAlpha = false;
            var canvasGroup = panel.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = panel.AddComponent<CanvasGroup>();
            }

            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            var layout = panel.GetComponent<VerticalLayoutGroup>();
            if (layout == null)
            {
                layout = panel.AddComponent<VerticalLayoutGroup>();
            }

            layout.padding = new RectOffset(12, 12, 10, 10);
            layout.spacing = 7f;
            layout.childAlignment = TextAnchor.UpperLeft;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            var fitter = panel.GetComponent<ContentSizeFitter>();
            if (fitter == null)
            {
                fitter = panel.AddComponent<ContentSizeFitter>();
            }

            fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            var layoutElement = panel.GetComponent<LayoutElement>();
            if (layoutElement == null)
            {
                layoutElement = panel.AddComponent<LayoutElement>();
            }

            layoutElement.preferredWidth = 340f;
            layoutElement.flexibleWidth = 0f;
            layoutElement.minWidth = 260f;
            layoutElement.flexibleHeight = 0f;
        }

        private static GameObject CreateHeader(Transform parent, out Image iconImage, out TMP_Text nameText, out TMP_Text categoryText)
        {
            var header = FindOrCreate("[HEADER] TooltipHeader", parent);
            var layout = header.GetComponent<HorizontalLayoutGroup>() ?? header.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = 10f;
            layout.childAlignment = TextAnchor.UpperLeft;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;
            SetLayout(header, 52f, -1f);

            var icon = FindOrCreate("[IMAGE] TooltipIcon", header.transform);
            var iconRect = icon.transform as RectTransform;
            iconRect.sizeDelta = new Vector2(52f, 52f);
            iconImage = icon.GetComponent<Image>() ?? icon.AddComponent<Image>();
            iconImage.color = Color.white;
            iconImage.preserveAspect = true;
            iconImage.raycastTarget = false;
            var iconElement = icon.GetComponent<LayoutElement>() ?? icon.AddComponent<LayoutElement>();
            iconElement.minWidth = 52f;
            iconElement.preferredWidth = 52f;
            iconElement.minHeight = 52f;
            iconElement.preferredHeight = 52f;
            iconElement.flexibleWidth = 0f;
            iconElement.flexibleHeight = 0f;

            var titleGroup = FindOrCreate("[GROUP] TooltipTitleGroup", header.transform);
            var titleLayout = titleGroup.GetComponent<VerticalLayoutGroup>() ?? titleGroup.AddComponent<VerticalLayoutGroup>();
            titleLayout.spacing = 2f;
            titleLayout.childAlignment = TextAnchor.UpperLeft;
            titleLayout.childControlWidth = true;
            titleLayout.childControlHeight = true;
            titleLayout.childForceExpandWidth = true;
            titleLayout.childForceExpandHeight = false;
            var titleElement = titleGroup.GetComponent<LayoutElement>() ?? titleGroup.AddComponent<LayoutElement>();
            titleElement.flexibleWidth = 1f;

            nameText = CreateText(titleGroup.transform, "[TEXT] TooltipName", "Item Name", 18f, Gold, TextAlignmentOptions.Left, 22f);
            categoryText = CreateText(titleGroup.transform, "[TEXT] TooltipCategory", "Category", 12f, Muted, TextAlignmentOptions.Left, 16f);
            return header;
        }

        private static Transform CreateBodyScroll(Transform parent)
        {
            var scroll = FindOrCreate("[SCROLL] TooltipBodyScroll", parent);
            var scrollRect = scroll.transform as RectTransform;
            scrollRect.anchorMin = new Vector2(0f, 1f);
            scrollRect.anchorMax = new Vector2(1f, 1f);
            scrollRect.pivot = new Vector2(0.5f, 1f);
            scrollRect.sizeDelta = Vector2.zero;
            var scrollElement = scroll.GetComponent<LayoutElement>() ?? scroll.AddComponent<LayoutElement>();
            scrollElement.minHeight = 0f;
            scrollElement.preferredHeight = 100f;
            scrollElement.flexibleHeight = 0f;

            var scrollComponent = scroll.GetComponent<ScrollRect>() ?? scroll.AddComponent<ScrollRect>();
            scrollComponent.horizontal = false;
            scrollComponent.vertical = true;
            scrollComponent.movementType = ScrollRect.MovementType.Clamped;
            scrollComponent.inertia = false;
            scrollComponent.scrollSensitivity = 24f;

            var viewport = FindOrCreate("[VIEWPORT] TooltipViewport", scroll.transform);
            var viewportRect = viewport.transform as RectTransform;
            SetStretch(viewportRect);
            var mask = viewport.GetComponent<RectMask2D>() ?? viewport.AddComponent<RectMask2D>();
            mask.padding = Vector4.zero;

            var content = FindOrCreate("[LAYOUT] TooltipContent", viewport.transform);
            var contentRect = content.transform as RectTransform;
            contentRect.anchorMin = new Vector2(0f, 1f);
            contentRect.anchorMax = new Vector2(1f, 1f);
            contentRect.pivot = new Vector2(0f, 1f);
            contentRect.offsetMin = Vector2.zero;
            contentRect.offsetMax = Vector2.zero;
            contentRect.anchoredPosition = Vector2.zero;
            var layout = content.GetComponent<VerticalLayoutGroup>() ?? content.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(0, 0, 0, 0);
            layout.spacing = 7f;
            layout.childAlignment = TextAnchor.UpperLeft;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;
            var fitter = content.GetComponent<ContentSizeFitter>() ?? content.AddComponent<ContentSizeFitter>();
            fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            scrollComponent.viewport = viewportRect;
            scrollComponent.content = contentRect;
            return content.transform;
        }

        private static TooltipSection CreateSection(Transform parent, string name, string label)
        {
            var groupObject = FindOrCreate(name, parent);
            var layout = groupObject.GetComponent<VerticalLayoutGroup>() ?? groupObject.AddComponent<VerticalLayoutGroup>();
            layout.spacing = 4f;
            layout.childAlignment = TextAnchor.UpperLeft;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;
            var groupElement = groupObject.GetComponent<LayoutElement>() ?? groupObject.AddComponent<LayoutElement>();
            groupElement.minHeight = 0f;
            groupElement.flexibleHeight = 0f;
            CreateText(groupObject.transform, "[HEADER] " + label + "Header", label, 10f, Gold, TextAlignmentOptions.Left, 0f);
            var rows = FindOrCreate("[GROUP] " + label + "Rows", groupObject.transform);
            var rowsLayout = rows.GetComponent<VerticalLayoutGroup>() ?? rows.AddComponent<VerticalLayoutGroup>();
            rowsLayout.spacing = 3f;
            rowsLayout.childAlignment = TextAnchor.UpperLeft;
            rowsLayout.childControlWidth = true;
            rowsLayout.childControlHeight = true;
            rowsLayout.childForceExpandWidth = true;
            rowsLayout.childForceExpandHeight = false;
            return new TooltipSection(groupObject.transform, rows.transform, null);
        }

        private static TooltipSection CreateFooterSection(Transform parent)
        {
            var groupObject = FindOrCreate("[GROUP] Footer", parent);
            var layout = groupObject.GetComponent<VerticalLayoutGroup>() ?? groupObject.AddComponent<VerticalLayoutGroup>();
            layout.spacing = 4f;
            layout.childAlignment = TextAnchor.UpperLeft;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;
            CreateText(groupObject.transform, "[HEADER] NOTESHeader", "NOTES", 10f, Gold, TextAlignmentOptions.Left, 0f);
            var body = CreateText(groupObject.transform, "[TEXT] FooterText", string.Empty, 12.5f, Text, TextAlignmentOptions.TopLeft, 0f);
            return new TooltipSection(groupObject.transform, null, body);
        }

        private static void CreateDivider(Transform parent, string name)
        {
            var divider = FindOrCreate(name, parent);
            var image = divider.GetComponent<Image>() ?? divider.AddComponent<Image>();
            image.color = new Color(Border.r, Border.g, Border.b, 0.55f);
            image.raycastTarget = false;
            SetLayout(divider, 1f, 1f);
        }

        private static TMP_Text CreateText(Transform parent, string name, string value, float size, Color color, TextAlignmentOptions alignment, float minHeight)
        {
            var textObject = FindOrCreate(name, parent);
            var text = textObject.GetComponent<TextMeshProUGUI>() ?? textObject.AddComponent<TextMeshProUGUI>();
            text.text = value;
            text.fontSize = size;
            text.color = color;
            text.alignment = alignment;
            text.textWrappingMode = TextWrappingModes.Normal;
            text.overflowMode = TextOverflowModes.Overflow;
            text.enableAutoSizing = false;
            text.raycastTarget = false;
            SetLayout(textObject, minHeight, -1f);
            textObject.GetComponent<LayoutElement>().flexibleWidth = 1f;
            return text;
        }

        private static GameObject FindOrCreate(string name, Transform parent)
        {
            var existing = parent.Find(name);
            if (existing != null)
            {
                return existing.gameObject;
            }

            var gameObject = new GameObject(name, typeof(RectTransform));
            gameObject.layer = LayerMask.NameToLayer("UI");
            gameObject.transform.SetParent(parent, false);
            return gameObject;
        }

        private static void SetStretch(RectTransform rect)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            rect.localScale = Vector3.one;
        }

        private static void SetLayout(GameObject target, float minHeight, float preferredHeight)
        {
            var layout = target.GetComponent<LayoutElement>() ?? target.AddComponent<LayoutElement>();
            layout.ignoreLayout = false;
            if (minHeight >= 0f)
            {
                layout.minHeight = minHeight;
            }

            if (preferredHeight >= 0f)
            {
                layout.preferredHeight = preferredHeight;
            }
        }

        private static void DisableRaycasts(GameObject root)
        {
            foreach (var graphic in root.GetComponentsInChildren<Graphic>(true))
            {
                graphic.raycastTarget = false;
            }
        }

        private static void RemoveGeneratedTooltipChildren(Transform panel)
        {
            for (var i = panel.childCount - 1; i >= 0; i--)
            {
                var child = panel.GetChild(i);
                if (IsGeneratedTooltipChild(child.name))
                {
                    Undo.DestroyObjectImmediate(child.gameObject);
                }
            }
        }

        private static bool IsGeneratedTooltipChild(string childName)
        {
            return childName == "[HEADER] TooltipHeader" ||
                   childName == "[DIVIDER] TooltipHeaderDivider" ||
                   childName == "[TEXT] TooltipDescription" ||
                   childName == "[GROUP] PrimaryStats" ||
                   childName == "[GROUP] SecondaryStats" ||
                   childName == "[GROUP] Effects" ||
                   childName == "[GROUP] Requirements" ||
                   childName == "[GROUP] Comparison" ||
                   childName == "[GROUP] Footer" ||
                   childName == "[SCROLL] TooltipBodyScroll";
        }

        private readonly struct TooltipSection
        {
            public TooltipSection(Transform group, Transform rows, TMP_Text body)
            {
                Group = group;
                Rows = rows;
                Body = body;
            }

            public Transform Group { get; }
            public Transform Rows { get; }
            public TMP_Text Body { get; }
        }
    }
}
