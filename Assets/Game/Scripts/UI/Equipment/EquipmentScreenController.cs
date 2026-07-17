using System.Collections.Generic;
using System.Linq;
using IdleGame.Equipment;
using IdleGame.Inventory;
using IdleGame.Items;
using IdleGame.UI.Shared;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IdleGame.UI.Equipment
{
    public sealed class EquipmentScreenController : MonoBehaviour
    {
        [SerializeField] private EquipmentSystem equipmentSystem;
        [SerializeField] private InventorySystem inventorySystem;
        [SerializeField] private TMP_Text mainHandText;
        [SerializeField] private TMP_Text offhandText;
        [SerializeField] private TMP_Text statSummaryText;
        [SerializeField] private TMP_Text statusText;
        [SerializeField] private Transform equipmentInventoryContainer;
        [SerializeField] private GameObject equipmentEntryTemplate;

        private readonly List<GameObject> generatedEntries = new();

        private void Awake()
        {
            AutoBind();
        }

        private void OnEnable()
        {
            if (equipmentSystem != null)
            {
                equipmentSystem.EquipmentChanged += Refresh;
            }

            if (inventorySystem != null)
            {
                inventorySystem.InventoryChanged += Refresh;
            }

            Refresh();
        }

        private void OnDisable()
        {
            if (equipmentSystem != null)
            {
                equipmentSystem.EquipmentChanged -= Refresh;
            }

            if (inventorySystem != null)
            {
                inventorySystem.InventoryChanged -= Refresh;
            }
        }

        public void ConfigureForEditor(EquipmentSystem equipment, InventorySystem inventory)
        {
            equipmentSystem = equipment;
            inventorySystem = inventory;
            AutoBind();
        }

        public void AutoBind()
        {
            mainHandText ??= HierarchySearch.FindText(transform, "[TEXT] MainHandEquippedValue");
            offhandText ??= HierarchySearch.FindText(transform, "[TEXT] OffhandEquippedValue");
            statSummaryText ??= HierarchySearch.FindText(transform, "[TEXT] EquipmentStatSummary");
            statusText ??= HierarchySearch.FindText(transform, "[TEXT] EquipmentStatus");
            equipmentInventoryContainer ??= HierarchySearch.FindDeep(transform, "[DYNAMIC CONTENT] EquipmentInventoryContainer");
            equipmentEntryTemplate ??= HierarchySearch.FindDeep(transform, "[ROW] EquipmentEntryTemplate")?.gameObject;
        }

        private void Refresh()
        {
            if (equipmentSystem == null)
            {
                return;
            }

            var mainHand = equipmentSystem.GetMainHandDefinition();
            var equipped = equipmentSystem.GetEquippedItems().ToList();
            var offhand = equipped.FirstOrDefault(item => item != null && item.Slot == EquipmentSlot.Offhand);

            if (mainHandText != null)
            {
                mainHandText.text = mainHand != null ? DescribeEquipment(mainHand) : "Empty";
            }

            if (offhandText != null)
            {
                offhandText.text = offhand != null ? DescribeEquipment(offhand) : "Empty";
            }

            if (statSummaryText != null)
            {
                var health = 100 + equipped.Sum(item => item.CombatMaxHealth);
                var defense = 5 + equipped.Sum(item => item.CombatDefense);
                var accuracy = 60 + equipped.Sum(item => item.CombatAccuracy);
                statSummaryText.text = $"Health {health}\nDefense {defense}\nAccuracy {accuracy}";
            }

            if (statusText != null && equipmentSystem.EquipmentLocked)
            {
                statusText.text = "Equipment changes are locked during active Combat.";
            }
            else if (statusText != null && string.IsNullOrWhiteSpace(statusText.text))
            {
                statusText.text = "Select an equipment item from Inventory to equip it.";
            }

            RefreshInventoryEntries();
        }

        private void RefreshInventoryEntries()
        {
            foreach (var entry in generatedEntries.Where(entry => entry != null))
            {
                Destroy(entry);
            }

            generatedEntries.Clear();
            if (inventorySystem == null || equipmentEntryTemplate == null || equipmentInventoryContainer == null)
            {
                return;
            }

            foreach (var stack in inventorySystem.Stacks.Where(stack => stack != null && stack.quantity > 0))
            {
                if (!inventorySystem.ItemDatabase.TryGetItem(stack.itemId, out var item) || item == null || item.Category != ItemCategory.Equipment)
                {
                    continue;
                }

                var row = Instantiate(equipmentEntryTemplate, equipmentInventoryContainer);
                row.name = "[ROW] EquipmentEntry";
                row.SetActive(true);
                var nameText = HierarchySearch.FindText(row.transform, "[TEXT] EquipmentName");
                var statsText = HierarchySearch.FindText(row.transform, "[TEXT] EquipmentStats");
                var button = HierarchySearch.FindButton(row.transform, "[BUTTON] EquipButton") ?? row.GetComponent<Button>();

                if (nameText != null)
                {
                    nameText.text = item.DisplayName;
                }

                if (statsText != null)
                {
                    statsText.text = item.Description;
                }

                if (button != null)
                {
                    var itemId = stack.itemId;
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() => TryEquip(itemId));
                }

                generatedEntries.Add(row);
            }
        }

        private void TryEquip(string itemId)
        {
            if (equipmentSystem == null)
            {
                return;
            }

            if (!equipmentSystem.TryEquipItem(itemId, out var reason) && statusText != null)
            {
                statusText.text = reason;
            }
            else if (statusText != null)
            {
                statusText.text = "Equipped.";
            }

            Refresh();
        }

        private static string DescribeEquipment(EquipmentDefinition equipment)
        {
            if (equipment == null)
            {
                return "Empty";
            }

            if (equipment.Slot == EquipmentSlot.MainHand)
            {
                return $"{equipment.DisplayName}\nDamage {equipment.CombatMinDamage}-{equipment.CombatMaxDamage}, {equipment.CombatAttackInterval:0.0}s, Accuracy +{equipment.CombatAccuracy}";
            }

            return $"{equipment.DisplayName}\nDefense +{equipment.CombatDefense}, Health +{equipment.CombatMaxHealth}";
        }
    }
}
