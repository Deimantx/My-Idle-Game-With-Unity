using System.Collections.Generic;
using System.Linq;
using IdleGame.Inventory;
using IdleGame.Items;
using IdleGame.UI.Shared;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IdleGame.UI.Inventory
{
    public sealed class InventoryScreenController : MonoBehaviour
    {
        [SerializeField] private InventorySystem inventorySystem;
        [SerializeField] private TMP_Text capacityText;
        [SerializeField] private Transform itemContainer;
        [SerializeField] private Transform emptyState;
        [SerializeField] private TMP_Text selectedItemNameText;
        [SerializeField] private TMP_Text selectedItemCategoryText;
        [SerializeField] private TMP_Text selectedItemQuantityText;
        [SerializeField] private TMP_Text selectedItemDescriptionText;
        [SerializeField] private TMP_Text selectedItemGlyphText;
        [SerializeField] private Image selectedItemIconImage;
        [SerializeField] private TMP_Text sellValueText;
        [SerializeField] private TMP_Text itemStatsText;
        [SerializeField] private TMP_Text sourceListText;

        private readonly List<InventorySlotView> slotPool = new();
        private string selectedItemId = string.Empty;

        private void Awake()
        {
            AutoBind();
        }

        private void OnEnable()
        {
            if (inventorySystem != null)
            {
                inventorySystem.InventoryChanged -= Refresh;
                inventorySystem.InventoryChanged += Refresh;
            }

            Refresh();
        }

        private void OnDisable()
        {
            if (inventorySystem != null)
            {
                inventorySystem.InventoryChanged -= Refresh;
            }
        }

        public void ConfigureForEditor(InventorySystem inventory)
        {
            inventorySystem = inventory;
            AutoBind();
        }

        public void SelectStack(string itemId)
        {
            selectedItemId = itemId;
            RefreshDetails();
        }

        public void AutoBind()
        {
            capacityText ??= HierarchySearch.FindText(transform, "[TEXT] InventoryCapacityValue");
            itemContainer ??= HierarchySearch.FindDeep(transform, "[DYNAMIC CONTENT] InventoryItemContainer");
            emptyState ??= HierarchySearch.FindDeep(transform, "[PANEL] InventoryEmptyState");
            selectedItemNameText ??= HierarchySearch.FindText(transform, "[TEXT] ItemName");
            selectedItemCategoryText ??= HierarchySearch.FindText(transform, "[TEXT] ItemCategory");
            selectedItemQuantityText ??= HierarchySearch.FindText(transform, "[TEXT] ItemQuantity");
            selectedItemDescriptionText ??= HierarchySearch.FindText(transform, "[TEXT] ItemDescription");
            var selectedItemSection = HierarchySearch.FindDeep(transform, "[SECTION] SelectedInventoryItemSection");
            var detailsGlyph = HierarchySearch.FindText(selectedItemSection, "[TEXT] ItemGlyph");
            var detailsIcon = HierarchySearch.FindImage(selectedItemSection, "[IMAGE] ItemIcon");
            if (detailsGlyph != null)
            {
                selectedItemGlyphText = detailsGlyph;
            }
            else
            {
                selectedItemGlyphText ??= HierarchySearch.FindText(transform, "[TEXT] ItemGlyph");
            }

            if (detailsIcon != null)
            {
                selectedItemIconImage = detailsIcon;
            }
            else if (selectedItemIconImage == null && selectedItemGlyphText != null)
            {
                selectedItemIconImage = selectedItemGlyphText.GetComponentInParent<Image>();
            }

            if (selectedItemIconImage == null)
            {
                selectedItemIconImage = HierarchySearch.FindImage(selectedItemSection, "[IMAGE] ItemIcon");
            }
            sellValueText ??= HierarchySearch.FindText(transform, "[TEXT] SellValue");
            itemStatsText ??= HierarchySearch.FindText(transform, "[TEXT] ItemStats");
            sourceListText ??= HierarchySearch.FindText(transform, "[TEXT] SourceList");

            RebuildSlotPool();
        }

        private void Refresh()
        {
            if (inventorySystem == null)
            {
                return;
            }

            if (capacityText != null)
            {
                capacityText.text = $"{inventorySystem.UsedSlots} / {inventorySystem.Capacity}";
            }

            var stacks = inventorySystem.Stacks
                .Where(stack => stack != null && stack.quantity > 0)
                .OrderBy(stack => stack.itemId)
                .ToList();

            if (string.IsNullOrWhiteSpace(selectedItemId) || stacks.All(stack => stack.itemId != selectedItemId))
            {
                selectedItemId = stacks.Count > 0 ? stacks[0].itemId : string.Empty;
            }

            EnsureSlotCapacity(stacks.Count);

            for (var i = 0; i < slotPool.Count; i++)
            {
                var slot = slotPool[i];
                if (i >= stacks.Count)
                {
                    slot.gameObject.SetActive(false);
                    continue;
                }

                var stack = stacks[i];
                inventorySystem.ItemDatabase.TryGetItem(stack.itemId, out var item);
                slot.gameObject.SetActive(true);
                slot.Bind(stack, item, this);
            }

            if (emptyState != null)
            {
                emptyState.gameObject.SetActive(stacks.Count == 0);
            }

            RefreshDetails();
        }

        private void RefreshDetails()
        {
            if (inventorySystem == null)
            {
                return;
            }

            var stack = inventorySystem.Stacks.FirstOrDefault(stack => stack != null && stack.itemId == selectedItemId && stack.quantity > 0);
            if (stack == null || !inventorySystem.ItemDatabase.TryGetItem(stack.itemId, out var item) || item == null)
            {
                SetDetailsEmpty();
                return;
            }

            if (selectedItemNameText != null)
            {
                selectedItemNameText.text = item.DisplayName;
            }

            if (selectedItemCategoryText != null)
            {
                selectedItemCategoryText.text = string.IsNullOrWhiteSpace(item.ItemType)
                    ? item.Category.ToString()
                    : $"{item.Category} - {item.ItemType}";
            }

            if (selectedItemQuantityText != null)
            {
                selectedItemQuantityText.text = $"Quantity Owned\n{stack.quantity:N0}";
            }

            if (selectedItemDescriptionText != null)
            {
                selectedItemDescriptionText.text = item.Description;
            }

            if (selectedItemGlyphText != null)
            {
                selectedItemGlyphText.text = MakeGlyph(item);
                selectedItemGlyphText.gameObject.SetActive(item.Icon == null);
            }

            if (selectedItemIconImage != null)
            {
                selectedItemIconImage.sprite = item.Icon;
                selectedItemIconImage.preserveAspect = true;
                selectedItemIconImage.color = item.Icon == null ? new Color(0.075f, 0.095f, 0.10f, 0.98f) : Color.white;
            }

            if (sellValueText != null)
            {
                sellValueText.text = item.SellValue.ToString();
            }

            if (itemStatsText != null)
            {
                itemStatsText.text = item.Stackable ? $"Stackable up to {item.MaxStack:N0}" : "Non-stackable";
            }

            if (sourceListText != null)
            {
                sourceListText.text = item.Category == ItemCategory.Resource ? "Profession rewards" : "Inventory";
            }
        }

        private void SetDetailsEmpty()
        {
            if (selectedItemNameText != null) selectedItemNameText.text = "No item selected";
            if (selectedItemCategoryText != null) selectedItemCategoryText.text = string.Empty;
            if (selectedItemQuantityText != null) selectedItemQuantityText.text = string.Empty;
            if (selectedItemDescriptionText != null) selectedItemDescriptionText.text = "Gain items from Woodcutting to fill your inventory.";
            if (selectedItemGlyphText != null)
            {
                selectedItemGlyphText.text = string.Empty;
                selectedItemGlyphText.gameObject.SetActive(true);
            }
            if (selectedItemIconImage != null)
            {
                selectedItemIconImage.sprite = null;
            }
            if (sellValueText != null) sellValueText.text = "0";
            if (itemStatsText != null) itemStatsText.text = string.Empty;
            if (sourceListText != null) sourceListText.text = string.Empty;
        }

        private void RebuildSlotPool()
        {
            slotPool.Clear();
            if (itemContainer == null)
            {
                return;
            }

            for (var i = 0; i < itemContainer.childCount; i++)
            {
                var child = itemContainer.GetChild(i);
                var slot = child.GetComponent<InventorySlotView>() ?? child.gameObject.AddComponent<InventorySlotView>();
                slot.AutoBind();
                slotPool.Add(slot);
            }
        }

        private void EnsureSlotCapacity(int count)
        {
            if (itemContainer == null || slotPool.Count == 0)
            {
                return;
            }

            var template = slotPool[0].gameObject;
            while (slotPool.Count < count)
            {
                var clone = Instantiate(template, itemContainer);
                clone.name = $"[SLOT] InventorySlot{slotPool.Count + 1:00}";
                var slot = clone.GetComponent<InventorySlotView>() ?? clone.AddComponent<InventorySlotView>();
                slot.AutoBind();
                slotPool.Add(slot);
            }
        }

        private static string MakeGlyph(ItemDefinition item)
        {
            if (item == null || string.IsNullOrWhiteSpace(item.DisplayName))
            {
                return "?";
            }

            var compact = item.DisplayName.Replace(" ", string.Empty);
            return compact.Length <= 4 ? compact.ToUpperInvariant() : compact.Substring(0, 4).ToUpperInvariant();
        }
    }
}
