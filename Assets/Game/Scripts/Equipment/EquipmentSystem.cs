using System;
using System.Collections.Generic;
using IdleGame.Inventory;
using IdleGame.Core.Bootstrap;
using IdleGame.Save;
using UnityEngine;

namespace IdleGame.Equipment
{
    public sealed class EquipmentSystem : MonoBehaviour, IGameService
    {
        [SerializeField] private EquipmentDatabase equipmentDatabase;
        [SerializeField] private InventorySystem inventorySystem;
        [SerializeField] private string mainHandItemId = string.Empty;
        [SerializeField] private string offhandItemId = string.Empty;

        public event Action EquipmentChanged;

        public int InitializationOrder => 20;
        public string MainHandItemId => mainHandItemId;
        public string OffhandItemId => offhandItemId;
        public bool EquipmentLocked { get; private set; }

        public void InitializeService()
        {
            LoadFromSave(SaveManager.Instance?.Data.equipment);
            if (string.IsNullOrWhiteSpace(mainHandItemId))
            {
                mainHandItemId = "weapon_worn_sword";
            }
        }

        public IEnumerable<EquipmentDefinition> GetEquippedItems()
        {
            if (equipmentDatabase == null)
            {
                yield break;
            }

            if (!string.IsNullOrWhiteSpace(mainHandItemId) && equipmentDatabase.TryGetEquipment(mainHandItemId, out var mainHand))
            {
                yield return mainHand;
            }

            if (!string.IsNullOrWhiteSpace(offhandItemId) && equipmentDatabase.TryGetEquipment(offhandItemId, out var offhand))
            {
                yield return offhand;
            }
        }

        public EquipmentSaveData CreateSaveData()
        {
            return new EquipmentSaveData
            {
                mainHandItemId = mainHandItemId,
                offhandItemId = offhandItemId
            };
        }

        public bool TryEquipItem(string itemId, out string reason)
        {
            reason = string.Empty;
            if (EquipmentLocked)
            {
                reason = "Equipment cannot be changed during active Combat.";
                return false;
            }

            if (equipmentDatabase == null || !equipmentDatabase.TryGetEquipment(itemId, out var equipment) || equipment == null)
            {
                reason = "That item is not equipment.";
                return false;
            }

            if (inventorySystem != null && inventorySystem.GetQuantity(itemId) <= 0)
            {
                reason = "The item is not in Inventory.";
                return false;
            }

            var displaced = GetEquippedItemId(equipment.Slot);
            var offhandDisplacedByTwoHanded = equipment.Slot == EquipmentSlot.MainHand && equipment.TwoHanded
                ? offhandItemId
                : string.Empty;

            var displacedCount = 0;
            if (!string.IsNullOrWhiteSpace(displaced))
            {
                displacedCount++;
            }

            if (!string.IsNullOrWhiteSpace(offhandDisplacedByTwoHanded) && offhandDisplacedByTwoHanded != displaced)
            {
                displacedCount++;
            }

            if (inventorySystem != null)
            {
                if (!inventorySystem.TryRemoveItem(itemId, 1))
                {
                    reason = "Could not remove the item from Inventory.";
                    return false;
                }

                if (displacedCount > 0 && inventorySystem.FreeSlots < displacedCount)
                {
                    inventorySystem.TryAddItem(itemId, 1);
                    reason = "Inventory needs room for displaced equipment.";
                    return false;
                }
            }

            SetEquippedItemId(equipment.Slot, itemId);

            if (equipment.Slot == EquipmentSlot.MainHand && equipment.TwoHanded)
            {
                if (!string.IsNullOrWhiteSpace(offhandDisplacedByTwoHanded))
                {
                    inventorySystem?.TryAddItem(offhandDisplacedByTwoHanded, 1);
                }

                offhandItemId = string.Empty;
            }

            if (!string.IsNullOrWhiteSpace(displaced))
            {
                inventorySystem?.TryAddItem(displaced, 1);
            }

            EquipmentChanged?.Invoke();
            SaveManager.Instance?.SaveNow();
            return true;
        }

        public bool TryUnequip(EquipmentSlot slot, out string reason)
        {
            reason = string.Empty;
            if (EquipmentLocked)
            {
                reason = "Equipment cannot be changed during active Combat.";
                return false;
            }

            var itemId = GetEquippedItemId(slot);
            if (string.IsNullOrWhiteSpace(itemId))
            {
                return true;
            }

            if (inventorySystem != null && !inventorySystem.CanAccept(itemId, 1))
            {
                reason = "Inventory is full.";
                return false;
            }

            SetEquippedItemId(slot, string.Empty);
            inventorySystem?.TryAddItem(itemId, 1);
            EquipmentChanged?.Invoke();
            SaveManager.Instance?.SaveNow();
            return true;
        }

        public void SetEquipmentLocked(bool locked)
        {
            if (EquipmentLocked == locked)
            {
                return;
            }

            EquipmentLocked = locked;
            EquipmentChanged?.Invoke();
        }

        public EquipmentDefinition GetMainHandDefinition()
        {
            return equipmentDatabase != null && !string.IsNullOrWhiteSpace(mainHandItemId) &&
                   equipmentDatabase.TryGetEquipment(mainHandItemId, out var definition)
                ? definition
                : null;
        }

        public void ConfigureForEditor(EquipmentDatabase database)
        {
            equipmentDatabase = database;
        }

        public void ConfigureInventoryForEditor(InventorySystem inventory)
        {
            inventorySystem = inventory;
        }

        private void LoadFromSave(EquipmentSaveData saveData)
        {
            mainHandItemId = saveData?.mainHandItemId ?? string.Empty;
            offhandItemId = saveData?.offhandItemId ?? string.Empty;
            EquipmentChanged?.Invoke();
        }

        private string GetEquippedItemId(EquipmentSlot slot)
        {
            return slot switch
            {
                EquipmentSlot.MainHand => mainHandItemId,
                EquipmentSlot.Offhand => offhandItemId,
                _ => string.Empty
            };
        }

        private void SetEquippedItemId(EquipmentSlot slot, string itemId)
        {
            switch (slot)
            {
                case EquipmentSlot.MainHand:
                    mainHandItemId = itemId ?? string.Empty;
                    break;
                case EquipmentSlot.Offhand:
                    offhandItemId = itemId ?? string.Empty;
                    break;
            }
        }
    }
}
