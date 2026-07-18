using System.Collections.Generic;
using System.Linq;
using IdleGame.Combat;
using IdleGame.Inventory;
using IdleGame.Items;
using IdleGame.Progression;
using IdleGame.UI.Shared;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IdleGame.UI.Combat
{
    public sealed class CombatScreenController : MonoBehaviour
    {
        private const int MaxVisibleCombatLogRows = 40;

        [SerializeField] private CombatSystem combatSystem;
        [SerializeField] private InventorySystem inventorySystem;
        [SerializeField] private ProfessionProgressionSystem progressionSystem;
        [SerializeField] private Transform selectionState;
        [SerializeField] private Transform activeState;
        [SerializeField] private Transform regionContainer;
        [SerializeField] private Transform activityTypeContainer;
        [SerializeField] private Transform locationContainer;
        [SerializeField] private Transform enemyContainer;
        [SerializeField] private Button regionTemplate;
        [SerializeField] private Button activityTypeTemplate;
        [SerializeField] private Button locationTemplate;
        [SerializeField] private Button enemyTemplate;
        [SerializeField] private TMP_Text breadcrumbText;
        [SerializeField] private TMP_Text detailsNameText;
        [SerializeField] private TMP_Text detailsDescriptionText;
        [SerializeField] private TMP_Text detailsRequirementsText;
        [SerializeField] private TMP_Text detailsStatsText;
        [SerializeField] private TMP_Text detailsLootText;
        [SerializeField] private Button startCombatButton;
        [SerializeField] private TMP_Text startCombatButtonText;
        [SerializeField] private TMP_Text notificationText;
        [SerializeField] private TMP_Text encounterTitleText;
        [SerializeField] private TMP_Text encounterAreaText;
        [SerializeField] private TMP_Text encounterEnemyText;
        [SerializeField] private TMP_Text encounterSessionText;
        [SerializeField] private TMP_Text warriorLevelText;
        [SerializeField] private RuntimeFillBar warriorXpBar;
        [Header("Portrait Sprites")]
        [SerializeField] private Sprite playerPortraitSprite;
        [SerializeField] private Sprite enemyPortraitSprite;
        [SerializeField] private Sprite companionPortraitSprite;
        [Header("Combat Icon Sprites")]
        [SerializeField] private Sprite autoAttackIconSprite;
        [SerializeField] private Sprite heavyStrikeIconSprite;
        [SerializeField] private Sprite potionIconSprite;
        [SerializeField] private Sprite enemyAttackIconSprite;
        [SerializeField] private Sprite companionSkillIconSprite;
        [SerializeField] private Image playerPortraitImage;
        [SerializeField] private TMP_Text playerPortraitPlaceholderText;
        [SerializeField] private Image enemyPortraitImage;
        [SerializeField] private TMP_Text enemyPortraitPlaceholderText;
        [SerializeField] private Image companionPortraitImage;
        [SerializeField] private TMP_Text companionPortraitPlaceholderText;
        [SerializeField] private Image currentActionIconImage;
        [SerializeField] private TMP_Text currentActionIconPlaceholderText;
        [SerializeField] private Image queuedActionIconImage;
        [SerializeField] private TMP_Text queuedActionIconPlaceholderText;
        [SerializeField] private Image heavyStrikeIconImage;
        [SerializeField] private TMP_Text heavyStrikeIconPlaceholderText;
        [SerializeField] private Image potionIconImage;
        [SerializeField] private TMP_Text potionIconPlaceholderText;
        [SerializeField] private Image enemyActionIconImage;
        [SerializeField] private TMP_Text enemyActionIconPlaceholderText;
        [SerializeField] private Image companionSkillIconImage;
        [SerializeField] private TMP_Text companionSkillIconPlaceholderText;
        [SerializeField] private TMP_Text playerNameText;
        [SerializeField] private TMP_Text playerStatsText;
        [SerializeField] private TMP_Text playerDamageValueText;
        [SerializeField] private TMP_Text playerAttackSpeedValueText;
        [SerializeField] private TMP_Text playerAccuracyValueText;
        [SerializeField] private TMP_Text playerDefenceValueText;
        [SerializeField] private TMP_Text playerCriticalValueText;
        [SerializeField] private TMP_Text currentActionNameText;
        [SerializeField] private TMP_Text currentActionRemainingText;
        [SerializeField] private Transform queuedActionPanel;
        [SerializeField] private TMP_Text queuedActionText;
        [SerializeField] private RuntimeFillBar playerHealthBar;
        [SerializeField] private RuntimeFillBar devotionBar;
        [SerializeField] private RuntimeFillBar playerAttackBar;
        [SerializeField] private TMP_Text enemyNameText;
        [SerializeField] private TMP_Text enemyStatsText;
        [SerializeField] private TMP_Text enemyDamageValueText;
        [SerializeField] private TMP_Text enemyAttackSpeedValueText;
        [SerializeField] private TMP_Text enemyAccuracyValueText;
        [SerializeField] private TMP_Text enemyDefenceValueText;
        [SerializeField] private RuntimeFillBar enemyHealthBar;
        [SerializeField] private RuntimeFillBar enemyAttackBar;
        [SerializeField] private Transform enemyAbilitiesPanel;
        [SerializeField] private Transform enemyStatusEffectsPanel;
        [SerializeField] private Transform enemyCombatDetailsPanel;
        [SerializeField] private TMP_Text enemyActionNameText;
        [SerializeField] private TMP_Text enemyActionDetailsText;
        [SerializeField] private TMP_Text enemyActionRemainingText;
        [SerializeField] private Button heavyStrikeButton;
        [SerializeField] private TMP_Text heavyStrikeText;
        [SerializeField] private TMP_Text heavyStrikeButtonText;
        [SerializeField] private TMP_Text heavyStrikeReasonText;
        [SerializeField] private Button potionButton;
        [SerializeField] private TMP_Text potionText;
        [SerializeField] private TMP_Text potionButtonText;
        [Header("Consumable Slot Sprites")]
        [SerializeField] private Sprite elixir1IconSprite;
        [SerializeField] private Sprite elixir2IconSprite;
        [SerializeField] private Sprite elixir3IconSprite;
        [SerializeField] private Sprite elixir4IconSprite;
        [SerializeField] private Sprite foodIconSprite;
        [SerializeField] private Image elixir1IconImage;
        [SerializeField] private Image elixir2IconImage;
        [SerializeField] private Image elixir3IconImage;
        [SerializeField] private Image elixir4IconImage;
        [SerializeField] private Image foodIconImage;
        [SerializeField] private TMP_Text elixir1IconPlaceholderText;
        [SerializeField] private TMP_Text elixir2IconPlaceholderText;
        [SerializeField] private TMP_Text elixir3IconPlaceholderText;
        [SerializeField] private TMP_Text elixir4IconPlaceholderText;
        [SerializeField] private TMP_Text foodIconPlaceholderText;
        [SerializeField] private Button elixir1Button;
        [SerializeField] private Button elixir2Button;
        [SerializeField] private Button elixir3Button;
        [SerializeField] private Button elixir4Button;
        [SerializeField] private Button foodButton;
        [SerializeField] private TMP_Text potionQuantityText;
        [SerializeField] private TMP_Text elixir1QuantityText;
        [SerializeField] private TMP_Text elixir2QuantityText;
        [SerializeField] private TMP_Text elixir3QuantityText;
        [SerializeField] private TMP_Text elixir4QuantityText;
        [SerializeField] private TMP_Text foodQuantityText;
        [SerializeField] private TMP_Text playerStatusEffectsText;
        [SerializeField] private Transform playerStatusEffectsPanel;
        [SerializeField] private Transform companionCombatPanel;
        [SerializeField] private TMP_Text companionSummaryText;
        [SerializeField] private RuntimeFillBar companionActionBar;
        [SerializeField] private Toggle autoRepeatToggle;
        [SerializeField] private Toggle heavyStrikeAutoToggle;
        [SerializeField] private TMP_Text enemyAbilitiesText;
        [SerializeField] private TMP_Text enemyStatusEffectsText;
        [SerializeField] private TMP_Text enemyCombatDetailsText;
        [SerializeField] private TMP_Text sessionStatsText;
        [SerializeField] private TMP_Text sessionDamageDealtText;
        [SerializeField] private TMP_Text sessionCompanionDamageText;
        [SerializeField] private TMP_Text sessionDamageTakenText;
        [SerializeField] private TMP_Text sessionHealingText;
        [SerializeField] private TMP_Text sessionDefeatedText;
        [SerializeField] private TMP_Text sessionDpsText;
        [SerializeField] private TMP_Text autoScrollStatusText;
        [SerializeField] private Button pauseScrollButton;
        [SerializeField] private TMP_Text pauseScrollButtonText;
        [SerializeField] private Button newLogEventsButton;
        [SerializeField] private TMP_Text newLogEventsButtonText;
        [SerializeField] private Button clearCombatLogButton;
        [SerializeField] private Transform combatLogContainer;
        [SerializeField] private GameObject combatLogRowTemplate;
        [SerializeField] private ScrollRect combatLogScrollRect;
        [SerializeField] private CombatLogAutoScrollController combatLogAutoScrollController;

        [Header("Combat Log Colors")]
        [SerializeField] private Color logDefaultColor = new(0.91f, 0.85f, 0.74f);
        [SerializeField] private Color logPlayerHitColor = new(0.55f, 0.9f, 0.68f);
        [SerializeField] private Color logPlayerMissColor = new(0.68f, 0.78f, 0.86f);
        [SerializeField] private Color logEnemyHitColor = new(1f, 0.52f, 0.45f);
        [SerializeField] private Color logEnemyMissColor = new(0.63f, 0.66f, 0.72f);
        [SerializeField] private Color logCriticalColor = new(1f, 0.78f, 0.32f);
        [SerializeField] private Color logLootColor = new(0.62f, 0.82f, 1f);
        [SerializeField] private Color logGoldColor = new(1f, 0.85f, 0.38f);
        [SerializeField] private Color logPotionColor = new(0.62f, 1f, 0.88f);
        [SerializeField] private Color logWarningColor = new(1f, 0.62f, 0.32f);
        [SerializeField] private Color logVictoryColor = new(0.72f, 1f, 0.5f);
        [SerializeField] private Color logDefeatColor = new(1f, 0.38f, 0.38f);

        private readonly List<GameObject> generatedSelection = new();
        private readonly List<GameObject> generatedLog = new();
        private readonly List<string> renderedLogMessages = new();
        private int lastDisplayedSessionKills = -1;
        private int lastDisplayedSessionSeconds = -1;
        private int lastDisplayedSessionStatsTick = -1;
        private int clearLogBeforeIndex;
        private int pendingLogEventsWhilePaused;

        private void Awake()
        {
            AutoBind();
        }

        private void OnEnable()
        {
            if (combatSystem != null)
            {
                combatSystem.StateChanged += Refresh;
                combatSystem.Notification += ShowNotification;
                combatSystem.LogAdded += OnLogAdded;
            }

            if (progressionSystem != null)
            {
                progressionSystem.ProgressChanged += OnProgressChanged;
            }

            if (inventorySystem != null)
            {
                inventorySystem.InventoryChanged += Refresh;
            }

            HookButtons();
            Refresh();
        }

        private void OnDisable()
        {
            if (combatSystem != null)
            {
                combatSystem.StateChanged -= Refresh;
                combatSystem.Notification -= ShowNotification;
                combatSystem.LogAdded -= OnLogAdded;
            }

            if (progressionSystem != null)
            {
                progressionSystem.ProgressChanged -= OnProgressChanged;
            }

            if (inventorySystem != null)
            {
                inventorySystem.InventoryChanged -= Refresh;
            }
        }

        public void ConfigureForEditor(CombatSystem combat, ProfessionProgressionSystem progression)
        {
            combatSystem = combat;
            progressionSystem = progression;
            AutoBind();
        }

        public void ConfigureInventoryForEditor(InventorySystem inventory)
        {
            inventorySystem = inventory;
        }

        public void AutoBind()
        {
            if (selectionState == null) selectionState = HierarchySearch.FindDeep(transform, "[STATE] CombatSelectionState");
            if (activeState == null) activeState = HierarchySearch.FindDeep(transform, "[STATE] ActiveCombatState");
            if (regionContainer == null) regionContainer = HierarchySearch.FindDeep(transform, "[DYNAMIC CONTENT] RegionContainer");
            if (activityTypeContainer == null) activityTypeContainer = HierarchySearch.FindDeep(transform, "[DYNAMIC CONTENT] ActivityTypeContainer");
            if (locationContainer == null) locationContainer = HierarchySearch.FindDeep(transform, "[DYNAMIC CONTENT] LocationContainer");
            if (enemyContainer == null) enemyContainer = HierarchySearch.FindDeep(transform, "[DYNAMIC CONTENT] EnemyContainer");
            if (regionTemplate == null) regionTemplate = HierarchySearch.FindButton(transform, "[BUTTON] RegionTemplate");
            if (activityTypeTemplate == null) activityTypeTemplate = HierarchySearch.FindButton(transform, "[BUTTON] ActivityTypeTemplate");
            if (locationTemplate == null) locationTemplate = HierarchySearch.FindButton(transform, "[BUTTON] LocationTemplate");
            if (enemyTemplate == null) enemyTemplate = HierarchySearch.FindButton(transform, "[BUTTON] EnemyTemplate");
            if (breadcrumbText == null) breadcrumbText = HierarchySearch.FindText(transform, "[TEXT] CombatBreadcrumb");
            if (detailsNameText == null) detailsNameText = HierarchySearch.FindText(transform, "[TEXT] EnemyDetailsName");
            if (detailsDescriptionText == null) detailsDescriptionText = HierarchySearch.FindText(transform, "[TEXT] EnemyDetailsDescription");
            if (detailsRequirementsText == null) detailsRequirementsText = HierarchySearch.FindText(transform, "[TEXT] EnemyRequirements");
            if (detailsStatsText == null) detailsStatsText = HierarchySearch.FindText(transform, "[TEXT] EnemyStats");
            if (detailsLootText == null) detailsLootText = HierarchySearch.FindText(transform, "[TEXT] EnemyLootPreview");
            if (startCombatButton == null) startCombatButton = HierarchySearch.FindButton(transform, "[BUTTON] StartCombatButton");
            if (startCombatButtonText == null) startCombatButtonText = HierarchySearch.FindText(startCombatButton != null ? startCombatButton.transform : null, "[TEXT] Label");
            if (notificationText == null) notificationText = HierarchySearch.FindText(transform, "[TEXT] CombatNotification");
            if (encounterTitleText == null) encounterTitleText = HierarchySearch.FindText(transform, "[TEXT] EncounterTitleText");
            if (encounterAreaText == null) encounterAreaText = HierarchySearch.FindText(transform, "[TEXT] EncounterAreaText");
            if (encounterEnemyText == null) encounterEnemyText = HierarchySearch.FindText(transform, "[TEXT] EncounterEnemyText");
            if (encounterSessionText == null) encounterSessionText = HierarchySearch.FindText(transform, "[TEXT] EncounterSessionText");
            if (warriorLevelText == null) warriorLevelText = HierarchySearch.FindText(transform, "[TEXT] WarriorLevelText");
            if (warriorXpBar == null) warriorXpBar = HierarchySearch.FindOrAddFillBar(transform, "[BAR] WarriorXPBar");
            if (playerPortraitImage == null) playerPortraitImage = HierarchySearch.FindDeep(transform, "[IMAGE] PlayerPortrait")?.GetComponent<Image>();
            if (playerPortraitPlaceholderText == null) playerPortraitPlaceholderText = HierarchySearch.FindText(transform, "[TEXT] PlayerPortraitPlaceholder");
            if (enemyPortraitImage == null) enemyPortraitImage = HierarchySearch.FindDeep(transform, "[IMAGE] EnemyPortrait")?.GetComponent<Image>();
            if (enemyPortraitPlaceholderText == null) enemyPortraitPlaceholderText = HierarchySearch.FindText(transform, "[TEXT] EnemyPortraitPlaceholder");
            if (companionPortraitImage == null) companionPortraitImage = HierarchySearch.FindDeep(transform, "[IMAGE] CompanionPortrait")?.GetComponent<Image>();
            if (companionPortraitPlaceholderText == null) companionPortraitPlaceholderText = HierarchySearch.FindText(transform, "[TEXT] CompanionPortraitPlaceholder");
            if (currentActionIconImage == null) currentActionIconImage = HierarchySearch.FindDeep(transform, "[IMAGE] CurrentActionIcon")?.GetComponent<Image>();
            if (currentActionIconPlaceholderText == null) currentActionIconPlaceholderText = HierarchySearch.FindText(transform, "[TEXT] CurrentActionIconLabel");
            if (queuedActionIconImage == null) queuedActionIconImage = HierarchySearch.FindDeep(transform, "[IMAGE] QueuedActionIcon")?.GetComponent<Image>();
            if (queuedActionIconPlaceholderText == null) queuedActionIconPlaceholderText = HierarchySearch.FindText(transform, "[TEXT] QueuedActionIconLabel");
            if (heavyStrikeIconImage == null) heavyStrikeIconImage = HierarchySearch.FindDeep(transform, "[IMAGE] HeavyStrikeIcon")?.GetComponent<Image>();
            if (heavyStrikeIconPlaceholderText == null) heavyStrikeIconPlaceholderText = HierarchySearch.FindText(transform, "[TEXT] HeavyStrikeIconLabel");
            if (potionIconImage == null) potionIconImage = HierarchySearch.FindDeep(transform, "[IMAGE] PotionIcon")?.GetComponent<Image>();
            if (potionIconPlaceholderText == null) potionIconPlaceholderText = HierarchySearch.FindText(transform, "[TEXT] PotionIconLabel");
            if (enemyActionIconImage == null) enemyActionIconImage = HierarchySearch.FindDeep(transform, "[IMAGE] EnemyActionIcon")?.GetComponent<Image>();
            if (enemyActionIconPlaceholderText == null) enemyActionIconPlaceholderText = HierarchySearch.FindText(transform, "[TEXT] EnemyActionIconLabel");
            if (companionSkillIconImage == null) companionSkillIconImage = HierarchySearch.FindDeep(transform, "[IMAGE] CompanionSkillIcon")?.GetComponent<Image>();
            if (companionSkillIconPlaceholderText == null) companionSkillIconPlaceholderText = HierarchySearch.FindText(transform, "[TEXT] CompanionSkillIconLabel");
            if (playerNameText == null) playerNameText = HierarchySearch.FindText(transform, "[TEXT] PlayerName");
            if (playerStatsText == null) playerStatsText = HierarchySearch.FindText(transform, "[TEXT] PlayerStats");
            if (playerDamageValueText == null) playerDamageValueText = HierarchySearch.FindText(transform, "[TEXT] PlayerDamageValue");
            if (playerAttackSpeedValueText == null) playerAttackSpeedValueText = HierarchySearch.FindText(transform, "[TEXT] PlayerAttackSpeedValue");
            if (playerAccuracyValueText == null) playerAccuracyValueText = HierarchySearch.FindText(transform, "[TEXT] PlayerAccuracyValue");
            if (playerDefenceValueText == null) playerDefenceValueText = HierarchySearch.FindText(transform, "[TEXT] PlayerDefenceValue");
            if (playerCriticalValueText == null) playerCriticalValueText = HierarchySearch.FindText(transform, "[TEXT] PlayerCriticalValue");
            if (currentActionNameText == null) currentActionNameText = HierarchySearch.FindText(transform, "[TEXT] CurrentActionNameText");
            if (currentActionRemainingText == null) currentActionRemainingText = HierarchySearch.FindText(transform, "[TEXT] CurrentActionRemainingText");
            if (queuedActionPanel == null) queuedActionPanel = HierarchySearch.FindDeep(transform, "[PANEL] QueuedActionPanel");
            if (queuedActionText == null) queuedActionText = HierarchySearch.FindText(transform, "[TEXT] QueuedActionText");
            if (playerHealthBar == null) playerHealthBar = HierarchySearch.FindOrAddFillBar(transform, "[BAR] PlayerHealthBar");
            if (devotionBar == null) devotionBar = HierarchySearch.FindOrAddFillBar(transform, "[BAR] DevotionBar");
            if (playerAttackBar == null) playerAttackBar = HierarchySearch.FindOrAddFillBar(transform, "[BAR] PlayerAttackBar");
            if (enemyNameText == null) enemyNameText = HierarchySearch.FindText(transform, "[TEXT] ActiveEnemyName");
            if (enemyStatsText == null) enemyStatsText = HierarchySearch.FindText(transform, "[TEXT] ActiveEnemyStats");
            if (enemyDamageValueText == null) enemyDamageValueText = HierarchySearch.FindText(transform, "[TEXT] EnemyDamageValue");
            if (enemyAttackSpeedValueText == null) enemyAttackSpeedValueText = HierarchySearch.FindText(transform, "[TEXT] EnemyAttackSpeedValue");
            if (enemyAccuracyValueText == null) enemyAccuracyValueText = HierarchySearch.FindText(transform, "[TEXT] EnemyAccuracyValue");
            if (enemyDefenceValueText == null) enemyDefenceValueText = HierarchySearch.FindText(transform, "[TEXT] EnemyDefenceValue");
            if (enemyHealthBar == null) enemyHealthBar = HierarchySearch.FindOrAddFillBar(transform, "[BAR] EnemyHealthBar");
            if (enemyAttackBar == null) enemyAttackBar = HierarchySearch.FindOrAddFillBar(transform, "[BAR] EnemyAttackBar");
            if (enemyAbilitiesPanel == null) enemyAbilitiesPanel = HierarchySearch.FindDeep(transform, "[PANEL] EnemyAbilitiesPanel");
            if (enemyStatusEffectsPanel == null) enemyStatusEffectsPanel = HierarchySearch.FindDeep(transform, "[PANEL] EnemyStatusEffectsPanel");
            if (enemyCombatDetailsPanel == null) enemyCombatDetailsPanel = HierarchySearch.FindDeep(transform, "[PANEL] EnemyCombatDetailsPanel");
            if (enemyActionNameText == null) enemyActionNameText = HierarchySearch.FindText(transform, "[TEXT] EnemyActionNameText");
            if (enemyActionDetailsText == null) enemyActionDetailsText = HierarchySearch.FindText(transform, "[TEXT] EnemyActionDetailsText");
            if (enemyActionRemainingText == null) enemyActionRemainingText = HierarchySearch.FindText(transform, "[TEXT] EnemyActionRemainingText");
            if (heavyStrikeButton == null) heavyStrikeButton = HierarchySearch.FindButton(transform, "[BUTTON] HeavyStrikeButton");
            if (heavyStrikeText == null) heavyStrikeText = HierarchySearch.FindText(transform, "[TEXT] HeavyStrikeInfoText");
            if (heavyStrikeButtonText == null) heavyStrikeButtonText = HierarchySearch.FindText(heavyStrikeButton != null ? heavyStrikeButton.transform : null, "[TEXT] Label");
            if (heavyStrikeReasonText == null) heavyStrikeReasonText = HierarchySearch.FindText(transform, "[TEXT] HeavyStrikeReasonText");
            if (potionButton == null) potionButton = HierarchySearch.FindButton(transform, "[BUTTON] PotionQuickSlot");
            if (potionButton == null) potionButton = HierarchySearch.FindButton(transform, "[BUTTON] HealingPotionButton");
            if (potionText == null) potionText = HierarchySearch.FindText(transform, "[TEXT] PotionInfoText");
            if (potionButtonText == null) potionButtonText = HierarchySearch.FindText(potionButton != null ? potionButton.transform : null, "[TEXT] Label");
            if (potionQuantityText == null) potionQuantityText = HierarchySearch.FindText(transform, "[TEXT] PotionQuantityText");
            if (elixir1IconImage == null) elixir1IconImage = HierarchySearch.FindDeep(transform, "[IMAGE] Elixir1Icon")?.GetComponent<Image>();
            if (elixir2IconImage == null) elixir2IconImage = HierarchySearch.FindDeep(transform, "[IMAGE] Elixir2Icon")?.GetComponent<Image>();
            if (elixir3IconImage == null) elixir3IconImage = HierarchySearch.FindDeep(transform, "[IMAGE] Elixir3Icon")?.GetComponent<Image>();
            if (elixir4IconImage == null) elixir4IconImage = HierarchySearch.FindDeep(transform, "[IMAGE] Elixir4Icon")?.GetComponent<Image>();
            if (foodIconImage == null) foodIconImage = HierarchySearch.FindDeep(transform, "[IMAGE] FoodIcon")?.GetComponent<Image>();
            if (elixir1IconPlaceholderText == null) elixir1IconPlaceholderText = HierarchySearch.FindText(transform, "[TEXT] Elixir1IconLabel");
            if (elixir2IconPlaceholderText == null) elixir2IconPlaceholderText = HierarchySearch.FindText(transform, "[TEXT] Elixir2IconLabel");
            if (elixir3IconPlaceholderText == null) elixir3IconPlaceholderText = HierarchySearch.FindText(transform, "[TEXT] Elixir3IconLabel");
            if (elixir4IconPlaceholderText == null) elixir4IconPlaceholderText = HierarchySearch.FindText(transform, "[TEXT] Elixir4IconLabel");
            if (foodIconPlaceholderText == null) foodIconPlaceholderText = HierarchySearch.FindText(transform, "[TEXT] FoodIconLabel");
            if (elixir1Button == null) elixir1Button = HierarchySearch.FindButton(transform, "[BUTTON] Elixir1QuickSlot");
            if (elixir2Button == null) elixir2Button = HierarchySearch.FindButton(transform, "[BUTTON] Elixir2QuickSlot");
            if (elixir3Button == null) elixir3Button = HierarchySearch.FindButton(transform, "[BUTTON] Elixir3QuickSlot");
            if (elixir4Button == null) elixir4Button = HierarchySearch.FindButton(transform, "[BUTTON] Elixir4QuickSlot");
            if (foodButton == null) foodButton = HierarchySearch.FindButton(transform, "[BUTTON] FoodQuickSlot");
            if (elixir1QuantityText == null) elixir1QuantityText = HierarchySearch.FindText(transform, "[TEXT] Elixir1QuantityText");
            if (elixir2QuantityText == null) elixir2QuantityText = HierarchySearch.FindText(transform, "[TEXT] Elixir2QuantityText");
            if (elixir3QuantityText == null) elixir3QuantityText = HierarchySearch.FindText(transform, "[TEXT] Elixir3QuantityText");
            if (elixir4QuantityText == null) elixir4QuantityText = HierarchySearch.FindText(transform, "[TEXT] Elixir4QuantityText");
            if (foodQuantityText == null) foodQuantityText = HierarchySearch.FindText(transform, "[TEXT] FoodQuantityText");
            if (playerStatusEffectsPanel == null) playerStatusEffectsPanel = HierarchySearch.FindDeep(transform, "[PANEL] PlayerStatusEffectsPanel");
            if (playerStatusEffectsText == null) playerStatusEffectsText = HierarchySearch.FindText(transform, "[TEXT] PlayerStatusEffectsText");
            if (companionCombatPanel == null) companionCombatPanel = HierarchySearch.FindDeep(transform, "[PANEL] CompanionCombatPanel");
            if (companionSummaryText == null) companionSummaryText = HierarchySearch.FindText(transform, "[TEXT] CompanionSummaryText");
            if (companionActionBar == null) companionActionBar = HierarchySearch.FindOrAddFillBar(transform, "[BAR] CompanionActionBar");
            if (autoRepeatToggle == null) autoRepeatToggle = HierarchySearch.FindDeep(transform, "[TOGGLE] AutoRepeatToggle")?.GetComponent<Toggle>();
            if (heavyStrikeAutoToggle == null) heavyStrikeAutoToggle = HierarchySearch.FindDeep(transform, "[TOGGLE] HeavyStrikeAutoToggle")?.GetComponent<Toggle>();
            if (enemyAbilitiesText == null) enemyAbilitiesText = HierarchySearch.FindText(transform, "[TEXT] EnemyAbilitiesText");
            if (enemyStatusEffectsText == null) enemyStatusEffectsText = HierarchySearch.FindText(transform, "[TEXT] EnemyStatusEffectsText");
            if (enemyCombatDetailsText == null) enemyCombatDetailsText = HierarchySearch.FindText(transform, "[TEXT] EnemyCombatDetailsText");
            if (sessionStatsText == null) sessionStatsText = HierarchySearch.FindText(transform, "[TEXT] SessionStatsText");
            if (sessionDamageDealtText == null) sessionDamageDealtText = HierarchySearch.FindText(transform, "[TEXT] SessionDamageDealtText");
            if (sessionCompanionDamageText == null) sessionCompanionDamageText = HierarchySearch.FindText(transform, "[TEXT] SessionCompanionDamageText");
            if (sessionDamageTakenText == null) sessionDamageTakenText = HierarchySearch.FindText(transform, "[TEXT] SessionDamageTakenText");
            if (sessionHealingText == null) sessionHealingText = HierarchySearch.FindText(transform, "[TEXT] SessionHealingText");
            if (sessionDefeatedText == null) sessionDefeatedText = HierarchySearch.FindText(transform, "[TEXT] SessionDefeatedText");
            if (sessionDpsText == null) sessionDpsText = HierarchySearch.FindText(transform, "[TEXT] SessionDpsText");
            if (autoScrollStatusText == null) autoScrollStatusText = HierarchySearch.FindText(transform, "[TEXT] AutoScrollStatusText");
            if (pauseScrollButton == null) pauseScrollButton = HierarchySearch.FindButton(transform, "[BUTTON] PauseScrollButton");
            if (pauseScrollButtonText == null) pauseScrollButtonText = HierarchySearch.FindText(pauseScrollButton != null ? pauseScrollButton.transform : null, "[TEXT] Label");
            if (newLogEventsButton == null) newLogEventsButton = HierarchySearch.FindButton(transform, "[BUTTON] NewLogEventsButton");
            if (newLogEventsButtonText == null) newLogEventsButtonText = HierarchySearch.FindText(newLogEventsButton != null ? newLogEventsButton.transform : null, "[TEXT] Label");
            if (clearCombatLogButton == null) clearCombatLogButton = HierarchySearch.FindButton(transform, "[BUTTON] ClearCombatLogButton");
            if (combatLogContainer == null) combatLogContainer = HierarchySearch.FindDeep(transform, "[DYNAMIC CONTENT] CombatLogContainer");
            if (combatLogRowTemplate == null) combatLogRowTemplate = HierarchySearch.FindDeep(transform, "[ROW] CombatLogRowTemplate")?.gameObject;
            if (combatLogScrollRect == null) combatLogScrollRect = HierarchySearch.FindDeep(transform, "[SCROLL] CombatLogScroll")?.GetComponent<ScrollRect>();
            if (combatLogAutoScrollController == null) combatLogAutoScrollController = combatLogScrollRect != null
                ? combatLogScrollRect.GetComponent<CombatLogAutoScrollController>()
                : null;
        }

        private void HookButtons()
        {
            if (startCombatButton != null)
            {
                startCombatButton.onClick.RemoveListener(StartCombat);
                startCombatButton.onClick.AddListener(StartCombat);
            }

            if (heavyStrikeButton != null)
            {
                heavyStrikeButton.onClick.RemoveListener(QueueHeavyStrike);
                heavyStrikeButton.onClick.AddListener(QueueHeavyStrike);
            }

            if (potionButton != null)
            {
                potionButton.onClick.RemoveListener(UseMinorPotion);
                potionButton.onClick.AddListener(UseMinorPotion);
            }

            if (clearCombatLogButton != null)
            {
                clearCombatLogButton.onClick.RemoveListener(ClearVisibleCombatLog);
                clearCombatLogButton.onClick.AddListener(ClearVisibleCombatLog);
            }

            if (pauseScrollButton != null)
            {
                pauseScrollButton.onClick.RemoveListener(ToggleLogAutoScroll);
                pauseScrollButton.onClick.AddListener(ToggleLogAutoScroll);
            }

            if (newLogEventsButton != null)
            {
                newLogEventsButton.onClick.RemoveListener(ResumeLogAutoScroll);
                newLogEventsButton.onClick.AddListener(ResumeLogAutoScroll);
            }

            if (autoRepeatToggle != null)
            {
                autoRepeatToggle.onValueChanged.RemoveListener(SetAutoRepeat);
                autoRepeatToggle.onValueChanged.AddListener(SetAutoRepeat);
            }

            if (heavyStrikeAutoToggle != null)
            {
                heavyStrikeAutoToggle.onValueChanged.RemoveListener(SetHeavyStrikeAuto);
                heavyStrikeAutoToggle.onValueChanged.AddListener(SetHeavyStrikeAuto);
            }
        }

        private void Refresh()
        {
            if (combatSystem == null)
            {
                return;
            }

            if (selectionState != null)
            {
                selectionState.gameObject.SetActive(!combatSystem.IsActive);
            }

            if (activeState != null)
            {
                activeState.gameObject.SetActive(combatSystem.IsActive);
            }

            RefreshProgression();
            RefreshSelection();
            RefreshDetails();
            RefreshActiveCombat();
            RefreshLog();
        }

        private void RefreshProgression()
        {
            if (progressionSystem == null)
            {
                return;
            }

            var id = CombatConstants.WarriorProgressionId;
            var level = progressionSystem.GetLevel(id);
            var xpInto = progressionSystem.GetXpIntoLevel(id);
            var xpNeeded = progressionSystem.GetXpNeededForCurrentLevel(id);

            if (warriorLevelText != null)
            {
                warriorLevelText.text = $"Warrior Level {level}";
            }

            warriorXpBar?.SetValue(progressionSystem.GetLevelProgress01(id), xpNeeded > 0 ? $"{xpInto} / {xpNeeded} XP" : "Max Level");
        }

        private void RefreshSelection()
        {
            ClearGenerated(generatedSelection);

            var catalog = combatSystem.Catalog;
            AddSelectionButton(regionContainer, regionTemplate, catalog.RegionDisplayName, combatSystem.SelectedRegionId == catalog.RegionId, () => combatSystem.SelectRegion(catalog.RegionId));
            if (combatSystem.SelectedRegionId == catalog.RegionId)
            {
                AddSelectionButton(activityTypeContainer, activityTypeTemplate, catalog.ActivityTypeDisplayName, combatSystem.SelectedActivityTypeId == catalog.ActivityTypeId, () => combatSystem.SelectActivityType(catalog.ActivityTypeId));
            }

            if (combatSystem.SelectedActivityTypeId == catalog.ActivityTypeId)
            {
                AddSelectionButton(locationContainer, locationTemplate, catalog.LocationDisplayName, combatSystem.SelectedLocationId == catalog.LocationId, () => combatSystem.SelectLocation(catalog.LocationId));
            }

            if (combatSystem.SelectedLocationId == catalog.LocationId)
            {
                foreach (var enemy in catalog.Enemies.Where(enemy => enemy != null))
                {
                    var unlocked = combatSystem.IsEnemyUnlocked(enemy);
                    var label = unlocked ? enemy.DisplayName : $"{enemy.DisplayName} - Requires Warrior {enemy.RequiredWarriorLevel}";
                    AddSelectionButton(enemyContainer, enemyTemplate, label, combatSystem.SelectedEnemy == enemy, () => combatSystem.SelectEnemy(enemy.EnemyId), unlocked);
                }
            }

            if (breadcrumbText != null)
            {
                var parts = new List<string>();
                if (combatSystem.SelectedRegionId == catalog.RegionId) parts.Add(catalog.RegionDisplayName);
                if (combatSystem.SelectedActivityTypeId == catalog.ActivityTypeId) parts.Add(catalog.ActivityTypeDisplayName);
                if (combatSystem.SelectedLocationId == catalog.LocationId) parts.Add(catalog.LocationDisplayName);
                if (combatSystem.SelectedEnemy != null) parts.Add(combatSystem.SelectedEnemy.DisplayName);
                breadcrumbText.text = parts.Count > 0 ? string.Join(" > ", parts) : "Select a Region";
            }
        }

        private void RefreshDetails()
        {
            var enemy = combatSystem.SelectedEnemy;
            if (enemy == null)
            {
                if (detailsNameText != null) detailsNameText.text = "Select an enemy";
                if (detailsDescriptionText != null) detailsDescriptionText.text = "Choose Greenvale, Areas, Greenvale Forest, then an enemy.";
                if (detailsRequirementsText != null) detailsRequirementsText.text = string.Empty;
                if (detailsStatsText != null) detailsStatsText.text = string.Empty;
                if (detailsLootText != null) detailsLootText.text = string.Empty;
                if (startCombatButtonText != null) startCombatButtonText.text = "Start Combat";
                return;
            }

            if (detailsNameText != null) detailsNameText.text = enemy.DisplayName;
            if (detailsDescriptionText != null) detailsDescriptionText.text = enemy.Description;
            if (detailsRequirementsText != null)
            {
                detailsRequirementsText.text = combatSystem.IsEnemyUnlocked(enemy)
                    ? "Requirement met"
                    : $"Requires Warrior Level {enemy.RequiredWarriorLevel}";
            }

            if (detailsStatsText != null)
            {
                detailsStatsText.text = $"Health {enemy.MaximumHealth}\nDamage {enemy.MinDamage}-{enemy.MaxDamage}\nAttack {enemy.AttackInterval:0.0}s\nAccuracy {enemy.Accuracy}\nDefense {enemy.Defense}\nCrit {enemy.CriticalChance:P0}";
            }

            if (detailsLootText != null)
            {
                detailsLootText.text = BuildLootPreview(enemy);
            }

            if (startCombatButtonText != null)
            {
                startCombatButtonText.text = combatSystem.IsStartConfirmationPending ? "Confirm Start" : "Start Combat";
            }
        }

        private void RefreshActiveCombat()
        {
            if (!combatSystem.IsActive || combatSystem.SelectedEnemy == null)
            {
                lastDisplayedSessionKills = -1;
                lastDisplayedSessionSeconds = -1;
                lastDisplayedSessionStatsTick = -1;
                return;
            }

            var stats = combatSystem.PlayerStats;
            RefreshEncounterHeader();
            SetIcon(playerPortraitImage, playerPortraitSprite, playerPortraitPlaceholderText, "WARRIOR");
            if (playerNameText != null) playerNameText.text = "Player - Warrior";
            if (playerStatsText != null)
            {
                playerStatsText.text = string.Empty;
            }

            SetText(playerDamageValueText, $"{stats.MinDamage}-{stats.MaxDamage}");
            SetText(playerAttackSpeedValueText, $"{stats.AttackInterval:0.0}s");
            SetText(playerAccuracyValueText, stats.Accuracy.ToString());
            SetText(playerDefenceValueText, stats.Defense.ToString());
            SetText(playerCriticalValueText, $"{stats.CriticalChance:P0}");
            playerHealthBar?.SetValue((float)combatSystem.PlayerHealth / stats.MaxHealth, $"{combatSystem.PlayerHealth} / {stats.MaxHealth}");
            devotionBar?.SetValue(combatSystem.PlayerDevotion / stats.MaxDevotion, $"{Mathf.FloorToInt(combatSystem.PlayerDevotion)} / {stats.MaxDevotion}");
            RefreshCurrentAction();
            RefreshPlayerActionControls();
            RefreshPlayerStatusAndCompanion();

            var enemy = combatSystem.SelectedEnemy;
            SetIcon(enemyPortraitImage, enemyPortraitSprite, enemyPortraitPlaceholderText, enemy.DisplayName.ToUpperInvariant());
            if (enemyNameText != null) enemyNameText.text = enemy.DisplayName;
            if (enemyStatsText != null)
            {
                enemyStatsText.text = string.Empty;
            }

            SetText(enemyDamageValueText, $"{enemy.MinDamage}-{enemy.MaxDamage}");
            SetText(enemyAttackSpeedValueText, $"{enemy.AttackInterval:0.0}s");
            SetText(enemyAccuracyValueText, enemy.Accuracy.ToString());
            SetText(enemyDefenceValueText, enemy.Defense.ToString());
            if (enemyAbilitiesText != null)
            {
                enemyAbilitiesText.text = "Special Abilities: None";
            }

            if (enemyAbilitiesPanel != null)
            {
                enemyAbilitiesPanel.gameObject.SetActive(true);
            }

            if (enemyStatusEffectsText != null)
            {
                enemyStatusEffectsText.text = string.Empty;
            }

            if (enemyStatusEffectsPanel != null)
            {
                enemyStatusEffectsPanel.gameObject.SetActive(false);
            }

            if (enemyCombatDetailsText != null)
            {
                enemyCombatDetailsText.text = string.Empty;
            }

            if (enemyCombatDetailsPanel != null)
            {
                enemyCombatDetailsPanel.gameObject.SetActive(false);
            }

            enemyHealthBar?.SetValue((float)combatSystem.EnemyHealth / enemy.MaximumHealth, $"{combatSystem.EnemyHealth} / {enemy.MaximumHealth}");
            RefreshEnemyTelegraph(enemy);

            if (autoRepeatToggle != null) autoRepeatToggle.SetIsOnWithoutNotify(combatSystem.AutoRepeat);
            if (heavyStrikeAutoToggle != null) heavyStrikeAutoToggle.SetIsOnWithoutNotify(combatSystem.HeavyStrikeAutoUse);
            RefreshSessionStats();
        }

        private void RefreshCurrentAction()
        {
            var remaining = combatSystem.PlayerActionRemainingSeconds;
            var duration = combatSystem.PlayerActionDurationSeconds;
            var label = combatSystem.PlayerActionLabel;
            var displayLabel = string.IsNullOrWhiteSpace(label) || label == "Ready" ? "Waiting" : label;
            SetActionIcon(currentActionIconImage, currentActionIconPlaceholderText, displayLabel);
            if (currentActionNameText != null)
            {
                currentActionNameText.text = displayLabel;
            }

            if (currentActionRemainingText != null)
            {
                currentActionRemainingText.text = remaining > 0f ? $"{remaining:0.0}s remaining" : "Ready";
            }

            playerAttackBar?.SetValue(duration <= 0f ? 0f : combatSystem.PlayerAttackProgress01, displayLabel);

            var hasQueue = combatSystem.HasQueuedCombatAction;
            if (queuedActionPanel != null)
            {
                queuedActionPanel.gameObject.SetActive(hasQueue);
            }

            SetActionIcon(queuedActionIconImage, queuedActionIconPlaceholderText, combatSystem.QueuedActionLabel);
            if (queuedActionText != null)
            {
                queuedActionText.gameObject.SetActive(hasQueue);
                queuedActionText.text = hasQueue ? combatSystem.QueuedActionLabel : string.Empty;
            }
        }

        private void RefreshPlayerActionControls()
        {
            SetIcon(heavyStrikeIconImage, heavyStrikeIconSprite, heavyStrikeIconPlaceholderText, "HS");
            var heavyStatus = GetHeavyStrikeStatus();
            if (heavyStrikeText != null)
            {
                heavyStrikeText.text = $"Heavy Strike\n{heavyStatus}";
            }

            if (heavyStrikeButton != null)
            {
                heavyStrikeButton.interactable = combatSystem.CanQueueHeavyStrike;
            }

            if (heavyStrikeButtonText != null)
            {
                heavyStrikeButtonText.text = "Use";
            }

            if (heavyStrikeReasonText != null)
            {
                heavyStrikeReasonText.text = string.Empty;
                heavyStrikeReasonText.gameObject.SetActive(false);
            }

            var potionItem = GetItem(CombatConstants.MinorHealingPotionItemId);
            var potionSprite = potionItem != null && potionItem.Icon != null ? potionItem.Icon : potionIconSprite;
            SetIcon(potionIconImage, potionSprite, potionIconPlaceholderText, "POT");
            RefreshDisabledConsumableSlots();
            if (potionText != null)
            {
                potionText.text = potionItem != null ? potionItem.DisplayName : "Minor Potion";
            }

            if (potionQuantityText != null)
            {
                var quantity = inventorySystem != null ? inventorySystem.GetQuantity(CombatConstants.MinorHealingPotionItemId) : 0;
                potionQuantityText.text = quantity.ToString();
                potionQuantityText.gameObject.SetActive(true);
            }

            if (potionButton != null)
            {
                var quantity = inventorySystem != null ? inventorySystem.GetQuantity(CombatConstants.MinorHealingPotionItemId) : 0;
                potionButton.interactable = combatSystem.IsActive && quantity > 0 && combatSystem.PotionCooldownRemaining <= 0f;
            }

            if (potionButtonText != null)
            {
                potionButtonText.text = string.Empty;
            }
        }

        private void RefreshPlayerStatusAndCompanion()
        {
            if (playerStatusEffectsPanel != null)
            {
                playerStatusEffectsPanel.gameObject.SetActive(false);
            }

            if (playerStatusEffectsText != null)
            {
                playerStatusEffectsText.text = string.Empty;
            }

            if (companionCombatPanel != null)
            {
                companionCombatPanel.gameObject.SetActive(false);
            }

            SetIcon(companionPortraitImage, companionPortraitSprite, companionPortraitPlaceholderText, "COMPANION");
            SetIcon(companionSkillIconImage, companionSkillIconSprite, companionSkillIconPlaceholderText, "CP");
        }

        private void RefreshEnemyTelegraph(CombatEnemyDefinition enemy)
        {
            if (enemy == null)
            {
                return;
            }

            var remaining = combatSystem.IsRespawning
                ? combatSystem.RespawnRemainingSeconds
                : GetEnemyAttackRemaining(enemy);
            var actionName = combatSystem.IsRespawning ? "Respawning" : "Attack";
            var details = combatSystem.IsRespawning
                ? "Next enemy appears soon"
                : $"Damage: {enemy.MinDamage}-{enemy.MaxDamage}";

            SetIcon(enemyActionIconImage, enemyAttackIconSprite, enemyActionIconPlaceholderText, "ATK");
            if (enemyActionNameText != null)
            {
                enemyActionNameText.text = actionName;
            }

            if (enemyActionDetailsText != null)
            {
                enemyActionDetailsText.text = details;
            }

            if (enemyActionRemainingText != null)
            {
                enemyActionRemainingText.text = remaining > 0.05f ? $"{remaining:0.0}s remaining" : "Resolving";
            }

            enemyAttackBar?.SetValue(combatSystem.IsRespawning ? 1f : combatSystem.EnemyAttackProgress01, actionName);
        }

        private string GetHeavyStrikeStatus()
        {
            if (combatSystem.IsHeavyStrikeQueued)
            {
                return "Queued";
            }

            if (combatSystem.IsPerformingHeavyStrike)
            {
                return "Casting";
            }

            if (combatSystem.HeavyStrikeCooldownRemaining > 0f)
            {
                return $"Cooldown: {combatSystem.HeavyStrikeCooldownRemaining:0.0}s";
            }

            return combatSystem.CanQueueHeavyStrike ? "Ready" : "Combat inactive";
        }

        private void SetActionIcon(Image image, TMP_Text placeholder, string actionLabel)
        {
            if (string.Equals(actionLabel, "Heavy Strike", System.StringComparison.OrdinalIgnoreCase))
            {
                SetIcon(image, heavyStrikeIconSprite, placeholder, "HS");
                return;
            }

            if (string.Equals(actionLabel, "Minor Potion", System.StringComparison.OrdinalIgnoreCase))
            {
                SetIcon(image, potionIconSprite, placeholder, "POT");
                return;
            }

            SetIcon(image, autoAttackIconSprite, placeholder, "ATK");
        }

        private void RefreshDisabledConsumableSlots()
        {
            SetIcon(elixir1IconImage, elixir1IconSprite, elixir1IconPlaceholderText, "E1");
            SetIcon(elixir2IconImage, elixir2IconSprite, elixir2IconPlaceholderText, "E2");
            SetIcon(elixir3IconImage, elixir3IconSprite, elixir3IconPlaceholderText, "E3");
            SetIcon(elixir4IconImage, elixir4IconSprite, elixir4IconPlaceholderText, "E4");
            SetIcon(foodIconImage, foodIconSprite, foodIconPlaceholderText, "FOOD");
            SetButtonInteractable(elixir1Button, false);
            SetButtonInteractable(elixir2Button, false);
            SetButtonInteractable(elixir3Button, false);
            SetButtonInteractable(elixir4Button, false);
            SetButtonInteractable(foodButton, false);
            SetText(elixir1QuantityText, "0");
            SetText(elixir2QuantityText, "0");
            SetText(elixir3QuantityText, "0");
            SetText(elixir4QuantityText, "0");
            SetText(foodQuantityText, "0");
            SetActive(elixir1QuantityText, true);
            SetActive(elixir2QuantityText, true);
            SetActive(elixir3QuantityText, true);
            SetActive(elixir4QuantityText, true);
            SetActive(foodQuantityText, true);
        }

        private static void SetButtonInteractable(Button button, bool interactable)
        {
            if (button != null)
            {
                button.interactable = interactable;
            }
        }

        private static void SetActive(Component component, bool active)
        {
            if (component != null)
            {
                component.gameObject.SetActive(active);
            }
        }

        private static void SetText(TMP_Text text, string value)
        {
            if (text != null)
            {
                text.text = value;
            }
        }

        private static void SetIcon(Image image, Sprite sprite, TMP_Text placeholder, string placeholderText)
        {
            if (image != null)
            {
                image.sprite = sprite;
                image.preserveAspect = true;
                image.color = sprite != null ? Color.white : new Color(0.08f, 0.11f, 0.15f, 1f);
                image.raycastTarget = false;
            }

            if (placeholder != null)
            {
                placeholder.text = placeholderText;
                placeholder.gameObject.SetActive(sprite == null);
            }
        }

        private void RefreshEncounterHeader()
        {
            var enemy = combatSystem.SelectedEnemy;
            if (enemy == null)
            {
                return;
            }

            if (encounterTitleText != null)
            {
                var areaName = combatSystem.Catalog != null ? combatSystem.Catalog.LocationDisplayName : "Combat Area";
                encounterTitleText.text = enemy.DisplayName;
            }

            if (encounterAreaText != null)
            {
                encounterAreaText.text = combatSystem.Catalog != null ? combatSystem.Catalog.LocationDisplayName.ToUpperInvariant() : "COMBAT AREA";
            }

            if (encounterEnemyText != null)
            {
                encounterEnemyText.text = enemy.DisplayName;
            }

            var seconds = combatSystem.SessionElapsedSeconds;
            var kills = combatSystem.SessionKillCount;
            if (encounterSessionText != null && (seconds != lastDisplayedSessionSeconds || kills != lastDisplayedSessionKills))
            {
                lastDisplayedSessionSeconds = seconds;
                lastDisplayedSessionKills = kills;
                encounterSessionText.text = $"Kills: {kills}\nTime: {FormatDuration(seconds)}";
            }
        }

        private void RefreshSessionStats()
        {
            if (sessionStatsText == null &&
                sessionDamageDealtText == null &&
                sessionCompanionDamageText == null &&
                sessionDamageTakenText == null &&
                sessionHealingText == null &&
                sessionDefeatedText == null &&
                sessionDpsText == null)
            {
                return;
            }

            var tick = Mathf.FloorToInt(Time.unscaledTime * 4f);
            if (tick == lastDisplayedSessionStatsTick)
            {
                return;
            }

            lastDisplayedSessionStatsTick = tick;
            if (sessionDamageDealtText != null)
            {
                sessionDamageDealtText.text = $"Damage {combatSystem.SessionDamageDealt:N0}";
            }

            if (sessionCompanionDamageText != null)
            {
                sessionCompanionDamageText.text = "Companion —";
            }

            if (sessionDamageTakenText != null)
            {
                sessionDamageTakenText.text = $"Taken {combatSystem.SessionDamageTaken:N0}";
            }

            if (sessionHealingText != null)
            {
                sessionHealingText.text = $"Healing {combatSystem.SessionHealingReceived:N0}";
            }

            if (sessionDefeatedText != null)
            {
                sessionDefeatedText.text = $"Defeated {combatSystem.SessionKillCount:N0}";
            }

            if (sessionDpsText != null)
            {
                sessionDpsText.text = $"DPS {combatSystem.SessionDamagePerSecond:0.0}";
            }

            if (sessionStatsText != null)
            {
                sessionStatsText.text = $"SESSION Damage {combatSystem.SessionDamageDealt:N0} Companion — Taken {combatSystem.SessionDamageTaken:N0} Healing {combatSystem.SessionHealingReceived:N0} Defeated {combatSystem.SessionKillCount:N0} DPS {combatSystem.SessionDamagePerSecond:0.0}";
            }
        }

        private void RefreshLog()
        {
            if (combatLogContainer == null || combatLogRowTemplate == null)
            {
                return;
            }

            RemoveUntrackedLogRows();

            if (autoScrollStatusText != null)
            {
                autoScrollStatusText.text = combatLogAutoScrollController != null && combatLogAutoScrollController.IsAutoScrollActive
                    ? "Auto Scroll: On"
                    : "Auto Scroll: Paused";
            }

            if (pauseScrollButtonText != null)
            {
                pauseScrollButtonText.text = combatLogAutoScrollController != null && combatLogAutoScrollController.IsAutoScrollActive ? "Pause" : "Resume";
            }

            if (newLogEventsButton != null)
            {
                var showNewEvents = combatLogAutoScrollController != null &&
                                    !combatLogAutoScrollController.IsAutoScrollActive &&
                                    pendingLogEventsWhilePaused > 0;
                newLogEventsButton.gameObject.SetActive(showNewEvents);
                if (newLogEventsButtonText != null)
                {
                    newLogEventsButtonText.text = $"\u2193 {pendingLogEventsWhilePaused} new";
                }
            }
            else if (newLogEventsButtonText != null)
            {
                newLogEventsButtonText.gameObject.SetActive(false);
            }

            var log = combatSystem.CombatLog;
            var firstDesiredIndex = Mathf.Max(0, log.Count - MaxVisibleCombatLogRows);
            firstDesiredIndex = Mathf.Max(firstDesiredIndex, clearLogBeforeIndex);
            var desiredMessages = log.Skip(firstDesiredIndex).ToList();
            if (renderedLogMessages.SequenceEqual(desiredMessages))
            {
                return;
            }

            combatLogAutoScrollController?.BeginContentRefresh();

            var overlap = FindLogOverlap(renderedLogMessages, desiredMessages);
            var removeCount = renderedLogMessages.Count - overlap;
            var removedHeight = 0f;
            for (var i = 0; i < removeCount; i++)
            {
                removedHeight += RemoveFirstLogRow();
            }

            combatLogAutoScrollController?.NotifyRowsRemovedFromTop(removedHeight);

            for (var i = overlap; i < desiredMessages.Count; i++)
            {
                AddLogRow(desiredMessages[i]);
            }

            combatLogAutoScrollController?.NotifyContentChanged();
        }

        private void RemoveUntrackedLogRows()
        {
            for (var i = generatedLog.Count - 1; i >= 0; i--)
            {
                if (generatedLog[i] == null)
                {
                    generatedLog.RemoveAt(i);
                    if (i < renderedLogMessages.Count)
                    {
                        renderedLogMessages.RemoveAt(i);
                    }
                }
            }

            for (var i = combatLogContainer.childCount - 1; i >= 0; i--)
            {
                var child = combatLogContainer.GetChild(i).gameObject;
                if (child == combatLogRowTemplate || child.name != "[ROW] CombatLogRow" || generatedLog.Contains(child))
                {
                    continue;
                }

                if (Application.isPlaying)
                {
                    child.SetActive(false);
                    Destroy(child);
                }
                else
                {
                    DestroyImmediate(child);
                }
            }
        }

        private int FindLogOverlap(IReadOnlyList<string> currentMessages, IReadOnlyList<string> desiredMessages)
        {
            var maxOverlap = Mathf.Min(currentMessages.Count, desiredMessages.Count);
            for (var overlap = maxOverlap; overlap > 0; overlap--)
            {
                var matches = true;
                var currentOffset = currentMessages.Count - overlap;
                for (var i = 0; i < overlap; i++)
                {
                    if (currentMessages[currentOffset + i] != desiredMessages[i])
                    {
                        matches = false;
                        break;
                    }
                }

                if (matches)
                {
                    return overlap;
                }
            }

            return 0;
        }

        private void AddLogRow(string message)
        {
            var row = Instantiate(combatLogRowTemplate, combatLogContainer);
            row.name = "[ROW] CombatLogRow";
            row.SetActive(true);

            var text = HierarchySearch.FindText(row.transform, "[TEXT] LogMessage");
            if (text != null)
            {
                text.text = message;
                text.color = GetLogColor(message);
            }

            generatedLog.Add(row);
            renderedLogMessages.Add(message);
        }

        private float RemoveFirstLogRow()
        {
            if (generatedLog.Count == 0)
            {
                if (renderedLogMessages.Count > 0)
                {
                    renderedLogMessages.RemoveAt(0);
                }

                return 0f;
            }

            var row = generatedLog[0];
            generatedLog.RemoveAt(0);
            if (renderedLogMessages.Count > 0)
            {
                renderedLogMessages.RemoveAt(0);
            }

            var removedHeight = GetLogRowLayoutHeight(row);
            row.SetActive(false);
            if (Application.isPlaying)
            {
                Destroy(row);
            }
            else
            {
                DestroyImmediate(row);
            }

            return removedHeight;
        }

        private float GetLogRowLayoutHeight(GameObject row)
        {
            if (row == null)
            {
                return 0f;
            }

            var rect = row.transform as RectTransform;
            var height = rect != null ? rect.rect.height : 0f;
            var layoutElement = row.GetComponent<LayoutElement>();
            if (layoutElement != null)
            {
                if (layoutElement.preferredHeight > 0f)
                {
                    height = layoutElement.preferredHeight;
                }
                else if (layoutElement.minHeight > 0f)
                {
                    height = layoutElement.minHeight;
                }
            }

            var layoutGroup = combatLogContainer != null ? combatLogContainer.GetComponent<VerticalLayoutGroup>() : null;
            if (layoutGroup != null && generatedLog.Count > 0)
            {
                height += layoutGroup.spacing;
            }

            return height;
        }

        private void AddSelectionButton(Transform container, Button template, string label, bool selected, UnityEngine.Events.UnityAction action, bool interactable = true)
        {
            if (container == null || template == null)
            {
                return;
            }

            var button = Instantiate(template, container);
            button.name = template.name.Replace("Template", "Entry");
            button.gameObject.SetActive(true);
            button.interactable = interactable;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(action);
            var text = HierarchySearch.FindText(button.transform, "[TEXT] Label");
            if (text != null)
            {
                text.text = selected ? $"> {label}" : label;
            }

            generatedSelection.Add(button.gameObject);
        }

        private string BuildLootPreview(CombatEnemyDefinition enemy)
        {
            var lines = new List<string> { $"Gold {enemy.MinGold}-{enemy.MaxGold}" };
            lines.AddRange(enemy.Loot.Where(loot => loot != null).Select(loot => $"{DisplayName(loot.itemId)} - {loot.chance:P0} x{loot.minQuantity}-{loot.maxQuantity}"));

            return string.Join("\n", lines);
        }

        private Color GetLogColor(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return logDefaultColor;
            }

            var lower = message.ToLowerInvariant();
            if (lower.Contains("critical"))
            {
                return logCriticalColor;
            }

            if (lower.Contains("defeated"))
            {
                return lower.Contains("player") ? logDefeatColor : logVictoryColor;
            }

            if (lower.Contains("potion"))
            {
                return logPotionColor;
            }

            if (lower.Contains("gold"))
            {
                return logGoldColor;
            }

            if (lower.Contains("loot") || lower.Contains("claimed") || lower.Contains("first-clear"))
            {
                return logLootColor;
            }

            if (lower.Contains("protected") || lower.Contains("full") || lower.Contains("requires") || lower.Contains("cannot"))
            {
                return logWarningColor;
            }

            if (lower.Contains("player attacks") || lower.Contains("heavy strike"))
            {
                return lower.Contains("miss") ? logPlayerMissColor : logPlayerHitColor;
            }

            if (combatSystem != null && combatSystem.SelectedEnemy != null)
            {
                var enemyName = combatSystem.SelectedEnemy.DisplayName.ToLowerInvariant();
                if (lower.Contains(enemyName))
                {
                    return lower.Contains("miss") ? logEnemyMissColor : logEnemyHitColor;
                }
            }

            return logDefaultColor;
        }

        private ItemDefinition GetItem(string itemId)
        {
            return inventorySystem != null &&
                   inventorySystem.ItemDatabase != null &&
                   inventorySystem.ItemDatabase.TryGetItem(itemId, out var item)
                ? item
                : null;
        }

        private string DisplayName(string itemId)
        {
            var item = GetItem(itemId);
            return item != null ? item.DisplayName : itemId;
        }

        private void ShowNotification(string message)
        {
            if (notificationText != null)
            {
                notificationText.text = message;
            }
        }

        private void StartCombat()
        {
            combatSystem?.StartCombat();
            Refresh();
        }

        private void QueueHeavyStrike()
        {
            combatSystem?.QueueHeavyStrike();
        }

        private void UseMinorPotion()
        {
            combatSystem?.TryUseMinorHealingPotion();
            Refresh();
        }

        private void SetAutoRepeat(bool value)
        {
            combatSystem?.SetAutoRepeat(value);
        }

        private void SetHeavyStrikeAuto(bool value)
        {
            combatSystem?.SetHeavyStrikeAutoUse(value);
        }

        private void ClearVisibleCombatLog()
        {
            if (combatSystem == null)
            {
                return;
            }

            clearLogBeforeIndex = combatSystem.CombatLog.Count;
            pendingLogEventsWhilePaused = 0;
            ClearGenerated(generatedLog);
            renderedLogMessages.Clear();
            RefreshLog();
        }

        private void ToggleLogAutoScroll()
        {
            if (combatLogAutoScrollController == null)
            {
                return;
            }

            if (combatLogAutoScrollController.IsAutoScrollActive)
            {
                combatLogAutoScrollController.PauseFromToolbar();
            }
            else
            {
                ResumeLogAutoScroll();
            }

            RefreshLog();
        }

        private void ResumeLogAutoScroll()
        {
            pendingLogEventsWhilePaused = 0;
            combatLogAutoScrollController?.ResumeAndScrollToBottom();
            RefreshLog();
        }

        private void OnProgressChanged(string progressId)
        {
            if (progressId == CombatConstants.WarriorProgressionId)
            {
                Refresh();
            }
        }

        private void OnLogAdded(string _)
        {
            if (combatLogAutoScrollController != null && !combatLogAutoScrollController.IsAutoScrollActive)
            {
                pendingLogEventsWhilePaused++;
            }

            RefreshLog();
        }

        private static void ClearGenerated(List<GameObject> objects)
        {
            foreach (var item in objects.Where(item => item != null))
            {
                if (Application.isPlaying)
                {
                    Destroy(item);
                }
                else
                {
                    DestroyImmediate(item);
                }
            }

            objects.Clear();
        }

        private static string FormatDuration(int seconds)
        {
            seconds = Mathf.Max(0, seconds);
            var minutes = seconds / 60;
            var secs = seconds % 60;
            return $"{minutes:00}:{secs:00}";
        }

        private float GetEnemyAttackRemaining(CombatEnemyDefinition enemy)
        {
            if (enemy == null)
            {
                return 0f;
            }

            return Mathf.Max(0f, enemy.AttackInterval * (1f - combatSystem.EnemyAttackProgress01));
        }
    }
}
