using IdleGame.Combat;
using UnityEngine;

namespace IdleGame.UI.Tooltips
{
    public sealed class CombatSkillTooltipProvider : MonoBehaviour, ITooltipDataProvider
    {
        [SerializeField] private CombatSystem combatSystem;
        [SerializeField] private string abilityId = CombatConstants.HeavyStrikeAbilityId;
        [SerializeField, TextArea] private string description = "A powerful strike dealing increased damage.";

        public void ConfigureForEditor(CombatSystem combat, string combatAbilityId)
        {
            combatSystem = combat;
            abilityId = combatAbilityId;
        }

        public TooltipData BuildTooltipData()
        {
            if (combatSystem == null || abilityId != CombatConstants.HeavyStrikeAbilityId)
            {
                return new TooltipData { DisplayName = "Combat Skill", Category = "Skill" };
            }

            var data = new TooltipData
            {
                SourceId = abilityId,
                DisplayName = combatSystem.HeavyStrikeDisplayName,
                Category = "Warrior Skill",
                Description = description
            };

            data.PrimaryStats.Add(new TooltipStatLine("Cooldown", $"{combatSystem.HeavyStrikeCooldownDurationSeconds:0.0}s"));
            data.PrimaryStats.Add(new TooltipStatLine("Action Time", $"{combatSystem.HeavyStrikeActionTimePreviewSeconds:0.0}s"));
            data.PrimaryStats.Add(new TooltipStatLine("Damage", $"{combatSystem.HeavyStrikeDamageMultiplierValue:0.##}x"));
            data.SecondaryStats.Add(new TooltipStatLine("Auto-cast", combatSystem.HeavyStrikeAutoUse ? "Enabled" : "Available"));
            if (combatSystem.HeavyStrikeCooldownRemaining > 0f)
            {
                data.SecondaryStats.Add(new TooltipStatLine("Current Cooldown", $"{combatSystem.HeavyStrikeCooldownRemaining:0.0}s", TooltipValueTone.Muted));
            }

            if (combatSystem.IsHeavyStrikeQueued)
            {
                data.SecondaryStats.Add(new TooltipStatLine("State", "Queued", TooltipValueTone.Positive));
            }
            else if (combatSystem.IsPerformingHeavyStrike)
            {
                data.SecondaryStats.Add(new TooltipStatLine("State", "Casting", TooltipValueTone.Positive));
            }
            else if (!string.IsNullOrWhiteSpace(combatSystem.HeavyStrikeUnavailableReason))
            {
                data.Requirements.Add(new TooltipStatLine("Unavailable", combatSystem.HeavyStrikeUnavailableReason, TooltipValueTone.Muted));
            }

            return data;
        }
    }
}
