using System;

namespace IdleGame.Combat
{
    public enum CombatAbilityActionTimeRule
    {
        FixedSeconds = 0,
        WeaponAttackIntervalMultiplier = 1
    }

    public enum CombatAbilityResourceInteraction
    {
        None = 0,
        ExplicitDevotion = 1
    }

    [Serializable]
    public sealed class CombatAbilityDefinition
    {
        public string abilityId = string.Empty;
        public string displayName = string.Empty;
        public CombatAbilityActionTimeRule actionTimeRule;
        public float fixedActionTimeSeconds;
        public float weaponIntervalMultiplier = 1f;
        public float cooldownSeconds;
        public float damageMultiplier = 1f;
        public int autoUsePriority;
        public bool requiresWarriorWeapon = true;
        public CombatAbilityResourceInteraction resourceInteraction;

        public float GetActionTime(PlayerCombatStats stats)
        {
            var interval = stats.AttackInterval;
            return actionTimeRule == CombatAbilityActionTimeRule.WeaponAttackIntervalMultiplier
                ? Math.Max(0.05f, interval * Math.Max(0.01f, weaponIntervalMultiplier))
                : Math.Max(0.05f, fixedActionTimeSeconds);
        }
    }
}
