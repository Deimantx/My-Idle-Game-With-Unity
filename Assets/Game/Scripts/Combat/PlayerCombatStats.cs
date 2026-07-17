using IdleGame.Equipment;
using UnityEngine;

namespace IdleGame.Combat
{
    public readonly struct PlayerCombatStats
    {
        public PlayerCombatStats(
            int maxHealth,
            int maxDevotion,
            int minDamage,
            int maxDamage,
            float attackInterval,
            int accuracy,
            int defense,
            float criticalChance,
            float criticalDamageMultiplier)
        {
            MaxHealth = maxHealth;
            MaxDevotion = maxDevotion;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            AttackInterval = attackInterval;
            Accuracy = accuracy;
            Defense = defense;
            CriticalChance = criticalChance;
            CriticalDamageMultiplier = criticalDamageMultiplier;
        }

        public int MaxHealth { get; }
        public int MaxDevotion { get; }
        public int MinDamage { get; }
        public int MaxDamage { get; }
        public float AttackInterval { get; }
        public int Accuracy { get; }
        public int Defense { get; }
        public float CriticalChance { get; }
        public float CriticalDamageMultiplier { get; }

        public static PlayerCombatStats Resolve(EquipmentSystem equipmentSystem)
        {
            var maxHealth = 100;
            var maxDevotion = 100;
            var minDamage = 1;
            var maxDamage = 2;
            var attackInterval = 2.5f;
            var accuracy = 60;
            var defense = 5;
            var criticalChance = 0.05f;
            var criticalDamage = 1.5f;
            var hasMainHand = false;

            if (equipmentSystem != null)
            {
                foreach (var equipment in equipmentSystem.GetEquippedItems())
                {
                    if (equipment == null)
                    {
                        continue;
                    }

                    maxHealth += equipment.CombatMaxHealth;
                    accuracy += equipment.CombatAccuracy;
                    defense += equipment.CombatDefense;
                    criticalChance += equipment.CombatCriticalChance;
                    criticalDamage += equipment.CombatCriticalDamageBonus;

                    if (equipment.Slot == EquipmentSlot.MainHand && equipment.IsWarriorCompatible)
                    {
                        hasMainHand = true;
                        minDamage = Mathf.Max(1, equipment.CombatMinDamage);
                        maxDamage = Mathf.Max(minDamage, equipment.CombatMaxDamage);
                        attackInterval = Mathf.Max(0.05f, equipment.CombatAttackInterval);
                    }
                }
            }

            if (!hasMainHand)
            {
                minDamage = 0;
                maxDamage = 0;
            }

            return new PlayerCombatStats(
                Mathf.Max(1, maxHealth),
                Mathf.Max(1, maxDevotion),
                minDamage,
                Mathf.Max(minDamage, maxDamage),
                Mathf.Max(0.05f, attackInterval),
                Mathf.Max(0, accuracy),
                Mathf.Max(0, defense),
                Mathf.Clamp01(criticalChance),
                Mathf.Max(1f, criticalDamage));
        }
    }
}
