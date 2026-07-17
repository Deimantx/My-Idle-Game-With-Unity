using IdleGame.Inventory;
using IdleGame.Items;
using IdleGame.UI.Shared;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IdleGame.UI.Inventory
{
    public sealed class InventorySlotView : MonoBehaviour
    {
        [SerializeField] private TMP_Text glyphText;
        [SerializeField] private TMP_Text quantityText;
        [SerializeField] private TMP_Text badgeText;
        [SerializeField] private Button button;

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
        }

        public void AutoBind()
        {
            glyphText ??= HierarchySearch.FindText(transform, "[TEXT] ItemGlyph");
            quantityText ??= HierarchySearch.FindText(transform, "[TEXT] Quantity");
            badgeText ??= HierarchySearch.FindText(transform, "[BADGE] ItemStateBadge");
            button ??= GetComponent<Button>() ?? gameObject.AddComponent<Button>();
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
