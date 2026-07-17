using System;
using System.Collections.Generic;
using IdleGame.Core.Bootstrap;
using IdleGame.Save;
using UnityEngine;

namespace IdleGame.Equipment
{
    public sealed class EquipmentSystem : MonoBehaviour, IGameService
    {
        [SerializeField] private EquipmentDatabase equipmentDatabase;
        [SerializeField] private string mainHandItemId = string.Empty;
        [SerializeField] private string offhandItemId = string.Empty;

        public event Action EquipmentChanged;

        public int InitializationOrder => 20;
        public string MainHandItemId => mainHandItemId;
        public string OffhandItemId => offhandItemId;

        public void InitializeService()
        {
            LoadFromSave(SaveManager.Instance?.Data.equipment);
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

        public void ConfigureForEditor(EquipmentDatabase database)
        {
            equipmentDatabase = database;
        }

        private void LoadFromSave(EquipmentSaveData saveData)
        {
            mainHandItemId = saveData?.mainHandItemId ?? string.Empty;
            offhandItemId = saveData?.offhandItemId ?? string.Empty;
            EquipmentChanged?.Invoke();
        }
    }
}
