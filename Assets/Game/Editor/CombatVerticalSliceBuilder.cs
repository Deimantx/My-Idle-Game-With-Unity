using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IdleGame.Activities;
using IdleGame.Combat;
using IdleGame.Core.Bootstrap;
using IdleGame.Equipment;
using IdleGame.Inventory;
using IdleGame.Items;
using IdleGame.Professions.Woodcutting;
using IdleGame.Progression;
using IdleGame.Save;
using IdleGame.UI;
using IdleGame.UI.Combat;
using IdleGame.UI.Equipment;
using IdleGame.UI.Shared;
using IdleGame.UI.Woodcutting;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace IdleGame.Editor
{
    public static class CombatVerticalSliceBuilder
    {
        private const string CombatDataFolder = "Assets/Game/Data/Combat";
        private const string ItemDataFolder = "Assets/Game/Data/Items";
        private const string EquipmentDataFolder = "Assets/Game/Data/Equipment";
        private static readonly Color Deep = HtmlColor("0D1117");
        private static readonly Color Panel = HtmlColor("121923");
        private static readonly Color Raised = HtmlColor("1A2430");
        private static readonly Color Border = HtmlColor("344455");
        private static readonly Color Gold = HtmlColor("D0A44B");
        private static readonly Color Blue = HtmlColor("3C9CB5");
        private static readonly Color Green = HtmlColor("1E6B34");
        private static readonly Color Red = HtmlColor("7A241F");
        private static readonly Color Text = HtmlColor("E8D9BE");
        private static readonly Color Muted = HtmlColor("AFA18A");

        [MenuItem("Tools/Idle Game/Build Combat Vertical Slice")]
        public static void Build()
        {
            EnsureFolders();
            var items = EnsureItems();
            var equipment = EnsureEquipment();
            var catalog = EnsureCombatCatalog();
            WireScene(catalog, items, equipment);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorSceneManager.SaveOpenScenes();
            Debug.Log("Combat vertical slice built.");
        }

        private static ItemDatabase EnsureItems()
        {
            var definitions = new List<ItemDefinition>
            {
                Item("weapon_worn_sword", "Worn Sword", "A damaged but usable sword. Better than fighting unarmed.", ItemCategory.Equipment, "Weapon", false, 1, 10, true, true),
                Item("weapon_rusted_dagger", "Rusted Dagger", "A quick, battered blade. A fast Warrior sidegrade.", ItemCategory.Equipment, "Weapon", false, 1, 8, true, true),
                Item("shield_worn_buckler", "Worn Buckler", "A battered shield that still turns aside early blows.", ItemCategory.Equipment, "Shield", false, 1, 12, true, true),
                Item("consumable_minor_healing_potion", "Minor Healing Potion", "Restores 30 Health during Combat.", ItemCategory.Consumable, "Potion", true, 999999, 5, true, true),
                Item("item_raw_meat", "Raw Meat", "Fresh meat from forest creatures. Useful for future cooking.", ItemCategory.Resource, "Meat", true, 999999, 1, true, true),
                Item("item_rat_tail", "Rat Tail", "A small combat trophy and future crafting material.", ItemCategory.CraftingMaterial, "Trophy", true, 999999, 2, true, true),
                Item("item_wolf_pelt", "Wolf Pelt", "A rough pelt suitable for future leatherworking.", ItemCategory.CraftingMaterial, "Hide", true, 999999, 4, true, true),
                Item("item_wolf_fang", "Wolf Fang", "A sharp fang used in future combat crafting.", ItemCategory.CraftingMaterial, "Trophy", true, 999999, 8, true, true),
                Item("item_cloth_scrap", "Cloth Scrap", "Rough cloth taken from bandits.", ItemCategory.CraftingMaterial, "Cloth", true, 999999, 3, true, true)
            };

            var database = AssetDatabase.LoadAssetAtPath<ItemDatabase>($"{ItemDataFolder}/ItemDatabase.asset");
            if (database == null)
            {
                database = ScriptableObject.CreateInstance<ItemDatabase>();
                AssetDatabase.CreateAsset(database, $"{ItemDataFolder}/ItemDatabase.asset");
            }

            var existing = database.Items.Where(item => item != null && definitions.All(definition => definition.ItemId != item.ItemId)).ToList();
            existing.AddRange(definitions);
            database.ConfigureForEditor(existing);
            EditorUtility.SetDirty(database);
            return database;
        }

        private static EquipmentDatabase EnsureEquipment()
        {
            var definitions = new List<EquipmentDefinition>
            {
                Equipment("weapon_worn_sword", "Worn Sword", EquipmentSlot.MainHand, false, true, 6, 10, 2.0f, 10, 0, 0),
                Equipment("weapon_rusted_dagger", "Rusted Dagger", EquipmentSlot.MainHand, false, true, 4, 7, 1.5f, 5, 0, 0),
                Equipment("shield_worn_buckler", "Worn Buckler", EquipmentSlot.Offhand, false, false, 0, 0, 2.5f, 0, 8, 10)
            };

            var database = AssetDatabase.LoadAssetAtPath<EquipmentDatabase>($"{EquipmentDataFolder}/EquipmentDatabase.asset");
            if (database == null)
            {
                database = ScriptableObject.CreateInstance<EquipmentDatabase>();
                AssetDatabase.CreateAsset(database, $"{EquipmentDataFolder}/EquipmentDatabase.asset");
            }

            var existing = database.Equipment.Where(item => item != null && definitions.All(definition => definition.ItemId != item.ItemId)).ToList();
            existing.AddRange(definitions);
            database.ConfigureForEditor(existing);
            EditorUtility.SetDirty(database);
            return database;
        }

        private static CombatCatalog EnsureCombatCatalog()
        {
            var rat = Enemy(
                "enemy_forest_rat", "Forest Rat", "A nervous forest scavenger. Weak, fast to defeat, and good first training.",
                1, 30, 2, 4, 2.4f, 35, 5, 0f, 1.5f, 1.0f, 2f, 1, 3,
                new[] { Loot("item_raw_meat", 0.5f, 1, 1), Loot("item_rat_tail", 0.25f, 1, 1) },
                new[] { Loot("consumable_minor_healing_potion", 1f, 1, 1, true) });
            var wolf = Enemy(
                "enemy_grey_wolf", "Grey Wolf", "A lean predator from deeper in Greenvale Forest.",
                2, 70, 4, 7, 2.2f, 50, 12, 0.03f, 1.5f, 1.2f, 3f, 3, 6,
                new[] { Loot("item_raw_meat", 0.5f, 1, 2), Loot("item_wolf_pelt", 0.6f, 1, 1), Loot("item_wolf_fang", 0.1f, 1, 1) },
                Array.Empty<CombatLootEntry>());
            var bandit = Enemy(
                "enemy_forest_bandit", "Forest Bandit", "A low-level outlaw preying on Greenvale travelers.",
                5, 120, 4, 7, 2.6f, 55, 18, 0.05f, 1.5f, 1.4f, 4f, 8, 15,
                new[] { Loot("item_cloth_scrap", 0.6f, 1, 2), Loot("weapon_rusted_dagger", 0.1f, 1, 1, true) },
                new[] { Loot("shield_worn_buckler", 1f, 1, 1, true) });

            var catalog = AssetDatabase.LoadAssetAtPath<CombatCatalog>($"{CombatDataFolder}/CombatCatalog.asset");
            if (catalog == null)
            {
                catalog = ScriptableObject.CreateInstance<CombatCatalog>();
                AssetDatabase.CreateAsset(catalog, $"{CombatDataFolder}/CombatCatalog.asset");
            }

            catalog.ConfigureForEditor(
                CombatConstants.RegionGreenvale,
                "Greenvale",
                "A temperate frontier Region containing wildlife and low-level bandits.",
                CombatConstants.ActivityTypeAreas,
                "Areas",
                CombatConstants.LocationGreenvaleForest,
                "Greenvale Forest",
                new[] { rat, wolf, bandit });
            EditorUtility.SetDirty(catalog);
            return catalog;
        }

        private static void WireScene(CombatCatalog catalog, ItemDatabase itemDatabase, EquipmentDatabase equipmentDatabase)
        {
            var gameSystems = GameObject.Find("[SYSTEMS] GameSystems");
            if (gameSystems == null)
            {
                throw new InvalidOperationException("Missing [SYSTEMS] GameSystems.");
            }

            var inventory = gameSystems.GetComponent<InventorySystem>();
            var equipment = gameSystems.GetComponent<EquipmentSystem>();
            var progression = gameSystems.GetComponent<ProfessionProgressionSystem>();
            var activeActivity = gameSystems.GetComponent<ActiveActivityService>();
            var woodcutting = gameSystems.GetComponent<WoodcuttingSystem>();
            var save = gameSystems.GetComponent<SaveManager>();
            var combat = gameSystems.GetComponent<CombatSystem>() ?? gameSystems.AddComponent<CombatSystem>();

            inventory.ConfigureForEditor(itemDatabase, 100);
            equipment.ConfigureForEditor(equipmentDatabase);
            equipment.ConfigureInventoryForEditor(inventory);
            combat.ConfigureForEditor(catalog, inventory, equipment, progression, activeActivity, woodcutting);
            save.ConfigureCombatForEditor(combat);

            var bootstrap = Object.FindAnyObjectByType<GameBootstrap>(FindObjectsInactive.Include);
            if (bootstrap != null)
            {
                var services = bootstrap.Services.Cast<MonoBehaviour>().Where(service => service != null).ToList();
                if (!services.Contains(combat))
                {
                    services.Add(combat);
                }

                bootstrap.ConfigureForEditor(services, true);
                EditorUtility.SetDirty(bootstrap);
            }

            RebuildCombatScreen(combat, progression, inventory);
            RebuildEquipmentScreen(equipment, inventory);
            RebuildActiveActivityCombatState(combat);

            EditorUtility.SetDirty(gameSystems);
            EditorSceneManager.MarkSceneDirty(gameSystems.scene);
        }

        private static void RebuildCombatScreen(CombatSystem combat, ProfessionProgressionSystem progression, InventorySystem inventory)
        {
            var screen = FindSceneObject("[SCREEN] CombatScreen");
            if (screen == null)
            {
                throw new InvalidOperationException("Missing [SCREEN] CombatScreen.");
            }

            Undo.RegisterFullObjectHierarchyUndo(screen.gameObject, "Build Combat Screen");
            ClearChildren(screen);
            var controller = screen.GetComponent<CombatScreenController>() ?? screen.gameObject.AddComponent<CombatScreenController>();
            if (screen.GetComponent<CombatControlRaycaster>() == null)
            {
                screen.gameObject.AddComponent<CombatControlRaycaster>();
            }

            controller.ConfigureForEditor(combat, progression);
            controller.ConfigureInventoryForEditor(inventory);

            ConfigureRoot(screen, Deep);

            var selection = PanelObject("[STATE] CombatSelectionState", screen, Panel, 0f, true);
            var selectionLayout = selection.AddComponent<HorizontalLayoutGroup>();
            selectionLayout.spacing = 10f;
            selectionLayout.padding = new RectOffset(10, 10, 10, 10);
            selectionLayout.childControlWidth = true;
            selectionLayout.childControlHeight = true;
            selectionLayout.childForceExpandHeight = true;
            selectionLayout.childForceExpandWidth = true;

            var left = Column("[PANEL] SelectionColumn", selection.transform, 0.42f, 520f);
            TextObject("[TEXT] CombatBreadcrumb", left.transform, "Greenvale > Areas > Greenvale Forest", 16f, Text, TextAlignmentOptions.Left, Stretch(), 30f);
            SelectionBlock(left.transform, "REGION", "[DYNAMIC CONTENT] RegionContainer", "[BUTTON] RegionTemplate");
            SelectionBlock(left.transform, "TYPE", "[DYNAMIC CONTENT] ActivityTypeContainer", "[BUTTON] ActivityTypeTemplate");
            SelectionBlock(left.transform, "LOCATION", "[DYNAMIC CONTENT] LocationContainer", "[BUTTON] LocationTemplate");
            SelectionBlock(left.transform, "ENEMY", "[DYNAMIC CONTENT] EnemyContainer", "[BUTTON] EnemyTemplate");

            var details = Column("[PANEL] EnemyDetailsPanel", selection.transform, 0.58f, 560f);
            TextObject("[TEXT] EnemyDetailsName", details.transform, "Select an enemy", 28f, Gold, TextAlignmentOptions.Center, Stretch(), 42f);
            TextObject("[TEXT] EnemyDetailsDescription", details.transform, "Choose an enemy to review requirements, stats, and possible loot.", 16f, Muted, TextAlignmentOptions.Center, Stretch(), 54f);
            TextObject("[TEXT] EnemyRequirements", details.transform, string.Empty, 17f, Text, TextAlignmentOptions.Left, Stretch(), 34f);
            TextObject("[TEXT] EnemyStats", details.transform, string.Empty, 16f, Text, TextAlignmentOptions.TopLeft, Stretch(), 140f);
            TextObject("[TEXT] EnemyLootPreview", details.transform, string.Empty, 16f, Text, TextAlignmentOptions.TopLeft, Stretch(), 160f);
            TextObject("[TEXT] CombatNotification", details.transform, string.Empty, 16f, Gold, TextAlignmentOptions.Center, Stretch(), 36f);
            ButtonObject("[BUTTON] StartCombatButton", details.transform, "Start Combat", Green, 58f);

            var active = PanelObject("[STATE] ActiveCombatState", screen, Panel, 0f, true);
            active.SetActive(false);
            var activeLayout = active.AddComponent<VerticalLayoutGroup>();
            activeLayout.spacing = 8f;
            activeLayout.padding = new RectOffset(8, 8, 8, 8);
            activeLayout.childControlWidth = true;
            activeLayout.childControlHeight = true;
            activeLayout.childForceExpandWidth = true;
            activeLayout.childForceExpandHeight = false;

            var header = PanelObject("[HEADER] CombatHeader", active.transform, Raised, 72f);
            var headerLayout = header.AddComponent<HorizontalLayoutGroup>();
            headerLayout.spacing = 8f;
            headerLayout.padding = new RectOffset(12, 12, 6, 6);
            headerLayout.childControlWidth = true;
            headerLayout.childControlHeight = true;
            headerLayout.childForceExpandHeight = true;

            var identity = HeaderSection("[SECTION] EncounterIdentity", header.transform, 0.35f, 280f);
            TextObject("[TEXT] EncounterAreaText", identity.transform, "GREENVALE FOREST", 11f, Muted, TextAlignmentOptions.Left, Stretch(), 16f);
            TextObject("[TEXT] EncounterEnemyText", identity.transform, "Forest Rat", 20f, Gold, TextAlignmentOptions.Left, Stretch(), 28f);
            var hiddenTitle = TextObject("[TEXT] EncounterTitleText", identity.transform, "Forest Rat", 1f, new Color(0f, 0f, 0f, 0f), TextAlignmentOptions.Left, Stretch(), 1f);
            hiddenTitle.gameObject.SetActive(false);

            var progressionSection = HeaderSection("[SECTION] WarriorProgression", header.transform, 0.35f, 320f);
            TextObject("[TEXT] WarriorLevelText", progressionSection.transform, "Warrior Level 1", 14f, Text, TextAlignmentOptions.Left, Stretch(), 18f);
            BarObject("[BAR] WarriorXPBar", progressionSection.transform, Stretch(), Blue, 18f);

            var session = HeaderSection("[SECTION] CombatSession", header.transform, 0.30f, 240f);
            TextObject("[TEXT] EncounterSessionText", session.transform, "Kills: 0\nTime: 00:00", 12f, Text, TextAlignmentOptions.Left, Stretch(), 30f);
            ToggleObject("[TOGGLE] AutoRepeatToggle", session.transform, "Auto Repeat", Stretch(), 24f);

            var columns = PanelObject("[LAYOUT] ActiveCombatColumns", active.transform, new Color(0f, 0f, 0f, 0f), 0f, true);
            var activeColumnsLayout = columns.AddComponent<HorizontalLayoutGroup>();
            activeColumnsLayout.spacing = 8f;
            activeColumnsLayout.childControlWidth = true;
            activeColumnsLayout.childControlHeight = true;
            activeColumnsLayout.childForceExpandHeight = true;

            var player = Column("[PANEL] PlayerCombatPanel", columns.transform, 0.32f, 300f);
            player.GetComponent<VerticalLayoutGroup>().spacing = 6f;
            var playerPortrait = PanelObject("[PANEL] PlayerPortraitRow", player.transform, new Color(0f, 0f, 0f, 0.10f), 190f);
            TextObject("[HEADER] PlayerSummaryHeader", playerPortrait.transform, "PLAYER SUMMARY", 12f, Gold, TextAlignmentOptions.Left, Anchor(0, 1, 1, 1, 8, -14, -8, 18));
            IconFrame("[FRAME] PlayerPortraitFrame", "[IMAGE] PlayerPortrait", "[TEXT] PlayerPortraitPlaceholder", playerPortrait.transform, "WARRIOR", 88f, Anchor(0, 1, 0, 1, 52, -70, 88, 88));
            TextObject("[TEXT] PlayerName", playerPortrait.transform, "Player - Warrior", 18f, Gold, TextAlignmentOptions.Left, Anchor(0, 1, 1, 1, 108, -34, -8, 24));
            BarObject("[BAR] PlayerHealthBar", playerPortrait.transform, Anchor(0, 1, 1, 1, 108, -58, -8, 20), Red);
            BarObject("[BAR] DevotionBar", playerPortrait.transform, Anchor(0, 1, 1, 1, 108, -82, -8, 20), Blue);
            var playerStatsGrid = StatGrid("[GRID] PlayerCombatStatsGrid", playerPortrait.transform, Anchor(0, 0, 1, 0, 8, 37, -8, 62));
            StatCell(playerStatsGrid.transform, "[TEXT] PlayerDamageLabel", "[TEXT] PlayerDamageValue", "Damage", "--");
            StatCell(playerStatsGrid.transform, "[TEXT] PlayerAttackSpeedLabel", "[TEXT] PlayerAttackSpeedValue", "Attack Speed", "--");
            StatCell(playerStatsGrid.transform, "[TEXT] PlayerAccuracyLabel", "[TEXT] PlayerAccuracyValue", "Accuracy", "--");
            StatCell(playerStatsGrid.transform, "[TEXT] PlayerDefenceLabel", "[TEXT] PlayerDefenceValue", "Defence", "--");
            StatCell(playerStatsGrid.transform, "[TEXT] PlayerCriticalLabel", "[TEXT] PlayerCriticalValue", "Critical", "--");
            StatCell(playerStatsGrid.transform, "[TEXT] PlayerFutureStatLabel", "[TEXT] PlayerFutureStatValue", string.Empty, string.Empty).SetActive(false);
            var playerStatsLegacy = TextObject("[TEXT] PlayerStats", playerPortrait.transform, string.Empty, 1f, new Color(0f, 0f, 0f, 0f), TextAlignmentOptions.TopLeft, Anchor(0, 0, 0, 0, 0, 0, 1, 1));
            playerStatsLegacy.gameObject.SetActive(false);

            var currentAction = PanelObject("[PANEL] CurrentActionCard", player.transform, new Color(0f, 0f, 0f, 0.12f), 76f);
            TextObject("[HEADER] CurrentActionHeader", currentAction.transform, "CURRENT ACTION", 12f, Gold, TextAlignmentOptions.Left, Anchor(0, 1, .55f, 1, 8, -14, 0, 18));
            TextObject("[TEXT] CurrentActionRemainingText", currentAction.transform, "Ready", 12f, Muted, TextAlignmentOptions.Right, Anchor(.50f, 1, 1, 1, 0, -14, -8, 18));
            IconFrame("[FRAME] CurrentActionIconFrame", "[IMAGE] CurrentActionIcon", "[TEXT] CurrentActionIconLabel", currentAction.transform, "ATK", 48f, Anchor(0, .5f, 0, .5f, 32, -11, 48, 48));
            TextObject("[TEXT] CurrentActionNameText", currentAction.transform, "Waiting", 14f, Text, TextAlignmentOptions.Left, Anchor(0, 0, 1, 1, 66, 24, -8, -30));
            BarObject("[BAR] PlayerAttackBar", currentAction.transform, Anchor(0, 0, 1, 0, 66, 10, -8, 14), Green);

            var queuedPanel = PanelObject("[PANEL] QueuedActionPanel", player.transform, new Color(0f, 0f, 0f, 0.10f), 34f);
            queuedPanel.name = "[PANEL] QueuedActionPanel";
            TextObject("[HEADER] QueuedActionHeader", queuedPanel.transform, "QUEUED:", 12f, Gold, TextAlignmentOptions.Left, Anchor(0, 0, 0, 1, 8, 0, 56, 0));
            IconFrame("[FRAME] QueuedActionIconFrame", "[IMAGE] QueuedActionIcon", "[TEXT] QueuedActionIconLabel", queuedPanel.transform, "HS", 24f, Anchor(0, .5f, 0, .5f, 78, 0, 24, 24));
            TextObject("[TEXT] QueuedActionText", queuedPanel.transform, string.Empty, 13f, Text, TextAlignmentOptions.Left, Anchor(0, 0, 1, 1, 108, 0, -8, 0));
            queuedPanel.SetActive(false);

            SectionHeading(player.transform, "[HEADER] SkillsSectionHeader", "SKILLS", 22f);
            var skillsList = PanelObject("[LIST] PlayerSkillsList", player.transform, new Color(0f, 0f, 0f, 0f), 0f, true);
            var skillsLayout = skillsList.AddComponent<VerticalLayoutGroup>();
            skillsLayout.spacing = 6f;
            skillsLayout.padding = new RectOffset(0, 0, 0, 0);
            skillsLayout.childControlWidth = true;
            skillsLayout.childControlHeight = true;
            skillsLayout.childForceExpandWidth = true;
            skillsLayout.childForceExpandHeight = false;
            var skillsElement = skillsList.GetComponent<LayoutElement>();
            skillsElement.preferredHeight = 76f;
            skillsElement.minHeight = 76f;
            skillsElement.flexibleHeight = 0f;
            var heavy = PanelObject("[PANEL] HeavyStrikeCard", skillsList.transform, new Color(0f, 0f, 0f, 0.12f), 76f);
            IconFrame("[FRAME] HeavyStrikeIconFrame", "[IMAGE] HeavyStrikeIcon", "[TEXT] HeavyStrikeIconLabel", heavy.transform, "HS", 48f, Anchor(0, .5f, 0, .5f, 32, 0, 48, 48));
            TextObject("[TEXT] HeavyStrikeInfoText", heavy.transform, "Heavy Strike\nReady", 13f, Text, TextAlignmentOptions.Left, Anchor(0, 0, 1, 1, 66, 24, -166, -8));
            BarObject("[BAR] HeavyStrikeCooldownBar", heavy.transform, Anchor(0, 0, 1, 0, 66, 12, -166, 10), Gold);
            var heavyReason = TextObject("[TEXT] HeavyStrikeReasonText", heavy.transform, string.Empty, 11f, Muted, TextAlignmentOptions.Left, Anchor(0, 0, 1, 0, 66, 7, -166, 16));
            heavyReason.gameObject.SetActive(false);
            ButtonObject("[BUTTON] HeavyStrikeButton", heavy.transform, "Use", Raised, Anchor(1, .5f, 1, .5f, -128, -10, 58, 24));
            ToggleObject("[TOGGLE] HeavyStrikeAutoToggle", heavy.transform, "Auto", Anchor(1, .5f, 1, .5f, -50, -10, 88, 24), 24f);

            SectionHeading(player.transform, "[HEADER] ConsumablesSectionHeader", "CONSUMABLES", 22f);
            var consumables = PanelObject("[PANEL] CombatConsumablesPanel", player.transform, new Color(0f, 0f, 0f, 0.12f), 142f);
            ConsumableSlot(consumables.transform, "[BUTTON] PotionQuickSlot", "[IMAGE] PotionIcon", "[TEXT] PotionIconLabel", "[TEXT] PotionInfoText", "[TEXT] PotionQuantityText", "POT", "Potion", true, Anchor(0, .5f, 1f / 3f, 1, 6, -6, -4, -6));
            ConsumableSlot(consumables.transform, "[BUTTON] Elixir1QuickSlot", "[IMAGE] Elixir1Icon", "[TEXT] Elixir1IconLabel", "[TEXT] Elixir1InfoText", "[TEXT] Elixir1QuantityText", "E1", "Elixir", false, Anchor(1f / 3f, .5f, 2f / 3f, 1, 4, -6, -4, -6));
            ConsumableSlot(consumables.transform, "[BUTTON] Elixir2QuickSlot", "[IMAGE] Elixir2Icon", "[TEXT] Elixir2IconLabel", "[TEXT] Elixir2InfoText", "[TEXT] Elixir2QuantityText", "E2", "Elixir", false, Anchor(2f / 3f, .5f, 1, 1, 4, -6, -6, -6));
            ConsumableSlot(consumables.transform, "[BUTTON] Elixir3QuickSlot", "[IMAGE] Elixir3Icon", "[TEXT] Elixir3IconLabel", "[TEXT] Elixir3InfoText", "[TEXT] Elixir3QuantityText", "E3", "Elixir", false, Anchor(0, 0, 1f / 3f, .5f, 6, 6, -4, 6));
            ConsumableSlot(consumables.transform, "[BUTTON] Elixir4QuickSlot", "[IMAGE] Elixir4Icon", "[TEXT] Elixir4IconLabel", "[TEXT] Elixir4InfoText", "[TEXT] Elixir4QuantityText", "E4", "Elixir", false, Anchor(1f / 3f, 0, 2f / 3f, .5f, 4, 6, -4, 6));
            ConsumableSlot(consumables.transform, "[BUTTON] FoodQuickSlot", "[IMAGE] FoodIcon", "[TEXT] FoodIconLabel", "[TEXT] FoodInfoText", "[TEXT] FoodQuantityText", "FOOD", "Food", false, Anchor(2f / 3f, 0, 1, .5f, 4, 6, -6, 6));
            var playerStatus = InfoBlock(player.transform, "PLAYER STATUS", "[TEXT] PlayerStatusEffectsText", string.Empty, 44f);
            playerStatus.name = "[PANEL] PlayerStatusEffectsPanel";
            playerStatus.SetActive(false);
            var companion = PanelObject("[PANEL] CompanionCombatPanel", player.transform, new Color(0f, 0f, 0f, 0.12f), 84f);
            companion.name = "[PANEL] CompanionCombatPanel";
            TextObject("[HEADER] CompanionHeader", companion.transform, "COMPANION", 12f, Gold, TextAlignmentOptions.Left, Anchor(0, 1, 1, 1, 8, -14, -8, 18));
            IconFrame("[FRAME] CompanionPortraitFrame", "[IMAGE] CompanionPortrait", "[TEXT] CompanionPortraitPlaceholder", companion.transform, "COMPANION", 56f, Anchor(0, .5f, 0, .5f, 36, -8, 56, 56));
            IconFrame("[FRAME] CompanionSkillIconFrame", "[IMAGE] CompanionSkillIcon", "[TEXT] CompanionSkillIconLabel", companion.transform, "CP", 40f, Anchor(1, .5f, 1, .5f, -28, -8, 40, 40));
            TextObject("[TEXT] CompanionSummaryText", companion.transform, string.Empty, 12f, Text, TextAlignmentOptions.Left, Anchor(0, 0, 1, 1, 74, 26, -78, -30));
            BarObject("[BAR] CompanionActionBar", companion.transform, Anchor(0, 0, 1, 0, 74, 10, -78, 12), Gold);
            companion.SetActive(false);

            var enemy = Column("[PANEL] EnemyCombatPanel", columns.transform, 0.32f, 300f);
            enemy.GetComponent<VerticalLayoutGroup>().spacing = 6f;
            var enemyPortrait = PanelObject("[PANEL] EnemyPortraitRow", enemy.transform, new Color(0f, 0f, 0f, 0.10f), 190f);
            TextObject("[HEADER] EnemySummaryHeader", enemyPortrait.transform, "ENEMY SUMMARY", 12f, Gold, TextAlignmentOptions.Left, Anchor(0, 1, 1, 1, 8, -14, -8, 18));
            IconFrame("[FRAME] EnemyPortraitFrame", "[IMAGE] EnemyPortrait", "[TEXT] EnemyPortraitPlaceholder", enemyPortrait.transform, "ENEMY", 88f, Anchor(0, 1, 0, 1, 52, -70, 88, 88));
            TextObject("[TEXT] ActiveEnemyName", enemyPortrait.transform, "Enemy", 18f, Gold, TextAlignmentOptions.Left, Anchor(0, 1, 1, 1, 108, -34, -8, 24));
            BarObject("[BAR] EnemyHealthBar", enemyPortrait.transform, Anchor(0, 1, 1, 1, 108, -58, -8, 20), Red);
            var enemyStatsGrid = StatGrid("[GRID] EnemyCombatStatsGrid", enemyPortrait.transform, Anchor(0, 0, 1, 0, 8, 37, -8, 62));
            StatCell(enemyStatsGrid.transform, "[TEXT] EnemyDamageLabel", "[TEXT] EnemyDamageValue", "Damage", "--");
            StatCell(enemyStatsGrid.transform, "[TEXT] EnemyAttackSpeedLabel", "[TEXT] EnemyAttackSpeedValue", "Attack Speed", "--");
            StatCell(enemyStatsGrid.transform, "[TEXT] EnemyAccuracyLabel", "[TEXT] EnemyAccuracyValue", "Accuracy", "--");
            StatCell(enemyStatsGrid.transform, "[TEXT] EnemyDefenceLabel", "[TEXT] EnemyDefenceValue", "Defence", "--");
            StatCell(enemyStatsGrid.transform, "[TEXT] EnemyFutureStat1Label", "[TEXT] EnemyFutureStat1Value", string.Empty, string.Empty).SetActive(false);
            StatCell(enemyStatsGrid.transform, "[TEXT] EnemyFutureStat2Label", "[TEXT] EnemyFutureStat2Value", string.Empty, string.Empty).SetActive(false);
            var enemyStatsLegacy = TextObject("[TEXT] ActiveEnemyStats", enemyPortrait.transform, string.Empty, 1f, new Color(0f, 0f, 0f, 0f), TextAlignmentOptions.TopLeft, Anchor(0, 0, 0, 0, 0, 0, 1, 1));
            enemyStatsLegacy.gameObject.SetActive(false);

            var telegraph = PanelObject("[PANEL] EnemyActionTelegraph", enemy.transform, new Color(0f, 0f, 0f, 0.12f), 84f);
            TextObject("[HEADER] EnemyActionHeader", telegraph.transform, "NEXT ENEMY ACTION", 12f, Gold, TextAlignmentOptions.Left, Anchor(0, 1, .55f, 1, 8, -14, 0, 18));
            TextObject("[TEXT] EnemyActionRemainingText", telegraph.transform, "0.0s remaining", 12f, Muted, TextAlignmentOptions.Right, Anchor(.50f, 1, 1, 1, 0, -14, -8, 18));
            IconFrame("[FRAME] EnemyActionIconFrame", "[IMAGE] EnemyActionIcon", "[TEXT] EnemyActionIconLabel", telegraph.transform, "ATK", 48f, Anchor(0, .5f, 0, .5f, 32, -12, 48, 48));
            TextObject("[TEXT] EnemyActionNameText", telegraph.transform, "Attack", 14f, Text, TextAlignmentOptions.Left, Anchor(0, 0, 1, 1, 66, 36, -8, -30));
            TextObject("[TEXT] EnemyActionDetailsText", telegraph.transform, "Damage: 2-4", 12f, Text, TextAlignmentOptions.Left, Anchor(0, 0, 1, 1, 66, 20, -8, -46));
            BarObject("[BAR] EnemyAttackBar", telegraph.transform, Anchor(0, 0, 1, 0, 66, 9, -8, 14), Blue);
            var enemyAbilities = PanelObject("[PANEL] EnemyAbilitiesPanel", enemy.transform, new Color(0f, 0f, 0f, 0.10f), 32f);
            enemyAbilities.name = "[PANEL] EnemyAbilitiesPanel";
            TextObject("[TEXT] EnemyAbilitiesText", enemyAbilities.transform, "Special Abilities: None", 12f, Text, TextAlignmentOptions.Left, Anchor(0, 0, 1, 1, 8, 0, -8, 0));
            var enemyStatus = PanelObject("[PANEL] EnemyStatusEffectsPanel", enemy.transform, new Color(0f, 0f, 0f, 0.10f), 32f);
            enemyStatus.name = "[PANEL] EnemyStatusEffectsPanel";
            TextObject("[TEXT] EnemyStatusEffectsText", enemyStatus.transform, string.Empty, 12f, Text, TextAlignmentOptions.Left, Anchor(0, 0, 1, 1, 8, 0, -8, 0));
            enemyStatus.SetActive(false);
            var enemyDetails = PanelObject("[PANEL] EnemyCombatDetailsPanel", enemy.transform, new Color(0f, 0f, 0f, 0.10f), 32f);
            enemyDetails.name = "[PANEL] EnemyCombatDetailsPanel";
            TextObject("[TEXT] EnemyCombatDetailsText", enemyDetails.transform, string.Empty, 12f, Text, TextAlignmentOptions.Left, Anchor(0, 0, 1, 1, 8, 0, -8, 0));
            enemyDetails.SetActive(false);
            var bossMechanics = PanelObject("[PANEL] EnemyBossMechanicsPanel", enemy.transform, new Color(0f, 0f, 0f, 0.10f), 32f);
            TextObject("[TEXT] EnemyBossMechanicsText", bossMechanics.transform, string.Empty, 12f, Text, TextAlignmentOptions.Left, Anchor(0, 0, 1, 1, 8, 0, -8, 0));
            bossMechanics.SetActive(false);
            var phaseInfo = PanelObject("[PANEL] EnemyPhaseInfoPanel", enemy.transform, new Color(0f, 0f, 0f, 0.10f), 32f);
            TextObject("[TEXT] EnemyPhaseInfoText", phaseInfo.transform, string.Empty, 12f, Text, TextAlignmentOptions.Left, Anchor(0, 0, 1, 1, 8, 0, -8, 0));
            phaseInfo.SetActive(false);

            var log = Column("[PANEL] CombatLogPanel", columns.transform, 0.36f, 340f);
            log.GetComponent<VerticalLayoutGroup>().spacing = 6f;
            var toolbar = PanelObject("[PANEL] CombatLogToolbar", log.transform, new Color(0f, 0f, 0f, 0.12f), 32f);
            TextObject("[HEADER] CombatLogHeader", toolbar.transform, "COMBAT LOG", 16f, Gold, TextAlignmentOptions.Left, Anchor(0, 0, .25f, 1, 8, 0, 0, 0));
            TextObject("[TEXT] AutoScrollStatusText", toolbar.transform, "Auto Scroll: On", 12f, Muted, TextAlignmentOptions.Left, Anchor(.24f, 0, .55f, 1, 0, 0, 0, 0));
            ButtonObject("[BUTTON] NewLogEventsButton", toolbar.transform, "0 new", Green, Anchor(.55f, .5f, .73f, .5f, 0, 0, -4, 22)).SetActive(false);
            ButtonObject("[BUTTON] PauseScrollButton", toolbar.transform, "Pause", Raised, Anchor(.73f, .5f, .86f, .5f, 0, 0, -4, 22));
            ButtonObject("[BUTTON] ClearCombatLogButton", toolbar.transform, "Clear", Raised, Anchor(.86f, .5f, 1, .5f, 0, 0, -8, 22));
            var logContainer = ScrollContentPanel("[DYNAMIC CONTENT] CombatLogContainer", log.transform, 0f);
            LogTemplate(logContainer.transform);
            var floatingRoot = PanelObject("[POOL] FloatingCombatTextRoot", active.transform, new Color(0f, 0f, 0f, 0f), 0f);
            floatingRoot.GetComponent<Image>().raycastTarget = false;
            var floatingElement = floatingRoot.AddComponent<LayoutElement>();
            floatingElement.ignoreLayout = true;
            StretchRect((RectTransform)floatingRoot.transform, 0f, 0f, 0f, 34f);
            floatingRoot.transform.SetAsLastSibling();
            for (var i = 0; i < 12; i++)
            {
                var floatText = TextObject(i == 0 ? "[TEXT] FloatingCombatTextTemplate" : "[TEXT] FloatingCombatText", floatingRoot.transform, string.Empty, 14f, Text, TextAlignmentOptions.Center, Anchor(.5f, .5f, .5f, .5f, 0, 0, 120, 28));
                floatText.gameObject.SetActive(false);
            }
            var footer = PanelObject("[FOOTER] SessionStatsFooter", active.transform, new Color(0f, 0f, 0f, 0.12f), 34f);
            var footerLayout = footer.AddComponent<HorizontalLayoutGroup>();
            footerLayout.spacing = 10f;
            footerLayout.padding = new RectOffset(8, 8, 5, 5);
            footerLayout.childControlWidth = true;
            footerLayout.childControlHeight = true;
            footerLayout.childForceExpandHeight = true;
            SessionFooterLabel(footer.transform, "SESSION", "[TEXT] SessionStatsTitle", 1.2f, Gold);
            SessionFooterLabel(footer.transform, "Damage", "[TEXT] SessionDamageDealtText", 1.6f, Text);
            SessionFooterLabel(footer.transform, "Companion", "[TEXT] SessionCompanionDamageText", 1.3f, Text);
            SessionFooterLabel(footer.transform, "Taken", "[TEXT] SessionDamageTakenText", 1.2f, Text);
            SessionFooterLabel(footer.transform, "Healing", "[TEXT] SessionHealingText", 1.2f, Text);
            SessionFooterLabel(footer.transform, "Defeated", "[TEXT] SessionDefeatedText", 1.2f, Text);
            SessionFooterLabel(footer.transform, "DPS", "[TEXT] SessionDpsText", 1f, Text);
            var hiddenStats = TextObject("[TEXT] SessionStatsText", footer.transform, string.Empty, 1f, new Color(0f, 0f, 0f, 0f), TextAlignmentOptions.Center, Stretch(), 1f);
            hiddenStats.gameObject.SetActive(false);

            controller.AutoBind();
            EditorUtility.SetDirty(screen.gameObject);
        }

        private static void RebuildEquipmentScreen(EquipmentSystem equipment, InventorySystem inventory)
        {
            var screen = FindSceneObject("[SCREEN] EquipmentScreen");
            if (screen == null)
            {
                return;
            }

            Undo.RegisterFullObjectHierarchyUndo(screen.gameObject, "Build Equipment Screen");
            ClearChildren(screen);
            var controller = screen.GetComponent<EquipmentScreenController>() ?? screen.gameObject.AddComponent<EquipmentScreenController>();
            controller.ConfigureForEditor(equipment, inventory);
            ConfigureRoot(screen, Deep);
            var header = PanelObject("[HEADER] EquipmentHeader", screen, Raised, 86f);
            TextObject("[TEXT] ScreenTitle", header.transform, "Equipment", 34f, Gold, TextAlignmentOptions.Left, Anchor(0, .5f, 0, .5f, 92, 14, 300, 42));
            TextObject("[TEXT] ScreenSubtitle", header.transform, "Equip Main-Hand weapons and Offhand shields for Combat.", 16f, Muted, TextAlignmentOptions.Left, Anchor(0, .5f, 0, .5f, 94, -20, 620, 28));

            var body = PanelObject("[LAYOUT] EquipmentBody", screen, Panel, 0f, true);
            var layout = body.AddComponent<HorizontalLayoutGroup>();
            layout.padding = new RectOffset(10, 10, 10, 10);
            layout.spacing = 10f;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandHeight = true;

            var slots = Column("[PANEL] EquippedSlotsPanel", body.transform, 0.42f, 500f);
            TextObject("[TEXT] MainHandEquippedValue", slots.transform, "Main-Hand", 17f, Text, TextAlignmentOptions.TopLeft, Stretch(), 100f);
            TextObject("[TEXT] OffhandEquippedValue", slots.transform, "Offhand", 17f, Text, TextAlignmentOptions.TopLeft, Stretch(), 100f);
            TextObject("[TEXT] EquipmentStatSummary", slots.transform, "Stats", 17f, Gold, TextAlignmentOptions.TopLeft, Stretch(), 100f);
            TextObject("[TEXT] EquipmentStatus", slots.transform, "Select an equipment item from Inventory to equip it.", 15f, Muted, TextAlignmentOptions.Left, Stretch(), 50f);

            var inventoryPanel = Column("[PANEL] EquipmentInventoryPanel", body.transform, 0.58f, 600f);
            TextObject("[HEADER] EquipmentInventoryHeader", inventoryPanel.transform, "EQUIPMENT IN INVENTORY", 19f, Gold, TextAlignmentOptions.Left, Stretch(), 34f);
            var container = ContentPanel("[DYNAMIC CONTENT] EquipmentInventoryContainer", inventoryPanel.transform, 420f);
            EquipmentTemplate(container.transform);
            controller.AutoBind();
            EditorUtility.SetDirty(screen.gameObject);
        }

        private static void RebuildActiveActivityCombatState(CombatSystem combat)
        {
            var bar = FindSceneObject("[PERSISTENT] ActiveActivityBar");
            if (bar == null)
            {
                return;
            }

            var controller = bar.GetComponent<ActiveActivityBarController>();
            var state = FindChildTransform(bar, "[STATE] CombatActivityState");
            if (state == null)
            {
                state = PanelObject("[STATE] CombatActivityState", bar, new Color(0f, 0f, 0f, 0f), 0f).transform;
                StretchRect((RectTransform)state, 0, 0, 0, 0);
            }

            Undo.RegisterFullObjectHierarchyUndo(state.gameObject, "Build Active Combat Bar");
            ClearChildren(state);
            TextObject("[TEXT] CombatDisciplineText", state, "Combat - Warrior", 20f, Text, TextAlignmentOptions.Left, Anchor(0, .5f, 0, .5f, 120, 16, 210, 28));
            TextObject("[TEXT] CombatEnemyText", state, "Enemy", 15f, Muted, TextAlignmentOptions.Left, Anchor(0, .5f, 0, .5f, 120, -14, 210, 24));
            BarObject("[BAR] CompactPlayerHealthBar", state, Anchor(.27f, .5f, .27f, .5f, 0, 18, 260, 24), Red);
            BarObject("[BAR] CompactDevotionBar", state, Anchor(.27f, .5f, .27f, .5f, 0, -14, 260, 24), Blue);
            BarObject("[BAR] CompactEnemyHealthBar", state, Anchor(.50f, .5f, .50f, .5f, 0, 0, 300, 28), Green);
            ButtonObject("[BUTTON] OpenCombatButton", state, "Open", Raised, Anchor(1, .5f, 1, .5f, -246, 0, 152, 52));
            ButtonObject("[BUTTON] QuitCombatButton", state, "Leave Combat", Red, Anchor(1, .5f, 1, .5f, -82, 0, 120, 58));

            controller?.ConfigureCombatForEditor(combat);
            EditorUtility.SetDirty(bar.gameObject);
        }

        private static ItemDefinition Item(string id, string name, string description, ItemCategory category, string type, bool stackable, long maxStack, int sellValue, bool sellable, bool destroyable)
        {
            var path = $"{ItemDataFolder}/{id}.asset";
            var asset = AssetDatabase.LoadAssetAtPath<ItemDefinition>(path);
            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<ItemDefinition>();
                AssetDatabase.CreateAsset(asset, path);
            }

            asset.ConfigureForEditor(id, name, description, category, type, stackable, maxStack, sellValue, sellable, destroyable);
            EditorUtility.SetDirty(asset);
            return asset;
        }

        private static EquipmentDefinition Equipment(string id, string name, EquipmentSlot slot, bool twoHanded, bool warrior, int minDamage, int maxDamage, float interval, int accuracy, int defense, int health)
        {
            var path = $"{EquipmentDataFolder}/{id}.asset";
            var asset = AssetDatabase.LoadAssetAtPath<EquipmentDefinition>(path);
            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<EquipmentDefinition>();
                AssetDatabase.CreateAsset(asset, path);
            }

            asset.ConfigureForEditor(id, name, slot, twoHanded);
            asset.ConfigureCombatForEditor(warrior, minDamage, maxDamage, interval, accuracy, defense, health, 0f, 0f);
            EditorUtility.SetDirty(asset);
            return asset;
        }

        private static CombatEnemyDefinition Enemy(string id, string name, string description, int level, int health, int minDamage, int maxDamage, float interval, int accuracy, int defense, float crit, float critDamage, float xp, float respawn, int minGold, int maxGold, IEnumerable<CombatLootEntry> loot, IEnumerable<CombatLootEntry> firstClear)
        {
            var path = $"{CombatDataFolder}/{id}.asset";
            var asset = AssetDatabase.LoadAssetAtPath<CombatEnemyDefinition>(path);
            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<CombatEnemyDefinition>();
                AssetDatabase.CreateAsset(asset, path);
            }

            asset.ConfigureForEditor(id, name, description, level, health, minDamage, maxDamage, interval, accuracy, defense, crit, critDamage, xp, respawn, minGold, maxGold, loot, firstClear);
            EditorUtility.SetDirty(asset);
            return asset;
        }

        private static CombatLootEntry Loot(string itemId, float chance, int min, int max, bool important = false)
        {
            return new CombatLootEntry { itemId = itemId, chance = chance, minQuantity = min, maxQuantity = max, important = important };
        }

        private static void SelectionBlock(Transform parent, string title, string containerName, string templateName)
        {
            TextObject("[HEADER] " + title, parent, title, 15f, Gold, TextAlignmentOptions.Left, Stretch(), 24f);
            var container = ContentPanel(containerName, parent, 70f);
            var template = ButtonObject(templateName, container.transform, title, Raised, 38f);
            template.gameObject.SetActive(false);
        }

        private static GameObject HeaderSection(string name, Transform parent, float flex, float minWidth)
        {
            var section = PanelObject(name, parent, new Color(0f, 0f, 0f, 0f), 0f, true);
            var element = section.GetComponent<LayoutElement>();
            element.flexibleWidth = flex;
            element.minWidth = minWidth;
            var layout = section.AddComponent<VerticalLayoutGroup>();
            layout.spacing = 4f;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;
            return section;
        }

        private static GameObject InfoBlock(Transform parent, string title, string textName, string body, float height)
        {
            var block = PanelObject("[PANEL] " + title.Replace(" ", string.Empty), parent, new Color(0f, 0f, 0f, 0.12f), height);
            TextObject("[HEADER] " + title.Replace(" ", string.Empty), block.transform, title, 14f, Gold, TextAlignmentOptions.Left, Anchor(0, 1, 1, 1, 8, -16, -8, 22));
            TextObject(textName, block.transform, body, 13f, Text, TextAlignmentOptions.TopLeft, Anchor(0, 0, 1, 1, 8, 4, -8, -30));
            return block;
        }

        private static GameObject IconFrame(string frameName, string imageName, string labelName, Transform parent, string label, float size, RectSpec rect)
        {
            var frame = PanelObject(frameName, parent, new Color(0.03f, 0.05f, 0.07f, 1f), 0f);
            SetRect((RectTransform)frame.transform, rect);
            var element = frame.AddComponent<LayoutElement>();
            element.preferredWidth = size;
            element.preferredHeight = size;
            element.minWidth = size;
            element.minHeight = size;

            var image = new GameObject(imageName, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            image.layer = LayerMask.NameToLayer("UI");
            image.transform.SetParent(frame.transform, false);
            StretchRect((RectTransform)image.transform, 4f, 4f, 4f, 4f);
            var imageComponent = image.GetComponent<Image>();
            imageComponent.color = new Color(0.08f, 0.11f, 0.15f, 1f);
            imageComponent.preserveAspect = true;
            imageComponent.raycastTarget = false;

            TextObject(labelName, frame.transform, label, 12f, Muted, TextAlignmentOptions.Center, Stretch());
            return frame;
        }

        private static GameObject PortraitRow(Transform parent, string portraitPrefix, string imageName, string placeholderName, string placeholder, string titleTextName, string title, RuntimeFillBar healthBar, RuntimeFillBar secondaryBar)
        {
            var row = PanelObject("[PANEL] " + portraitPrefix + "PortraitRow", parent, new Color(0f, 0f, 0f, 0.10f), 88f);
            var layout = row.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = 10f;
            layout.padding = new RectOffset(8, 8, 8, 8);
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandHeight = false;

            IconFrame("[FRAME] " + portraitPrefix + "PortraitFrame", imageName, placeholderName, row.transform, placeholder, 72f, Stretch());
            var info = PanelObject("[PANEL] " + portraitPrefix + "PortraitInfo", row.transform, new Color(0f, 0f, 0f, 0f), 0f, true);
            var infoElement = info.GetComponent<LayoutElement>();
            infoElement.flexibleWidth = 1f;
            var infoLayout = info.AddComponent<VerticalLayoutGroup>();
            infoLayout.spacing = 5f;
            infoLayout.childControlWidth = true;
            infoLayout.childControlHeight = true;
            infoLayout.childForceExpandWidth = true;
            TextObject(titleTextName, info.transform, title, 20f, Gold, TextAlignmentOptions.Left, Stretch(), 24f);
            healthBar.transform.SetParent(info.transform, false);
            secondaryBar.transform.SetParent(info.transform, false);
            return row;
        }

        private static void LogTemplate(Transform parent)
        {
            var row = PanelObject("[ROW] CombatLogRowTemplate", parent, new Color(0f, 0f, 0f, 0.12f), 24f);
            row.SetActive(false);
            TextObject("[TEXT] LogMessage", row.transform, "Combat log entry", 13f, Text, TextAlignmentOptions.Left, Stretch());
        }

        private static void EquipmentTemplate(Transform parent)
        {
            var row = PanelObject("[ROW] EquipmentEntryTemplate", parent, Raised, 58f);
            row.SetActive(false);
            TextObject("[TEXT] EquipmentName", row.transform, "Equipment", 16f, Gold, TextAlignmentOptions.Left, Anchor(0, 0, .35f, 1, 10, 0, 0, 0));
            TextObject("[TEXT] EquipmentStats", row.transform, "Stats", 13f, Text, TextAlignmentOptions.Left, Anchor(.35f, 0, .82f, 1, 0, 0, -8, 0));
            ButtonObject("[BUTTON] EquipButton", row.transform, "Equip", Green, Anchor(.84f, .5f, 1, .5f, -40, 0, -10, 34));
        }

        private static GameObject Column(string name, Transform parent, float flex, float minWidth)
        {
            var column = PanelObject(name, parent, Deep, 0f, true);
            var element = column.GetComponent<LayoutElement>();
            element.flexibleWidth = flex;
            element.minWidth = minWidth;
            var layout = column.AddComponent<VerticalLayoutGroup>();
            layout.spacing = 8f;
            layout.padding = new RectOffset(10, 10, 10, 10);
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;
            return column;
        }

        private static GameObject ContentPanel(string name, Transform parent, float height)
        {
            var panel = PanelObject(name, parent, new Color(0f, 0f, 0f, 0.15f), height);
            var layout = panel.AddComponent<VerticalLayoutGroup>();
            layout.spacing = 6f;
            layout.padding = new RectOffset(6, 6, 6, 6);
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;
            return panel;
        }

        private static GameObject ScrollContentPanel(string contentName, Transform parent, float height)
        {
            var scroll = PanelObject("[SCROLL] CombatLogScroll", parent, new Color(0f, 0f, 0f, 0.15f), height, height <= 0f);
            var scrollRect = scroll.AddComponent<ScrollRect>();
            var autoScroll = scroll.AddComponent<CombatLogAutoScrollController>();
            scrollRect.horizontal = false;
            scrollRect.vertical = true;
            scrollRect.movementType = ScrollRect.MovementType.Clamped;
            scrollRect.scrollSensitivity = 20f;

            var viewport = PanelObject("[VIEWPORT] CombatLogViewport", scroll.transform, new Color(0f, 0f, 0f, 0.05f), 0f);
            StretchRect((RectTransform)viewport.transform, 0f, 0f, 0f, 0f);
            var mask = viewport.AddComponent<Mask>();
            mask.showMaskGraphic = false;

            var content = PanelObject(contentName, viewport.transform, new Color(0f, 0f, 0f, 0f), 0f);
            var contentRect = (RectTransform)content.transform;
            contentRect.anchorMin = new Vector2(0f, 1f);
            contentRect.anchorMax = new Vector2(1f, 1f);
            contentRect.pivot = new Vector2(0.5f, 1f);
            contentRect.anchoredPosition = Vector2.zero;
            contentRect.sizeDelta = Vector2.zero;

            var layout = content.AddComponent<VerticalLayoutGroup>();
            layout.spacing = 4f;
            layout.padding = new RectOffset(6, 6, 6, 6);
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;
            var fitter = content.AddComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            scrollRect.viewport = (RectTransform)viewport.transform;
            scrollRect.content = contentRect;
            autoScroll.ConfigureForEditor(scrollRect);
            return content;
        }

        private static void ConfigureRoot(Transform screen, Color color)
        {
            var image = screen.GetComponent<Image>() ?? screen.gameObject.AddComponent<Image>();
            image.color = color;
            var outline = screen.GetComponent<Outline>() ?? screen.gameObject.AddComponent<Outline>();
            outline.effectColor = Border;
            outline.effectDistance = new Vector2(1f, -1f);
            var layout = screen.GetComponent<VerticalLayoutGroup>() ?? screen.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(12, 12, 10, 10);
            layout.spacing = 10f;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;
        }

        private static GameObject ToggleObject(string name, Transform parent, string label)
        {
            var root = PanelObject(name, parent, new Color(0f, 0f, 0f, 0f), 34f);
            var toggle = root.AddComponent<Toggle>();
            var box = PanelObject("[IMAGE] Checkbox", root.transform, new Color(0.02f, 0.03f, 0.04f, 1f), 0f);
            SetRect((RectTransform)box.transform, new Vector2(0, .5f), new Vector2(0, .5f), new Vector2(15, 0), new Vector2(22, 22));
            box.GetComponent<Image>().raycastTarget = true;
            var check = PanelObject("[IMAGE] Checkmark", box.transform, Green, 0f);
            SetRect((RectTransform)check.transform, new Vector2(.5f, .5f), new Vector2(.5f, .5f), Vector2.zero, new Vector2(14, 14));
            check.GetComponent<Image>().raycastTarget = false;
            toggle.targetGraphic = box.GetComponent<Image>();
            toggle.graphic = check.GetComponent<Image>();
            TextObject("[TEXT] Label", root.transform, label, 15f, Text, TextAlignmentOptions.Left, Anchor(0, 0, 1, 1, 44, 0, -4, 0));
            toggle.isOn = true;
            return root;
        }

        private static GameObject ToggleObject(string name, Transform parent, string label, RectSpec rect)
        {
            var root = ToggleObject(name, parent, label);
            SetRect((RectTransform)root.transform, rect);
            return root;
        }

        private static GameObject ToggleObject(string name, Transform parent, string label, RectSpec rect, float preferredHeight)
        {
            var root = ToggleObject(name, parent, label);
            SetRect((RectTransform)root.transform, rect);
            var element = root.GetComponent<LayoutElement>();
            element.preferredHeight = preferredHeight;
            element.minHeight = preferredHeight;
            return root;
        }

        private static void SessionFooterLabel(Transform parent, string value, string textName, float flex, Color color)
        {
            if (textName == "[TEXT] SessionStatsTitle")
            {
                var title = TextObject(textName, parent, value, 12f, color, TextAlignmentOptions.Center, Stretch());
                var titleElement = title.gameObject.AddComponent<LayoutElement>();
                titleElement.flexibleWidth = flex;
                titleElement.minWidth = 60f;
                return;
            }

            var item = PanelObject("[ITEM] " + textName.Replace("[TEXT] ", string.Empty).Replace("Text", "FooterItem"), parent, new Color(0f, 0f, 0f, 0f), 0f, true);
            var element = item.GetComponent<LayoutElement>();
            element.flexibleWidth = flex;
            element.minWidth = 64f;
            var layout = item.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = 4f;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandHeight = true;
            TextObject(textName.Replace("Text", "Label"), item.transform, value, 11f, Muted, TextAlignmentOptions.Right, Stretch());
            var initialValue = textName == "[TEXT] SessionCompanionDamageText" ? "—" : "0";
            TextObject(textName, item.transform, initialValue, 12f, color, TextAlignmentOptions.Left, Stretch());
        }

        private static void SectionHeading(Transform parent, string name, string label, float height)
        {
            var row = PanelObject(name, parent, new Color(0f, 0f, 0f, 0f), height);
            TextObject("[TEXT] Label", row.transform, label, 12f, Gold, TextAlignmentOptions.Left, Anchor(0, 0, 1, 1, 4, 0, -4, 0));
        }

        private static GameObject StatGrid(string name, Transform parent, RectSpec rect)
        {
            var grid = PanelObject(name, parent, new Color(0f, 0f, 0f, 0.08f), 0f);
            SetRect((RectTransform)grid.transform, rect);
            var layout = grid.AddComponent<GridLayoutGroup>();
            layout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            layout.constraintCount = 2;
            layout.cellSize = new Vector2(132f, 24f);
            layout.spacing = new Vector2(10f, 4f);
            layout.padding = new RectOffset(8, 8, 5, 5);
            return grid;
        }

        private static GameObject StatCell(Transform parent, string labelName, string valueName, string label, string value)
        {
            var cell = PanelObject("[CELL] StatCell", parent, new Color(0f, 0f, 0f, 0f), 0f);
            TextObject(labelName, cell.transform, label, 10f, Muted, TextAlignmentOptions.Left, Anchor(0, .5f, .68f, .5f, 0, 5, 0, 12));
            TextObject(valueName, cell.transform, value, 12f, Text, TextAlignmentOptions.Left, Anchor(.70f, .5f, 1, .5f, 0, 5, 0, 14));
            return cell;
        }

        private static GameObject ConsumableSlot(
            Transform parent,
            string slotName,
            string imageName,
            string placeholderName,
            string infoName,
            string quantityName,
            string placeholder,
            string label,
            bool interactable,
            RectSpec rect)
        {
            var slot = ButtonObject(slotName, parent, string.Empty, interactable ? Raised : new Color(0.06f, 0.08f, 0.10f, 1f), rect);
            slot.GetComponent<Button>().interactable = interactable;
            var iconFrame = IconFrame("[FRAME] " + slotName.Replace("[BUTTON] ", string.Empty) + "IconFrame", imageName, placeholderName, slot.transform, placeholder, 52f, Anchor(.5f, 1, .5f, 1, 0, -35, 52, 52));
            var quantityBackground = PanelObject("[IMAGE] QuantityBackground", iconFrame.transform, new Color(0f, 0f, 0f, 0.58f), 0f);
            SetRect((RectTransform)quantityBackground.transform, Anchor(1, 0, 1, 0, -14, 11, 26, 18));
            quantityBackground.GetComponent<Image>().raycastTarget = false;
            quantityBackground.GetComponent<Outline>().effectColor = new Color(0f, 0f, 0f, 0.75f);
            var quantityText = TextObject(quantityName, iconFrame.transform, "0", 11f, Text, TextAlignmentOptions.BottomRight, Anchor(1, 0, 1, 0, -15, 10, 24, 16));
            quantityText.raycastTarget = false;
            var infoText = TextObject(infoName, slot.transform, label, 11f, interactable ? Text : Muted, TextAlignmentOptions.Center, Anchor(0, 0, 1, 0, 6, 7, -6, 18));
            infoText.textWrappingMode = TextWrappingModes.NoWrap;
            infoText.overflowMode = TextOverflowModes.Ellipsis;
            return slot;
        }

        private static GameObject ButtonObject(string name, Transform parent, string label, Color color, float height)
        {
            var button = PanelObject(name, parent, color, height);
            var buttonImage = button.GetComponent<Image>();
            buttonImage.raycastTarget = true;
            var buttonComponent = button.AddComponent<Button>();
            buttonComponent.targetGraphic = buttonImage;
            TextObject("[TEXT] Label", button.transform, label, 16f, Text, TextAlignmentOptions.Center, Stretch());
            return button;
        }

        private static GameObject ButtonObject(string name, Transform parent, string label, Color color, RectSpec rect)
        {
            var button = PanelObject(name, parent, color, 0f);
            SetRect((RectTransform)button.transform, rect);
            var buttonImage = button.GetComponent<Image>();
            buttonImage.raycastTarget = true;
            var buttonComponent = button.AddComponent<Button>();
            buttonComponent.targetGraphic = buttonImage;
            TextObject("[TEXT] Label", button.transform, label, 16f, Text, TextAlignmentOptions.Center, Stretch());
            return button;
        }

        private static RuntimeFillBar BarObject(string name, Transform parent, RectSpec rect, Color fillColor, float preferredHeight = 0f)
        {
            var bar = PanelObject(name, parent, new Color(0.02f, 0.02f, 0.02f, 1f), preferredHeight);
            bar.GetComponent<Image>().raycastTarget = false;
            SetRect((RectTransform)bar.transform, rect);
            var fill = new GameObject("Fill", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            fill.transform.SetParent(bar.transform, false);
            var fillRect = (RectTransform)fill.transform;
            fillRect.anchorMin = Vector2.zero;
            fillRect.anchorMax = new Vector2(0.5f, 1f);
            fillRect.offsetMin = Vector2.zero;
            fillRect.offsetMax = Vector2.zero;
            fill.GetComponent<Image>().color = fillColor;
            fill.GetComponent<Image>().raycastTarget = false;
            var text = TextObject("[TEXT] ValueText", bar.transform, "--", 13f, Text, TextAlignmentOptions.Center, Stretch());
            var runtime = bar.AddComponent<RuntimeFillBar>();
            runtime.ConfigureForEditor(fillRect, text);
            return runtime;
        }

        private static GameObject PanelObject(string name, Transform parent, Color color, float preferredHeight, bool flexibleHeight = false)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Outline));
            go.layer = LayerMask.NameToLayer("UI");
            go.transform.SetParent(parent, false);
            go.GetComponent<Image>().color = color;
            var outline = go.GetComponent<Outline>();
            outline.effectColor = Border;
            outline.effectDistance = new Vector2(1f, -1f);
            if (preferredHeight > 0f || flexibleHeight)
            {
                var element = go.AddComponent<LayoutElement>();
                if (preferredHeight > 0f)
                {
                    element.preferredHeight = preferredHeight;
                    element.minHeight = preferredHeight;
                    element.flexibleHeight = 0f;
                }

                if (flexibleHeight)
                {
                    element.flexibleHeight = 1f;
                }
            }

            return go;
        }

        private static TMP_Text TextObject(string name, Transform parent, string value, float size, Color color, TextAlignmentOptions alignment, RectSpec rect, float preferredHeight = 0f)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
            go.layer = LayerMask.NameToLayer("UI");
            go.transform.SetParent(parent, false);
            SetRect((RectTransform)go.transform, rect);
            if (preferredHeight > 0f)
            {
                var element = go.AddComponent<LayoutElement>();
                element.preferredHeight = preferredHeight;
                element.minHeight = preferredHeight;
            }

            var text = go.GetComponent<TextMeshProUGUI>();
            text.text = value;
            text.fontSize = size;
            text.color = color;
            text.alignment = alignment;
            text.textWrappingMode = TextWrappingModes.Normal;
            text.overflowMode = TextOverflowModes.Ellipsis;
            text.raycastTarget = false;
            return text;
        }

        private static Transform FindSceneObject(string name)
        {
            return Object.FindObjectsByType<Transform>(FindObjectsInactive.Include)
                .FirstOrDefault(transform => transform.name == name && transform.gameObject.scene.IsValid());
        }

        private static Transform FindChildTransform(Transform root, string name)
        {
            if (root == null)
            {
                return null;
            }

            foreach (Transform child in root)
            {
                if (child.name == name)
                {
                    return child;
                }

                var nested = FindChildTransform(child, name);
                if (nested != null)
                {
                    return nested;
                }
            }

            return null;
        }

        private static RectTransform HierarchyChild(Transform root, string name)
        {
            return FindChildTransform(root, name) as RectTransform;
        }

        private static void ClearChildren(Transform parent)
        {
            for (var i = parent.childCount - 1; i >= 0; i--)
            {
                Object.DestroyImmediate(parent.GetChild(i).gameObject);
            }
        }

        private static void StretchRect(RectTransform rect, float left, float right, float top, float bottom)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = new Vector2(left, bottom);
            rect.offsetMax = new Vector2(-right, -top);
        }

        private static void SetRect(RectTransform rect, RectSpec spec)
        {
            rect.anchorMin = spec.anchorMin;
            rect.anchorMax = spec.anchorMax;
            var stretchX = !Mathf.Approximately(spec.anchorMin.x, spec.anchorMax.x);
            var stretchY = !Mathf.Approximately(spec.anchorMin.y, spec.anchorMax.y);
            if (!stretchX && !stretchY)
            {
                rect.anchoredPosition = spec.position;
                rect.sizeDelta = spec.size;
                return;
            }

            if (stretchX && stretchY)
            {
                rect.offsetMin = spec.position;
                rect.offsetMax = spec.size;
                return;
            }

            if (stretchX)
            {
                rect.offsetMin = new Vector2(spec.position.x, rect.offsetMin.y);
                rect.offsetMax = new Vector2(spec.size.x, rect.offsetMax.y);
                rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, spec.position.y);
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, spec.size.y);
                return;
            }

            rect.anchoredPosition = new Vector2(spec.position.x, rect.anchoredPosition.y);
            rect.sizeDelta = new Vector2(spec.size.x, rect.sizeDelta.y);
            rect.offsetMin = new Vector2(rect.offsetMin.x, spec.position.y);
            rect.offsetMax = new Vector2(rect.offsetMax.x, spec.size.y);
        }

        private static void SetRect(RectTransform rect, Vector2 anchorMin, Vector2 anchorMax, Vector2 position, Vector2 size)
        {
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.anchoredPosition = position;
            rect.sizeDelta = size;
        }

        private static RectSpec Stretch()
        {
            return new RectSpec(Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        }

        private static RectSpec Anchor(float ax, float ay, float bx, float by, float x, float y, float w, float h)
        {
            return new RectSpec(new Vector2(ax, ay), new Vector2(bx, by), new Vector2(x, y), new Vector2(w, h));
        }

        private static Color HtmlColor(string hex)
        {
            if (ColorUtility.TryParseHtmlString("#" + hex, out var color))
            {
                return color;
            }

            throw new InvalidOperationException("Invalid color " + hex);
        }

        private static void EnsureFolders()
        {
            EnsureFolder(CombatDataFolder);
            EnsureFolder(ItemDataFolder);
            EnsureFolder(EquipmentDataFolder);
        }

        private static void EnsureFolder(string folder)
        {
            if (AssetDatabase.IsValidFolder(folder))
            {
                return;
            }

            var parent = Path.GetDirectoryName(folder)?.Replace("\\", "/");
            var name = Path.GetFileName(folder);
            if (!AssetDatabase.IsValidFolder(parent))
            {
                EnsureFolder(parent);
            }

            AssetDatabase.CreateFolder(parent, name);
        }

        private readonly struct RectSpec
        {
            public readonly Vector2 anchorMin;
            public readonly Vector2 anchorMax;
            public readonly Vector2 position;
            public readonly Vector2 size;

            public RectSpec(Vector2 anchorMin, Vector2 anchorMax, Vector2 position, Vector2 size)
            {
                this.anchorMin = anchorMin;
                this.anchorMax = anchorMax;
                this.position = position;
                this.size = size;
            }
        }
    }
}
