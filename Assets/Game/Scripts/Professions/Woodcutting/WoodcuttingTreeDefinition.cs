using System.Collections.Generic;
using System.Linq;
using IdleGame.Core.Identifiers;
using UnityEngine;

namespace IdleGame.Professions.Woodcutting
{
    [CreateAssetMenu(menuName = "Idle Game/Woodcutting/Tree Definition", fileName = "WoodcuttingTreeDefinition")]
    public sealed class WoodcuttingTreeDefinition : ScriptableObject
    {
        [SerializeField] private string treeId = string.Empty;
        [SerializeField] private string displayName = string.Empty;
        [SerializeField, TextArea] private string description = string.Empty;
        [SerializeField] private int requiredLevel = 1;
        [SerializeField] private float maximumDurability = 25f;
        [SerializeField] private float respawnSeconds = 3f;
        [SerializeField] private long completionXp = 10;
        [SerializeField] private List<WoodcuttingThresholdReward> thresholdRewards = new();

        public string TreeId => treeId;
        public string DisplayName => string.IsNullOrWhiteSpace(displayName) ? treeId : displayName;
        public string Description => description;
        public int RequiredLevel => requiredLevel;
        public float MaximumDurability => maximumDurability;
        public float RespawnSeconds => respawnSeconds;
        public long CompletionXp => completionXp;
        public IReadOnlyList<WoodcuttingThresholdReward> ThresholdRewards => thresholdRewards;

        public void ConfigureForEditor(
            string stableTreeId,
            string treeDisplayName,
            string treeDescription,
            int levelRequirement,
            float maxDurability,
            float respawnTime,
            long xp,
            IEnumerable<WoodcuttingThresholdReward> rewards)
        {
            StableId.ThrowIfInvalid(stableTreeId, nameof(stableTreeId));
            treeId = stableTreeId;
            displayName = treeDisplayName;
            description = treeDescription;
            requiredLevel = levelRequirement;
            maximumDurability = maxDurability;
            respawnSeconds = respawnTime;
            completionXp = xp;
            thresholdRewards = rewards?.ToList() ?? new List<WoodcuttingThresholdReward>();
        }
    }
}
