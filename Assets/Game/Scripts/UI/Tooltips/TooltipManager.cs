using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IdleGame.UI.Tooltips
{
    public enum TooltipRequestMode
    {
        Hover,
        LongPress,
        Selection
    }

    public sealed class TooltipManager : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private RectTransform canvasRect;
        [SerializeField] private RectTransform tooltipRoot;
        [SerializeField] private CanvasGroup tooltipCanvasGroup;
        [SerializeField] private RectTransform tooltipContent;
        [SerializeField] private RectTransform scrollViewport;
        [SerializeField] private ScrollRect bodyScrollRect;
        [SerializeField] private LayoutElement bodyScrollLayoutElement;
        [SerializeField] private LayoutElement rootLayoutElement;
        [SerializeField] private Image iconImage;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text categoryText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Transform primaryStatsGroup;
        [SerializeField] private Transform primaryStatsRows;
        [SerializeField] private Transform secondaryStatsGroup;
        [SerializeField] private Transform secondaryStatsRows;
        [SerializeField] private Transform effectsGroup;
        [SerializeField] private Transform effectsRows;
        [SerializeField] private Transform requirementsGroup;
        [SerializeField] private Transform requirementsRows;
        [SerializeField] private Transform comparisonGroup;
        [SerializeField] private Transform comparisonRows;
        [SerializeField] private Transform footerGroup;
        [SerializeField] private TMP_Text footerText;
        [SerializeField] private float hoverDelaySeconds = 0.5f;
        [SerializeField] private float longPressDelaySeconds = 0.5f;
        [SerializeField] private Vector2 pointerOffset = new(22f, 18f);
        [SerializeField] private float edgePadding = 10f;
        [SerializeField] private float visibleRefreshInterval = 0.2f;
        [SerializeField] private float minimumWidth = 260f;
        [SerializeField] private float preferredWidth = 360f;
        [SerializeField] private float maximumWidth = 440f;
        [SerializeField, Range(0.5f, 0.95f)] private float maximumCanvasHeightFraction = 0.78f;

        private static TooltipManager instance;
        private readonly StringBuilder builder = new();
        private readonly List<TooltipStatRow> primaryRows = new();
        private readonly List<TooltipStatRow> secondaryRows = new();
        private readonly List<TooltipStatRow> effectRows = new();
        private readonly List<TooltipStatRow> requirementRows = new();
        private readonly List<TooltipStatRow> comparisonRowPool = new();
        private Coroutine pendingCoroutine;
        private GameObject requestedSource;
        private ITooltipDataProvider requestedProvider;
        private GameObject visibleSource;
        private ITooltipDataProvider visibleProvider;
        private TooltipRequestMode requestedMode;
        private TooltipRequestMode visibleMode;
        private Vector2 requestedPointerPosition;
        private Vector2 visiblePointerPosition;
        private bool isVisible;
        private int requestVersion;
        private float refreshTimer;
        private bool validationLogged;
        private IdleGame.UI.ScreenManager screenManager;

        public static TooltipManager Instance => instance;
        public bool IsVisible => isVisible;

        private void Awake()
        {
            instance = this;
            AutoBind();
            HideImmediate();
        }

        private void OnEnable()
        {
            if (instance == null)
            {
                instance = this;
            }

            BindScreenManager();
        }

        private void OnDisable()
        {
            UnbindScreenManager();
            CancelPending();
            if (instance == this)
            {
                instance = null;
            }
        }

        private void Update()
        {
            if (!isVisible)
            {
                return;
            }

            if (visibleSource == null || !visibleSource.activeInHierarchy || visibleProvider == null)
            {
                Hide(visibleSource);
                return;
            }

            if (visibleMode == TooltipRequestMode.Hover)
            {
                UpdatePosition(visiblePointerPosition);
            }

            refreshTimer -= Time.unscaledDeltaTime;
            if (refreshTimer <= 0f)
            {
                refreshTimer = visibleRefreshInterval;
                RefreshVisible();
            }
        }

        public void ConfigureForEditor(
            Canvas canvasReference,
            RectTransform canvasRectReference,
            RectTransform tooltipRootReference,
            CanvasGroup canvasGroupReference,
            Image iconReference,
            TMP_Text nameReference,
            TMP_Text categoryReference,
            TMP_Text descriptionReference,
            Transform primaryGroup,
            Transform primaryRowsReference,
            Transform secondaryGroup,
            Transform secondaryRowsReference,
            Transform effectsSection,
            Transform effectsRowsReference,
            Transform requirementsSection,
            Transform requirementsRowsReference,
            Transform comparisonSection,
            Transform comparisonRowsReference,
            Transform footerSection,
            TMP_Text footerBody)
        {
            canvas = canvasReference;
            canvasRect = canvasRectReference;
            tooltipRoot = tooltipRootReference;
            tooltipCanvasGroup = canvasGroupReference;
            iconImage = iconReference;
            nameText = nameReference;
            categoryText = categoryReference;
            descriptionText = descriptionReference;
            primaryStatsGroup = primaryGroup;
            primaryStatsRows = primaryRowsReference;
            secondaryStatsGroup = secondaryGroup;
            secondaryStatsRows = secondaryRowsReference;
            effectsGroup = effectsSection;
            effectsRows = effectsRowsReference;
            requirementsGroup = requirementsSection;
            requirementsRows = requirementsRowsReference;
            comparisonGroup = comparisonSection;
            comparisonRows = comparisonRowsReference;
            footerGroup = footerSection;
            footerText = footerBody;
            AutoBind();
        }

        public void RequestShow(GameObject source, ITooltipDataProvider provider, Vector2 pointerPosition, TooltipRequestMode mode)
        {
            if (source == null || provider == null || !source.activeInHierarchy)
            {
                return;
            }

            requestVersion++;
            requestedSource = source;
            requestedProvider = provider;
            requestedPointerPosition = pointerPosition;
            requestedMode = mode;
            visiblePointerPosition = pointerPosition;

            CancelPending();
            if (isVisible && visibleSource != source)
            {
                HideImmediate();
            }

            pendingCoroutine = StartCoroutine(ShowAfterDelay(requestVersion, mode == TooltipRequestMode.LongPress ? longPressDelaySeconds : hoverDelaySeconds));
        }

        public void UpdatePointer(GameObject source, Vector2 pointerPosition)
        {
            if (source == requestedSource)
            {
                requestedPointerPosition = pointerPosition;
            }

            if (source == visibleSource && visibleMode == TooltipRequestMode.Hover)
            {
                visiblePointerPosition = pointerPosition;
                UpdatePosition(pointerPosition);
            }
        }

        public void CancelRequest(GameObject source)
        {
            if (source != null && source != requestedSource)
            {
                return;
            }

            requestVersion++;
            requestedSource = null;
            requestedProvider = null;
            CancelPending();
        }

        public void Hide(GameObject source)
        {
            if (source != null && source != visibleSource && source != requestedSource)
            {
                return;
            }

            requestVersion++;
            requestedSource = null;
            requestedProvider = null;
            CancelPending();
            HideImmediate();
        }

        public static void HideGlobal()
        {
            if (instance != null)
            {
                instance.Hide(null);
            }
        }

        private IEnumerator ShowAfterDelay(int version, float delay)
        {
            yield return new WaitForSecondsRealtime(Mathf.Max(0f, delay));

            if (version != requestVersion ||
                requestedSource == null ||
                !requestedSource.activeInHierarchy ||
                requestedProvider == null)
            {
                yield break;
            }

            ShowNow(requestedSource, requestedProvider, requestedPointerPosition, requestedMode);
        }

        private void ShowNow(GameObject source, ITooltipDataProvider provider, Vector2 pointerPosition, TooltipRequestMode mode)
        {
            var data = provider.BuildTooltipData();
            if (data == null || !data.HasUsefulContent)
            {
                Debug.LogWarning($"TooltipManager: '{source.name}' produced empty tooltip data.", source);
                return;
            }

            visibleSource = source;
            visibleProvider = provider;
            visibleMode = mode;
            visiblePointerPosition = pointerPosition;
            isVisible = true;
            ApplyData(data);
            SetVisible(true);
            RebuildAndConstrainLayout();
            UpdatePosition(pointerPosition);
            refreshTimer = visibleRefreshInterval;
        }

        private void RefreshVisible()
        {
            if (visibleProvider == null)
            {
                HideImmediate();
                return;
            }

            var data = visibleProvider.BuildTooltipData();
            if (data == null || !data.HasUsefulContent)
            {
                HideImmediate();
                return;
            }

            ApplyData(data);
            RebuildAndConstrainLayout();
            UpdatePosition(visiblePointerPosition);
        }

        private void ApplyData(TooltipData data)
        {
            if (iconImage != null)
            {
                iconImage.sprite = data.Icon;
                iconImage.enabled = data.Icon != null;
            }

            SetText(nameText, data.DisplayName);
            SetText(categoryText, data.Category);
            SetText(descriptionText, data.Description);
            ConfigureIconVisibility(data.Icon);
            SetSection(primaryStatsGroup, primaryStatsRows, primaryRows, data.PrimaryStats);
            SetSection(secondaryStatsGroup, secondaryStatsRows, secondaryRows, data.SecondaryStats);
            SetSection(effectsGroup, effectsRows, effectRows, data.Effects);
            SetSection(requirementsGroup, requirementsRows, requirementRows, data.Requirements);
            SetSection(comparisonGroup, comparisonRows, comparisonRowPool, data.Comparison);
            SetFooter(footerGroup, footerText, data.Footer);
        }

        private void UpdatePosition(Vector2 screenPosition)
        {
            if (tooltipRoot == null || canvasRect == null)
            {
                return;
            }

            var camera = canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay
                ? canvas.worldCamera
                : null;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPosition, camera, out var localPointer);
            var tooltipSize = tooltipRoot.rect.size;
            if (tooltipSize.x <= 0f || tooltipSize.y <= 0f)
            {
                RebuildAndConstrainLayout();
                tooltipSize = tooltipRoot.rect.size;
            }

            var canvasSize = canvasRect.rect.size;
            var x = localPointer.x + pointerOffset.x;
            var y = localPointer.y - pointerOffset.y;
            if (x + tooltipSize.x > canvasSize.x * 0.5f - edgePadding)
            {
                x = localPointer.x - pointerOffset.x - tooltipSize.x;
            }

            if (y - tooltipSize.y < -canvasSize.y * 0.5f + edgePadding)
            {
                y = localPointer.y + pointerOffset.y + tooltipSize.y;
            }

            x = Mathf.Clamp(x, -canvasSize.x * 0.5f + edgePadding, canvasSize.x * 0.5f - tooltipSize.x - edgePadding);
            y = Mathf.Clamp(y, -canvasSize.y * 0.5f + tooltipSize.y + edgePadding, canvasSize.y * 0.5f - edgePadding);
            tooltipRoot.anchoredPosition = new Vector2(x, y);
        }

        private void AutoBind()
        {
            if (canvas == null)
            {
                canvas = GetComponentInParent<Canvas>();
            }

            if (canvasRect == null && canvas != null)
            {
                canvasRect = canvas.transform as RectTransform;
            }

            if (tooltipRoot == null)
            {
                tooltipRoot = transform as RectTransform;
            }

            if (rootLayoutElement == null && tooltipRoot != null)
            {
                rootLayoutElement = tooltipRoot.GetComponent<LayoutElement>();
            }

            if (tooltipContent == null && tooltipRoot != null)
            {
                var content = tooltipRoot.Find("[SCROLL] TooltipBodyScroll/[VIEWPORT] TooltipViewport/[LAYOUT] TooltipContent");
                tooltipContent = content as RectTransform;
            }

            if (scrollViewport == null && tooltipRoot != null)
            {
                var viewport = tooltipRoot.Find("[SCROLL] TooltipBodyScroll/[VIEWPORT] TooltipViewport");
                scrollViewport = viewport as RectTransform;
            }

            if (bodyScrollRect == null && tooltipRoot != null)
            {
                bodyScrollRect = tooltipRoot.GetComponentInChildren<ScrollRect>(true);
            }

            if (bodyScrollLayoutElement == null && bodyScrollRect != null)
            {
                bodyScrollLayoutElement = bodyScrollRect.GetComponent<LayoutElement>();
            }

            if (tooltipCanvasGroup == null && tooltipRoot != null)
            {
                tooltipCanvasGroup = tooltipRoot.GetComponent<CanvasGroup>();
            }

            if (tooltipCanvasGroup != null)
            {
                tooltipCanvasGroup.blocksRaycasts = false;
                tooltipCanvasGroup.interactable = false;
            }

            DisableTooltipRaycasts();
            ValidateConfigurationOnce();
            BindScreenManager();
        }

        private void BindScreenManager()
        {
            if (screenManager != null)
            {
                return;
            }

            screenManager = FindAnyObjectByType<IdleGame.UI.ScreenManager>();
            if (screenManager != null)
            {
                screenManager.ScreenChanged -= OnScreenChanged;
                screenManager.ScreenChanged += OnScreenChanged;
            }
        }

        private void UnbindScreenManager()
        {
            if (screenManager != null)
            {
                screenManager.ScreenChanged -= OnScreenChanged;
                screenManager = null;
            }
        }

        private void OnScreenChanged(string _)
        {
            Hide(null);
        }

        private void SetVisible(bool visible)
        {
            if (tooltipRoot != null)
            {
                tooltipRoot.gameObject.SetActive(visible);
            }

            if (tooltipCanvasGroup != null)
            {
                tooltipCanvasGroup.alpha = visible ? 1f : 0f;
                tooltipCanvasGroup.blocksRaycasts = false;
                tooltipCanvasGroup.interactable = false;
            }

            if (bodyScrollRect != null)
            {
                bodyScrollRect.verticalNormalizedPosition = 1f;
            }
        }

        private void HideImmediate()
        {
            isVisible = false;
            visibleSource = null;
            visibleProvider = null;
            SetVisible(false);
        }

        private void CancelPending()
        {
            if (pendingCoroutine != null)
            {
                StopCoroutine(pendingCoroutine);
                pendingCoroutine = null;
            }
        }

        private static void SetText(TMP_Text target, string value)
        {
            if (target == null)
            {
                return;
            }

            target.text = value ?? string.Empty;
            target.gameObject.SetActive(!string.IsNullOrWhiteSpace(target.text));
            target.raycastTarget = false;
            target.enableAutoSizing = false;
            target.textWrappingMode = TextWrappingModes.Normal;
            target.overflowMode = TextOverflowModes.Overflow;
        }

        private void SetSection(Transform group, Transform rowsRoot, List<TooltipStatRow> rowPool, IReadOnlyList<TooltipStatLine> lines)
        {
            if (group == null || rowsRoot == null || rowPool == null)
            {
                return;
            }

            var visibleCount = 0;
            foreach (var line in lines)
            {
                if (!line.IsValid)
                {
                    continue;
                }

                var row = GetOrCreateRow(rowsRoot, rowPool, visibleCount);
                row.Set(line);
                visibleCount++;
            }

            for (var i = visibleCount; i < rowPool.Count; i++)
            {
                rowPool[i].SetActive(false);
            }

            group.gameObject.SetActive(visibleCount > 0);
        }

        private void SetFooter(Transform group, TMP_Text text, System.Collections.Generic.IReadOnlyList<string> lines)
        {
            if (group == null || text == null)
            {
                return;
            }

            builder.Clear();
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                if (builder.Length > 0)
                {
                    builder.AppendLine();
                }

                builder.Append(line);
            }

            text.text = builder.ToString();
            text.raycastTarget = false;
            text.enableAutoSizing = false;
            text.textWrappingMode = TextWrappingModes.Normal;
            text.overflowMode = TextOverflowModes.Overflow;
            group.gameObject.SetActive(builder.Length > 0);
        }

        private TooltipStatRow GetOrCreateRow(Transform rowsRoot, List<TooltipStatRow> rowPool, int index)
        {
            if (index < rowPool.Count)
            {
                return rowPool[index];
            }

            var rowObject = new GameObject($"[ROW] TooltipStatRow {index + 1:00}", typeof(RectTransform));
            rowObject.layer = rowsRoot.gameObject.layer;
            rowObject.transform.SetParent(rowsRoot, false);

            var layout = rowObject.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = 10f;
            layout.childAlignment = TextAnchor.UpperLeft;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = false;
            layout.childForceExpandHeight = false;

            var rowElement = rowObject.AddComponent<LayoutElement>();
            rowElement.minHeight = 18f;

            var label = CreateRuntimeText("[TEXT] StatLabel", rowObject.transform, TextAlignmentOptions.TopLeft, new Color(0.62f, 0.66f, 0.72f));
            var labelElement = label.GetComponent<LayoutElement>();
            labelElement.flexibleWidth = 1f;
            labelElement.minWidth = 115f;
            labelElement.preferredWidth = 190f;

            var value = CreateRuntimeText("[TEXT] StatValue", rowObject.transform, TextAlignmentOptions.TopRight, new Color(0.91f, 0.85f, 0.73f));
            var valueElement = value.GetComponent<LayoutElement>();
            valueElement.minWidth = 80f;
            valueElement.preferredWidth = 120f;
            valueElement.flexibleWidth = 0f;

            var row = new TooltipStatRow(rowObject, label, value);
            rowPool.Add(row);
            return row;
        }

        private static TMP_Text CreateRuntimeText(string name, Transform parent, TextAlignmentOptions alignment, Color color)
        {
            var textObject = new GameObject(name, typeof(RectTransform));
            textObject.layer = parent.gameObject.layer;
            textObject.transform.SetParent(parent, false);
            var text = textObject.AddComponent<TextMeshProUGUI>();
            text.fontSize = 12.5f;
            text.color = color;
            text.alignment = alignment;
            text.enableAutoSizing = false;
            text.textWrappingMode = TextWrappingModes.Normal;
            text.overflowMode = TextOverflowModes.Overflow;
            text.raycastTarget = false;
            var layout = textObject.AddComponent<LayoutElement>();
            layout.minHeight = 18f;
            return text;
        }

        private void ConfigureIconVisibility(Sprite icon)
        {
            if (iconImage == null)
            {
                return;
            }

            var iconElement = iconImage.GetComponent<LayoutElement>();
            var visible = icon != null;
            iconImage.enabled = visible;
            iconImage.gameObject.SetActive(visible);
            if (iconElement != null)
            {
                iconElement.ignoreLayout = !visible;
            }
        }

        private void RebuildAndConstrainLayout()
        {
            if (tooltipRoot == null)
            {
                return;
            }

            if (rootLayoutElement != null)
            {
                rootLayoutElement.minWidth = minimumWidth;
                rootLayoutElement.preferredWidth = CalculateTargetWidth();
                rootLayoutElement.flexibleWidth = 0f;
            }

            tooltipRoot.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, CalculateTargetWidth());

            Canvas.ForceUpdateCanvases();
            if (tooltipContent != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(tooltipContent);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(tooltipRoot);
            Canvas.ForceUpdateCanvases();

            var canvasHeight = canvasRect != null ? canvasRect.rect.height : Screen.height;
            var maxHeight = Mathf.Max(220f, canvasHeight * maximumCanvasHeightFraction);
            var rootPaddingAndHeader = CalculateRootNonBodyHeight();
            var bodyPreferredHeight = tooltipContent != null
                ? LayoutUtility.GetPreferredHeight(tooltipContent)
                : 0f;
            var bodyHeight = Mathf.Max(0f, bodyPreferredHeight);
            var rootPreferredHeight = LayoutUtility.GetPreferredHeight(tooltipRoot);

            if (bodyScrollLayoutElement != null && bodyScrollRect != null)
            {
                var maxBodyHeight = Mathf.Max(80f, maxHeight - rootPaddingAndHeader);
                var clampedBodyHeight = Mathf.Min(bodyHeight, maxBodyHeight);
                bodyScrollLayoutElement.preferredHeight = clampedBodyHeight;
                bodyScrollLayoutElement.flexibleHeight = 0f;
                bodyScrollRect.enabled = bodyHeight > maxBodyHeight + 0.5f;
                bodyScrollRect.verticalNormalizedPosition = 1f;
            }

            if (scrollViewport != null && tooltipContent != null)
            {
                tooltipContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, scrollViewport.rect.width);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(tooltipRoot);
            Canvas.ForceUpdateCanvases();
            var finalHeight = Mathf.Min(Mathf.Max(rootPreferredHeight, LayoutUtility.GetPreferredHeight(tooltipRoot)), maxHeight);
            tooltipRoot.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, finalHeight);
            LayoutRebuilder.ForceRebuildLayoutImmediate(tooltipRoot);
        }

        private float CalculateTargetWidth()
        {
            var width = preferredWidth;
            if (nameText != null && !string.IsNullOrWhiteSpace(nameText.text) && nameText.text.Length > 28)
            {
                width += Mathf.Min(60f, (nameText.text.Length - 28) * 2f);
            }

            if (descriptionText != null && !string.IsNullOrWhiteSpace(descriptionText.text) && descriptionText.text.Length > 120)
            {
                width += 30f;
            }

            return Mathf.Clamp(width, minimumWidth, maximumWidth);
        }

        private float CalculateRootNonBodyHeight()
        {
            if (tooltipRoot == null || bodyScrollRect == null)
            {
                return 0f;
            }

            var total = LayoutUtility.GetPreferredHeight(tooltipRoot);
            var body = LayoutUtility.GetPreferredHeight(bodyScrollRect.transform as RectTransform);
            return Mathf.Max(0f, total - body);
        }

        private void DisableTooltipRaycasts()
        {
            if (tooltipRoot == null)
            {
                return;
            }

            foreach (var graphic in tooltipRoot.GetComponentsInChildren<Graphic>(true))
            {
                graphic.raycastTarget = false;
            }
        }

        private void ValidateConfigurationOnce()
        {
            if (validationLogged)
            {
                return;
            }

            validationLogged = true;
            if (canvas == null)
            {
                Debug.LogWarning("TooltipManager: Tooltip Canvas reference is missing.", this);
            }

            if (canvasRect == null)
            {
                Debug.LogWarning("TooltipManager: Canvas RectTransform reference is missing.", this);
            }

            if (tooltipRoot == null)
            {
                Debug.LogWarning("TooltipManager: Tooltip root RectTransform reference is missing.", this);
            }

            if (tooltipRoot != null && tooltipRoot.GetComponent<VerticalLayoutGroup>() == null)
            {
                Debug.LogWarning("TooltipManager: Tooltip root needs a VerticalLayoutGroup for dynamic sizing.", this);
            }

            if (tooltipContent == null || tooltipContent.GetComponent<VerticalLayoutGroup>() == null)
            {
                Debug.LogWarning("TooltipManager: TooltipContent is missing or has no VerticalLayoutGroup.", this);
            }

            WarnIfNull(nameText, "Name Text");
            WarnIfNull(categoryText, "Category Text");
            WarnIfNull(descriptionText, "Description Text");
            WarnIfNull(primaryStatsGroup, "Primary Stats Section");
            WarnIfNull(primaryStatsRows, "Primary Stats Rows");
            WarnIfNull(secondaryStatsGroup, "Details Section");
            WarnIfNull(secondaryStatsRows, "Details Rows");
            WarnIfNull(effectsGroup, "Effects Section");
            WarnIfNull(effectsRows, "Effects Rows");
            WarnIfNull(requirementsGroup, "Requirements Section");
            WarnIfNull(requirementsRows, "Requirements Rows");
            WarnIfNull(comparisonGroup, "Comparison Section");
            WarnIfNull(comparisonRows, "Comparison Rows");
            WarnIfZeroHeight(nameText, "Name Text");
            WarnIfZeroHeight(categoryText, "Category Text");
            WarnIfZeroHeight(descriptionText, "Description Text");
        }

        private void WarnIfNull(Object target, string label)
        {
            if (target == null)
            {
                Debug.LogWarning($"TooltipManager: {label} reference is missing.", this);
            }
        }

        private void WarnIfZeroHeight(TMP_Text text, string label)
        {
            if (text == null)
            {
                return;
            }

            var rect = text.transform as RectTransform;
            if (rect != null && rect.rect.height <= 0.1f && text.gameObject.activeInHierarchy)
            {
                Debug.LogWarning($"TooltipManager: {label} has a zero-height RectTransform.", text);
            }
        }

        private readonly struct TooltipStatRow
        {
            private readonly GameObject root;
            private readonly TMP_Text labelText;
            private readonly TMP_Text valueText;

            public TooltipStatRow(GameObject rowRoot, TMP_Text label, TMP_Text value)
            {
                root = rowRoot;
                labelText = label;
                valueText = value;
            }

            public void Set(TooltipStatLine line)
            {
                root.SetActive(true);
                labelText.text = line.Label;
                valueText.text = line.Value;
                valueText.color = GetToneColor(line.Tone);
            }

            public void SetActive(bool active)
            {
                root.SetActive(active);
            }
        }

        private static Color GetToneColor(TooltipValueTone tone)
        {
            return tone switch
            {
                TooltipValueTone.Positive => new Color(0.45f, 0.85f, 0.53f),
                TooltipValueTone.Negative => new Color(0.94f, 0.42f, 0.38f),
                TooltipValueTone.Muted => new Color(0.55f, 0.59f, 0.64f),
                _ => new Color(0.91f, 0.85f, 0.73f)
            };
        }
    }
}
