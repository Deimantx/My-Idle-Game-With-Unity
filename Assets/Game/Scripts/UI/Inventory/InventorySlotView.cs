using IdleGame.Inventory;
using IdleGame.Items;
using IdleGame.UI.Shared;
using IdleGame.UI.Tooltips;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IdleGame.UI.Inventory
{
    public sealed class InventorySlotView : MonoBehaviour
    {
        [SerializeField] private TMP_Text glyphText;
        [SerializeField] private Image iconImage;
        [SerializeField] private TMP_Text quantityText;
        [SerializeField] private TMP_Text badgeText;
        [SerializeField] private Button button;
        [SerializeField] private TooltipTrigger tooltipTrigger;
        [SerializeField] private ItemTooltipProvider tooltipProvider;

        private InventoryStack stack;
        private ItemDefinition item;
        private InventoryScreenController owner;

        public void Bind(InventoryStack inventoryStack, ItemDefinition itemDefinition, InventoryScreenController screenController)
        {
            AutoBind();
            stack = inventoryStack;
            item = itemDefinition;
            owner = screenController;

            if (glyphText != null)
            {
                glyphText.text = MakeGlyph(itemDefinition);
                glyphText.gameObject.SetActive(itemDefinition == null || itemDefinition.Icon == null);
            }

            if (iconImage != null)
            {
                iconImage.sprite = itemDefinition != null ? itemDefinition.Icon : null;
                iconImage.preserveAspect = true;
                iconImage.color = itemDefinition != null && itemDefinition.Icon != null ? Color.white : new Color(0f, 0f, 0f, 0f);
                iconImage.gameObject.SetActive(itemDefinition != null && itemDefinition.Icon != null);
            }

            if (quantityText != null)
            {
                quantityText.text = FormatQuantity(inventoryStack.quantity);
            }

            if (badgeText != null)
            {
                badgeText.text = inventoryStack.isNew ? "N" : inventoryStack.locked ? "L" : inventoryStack.favorite ? "F" : string.Empty;
                badgeText.transform.parent.gameObject.SetActive(!string.IsNullOrEmpty(badgeText.text));
            }

            if (button != null)
            {
                button.onClick.RemoveListener(OnClicked);
                button.onClick.AddListener(OnClicked);
            }

            if (tooltipProvider != null)
            {
                tooltipProvider.ConfigureForEditor(
                    inventoryStack.itemId,
                    itemDefinition,
                    screenController != null ? screenController.InventorySystem : null,
                    null,
                    null,
                    true,
                    false);
            }
        }

        public void AutoBind()
        {
            glyphText ??= HierarchySearch.FindText(transform, "[TEXT] ItemGlyph");
            iconImage ??= HierarchySearch.FindImage(transform, "[IMAGE] ItemIcon");
            quantityText ??= HierarchySearch.FindText(transform, "[TEXT] Quantity");
            badgeText ??= HierarchySearch.FindText(transform, "[BADGE] ItemStateBadge");
            button ??= GetComponent<Button>() ?? gameObject.AddComponent<Button>();
            tooltipProvider ??= GetComponent<ItemTooltipProvider>() ?? gameObject.AddComponent<ItemTooltipProvider>();
            tooltipTrigger ??= GetComponent<TooltipTrigger>() ?? gameObject.AddComponent<TooltipTrigger>();
            tooltipTrigger.ConfigureForEditor(tooltipProvider);
        }

        private void OnClicked()
        {
            if (owner != null && stack != null)
            {
                owner.SelectStack(stack.itemId);
            }
        }

        private static string MakeGlyph(ItemDefinition itemDefinition)
        {
            if (itemDefinition == null || string.IsNullOrWhiteSpace(itemDefinition.DisplayName))
            {
                return "?";
            }

            var compact = itemDefinition.DisplayName.Replace(" ", string.Empty);
            return compact.Length <= 4 ? compact.ToUpperInvariant() : compact.Substring(0, 4).ToUpperInvariant();
        }

        private static string FormatQuantity(long quantity)
        {
            return quantity >= 1000 ? quantity.ToString("N0") : quantity.ToString();
        }
    }
}
