using System.Linq;

namespace IdleGame.Equipment
{
    public sealed class ProfessionModifierResolver
    {
        private readonly EquipmentSystem equipmentSystem;

        public ProfessionModifierResolver(EquipmentSystem equipmentSystem)
        {
            this.equipmentSystem = equipmentSystem;
        }

        public ProfessionModifierResult Resolve(string professionId, float basePower, float baseActionInterval)
        {
            var result = new ProfessionModifierResult
            {
                basePower = basePower,
                baseActionInterval = baseActionInterval
            };

            if (equipmentSystem == null)
            {
                result.finalPower = basePower;
                result.finalActionInterval = baseActionInterval;
                return result;
            }

            foreach (var modifier in equipmentSystem.GetEquippedItems()
                         .SelectMany(item => item.ProfessionModifiers)
                         .Where(modifier => modifier != null && modifier.professionId == professionId))
            {
                result.flatPowerFromEquipment += modifier.flatPower;
                result.actionSpeedPercent += modifier.actionSpeedPercent;
                result.xpPercent += modifier.xpPercent;
                result.outputPercent += modifier.outputPercent;
            }

            result.finalPower = basePower + result.flatPowerFromEquipment;
            var speedMultiplier = 1f + (result.actionSpeedPercent / 100f);
            result.finalActionInterval = speedMultiplier <= 0f ? baseActionInterval : baseActionInterval / speedMultiplier;
            return result;
        }
    }

    public struct ProfessionModifierResult
    {
        public float basePower;
        public float flatPowerFromEquipment;
        public float finalPower;
        public float baseActionInterval;
        public float finalActionInterval;
        public float actionSpeedPercent;
        public float xpPercent;
        public float outputPercent;
    }
}
