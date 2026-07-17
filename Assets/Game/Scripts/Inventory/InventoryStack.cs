using System;

namespace IdleGame.Inventory
{
    [Serializable]
    public sealed class InventoryStack
    {
        public string itemId;
        public long quantity;
        public bool locked;
        public bool favorite;
        public bool isNew;

        public InventoryStack()
        {
        }

        public InventoryStack(string itemId, long quantity)
        {
            this.itemId = itemId;
            this.quantity = quantity;
            isNew = true;
        }
    }
}
