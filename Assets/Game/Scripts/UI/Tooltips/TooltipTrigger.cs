using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace IdleGame.UI.Tooltips
{
    public sealed class TooltipTrigger : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerDownHandler,
        IPointerUpHandler,
        IPointerMoveHandler,
        IBeginDragHandler,
        IDragHandler,
        IEndDragHandler,
        ISelectHandler,
        IDeselectHandler
    {
        [SerializeField] private MonoBehaviour providerBehaviour;
        [SerializeField] private float dragCancelThreshold = 14f;

        private ITooltipDataProvider provider;
        private ScrollRect parentScrollRect;
        private Vector2 pressPosition;
        private bool pointerInside;
        private bool longPressActive;
        private bool warningLogged;

        public ITooltipDataProvider Provider
        {
            get
            {
                if (provider == null)
                {
                    AutoBind();
                }

                return provider;
            }
        }

        private void Awake()
        {
            AutoBind();
        }

        private void OnDisable()
        {
            TooltipManager.Instance?.Hide(gameObject);
            pointerInside = false;
            longPressActive = false;
        }

        public void ConfigureForEditor(MonoBehaviour tooltipProvider)
        {
            providerBehaviour = tooltipProvider;
            provider = tooltipProvider as ITooltipDataProvider;
            parentScrollRect = GetComponentInParent<ScrollRect>();
        }

        public void AutoBind()
        {
            if (providerBehaviour == null)
            {
                providerBehaviour = GetComponent<MonoBehaviour>();
            }

            provider = providerBehaviour as ITooltipDataProvider;
            parentScrollRect = GetComponentInParent<ScrollRect>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            pointerInside = true;
            if (IsTouchPointer(eventData))
            {
                return;
            }

            Request(eventData.position, TooltipRequestMode.Hover);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            pointerInside = false;
            TooltipManager.Instance?.Hide(gameObject);
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (!pointerInside)
            {
                return;
            }

            TooltipManager.Instance?.UpdatePointer(gameObject, eventData.position);
            if (longPressActive && Vector2.Distance(eventData.position, pressPosition) > dragCancelThreshold)
            {
                Cancel();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!IsTouchPointer(eventData))
            {
                return;
            }

            pressPosition = eventData.position;
            longPressActive = true;
            Request(eventData.position, TooltipRequestMode.LongPress);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (longPressActive && TooltipManager.Instance != null && TooltipManager.Instance.IsVisible)
            {
                eventData.eligibleForClick = false;
            }

            longPressActive = false;
            TooltipManager.Instance?.Hide(gameObject);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Cancel();
            parentScrollRect?.OnBeginDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (longPressActive && Vector2.Distance(eventData.position, pressPosition) > dragCancelThreshold)
            {
                Cancel();
            }

            parentScrollRect?.OnDrag(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Cancel();
            parentScrollRect?.OnEndDrag(eventData);
        }

        public void OnSelect(BaseEventData eventData)
        {
            Request(transform.position, TooltipRequestMode.Selection);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            TooltipManager.Instance?.Hide(gameObject);
        }

        private void Request(Vector2 pointerPosition, TooltipRequestMode mode)
        {
            if (TooltipManager.Instance == null)
            {
                return;
            }

            var dataProvider = Provider;
            if (dataProvider == null)
            {
                if (!warningLogged)
                {
                    warningLogged = true;
                    Debug.LogWarning($"TooltipTrigger on '{name}' has no tooltip data provider.", this);
                }

                return;
            }

            TooltipManager.Instance.RequestShow(gameObject, dataProvider, pointerPosition, mode);
        }

        private void Cancel()
        {
            longPressActive = false;
            TooltipManager.Instance?.CancelRequest(gameObject);
            TooltipManager.Instance?.Hide(gameObject);
        }

        private static bool IsTouchPointer(PointerEventData eventData)
        {
            if (eventData is ExtendedPointerEventData extendedEventData)
            {
                return extendedEventData.pointerType == UIPointerType.Touch;
            }

            return eventData != null && eventData.pointerId >= 0 && Input.touchSupported;
        }
    }
}
