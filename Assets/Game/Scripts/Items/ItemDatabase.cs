using System;
using System.Collections.Generic;
using System.Linq;
using IdleGame.Core.Identifiers;
using UnityEngine;

namespace IdleGame.Items
{
    [CreateAssetMenu(menuName = "Idle Game/Items/Item Database", fileName = "ItemDatabase")]
    public sealed class ItemDatabase : ScriptableObject
    {
        [SerializeField] private List<ItemDefinition> items = new();

        private Dictionary<string, ItemDefinition> lookup;

        public IReadOnlyList<ItemDefinition> Items => items;

        public bool TryGetItem(string itemId, out ItemDefinition definition)
        {
            BuildLookupIfNeeded();
            return lookup.TryGetValue(itemId, out definition);
        }

        public ItemDefinition GetRequired(string itemId)
        {
            if (!TryGetItem(itemId, out var definition) || definition == null)
            {
                throw new KeyNotFoundException($"Item '{itemId}' is not assigned in {name}.");
            }

            return definition;
        }

        public void ConfigureForEditor(IEnumerable<ItemDefinition> definitions)
        {
            items = definitions?.Where(item => item != null).ToList() ?? new List<ItemDefinition>();
            lookup = null;
        }

        private void BuildLookupIfNeeded()
        {
            if (lookup != null)
            {
                return;
            }

            lookup = new Dictionary<string, ItemDefinition>(StringComparer.Ordinal);

            foreach (var item in items.Where(item => item != null))
            {
                StableId.ThrowIfInvalid(item.ItemId, nameof(item.ItemId));
                if (!lookup.TryAdd(item.ItemId, item))
                {
                    Debug.LogError($"Duplicate item id '{item.ItemId}' in {name}.", this);
                }
            }
        }
    }
}
