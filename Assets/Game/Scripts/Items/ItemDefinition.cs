using IdleGame.Core.Identifiers;
using UnityEngine;

namespace IdleGame.Items
{
    [CreateAssetMenu(menuName = "Idle Game/Items/Item Definition", fileName = "ItemDefinition")]
    public sealed class ItemDefinition : ScriptableObject
    {
        [SerializeField] private string itemId = string.Empty;
        [SerializeField] private string displayName = string.Empty;
        [SerializeField, TextArea] private string description = string.Empty;
        [SerializeField] private ItemCategory category = ItemCategory.Resource;
        [SerializeField] private string itemType = string.Empty;
        [SerializeField] private Sprite icon;
        [SerializeField] private bool stackable = true;
        [SerializeField] private long maxStack = 999999;
        [SerializeField] private int sellValue;
        [SerializeField] private bool canBeSold = true;
        [SerializeField] private bool canBeDestroyed = true;

        public string ItemId => itemId;
        public string DisplayName => string.IsNullOrWhiteSpace(displayName) ? itemId : displayName;
        public string Description => description;
        public ItemCategory Category => category;
        public string ItemType => itemType;
        public Sprite Icon => icon;
        public bool Stackable => stackable;
        public long MaxStack => stackable ? Mathf.Max(1, (int)Mathf.Min(maxStack, int.MaxValue)) : 1;
        public int SellValue => sellValue;
        public bool CanBeSold => canBeSold;
        public bool CanBeDestroyed => canBeDestroyed;

        public void ConfigureForEditor(
            string stableItemId,
            string itemDisplayName,
            string itemDescription,
            ItemCategory itemCategory,
            string type,
            bool isStackable,
            long maximumStack,
            int value,
            bool sellable,
            bool destroyable)
        {
            StableId.ThrowIfInvalid(stableItemId, nameof(stableItemId));
            itemId = stableItemId;
            displayName = itemDisplayName;
            description = itemDescription;
            category = itemCategory;
            itemType = type;
            stackable = isStackable;
            maxStack = maximumStack;
            sellValue = value;
            canBeSold = sellable;
            canBeDestroyed = destroyable;
        }
    }
}
