using System;
using IdleGame.Core.Identifiers;
using UnityEngine;

namespace IdleGame.Combat
{
    [Serializable]
    public sealed class CombatLootEntry
    {
        public string itemId = string.Empty;
        [Range(0f, 1f)] public float chance = 1f;
        public int minQuantity = 1;
        public int maxQuantity = 1;
        public bool important;

        public bool IsValid => StableId.IsValid(itemId) && maxQuantity > 0 && maxQuantity >= minQuantity;
    }
}
