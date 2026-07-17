using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IdleGame.UI.Combat
{
    [RequireComponent(typeof(ScrollRect))]
    public sealed class CombatLogAutoScrollController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IScrollHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private bool diagnostics;

        private bool pointerInside;
        private bool userPausedAutoScroll;
        private bool isDragging;
        private bool autoScrollEnabled = true;
        private bool pendingScrollRestore;
        private bool pendingScrollToBottom;
        private bool hasCapturedPosition;
        private Vector2 capturedContentPosition;
        private int pendingLayoutPasses;

        public bool IsAutoScrollActive => autoScrollEnabled && !userPausedAutoScroll;
        public bool IsPausedByUser => userPausedAutoScroll;

        private void Awake()
        {
            scrollRect ??= GetComponent<ScrollRect>();
        }

        private void LateUpdate()
        {
            if (pendingLayoutPasses <= 0)
            {
                return;
            }

            pendingLayoutPasses--;
            ApplyScrollAfterLayout();
        }

        public void BeginContentRefresh()
        {
            if (IsAutoScrollActive || scrollRect == null || scrollRect.content == null)
            {
                return;
            }

            capturedContentPosition = scrollRect.content.anchoredPosition;
            hasCapturedPosition = true;
            Diagnostic("Position captured.");
        }

        public void NotifyRowsRemovedFromTop(float removedHeight)
        {
            if (!hasCapturedPosition || removedHeight <= 0f)
            {
                return;
            }

            capturedContentPosition.y = Mathf.Max(0f, capturedContentPosition.y - removedHeight);
        }

        public void NotifyContentChanged()
        {
            if (IsAutoScrollActive)
            {
                QueuePostLayoutScrollToBottom();
                return;
            }

            if (hasCapturedPosition)
            {
                QueuePostLayoutRestore();
            }
        }

        public void PauseFromToolbar()
        {
            pointerInside = true;
            PauseAutoScroll();
        }

        public void ResumeAndScrollToBottom()
        {
            userPausedAutoScroll = false;
            autoScrollEnabled = true;
            hasCapturedPosition = false;
            QueuePostLayoutScrollToBottom();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            pointerInside = true;
            Diagnostic("Pointer entered.");
        }

        public void OnScroll(PointerEventData eventData)
        {
            pointerInside = true;
            PauseAutoScroll();
            Diagnostic("Mouse-wheel interaction detected.");
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            isDragging = true;
            PauseAutoScroll();
            Diagnostic("Drag began.");
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isDragging = false;
            if (!pointerInside)
            {
                userPausedAutoScroll = false;
                autoScrollEnabled = true;
                QueuePostLayoutScrollToBottom();
            }

            Diagnostic("Drag ended.");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            pointerInside = false;
            if (isDragging)
            {
                isDragging = false;
            }

            userPausedAutoScroll = false;
            autoScrollEnabled = true;
            hasCapturedPosition = false;
            QueuePostLayoutScrollToBottom();
            Diagnostic("Pointer exited. Auto-scroll resumed.");
        }

        public void ConfigureForEditor(ScrollRect targetScrollRect)
        {
            scrollRect = targetScrollRect;
        }

        private void PauseAutoScroll()
        {
            if (scrollRect != null)
            {
                scrollRect.StopMovement();
            }

            userPausedAutoScroll = true;
            autoScrollEnabled = false;
            pendingScrollToBottom = false;
            pendingScrollRestore = false;
            Diagnostic("Auto-scroll paused.");
        }

        private void QueuePostLayoutRestore()
        {
            pendingScrollRestore = true;
            pendingScrollToBottom = false;
            pendingLayoutPasses = 1;
            ApplyScrollAfterLayout();
        }

        private void QueuePostLayoutScrollToBottom()
        {
            pendingScrollToBottom = true;
            pendingScrollRestore = false;
            pendingLayoutPasses = 1;
            ApplyScrollAfterLayout();
        }

        private void ApplyScrollAfterLayout()
        {
            Canvas.ForceUpdateCanvases();
            if (scrollRect != null && scrollRect.content != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);
            }

            Canvas.ForceUpdateCanvases();

            if (scrollRect != null)
            {
                scrollRect.StopMovement();

                if (pendingScrollRestore && !IsAutoScrollActive && scrollRect.content != null)
                {
                    scrollRect.content.anchoredPosition = capturedContentPosition;
                    Diagnostic("Position restored.");
                }
                else if (pendingScrollToBottom && IsAutoScrollActive)
                {
                    SetContentToBottom();
                    scrollRect.verticalNormalizedPosition = 0f;
                    Diagnostic("Scroll-to-bottom executed.");
                }
            }

            pendingScrollRestore = false;
            pendingScrollToBottom = false;
            hasCapturedPosition = false;
        }

        private void Diagnostic(string message)
        {
            if (diagnostics)
            {
                Debug.Log($"[CombatLogAutoScroll] {message}", this);
            }
        }

        private void SetContentToBottom()
        {
            if (scrollRect == null || scrollRect.content == null || scrollRect.viewport == null)
            {
                return;
            }

            var content = scrollRect.content;
            var bottomY = Mathf.Max(0f, content.rect.height - scrollRect.viewport.rect.height);
            content.anchoredPosition = new Vector2(content.anchoredPosition.x, bottomY);
        }
    }
}
