using System;
using System.Collections.Generic;
using System.Linq;
using IdleGame.Core.Identifiers;
using UnityEngine;

namespace IdleGame.Combat
{
    [CreateAssetMenu(menuName = "Idle Game/Combat/Combat Catalog", fileName = "CombatCatalog")]
    public sealed class CombatCatalog : ScriptableObject
    {
        [SerializeField] private string regionId = CombatConstants.RegionGreenvale;
        [SerializeField] private string regionDisplayName = "Greenvale";
        [SerializeField, TextArea] private string regionDescription = string.Empty;
        [SerializeField] private string activityTypeId = CombatConstants.ActivityTypeAreas;
        [SerializeField] private string activityTypeDisplayName = "Areas";
        [SerializeField] private string locationId = CombatConstants.LocationGreenvaleForest;
        [SerializeField] private string locationDisplayName = "Greenvale Forest";
        [SerializeField] private List<CombatEnemyDefinition> enemies = new();

        private Dictionary<string, CombatEnemyDefinition> enemyLookup;

        public string RegionId => regionId;
        public string RegionDisplayName => regionDisplayName;
        public string RegionDescription => regionDescription;
        public string ActivityTypeId => activityTypeId;
        public string ActivityTypeDisplayName => activityTypeDisplayName;
        public string LocationId => locationId;
        public string LocationDisplayName => locationDisplayName;
        public IReadOnlyList<CombatEnemyDefinition> Enemies => enemies;

        public bool TryGetEnemy(string enemyId, out CombatEnemyDefinition enemy)
        {
            BuildLookupIfNeeded();
            return enemyLookup.TryGetValue(enemyId, out enemy);
        }

        public CombatEnemyDefinition GetFirstEnemy()
        {
            return enemies.FirstOrDefault(enemy => enemy != null);
        }

        public void ConfigureForEditor(
            string stableRegionId,
            string regionName,
            string regionText,
            string stableActivityTypeId,
            string activityName,
            string stableLocationId,
            string locationName,
            IEnumerable<CombatEnemyDefinition> enemyDefinitions)
        {
            StableId.ThrowIfInvalid(stableRegionId, nameof(stableRegionId));
            StableId.ThrowIfInvalid(stableActivityTypeId, nameof(stableActivityTypeId));
            StableId.ThrowIfInvalid(stableLocationId, nameof(stableLocationId));
            regionId = stableRegionId;
            regionDisplayName = regionName;
            regionDescription = regionText;
            activityTypeId = stableActivityTypeId;
            activityTypeDisplayName = activityName;
            locationId = stableLocationId;
            locationDisplayName = locationName;
            enemies = enemyDefinitions?.Where(enemy => enemy != null).ToList() ?? new List<CombatEnemyDefinition>();
            enemyLookup = null;
        }

        private void BuildLookupIfNeeded()
        {
            if (enemyLookup != null)
            {
                return;
            }

            enemyLookup = new Dictionary<string, CombatEnemyDefinition>(StringComparer.Ordinal);
            foreach (var enemy in enemies.Where(enemy => enemy != null))
            {
                StableId.ThrowIfInvalid(enemy.EnemyId, nameof(enemy.EnemyId));
                enemyLookup.TryAdd(enemy.EnemyId, enemy);
            }
        }
    }
}
