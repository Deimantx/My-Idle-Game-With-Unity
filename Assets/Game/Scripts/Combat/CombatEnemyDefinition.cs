using System.Collections.Generic;
using IdleGame.Core.Identifiers;
using UnityEngine;

namespace IdleGame.Combat
{
    [CreateAssetMenu(menuName = "Idle Game/Combat/Enemy Definition", fileName = "CombatEnemyDefinition")]
    public sealed class CombatEnemyDefinition : ScriptableObject
    {
        [SerializeField] private string enemyId = string.Empty;
        [SerializeField] private string displayName = string.Empty;
        [SerializeField, TextArea] private string description = string.Empty;
        [SerializeField] private int requiredWarriorLevel = 1;
        [SerializeField] private int maximumHealth = 30;
        [SerializeField] private int minDamage = 1;
        [SerializeField] private int maxDamage = 2;
        [SerializeField] private float attackInterval = 2f;
        [SerializeField] private int accuracy = 35;
        [SerializeField] private int defense = 5;
        [SerializeField, Range(0f, 1f)] private float criticalChance;
        [SerializeField] private float criticalDamageMultiplier = 1.5f;
        [SerializeField] private float warriorXpCoefficient = 1f;
        [SerializeField] private float respawnSeconds = 2f;
        [SerializeField] private int minGold = 1;
        [SerializeField] private int maxGold = 3;
        [SerializeField] private List<CombatLootEntry> loot = new();
        [SerializeField] private List<CombatLootEntry> firstClearRewards = new();

        public string EnemyId => enemyId;
        public string DisplayName => string.IsNullOrWhiteSpace(displayName) ? enemyId : displayName;
        public string Description => description;
        public int RequiredWarriorLevel => requiredWarriorLevel;
        public int MaximumHealth => maximumHealth;
        public int MinDamage => minDamage;
        public int MaxDamage => maxDamage;
        public float AttackInterval => attackInterval;
        public int Accuracy => accuracy;
        public int Defense => defense;
        public float CriticalChance => criticalChance;
        public float CriticalDamageMultiplier => criticalDamageMultiplier;
        public float WarriorXpCoefficient => warriorXpCoefficient;
        public float RespawnSeconds => respawnSeconds;
        public int MinGold => minGold;
        public int MaxGold => maxGold;
        public IReadOnlyList<CombatLootEntry> Loot => loot;
        public IReadOnlyList<CombatLootEntry> FirstClearRewards => firstClearRewards;

        public void ConfigureForEditor(
            string stableEnemyId,
            string enemyDisplayName,
            string enemyDescription,
            int requiredLevel,
            int maxHealth,
            int damageMin,
            int damageMax,
            float interval,
            int enemyAccuracy,
            int enemyDefense,
            float critChance,
            float critDamage,
            float xpCoefficient,
            float respawnTime,
            int goldMin,
            int goldMax,
            IEnumerable<CombatLootEntry> lootEntries,
            IEnumerable<CombatLootEntry> firstClearEntries)
        {
            StableId.ThrowIfInvalid(stableEnemyId, nameof(stableEnemyId));
            enemyId = stableEnemyId;
            displayName = enemyDisplayName;
            description = enemyDescription;
            requiredWarriorLevel = Mathf.Max(1, requiredLevel);
            maximumHealth = Mathf.Max(1, maxHealth);
            minDamage = Mathf.Max(0, damageMin);
            maxDamage = Mathf.Max(minDamage, damageMax);
            attackInterval = Mathf.Max(0.05f, interval);
            accuracy = Mathf.Max(0, enemyAccuracy);
            defense = Mathf.Max(0, enemyDefense);
            criticalChance = Mathf.Clamp01(critChance);
            criticalDamageMultiplier = Mathf.Max(1f, critDamage);
            warriorXpCoefficient = Mathf.Max(0f, xpCoefficient);
            respawnSeconds = Mathf.Max(0f, respawnTime);
            minGold = Mathf.Max(0, goldMin);
            maxGold = Mathf.Max(minGold, goldMax);
            loot = lootEntries != null ? new List<CombatLootEntry>(lootEntries) : new List<CombatLootEntry>();
            firstClearRewards = firstClearEntries != null ? new List<CombatLootEntry>(firstClearEntries) : new List<CombatLootEntry>();
        }
    }
}
