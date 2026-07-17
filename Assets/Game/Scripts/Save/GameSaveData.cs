using System;
using System.Collections.Generic;

namespace IdleGame.Save
{
    [Serializable]
    public sealed class GameSaveData
    {
        public int saveVersion = 1;
        public InventorySaveData inventory = new();
        public EquipmentSaveData equipment = new();
        public ProfessionSaveData professions = new();
        public WoodcuttingSaveData woodcutting = new();
        public CombatSaveData combat = new();
        public string lastSaveUtc = string.Empty;
    }

    [Serializable]
    public sealed class InventorySaveData
    {
        public int capacity = 100;
        public long gold = 50;
        public List<InventoryStackSaveData> stacks = new();
    }

    [Serializable]
    public sealed class InventoryStackSaveData
    {
        public string itemId = string.Empty;
        public long quantity;
        public bool locked;
        public bool favorite;
        public bool isNew;
    }

    [Serializable]
    public sealed class EquipmentSaveData
    {
        public string mainHandItemId = string.Empty;
        public string offhandItemId = string.Empty;
    }

    [Serializable]
    public sealed class ProfessionSaveData
    {
        public List<ProfessionProgressSaveData> progress = new();
    }

    [Serializable]
    public sealed class ProfessionProgressSaveData
    {
        public string professionId = string.Empty;
        public int level = 1;
        public long xp;
    }

    [Serializable]
    public sealed class WoodcuttingSaveData
    {
        public string selectedTreeId = "tree_sproutwood";
        public bool isActive;
        public float currentDurability = 25f;
        public float actionProgressSeconds;
        public bool isRespawning;
        public float respawnRemainingSeconds;
        public List<string> completedThresholdKeys = new();
    }

    [Serializable]
    public sealed class CombatSaveData
    {
        public string selectedRegionId = "region_greenvale";
        public string selectedActivityTypeId = "combat_type_areas";
        public string selectedLocationId = "location_greenvale_forest";
        public string selectedEnemyId = "enemy_forest_rat";
        public bool autoRepeat = true;
        public bool heavyStrikeAutoUse = true;
        public List<string> firstClearEnemyIds = new();
        public List<CombatKillCountSaveData> killCounts = new();
    }

    [Serializable]
    public sealed class CombatKillCountSaveData
    {
        public string enemyId = string.Empty;
        public int count;
    }
}
