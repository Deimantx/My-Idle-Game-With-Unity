using System.Collections.Generic;
using IdleGame.UI.Shared;
using IdleGame.UI.Tooltips;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IdleGame.UI.Combat
{
    public sealed class CombatControlRaycaster : BaseRaycaster
    {
        [SerializeField] private Button heavyStrikeButton;
        [SerializeField] private Toggle heavyStrikeAutoToggle;
        [SerializeField] private Toggle autoRepeatToggle;

        private Canvas parentCanvas;

        public override Camera eventCamera
        {
            get
            {
                var canvas = GetParentCanvas();
                if (canvas == null || canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                {
                    return null;
                }

                return canvas.worldCamera != null ? canvas.worldCamera : Camera.main;
            }
        }

        public override int sortOrderPriority => 1000;
        public override int renderOrderPriority => 1000;

        protected override void Awake()
        {
            base.Awake();
            AutoBind();
        }

        public void ConfigureForEditor(Button heavyStrike, Toggle heavyStrikeAuto, Toggle autoRepeat)
        {
            heavyStrikeButton = heavyStrike;
            heavyStrikeAutoToggle = heavyStrikeAuto;
            autoRepeatToggle = autoRepeat;
            parentCanvas = null;
        }

        public void AutoBind()
        {
            if (heavyStrikeButton == null)
            {
                heavyStrikeButton = HierarchySearch.FindButton(transform, "[BUTTON] HeavyStrikeButton");
            }

            if (heavyStrikeAutoToggle == null)
            {
                heavyStrikeAutoToggle = HierarchySearch.FindDeep(transform, "[TOGGLE] HeavyStrikeAutoToggle")?.GetComponent<Toggle>();
            }

            if (autoRepeatToggle == null)
            {
                autoRepeatToggle = HierarchySearch.FindDeep(transform, "[TOGGLE] AutoRepeatToggle")?.GetComponent<Toggle>();
            }

            parentCanvas = null;
        }

        public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
        {
            if (eventData == null || resultAppendList == null || !isActiveAndEnabled)
            {
                return;
            }

            AutoBind();
            AddButtonHit(heavyStrikeButton, eventData, resultAppendList);
            AddToggleHit(heavyStrikeAutoToggle, eventData, resultAppendList);
            AddToggleHit(autoRepeatToggle, eventData, resultAppendList);
            AddTooltipTriggerHits(eventData, resultAppendList);
        }

        private void AddButtonHit(Button button, PointerEventData eventData, List<RaycastResult> results)
        {
            if (button == null || !button.isActiveAndEnabled || !button.interactable)
            {
                return;
            }

            AddHit(button.gameObject, button.transform as RectTransform, eventData, results);
        }

        private void AddToggleHit(Toggle toggle, PointerEventData eventData, List<RaycastResult> results)
        {
            if (toggle == null || !toggle.isActiveAndEnabled || !toggle.interactable)
            {
                return;
            }

            AddHit(toggle.gameObject, toggle.transform as RectTransform, eventData, results);
        }

        private void AddTooltipTriggerHits(PointerEventData eventData, List<RaycastResult> results)
        {
            var triggers = GetComponentsInChildren<TooltipTrigger>(false);
            foreach (var trigger in triggers)
            {
                if (trigger == null || !trigger.isActiveAndEnabled || trigger.Provider == null)
                {
                    continue;
                }

                AddHit(trigger.gameObject, trigger.transform as RectTransform, eventData, results);
            }
        }

        private void AddHit(GameObject target, RectTransform rect, PointerEventData eventData, List<RaycastResult> results)
        {
            if (target == null || rect == null ||
                !RectTransformUtility.RectangleContainsScreenPoint(rect, eventData.position, eventCamera))
            {
                return;
            }

            var canvas = GetParentCanvas();
            results.Add(new RaycastResult
            {
                gameObject = target,
                module = this,
                distance = 0f,
                depth = short.MaxValue,
                index = results.Count,
                sortingLayer = canvas != null ? canvas.sortingLayerID : 0,
                sortingOrder = canvas != null ? canvas.sortingOrder : 0,
                screenPosition = eventData.position,
                displayIndex = canvas != null ? canvas.targetDisplay : 0
            });
        }

        private Canvas GetParentCanvas()
        {
            if (parentCanvas == null)
            {
                parentCanvas = GetComponentInParent<Canvas>();
            }

            return parentCanvas;
        }
    }
}
