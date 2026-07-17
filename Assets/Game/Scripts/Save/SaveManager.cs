using System;
using System.IO;
using IdleGame.Core.Bootstrap;
using IdleGame.Equipment;
using IdleGame.Inventory;
using IdleGame.Professions.Woodcutting;
using IdleGame.Progression;
using UnityEngine;

namespace IdleGame.Save
{
    public sealed class SaveManager : MonoBehaviour, IGameService
    {
        [SerializeField] private string fileName = "idle_save.json";
        [SerializeField] private InventorySystem inventorySystem;
        [SerializeField] private EquipmentSystem equipmentSystem;
        [SerializeField] private ProfessionProgressionSystem progressionSystem;
        [SerializeField] private WoodcuttingSystem woodcuttingSystem;

        public static SaveManager Instance { get; private set; }

        public int InitializationOrder => 0;
        public GameSaveData Data { get; private set; } = new();
        public string SavePath => Path.Combine(Application.persistentDataPath, fileName);

        public void InitializeService()
        {
            Instance = this;
            LoadFromDisk();
        }

        public void SaveNow()
        {
            if (Data == null)
            {
                Data = new GameSaveData();
            }

            if (inventorySystem != null)
            {
                Data.inventory = inventorySystem.CreateSaveData();
            }

            if (equipmentSystem != null)
            {
                Data.equipment = equipmentSystem.CreateSaveData();
            }

            if (progressionSystem != null)
            {
                Data.professions = progressionSystem.CreateSaveData();
            }

            if (woodcuttingSystem != null)
            {
                Data.woodcutting = woodcuttingSystem.CreateSaveData();
            }

            Data.lastSaveUtc = DateTime.UtcNow.ToString("O");
            Directory.CreateDirectory(Application.persistentDataPath);

            var json = JsonUtility.ToJson(Data, true);
            var tempPath = SavePath + ".tmp";
            var backupPath = SavePath + ".bak";
            File.WriteAllText(tempPath, json);

            if (File.Exists(SavePath))
            {
                File.Copy(SavePath, backupPath, true);
            }

            if (File.Exists(SavePath))
            {
                File.Delete(SavePath);
            }

            File.Move(tempPath, SavePath);
        }

        public void ConfigureForEditor(
            InventorySystem inventory,
            EquipmentSystem equipment,
            ProfessionProgressionSystem progression,
            WoodcuttingSystem woodcutting)
        {
            inventorySystem = inventory;
            equipmentSystem = equipment;
            progressionSystem = progression;
            woodcuttingSystem = woodcutting;
        }

        private void LoadFromDisk()
        {
            Data = File.Exists(SavePath)
                ? TryLoadFile(SavePath) ?? TryLoadFile(SavePath + ".bak") ?? new GameSaveData()
                : new GameSaveData();

            Data.inventory ??= new InventorySaveData();
            Data.equipment ??= new EquipmentSaveData();
            Data.professions ??= new ProfessionSaveData();
            Data.woodcutting ??= new WoodcuttingSaveData();
        }

        private static GameSaveData TryLoadFile(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            try
            {
                return JsonUtility.FromJson<GameSaveData>(File.ReadAllText(path));
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"Failed to load save file '{path}': {exception.Message}");
                return null;
            }
        }
    }
}
