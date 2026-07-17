using UnityEngine;

namespace IdleGame.Combat
{
    public static class CombatFormula
    {
        public static float GetHitChance01(float attackerAccuracy, float defenderDefense)
        {
            var denominator = Mathf.Max(0.001f, attackerAccuracy + Mathf.Max(0f, defenderDefense));
            return Mathf.Clamp(attackerAccuracy / denominator, 0.10f, 0.95f);
        }

        public static int MitigateDamage(int rolledDamage, float targetDefense, bool critical, float criticalDamageMultiplier)
        {
            var mitigated = rolledDamage * 100f / (100f + Mathf.Max(0f, targetDefense));
            if (critical)
            {
                mitigated *= Mathf.Max(1f, criticalDamageMultiplier);
            }

            return Mathf.Max(1, Mathf.RoundToInt(mitigated));
        }
    }
}
