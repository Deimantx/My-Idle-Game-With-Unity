using System;

namespace IdleGame.Combat
{
    [Serializable]
    public sealed class DevotionEffectDefinition
    {
        public string devotionEffectId = string.Empty;
        public string displayName = string.Empty;
        public float devotionDrainPerSecond;
        public float damageMultiplier = 1f;
        public int requiredWarriorLevel;
        public bool deactivateAtZero = true;
    }
}
