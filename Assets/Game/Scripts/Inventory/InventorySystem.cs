using System;
using System.Collections.Generic;
using System.Linq;
using IdleGame.Core.Bootstrap;
using IdleGame.Core.Identifiers;
using IdleGame.Items;
using IdleGame.Save;
using UnityEngine;

namespace IdleGame.Inventory
{
    public sealed class InventorySystem : MonoBehaviour, IGameService
    {
        [SerializeField] private ItemDatabase itemDatabase;
        [SerializeField] private int capacity = 100;
        [SerializeField] private List<InventoryStack> stacks = new();

        public event Action InventoryChanged;
        public event Action<string, long> ItemAdded;

        public int InitializationOrder => 10;
        public int Capacity => capacity;
        public int UsedSlots => stacks.Count(stack => stack != null && stack.quantity > 0);
        public int FreeSlots => Mathf.Max(0, capacity - UsedSlots);
        public IReadOnlyList<InventoryStack> Stacks => stacks;
        public ItemDatabase ItemDatabase => itemDatabase;

        public void InitializeService()
        {
            if (itemDatabase == null)
            {
                throw new InvalidOperationException($"{nameof(InventorySystem)} needs an item database.");
            }

            if (SaveManager.Instance != null)
            {
                LoadFromSave(SaveManager.Instance.Data.inventory);
            }
        }

        public bool CanAccept(string itemId, long quantity)
        {
            if (quantity <= 0)
            {
                return true;
            }

            StableId.ThrowIfInvalid(itemId, nameof(itemId));
            if (!itemDatabase.TryGetItem(itemId, out var item) || item == null)
            {
                Debug.LogError($"Cannot add missing item '{itemId}' to inventory.", this);
                return false;
            }

            if (!item.Stackable)
            {
                return FreeSlots >= quantity;
            }

            var remaining = quantity;
            foreach (var stack in stacks.Where(stack => stack != null && stack.itemId == itemId && stack.quantity > 0))
            {
                remaining -= Math.Max(0, item.MaxStack - stack.quantity);
                if (remaining <= 0)
                {
                    return true;
                }
            }

            return remaining <= 0 || FreeSlots > 0;
        }

        public bool TryAddItem(string itemId, long quantity)
        {
            if (!CanAccept(itemId, quantity))
            {
                return false;
            }

            if (quantity <= 0)
            {
                return true;
            }

            var item = itemDatabase.GetRequired(itemId);
            var remaining = quantity;

            if (item.Stackable)
            {
                foreach (var stack in stacks.Where(stack => stack != null && stack.itemId == itemId && stack.quantity > 0))
                {
                    var available = Math.Max(0, item.MaxStack - stack.quantity);
                    var added = Math.Min(available, remaining);
                    stack.quantity += added;
                    remaining -= added;
                    if (remaining <= 0)
                    {
                        break;
                    }
                }
            }

            while (remaining > 0)
            {
                if (UsedSlots >= capacity)
                {
                    return false;
                }

                var quantityForStack = item.Stackable ? Math.Min(item.MaxStack, remaining) : 1;
                stacks.Add(new InventoryStack(itemId, quantityForStack));
                remaining -= quantityForStack;
            }

            ItemAdded?.Invoke(itemId, quantity);
            InventoryChanged?.Invoke();
            SaveManager.Instance?.SaveNow();
            return true;
        }

        public long GetQuantity(string itemId)
        {
            return stacks.Where(stack => stack != null && stack.itemId == itemId).Sum(stack => Math.Max(0, stack.quantity));
        }

        public InventorySaveData CreateSaveData()
        {
            return new InventorySaveData
            {
                capacity = capacity,
                stacks = stacks
                    .Where(stack => stack != null && stack.quantity > 0 && StableId.IsValid(stack.itemId))
                    .Select(stack => new InventoryStackSaveData
                    {
                        itemId = stack.itemId,
                        quantity = stack.quantity,
                        locked = stack.locked,
                        favorite = stack.favorite,
                        isNew = stack.isNew
                    })
                    .ToList()
            };
        }

        public void LoadFromSave(InventorySaveData saveData)
        {
            stacks.Clear();
            capacity = saveData != null && saveData.capacity > 0 ? saveData.capacity : 100;

            if (saveData?.stacks == null)
            {
                return;
            }

            foreach (var savedStack in saveData.stacks)
            {
                if (savedStack == null || savedStack.quantity <= 0 || !StableId.IsValid(savedStack.itemId))
                {
                    continue;
                }

                stacks.Add(new InventoryStack(savedStack.itemId, savedStack.quantity)
                {
                    locked = savedStack.locked,
                    favorite = savedStack.favorite,
                    isNew = savedStack.isNew
                });
            }

            InventoryChanged?.Invoke();
        }

        public void ConfigureForEditor(ItemDatabase database, int startingCapacity)
        {
            itemDatabase = database;
            capacity = startingCapacity;
        }
    }
}
