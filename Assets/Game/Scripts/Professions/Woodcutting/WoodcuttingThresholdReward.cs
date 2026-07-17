using System;
using System.Collections.Generic;

namespace IdleGame.Professions.Woodcutting
{
    [Serializable]
    public sealed class WoodcuttingThresholdReward
    {
        public string thresholdKey = string.Empty;
        public float remainingDurabilityPercent;
        public List<WoodcuttingReward> rewards = new();
    }
}
