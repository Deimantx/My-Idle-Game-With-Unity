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
        [SerializeField] private bool warriorCompatible;
        [SerializeField] private int combatMinDamage;
        [SerializeField] private int combatMaxDamage;
        [SerializeField] private float combatAttackInterval = 2.5f;
        [SerializeField] private int combatAccuracy;
        [SerializeField] private int combatDefense;
        [SerializeField] private int combatMaxHealth;
        [SerializeField, Range(0f, 1f)] private float combatCriticalChance;
        [SerializeField] private float combatCriticalDamageBonus;
        [SerializeField] private List<ProfessionModifier> professionModifiers = new();

        public string ItemId => itemId;
        public string DisplayName => string.IsNullOrWhiteSpace(displayName) ? itemId : displayName;
        public EquipmentSlot Slot => slot;
        public bool TwoHanded => twoHanded;
        public bool IsWarriorCompatible => warriorCompatible;
        public int CombatMinDamage => combatMinDamage;
        public int CombatMaxDamage => combatMaxDamage;
        public float CombatAttackInterval => combatAttackInterval;
        public int CombatAccuracy => combatAccuracy;
        public int CombatDefense => combatDefense;
        public int CombatMaxHealth => combatMaxHealth;
        public float CombatCriticalChance => combatCriticalChance;
        public float CombatCriticalDamageBonus => combatCriticalDamageBonus;
        public IReadOnlyList<ProfessionModifier> ProfessionModifiers => professionModifiers;

        public void ConfigureForEditor(string stableItemId, string itemDisplayName, EquipmentSlot equipmentSlot, bool isTwoHanded)
        {
            StableId.ThrowIfInvalid(stableItemId, nameof(stableItemId));
            itemId = stableItemId;
            displayName = itemDisplayName;
            slot = equipmentSlot;
            twoHanded = isTwoHanded;
        }

        public void ConfigureCombatForEditor(
            bool compatibleWithWarrior,
            int minDamage,
            int maxDamage,
            float attackSeconds,
            int accuracyBonus,
            int defenseBonus,
            int maxHealthBonus,
            float criticalChanceBonus,
            float criticalDamageMultiplierBonus)
        {
            warriorCompatible = compatibleWithWarrior;
            combatMinDamage = Mathf.Max(0, minDamage);
            combatMaxDamage = Mathf.Max(combatMinDamage, maxDamage);
            combatAttackInterval = Mathf.Max(0.05f, attackSeconds);
            combatAccuracy = accuracyBonus;
            combatDefense = defenseBonus;
            combatMaxHealth = maxHealthBonus;
            combatCriticalChance = Mathf.Clamp(criticalChanceBonus, -1f, 1f);
            combatCriticalDamageBonus = Mathf.Max(0f, criticalDamageMultiplierBonus);
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
