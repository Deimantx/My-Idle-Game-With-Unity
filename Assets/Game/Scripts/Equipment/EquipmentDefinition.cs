using System;
using System.Collections.Generic;
using IdleGame.Core.Identifiers;
using UnityEngine;

namespace IdleGame.Equipment
{
    [CreateAssetMenu(menuName = "Idle Game/Equipment/Equipment Definition", fileName = "EquipmentDefinition")]
    public sealed class EquipmentDefinition : ScriptableObject
    {
        [SerializeField] private string itemId = string.Empty;
        [SerializeField] private string displayName = string.Empty;
        [SerializeField] private EquipmentSlot slot = EquipmentSlot.MainHand;
        [SerializeField] private bool twoHanded;
        [SerializeField] private List<ProfessionModifier> professionModifiers = new();

        public string ItemId => itemId;
        public string DisplayName => string.IsNullOrWhiteSpace(displayName) ? itemId : displayName;
        public EquipmentSlot Slot => slot;
        public bool TwoHanded => twoHanded;
        public IReadOnlyList<ProfessionModifier> ProfessionModifiers => professionModifiers;

        public void ConfigureForEditor(string stableItemId, string itemDisplayName, EquipmentSlot equipmentSlot, bool isTwoHanded)
        {
            StableId.ThrowIfInvalid(stableItemId, nameof(stableItemId));
            itemId = stableItemId;
            displayName = itemDisplayName;
            slot = equipmentSlot;
            twoHanded = isTwoHanded;
        }
    }

    [Serializable]
    public sealed class ProfessionModifier
    {
        public string professionId = string.Empty;
        public float flatPower;
        public float actionSpeedPercent;
        public float xpPercent;
        public float outputPercent;
    }
}
