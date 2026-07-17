using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IdleGame.Progression
{
    [CreateAssetMenu(menuName = "Idle Game/Progression/Experience Table", fileName = "ExperienceTable")]
    public sealed class ExperienceTable : ScriptableObject
    {
        [SerializeField] private int maximumLevel = 100;
        [SerializeField] private List<long> totalXpByLevel = new()
        {
            0, 50, 125, 225, 350, 500, 700, 950, 1250, 1600, 2000, 2500, 3100, 3800, 4600
        };

        public int MaximumLevel => maximumLevel;

        public int GetLevelForXp(long xp)
        {
            var level = 1;
            for (var i = 0; i < totalXpByLevel.Count; i++)
            {
                if (xp >= totalXpByLevel[i])
                {
                    level = i + 1;
                }
            }

            return Mathf.Clamp(level, 1, maximumLevel);
        }

        public long GetTotalXpForLevel(int level)
        {
            if (level <= 1)
            {
                return 0;
            }

            var index = Mathf.Clamp(level - 1, 0, totalXpByLevel.Count - 1);
            if (index < totalXpByLevel.Count)
            {
                return totalXpByLevel[index];
            }

            return totalXpByLevel.LastOrDefault();
        }

        public long GetXpIntoLevel(long xp)
        {
            return xp - GetTotalXpForLevel(GetLevelForXp(xp));
        }

        public long GetXpNeededForCurrentLevel(long xp)
        {
            var level = GetLevelForXp(xp);
            if (level >= maximumLevel)
            {
                return 0;
            }

            return GetTotalXpForLevel(level + 1) - GetTotalXpForLevel(level);
        }
    }
}
