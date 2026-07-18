using IdleGame.Combat;
using IdleGame.Equipment;
using IdleGame.Inventory;
using IdleGame.Items;
using UnityEngine;

namespace IdleGame.UI.Tooltips
{
    public sealed class ItemTooltipProvider : MonoBehaviour, ITooltipDataProvider
    {
        [SerializeField] private string itemId = string.Empty;
        [SerializeField] private ItemDefinition itemDefinition;
        [SerializeField] private InventorySystem inventorySystem;
        [SerializeField] private EquipmentSystem equipmentSystem;
        [SerializeField] private CombatSystem combatSystem;
        [SerializeField] private bool includeQuantity = true;
        [SerializeField] private bool includeComparison = true;

        public void ConfigureForEditor(
            string stableItemId,
            ItemDefinition item,
            InventorySystem inventory,
            EquipmentSystem equipment = null,
            CombatSystem combat = null,
            bool showQuantity = true,
            bool showComparison = true)
        {
            itemId = stableItemId ?? string.Empty;
            itemDefinition = item;
            inventorySystem = inventory;
            equipmentSystem = equipment;
            combatSystem = combat;
            includeQuantity = showQuantity;
            includeComparison = showComparison;
        }

        public TooltipData BuildTooltipData()
        {
            ResolveItem();
            if (itemDefinition == null)
            {
                return new TooltipData { DisplayName = "Unknown Item", SourceId = itemId };
            }

            var data = new TooltipData
            {
                SourceId = itemDefinition.ItemId,
                DisplayName = itemDefinition.DisplayName,
                Category = BuildCategory(itemDefinition),
                Description = itemDefinition.Description,
                Icon = itemDefinition.Icon
            };

            if (includeQuantity && inventorySystem != null)
            {
                var quantity = inventorySystem.GetQuantity(itemDefinition.ItemId);
                data.SecondaryStats.Add(new TooltipStatLine("Quantity", quantity.ToString("N0")));
            }

            // Stack capacity is intentionally not exposed in tooltips.

            if (itemDefinition.CanBeSold && itemDefinition.SellValue > 0)
            {
                data.SecondaryStats.Add(new TooltipStatLine("Sell Value", itemDefinition.SellValue.ToString()));
            }

            if (!itemDefinition.CanBeDestroyed)
            {
                data.Footer.Add("Cannot be destroyed.");
            }

            AddConsumableStats(data);
            AddEquipmentStats(data);
            return data;
        }

        private void ResolveItem()
        {
            if (itemDefinition != null)
            {
                return;
            }

            if (inventorySystem != null &&
                inventorySystem.ItemDatabase != null &&
                !string.IsNullOrWhiteSpace(itemId) &&
                inventorySystem.ItemDatabase.TryGetItem(itemId, out var item))
            {
                itemDefinition = item;
            }
        }

        private void AddConsumableStats(TooltipData data)
        {
            if (itemDefinition.Category != ItemCategory.Consumable || combatSystem == null)
            {
                return;
            }

            if (itemDefinition.ItemId == CombatConstants.MinorHealingPotionItemId)
            {
                data.Effects.Add(new TooltipStatLine("Healing", $"{combatSystem.MinorPotionHealingAmount} HP"));
            }
        }

        private void AddEquipmentStats(TooltipData data)
        {
            var database = equipmentSystem != null ? equipmentSystem.EquipmentDatabase : null;
            if (database == null || !database.TryGetEquipment(itemDefinition.ItemId, out var equipment) || equipment == null)
            {
                return;
            }

            AppendEquipmentStats(data.PrimaryStats, equipment);
            if (equipment.IsWarriorCompatible)
            {
                data.Requirements.Add(new TooltipStatLine("Discipline", "Warrior"));
            }

            if (!includeComparison || equipmentSystem == null)
            {
                return;
            }

            var equipped = equipmentSystem.GetEquippedDefinition(equipment.Slot);
            if (equipped == null || equipped.ItemId == equipment.ItemId)
            {
                return;
            }

            AppendComparison(data.Comparison, "Min Damage", equipment.CombatMinDamage, equipped.CombatMinDamage);
            AppendComparison(data.Comparison, "Max Damage", equipment.CombatMaxDamage, equipped.CombatMaxDamage);
            AppendComparison(data.Comparison, "Accuracy", equipment.CombatAccuracy, equipped.CombatAccuracy);
            AppendComparison(data.Comparison, "Defence", equipment.CombatDefense, equipped.CombatDefense);
            AppendComparison(data.Comparison, "Maximum HP", equipment.CombatMaxHealth, equipped.CombatMaxHealth);
            AppendComparison(data.Comparison, "Critical Chance", Mathf.RoundToInt(equipment.CombatCriticalChance * 100f), Mathf.RoundToInt(equipped.CombatCriticalChance * 100f), "%");
        }

        private static void AppendEquipmentStats(System.Collections.Generic.List<TooltipStatLine> lines, EquipmentDefinition equipment)
        {
            lines.Add(new TooltipStatLine("Slot", equipment.Slot.ToString()));
            if (equipment.CombatMaxDamage > 0)
            {
                lines.Add(new TooltipStatLine("Damage", $"{equipment.CombatMinDamage}-{equipment.CombatMaxDamage}"));
                lines.Add(new TooltipStatLine("Attack Speed", $"{equipment.CombatAttackInterval:0.0}s"));
            }

            AddModifier(lines, "Accuracy", equipment.CombatAccuracy);
            AddModifier(lines, "Defence", equipment.CombatDefense);
            AddModifier(lines, "Maximum HP", equipment.CombatMaxHealth);
            if (Mathf.Abs(equipment.CombatCriticalChance) > 0.0001f)
            {
                AddModifier(lines, "Critical Chance", Mathf.RoundToInt(equipment.CombatCriticalChance * 100f), "%");
            }
        }

        private static void AddModifier(System.Collections.Generic.List<TooltipStatLine> lines, string label, int value, string suffix = "")
        {
            if (value == 0)
            {
                return;
            }

            lines.Add(new TooltipStatLine(label, value > 0 ? $"+{value}{suffix}" : $"{value}{suffix}", value >= 0 ? TooltipValueTone.Positive : TooltipValueTone.Negative));
        }

        private static void AppendComparison(System.Collections.Generic.List<TooltipStatLine> lines, string label, int candidate, int equipped, string suffix = "")
        {
            var delta = candidate - equipped;
            if (delta == 0)
            {
                return;
            }

            lines.Add(new TooltipStatLine(label, delta > 0 ? $"+{delta}{suffix}" : $"{delta}{suffix}", delta > 0 ? TooltipValueTone.Positive : TooltipValueTone.Negative));
        }

        private static string BuildCategory(ItemDefinition item)
        {
            if (item == null)
            {
                return string.Empty;
            }

            return string.IsNullOrWhiteSpace(item.ItemType)
                ? item.Category.ToString()
                : $"{item.ItemType} - {item.Category}";
        }
    }
}
