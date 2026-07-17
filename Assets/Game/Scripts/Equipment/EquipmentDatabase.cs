using System;
using System.Collections.Generic;
using System.Linq;
using IdleGame.Core.Identifiers;
using UnityEngine;

namespace IdleGame.Equipment
{
    [CreateAssetMenu(menuName = "Idle Game/Equipment/Equipment Database", fileName = "EquipmentDatabase")]
    public sealed class EquipmentDatabase : ScriptableObject
    {
        [SerializeField] private List<EquipmentDefinition> equipment = new();

        private Dictionary<string, EquipmentDefinition> lookup;

        public IReadOnlyList<EquipmentDefinition> Equipment => equipment;

        public bool TryGetEquipment(string itemId, out EquipmentDefinition definition)
        {
            BuildLookupIfNeeded();
            return lookup.TryGetValue(itemId, out definition);
        }

        public void ConfigureForEditor(IEnumerable<EquipmentDefinition> definitions)
        {
            equipment = definitions?.Where(definition => definition != null).ToList() ?? new List<EquipmentDefinition>();
            lookup = null;
        }

        private void BuildLookupIfNeeded()
        {
            if (lookup != null)
            {
                return;
            }

            lookup = new Dictionary<string, EquipmentDefinition>(StringComparer.Ordinal);
            foreach (var definition in equipment.Where(definition => definition != null))
            {
                StableId.ThrowIfInvalid(definition.ItemId, nameof(definition.ItemId));
                lookup.TryAdd(definition.ItemId, definition);
            }
        }
    }
}
