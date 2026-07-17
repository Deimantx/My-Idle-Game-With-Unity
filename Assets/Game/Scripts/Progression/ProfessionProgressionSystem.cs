using System;
using System.Collections.Generic;
using System.Linq;
using IdleGame.Core.Bootstrap;
using IdleGame.Core.Identifiers;
using IdleGame.Save;
using UnityEngine;

namespace IdleGame.Progression
{
    public sealed class ProfessionProgressionSystem : MonoBehaviour, IGameService
    {
        [SerializeField] private ExperienceTable experienceTable;
        [SerializeField] private List<ProfessionProgress> professions = new();

        public event Action<string> ProgressChanged;
        public event Action<string, int> LevelChanged;

        public int InitializationOrder => 30;

        public void InitializeService()
        {
            if (experienceTable == null)
            {
                throw new InvalidOperationException($"{nameof(ProfessionProgressionSystem)} needs an experience table.");
            }

            LoadFromSave(SaveManager.Instance?.Data.professions);
            EnsureProfession("profession_woodcutting");
        }

        public int GetLevel(string professionId)
        {
            return GetOrCreate(professionId).level;
        }

        public long GetXp(string professionId)
        {
            return GetOrCreate(professionId).xp;
        }

        public long GetXpIntoLevel(string professionId)
        {
            return experienceTable.GetXpIntoLevel(GetXp(professionId));
        }

        public long GetXpNeededForCurrentLevel(string professionId)
        {
            return experienceTable.GetXpNeededForCurrentLevel(GetXp(professionId));
        }

        public float GetLevelProgress01(string professionId)
        {
            var needed = GetXpNeededForCurrentLevel(professionId);
            return needed <= 0 ? 1f : Mathf.Clamp01((float)GetXpIntoLevel(professionId) / needed);
        }

        public void AddExperience(string professionId, long xp)
        {
            if (xp <= 0)
            {
                return;
            }

            var progress = GetOrCreate(professionId);
            var previousLevel = progress.level;
            progress.xp += xp;
            progress.level = experienceTable.GetLevelForXp(progress.xp);

            ProgressChanged?.Invoke(professionId);
            if (progress.level != previousLevel)
            {
                LevelChanged?.Invoke(professionId, progress.level);
            }

            SaveManager.Instance?.SaveNow();
        }

        public ProfessionSaveData CreateSaveData()
        {
            return new ProfessionSaveData
            {
                progress = professions
                    .Where(progress => progress != null && StableId.IsValid(progress.professionId))
                    .Select(progress => new ProfessionProgressSaveData
                    {
                        professionId = progress.professionId,
                        level = progress.level,
                        xp = progress.xp
                    })
                    .ToList()
            };
        }

        public void ConfigureForEditor(ExperienceTable table)
        {
            experienceTable = table;
        }

        private void LoadFromSave(ProfessionSaveData saveData)
        {
            professions.Clear();
            if (saveData?.progress == null)
            {
                return;
            }

            foreach (var saved in saveData.progress)
            {
                if (saved == null || !StableId.IsValid(saved.professionId))
                {
                    continue;
                }

                var xp = Math.Max(0, saved.xp);
                professions.Add(new ProfessionProgress
                {
                    professionId = saved.professionId,
                    xp = xp,
                    level = experienceTable.GetLevelForXp(xp)
                });
            }
        }

        private void EnsureProfession(string professionId)
        {
            GetOrCreate(professionId);
        }

        private ProfessionProgress GetOrCreate(string professionId)
        {
            StableId.ThrowIfInvalid(professionId, nameof(professionId));
            var progress = professions.FirstOrDefault(progress => progress != null && progress.professionId == professionId);
            if (progress != null)
            {
                return progress;
            }

            progress = new ProfessionProgress { professionId = professionId, level = 1, xp = 0 };
            professions.Add(progress);
            return progress;
        }

        [Serializable]
        private sealed class ProfessionProgress
        {
            public string professionId = string.Empty;
            public int level = 1;
            public long xp;
        }
    }
}
