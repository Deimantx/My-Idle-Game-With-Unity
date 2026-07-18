using System.Collections.Generic;
using System.Linq;
using IdleGame.Combat;
using IdleGame.Inventory;
using IdleGame.Items;
using IdleGame.Progression;
using IdleGame.UI.Shared;
using IdleGame.UI.Tooltips;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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
        [SerializeField] private Transform dropdownLayer;
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
        [SerializeField] private RuntimeFillBar heavyStrikeCooldownBar;
        [SerializeField] private Image heavyStrikeCardImage;
        [SerializeField] private Image playerPortraitFrameImage;
        [SerializeField] private Image enemyPortraitFrameImage;
        [SerializeField] private Image potionSlotImage;
        [SerializeField] private Transform skillsListContainer;
        [SerializeField] private Transform floatingCombatTextRoot;
        [SerializeField] private TMP_Text floatingCombatTextTemplate;
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
        private readonly List<GameObject> generatedDetails = new();
        private readonly List<GameObject> generatedDropdown = new();
        private readonly List<GameObject> generatedLog = new();
        private readonly List<string> renderedLogMessages = new();
        private readonly List<FloatingCombatText> floatingCombatTexts = new();
        private readonly Dictionary<Image, Color> baseImageColors = new();
        private readonly Dictionary<Image, Color> flashColors = new();
        private readonly Dictionary<Image, float> flashTimers = new();
        private readonly List<Image> flashScratch = new();
        private readonly List<Image> flashCompleted = new();
        private int lastDisplayedSessionKills = -1;
        private int lastDisplayedSessionSeconds = -1;
        private int lastDisplayedSessionStatsTick = -1;
        private int lastHeavyStrikeTextTick = -1;
        private string lastHeavyStrikeStatus = string.Empty;
        private int clearLogBeforeIndex;
        private int pendingLogEventsWhilePaused;
        private bool bindingsValidated;
        private SelectorKind openSelector = SelectorKind.None;

        private void Awake()
        {
            AutoBind();
            ValidateBindings();
        }

        private void OnEnable()
        {
            if (combatSystem != null)
            {
                combatSystem.StateChanged += Refresh;
                combatSystem.Notification += ShowNotification;
                combatSystem.LogAdded += OnLogAdded;
                combatSystem.Feedback += OnCombatFeedback;
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
                combatSystem.Feedback -= OnCombatFeedback;
            }

            if (progressionSystem != null)
            {
                progressionSystem.ProgressChanged -= OnProgressChanged;
            }

            if (inventorySystem != null)
            {
                inventorySystem.InventoryChanged -= Refresh;
            }

            TooltipManager.HideGlobal();
            CloseDropdown();
            UnhookButtons();
        }

        private void Update()
        {
            TickVisualFeedback(Time.unscaledDeltaTime);
            UpdateHeavyStrikeCooldownBar();
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
            ConfigureTooltips();
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
            if (dropdownLayer == null) dropdownLayer = HierarchySearch.FindDeep(transform.root, "[OVERLAY] DropdownLayer");
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
            if (playerPortraitFrameImage == null) playerPortraitFrameImage = HierarchySearch.FindDeep(transform, "[FRAME] PlayerPortraitFrame")?.GetComponent<Image>();
            if (enemyPortraitFrameImage == null) enemyPortraitFrameImage = HierarchySearch.FindDeep(transform, "[FRAME] EnemyPortraitFrame")?.GetComponent<Image>();
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
            if (heavyStrikeCooldownBar == null) heavyStrikeCooldownBar = HierarchySearch.FindOrAddFillBar(transform, "[BAR] HeavyStrikeCooldownBar");
            if (heavyStrikeCardImage == null) heavyStrikeCardImage = HierarchySearch.FindDeep(transform, "[PANEL] HeavyStrikeCard")?.GetComponent<Image>();
            if (skillsListContainer == null) skillsListContainer = HierarchySearch.FindDeep(transform, "[LIST] PlayerSkillsList");
            if (potionButton == null) potionButton = HierarchySearch.FindButton(transform, "[BUTTON] PotionQuickSlot");
            if (potionButton == null) potionButton = HierarchySearch.FindButton(transform, "[BUTTON] HealingPotionButton");
            if (potionSlotImage == null) potionSlotImage = potionButton != null ? potionButton.GetComponent<Image>() : null;
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
            if (floatingCombatTextRoot == null) floatingCombatTextRoot = HierarchySearch.FindDeep(transform, "[POOL] FloatingCombatTextRoot");
            if (floatingCombatTextTemplate == null) floatingCombatTextTemplate = HierarchySearch.FindText(transform, "[TEXT] FloatingCombatTextTemplate");

            ConfigureInteractiveControls();
            ConfigureTooltips();
            InitializeFeedbackPools();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                ValidateBindings(false);
            }
        }
#endif

        private void InitializeFeedbackPools()
        {
            floatingCombatTexts.RemoveAll(entry => entry.Text == null);
            if (floatingCombatTextRoot == null || floatingCombatTextTemplate == null)
            {
                return;
            }

            floatingCombatTextTemplate.gameObject.SetActive(false);
            floatingCombatTextTemplate.raycastTarget = false;
            foreach (Transform child in floatingCombatTextRoot)
            {
                var text = child.GetComponent<TMP_Text>();
                if (text == null || floatingCombatTexts.Any(entry => entry.Text == text))
                {
                    continue;
                }

                text.raycastTarget = false;
                floatingCombatTexts.Add(new FloatingCombatText(text));
            }
        }

        private void ConfigureInteractiveControls()
        {
            ConfigureButton(heavyStrikeButton);
            ConfigureButton(potionButton);
            ConfigureButton(startCombatButton);
            ConfigureButton(clearCombatLogButton);
            ConfigureButton(pauseScrollButton);
            ConfigureButton(newLogEventsButton);
            ConfigureToggle(heavyStrikeAutoToggle);
            ConfigureToggle(autoRepeatToggle);
            var controlRaycaster = GetComponent<CombatControlRaycaster>();
            if (controlRaycaster != null)
            {
                controlRaycaster.ConfigureForEditor(heavyStrikeButton, heavyStrikeAutoToggle, autoRepeatToggle);
            }

            SetGraphicRaycastTarget(FindNamed("[SCREENS] ScreenContainer"), false);
            SetGraphicRaycastTarget(floatingCombatTextRoot, false, true);
            DisableBarRaycasts(warriorXpBar);
            DisableBarRaycasts(playerHealthBar);
            DisableBarRaycasts(devotionBar);
            DisableBarRaycasts(playerAttackBar);
            DisableBarRaycasts(enemyHealthBar);
            DisableBarRaycasts(enemyAttackBar);
            DisableBarRaycasts(heavyStrikeCooldownBar);
            DisableBarRaycasts(companionActionBar);
        }

        private void ConfigureTooltips()
        {
            ConfigureHeavyStrikeTooltip(heavyStrikeButton != null ? heavyStrikeButton.gameObject : null);
            var heavyStrikeIconFrame = HierarchySearch.FindDeep(transform, "[FRAME] HeavyStrikeIconFrame");
            ConfigureHeavyStrikeTooltip(heavyStrikeIconFrame != null ? heavyStrikeIconFrame.gameObject : null);

            var potionItem = GetItem(CombatConstants.MinorHealingPotionItemId);
            if (potionItem != null)
            {
                ConfigureItemTooltip(potionButton != null ? potionButton.gameObject : null, CombatConstants.MinorHealingPotionItemId, potionItem, true);
            }
            else
            {
                ConfigureStaticTooltip(potionButton != null ? potionButton.gameObject : null, "Empty Potion Slot", "Potion", "No potion is assigned.");
            }

            ConfigureStaticTooltip(elixir1Button != null ? elixir1Button.gameObject : null, "Empty Elixir Slot", "Elixir", "No elixir is assigned.");
            ConfigureStaticTooltip(elixir2Button != null ? elixir2Button.gameObject : null, "Empty Elixir Slot", "Elixir", "No elixir is assigned.");
            ConfigureStaticTooltip(elixir3Button != null ? elixir3Button.gameObject : null, "Empty Elixir Slot", "Elixir", "No elixir is assigned.");
            ConfigureStaticTooltip(elixir4Button != null ? elixir4Button.gameObject : null, "Empty Elixir Slot", "Elixir", "No elixir is assigned.");
            ConfigureStaticTooltip(foodButton != null ? foodButton.gameObject : null, "Empty Food Slot", "Food", "No food is assigned.");
        }

        private void ConfigureHeavyStrikeTooltip(GameObject target)
        {
            if (target == null || combatSystem == null)
            {
                return;
            }

            EnableTooltipRaycast(target);
            var provider = target.GetComponent<CombatSkillTooltipProvider>() ?? target.AddComponent<CombatSkillTooltipProvider>();
            provider.ConfigureForEditor(combatSystem, CombatConstants.HeavyStrikeAbilityId);
            var trigger = target.GetComponent<TooltipTrigger>() ?? target.AddComponent<TooltipTrigger>();
            trigger.ConfigureForEditor(provider);
        }

        private void ConfigureItemTooltip(GameObject target, string itemId, ItemDefinition item, bool includeQuantity)
        {
            if (target == null)
            {
                return;
            }

            EnableTooltipRaycast(target);
            var provider = target.GetComponent<ItemTooltipProvider>() ?? target.AddComponent<ItemTooltipProvider>();
            provider.ConfigureForEditor(itemId, item, inventorySystem, null, combatSystem, includeQuantity, false);
            var trigger = target.GetComponent<TooltipTrigger>() ?? target.AddComponent<TooltipTrigger>();
            trigger.ConfigureForEditor(provider);
        }

        private void ConfigureStaticTooltip(GameObject target, string title, string category, string description)
        {
            if (target == null)
            {
                return;
            }

            EnableTooltipRaycast(target);
            var provider = target.GetComponent<StaticTooltipProvider>() ?? target.AddComponent<StaticTooltipProvider>();
            provider.ConfigureForEditor(title, category, description);
            var trigger = target.GetComponent<TooltipTrigger>() ?? target.AddComponent<TooltipTrigger>();
            trigger.ConfigureForEditor(provider);
        }

        private static void EnableTooltipRaycast(GameObject target)
        {
            var image = target != null ? target.GetComponent<Image>() : null;
            if (image != null)
            {
                image.raycastTarget = true;
            }
        }

        private static void ConfigureButton(Button button)
        {
            if (button == null)
            {
                return;
            }

            var image = button.GetComponent<Image>();
            if (image != null)
            {
                image.raycastTarget = true;
                if (button.targetGraphic == null)
                {
                    button.targetGraphic = image;
                }
            }
        }

        private static void ConfigureToggle(Toggle toggle)
        {
            if (toggle == null)
            {
                return;
            }

            var rootImage = toggle.GetComponent<Image>();
            if (rootImage != null)
            {
                rootImage.raycastTarget = true;
            }

            if (toggle.targetGraphic == null)
            {
                var checkbox = HierarchySearch.FindImage(toggle.transform, "[IMAGE] Checkbox");
                if (checkbox != null)
                {
                    toggle.targetGraphic = checkbox;
                }
            }

            if (toggle.graphic == null)
            {
                var checkmark = HierarchySearch.FindImage(toggle.transform, "[IMAGE] Checkmark");
                if (checkmark != null)
                {
                    toggle.graphic = checkmark;
                }
            }

            if (toggle.targetGraphic != null)
            {
                toggle.targetGraphic.raycastTarget = true;
            }

            if (toggle.graphic != null)
            {
                toggle.graphic.raycastTarget = false;
            }
        }

        private static void DisableBarRaycasts(RuntimeFillBar bar)
        {
            if (bar == null)
            {
                return;
            }

            SetGraphicRaycastTarget(bar.transform, false, true);
        }

        private static void SetGraphicRaycastTarget(Transform root, bool raycastTarget, bool includeChildren = false)
        {
            if (root == null)
            {
                return;
            }

            var graphics = includeChildren
                ? root.GetComponentsInChildren<Graphic>(true)
                : root.GetComponents<Graphic>();
            foreach (var graphic in graphics)
            {
                if (graphic == null)
                {
                    continue;
                }

                graphic.raycastTarget = raycastTarget;
            }
        }

        private Transform FindNamed(string objectName)
        {
            if (string.IsNullOrWhiteSpace(objectName))
            {
                return null;
            }

            var current = transform;
            while (current != null)
            {
                if (current.name == objectName)
                {
                    return current;
                }

                current = current.parent;
            }

            return HierarchySearch.FindDeep(transform.root, objectName);
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        [System.Diagnostics.Conditional("DEVELOPMENT_BUILD")]
        private void ValidateBindings(bool runtimeOnlyOnce = true)
        {
            if (runtimeOnlyOnce && bindingsValidated)
            {
                return;
            }

            bindingsValidated = true;
            ValidateReference(combatSystem, nameof(combatSystem));
            ValidateReference(heavyStrikeButton, nameof(heavyStrikeButton));
            ValidateReference(heavyStrikeAutoToggle, nameof(heavyStrikeAutoToggle));
            ValidateReference(autoRepeatToggle, nameof(autoRepeatToggle));
            ValidateButtonBinding(heavyStrikeButton, nameof(heavyStrikeButton));
            ValidateToggleBinding(heavyStrikeAutoToggle, nameof(heavyStrikeAutoToggle));
            ValidateToggleBinding(autoRepeatToggle, nameof(autoRepeatToggle));

            var eventSystems = FindObjectsByType<EventSystem>(FindObjectsInactive.Exclude);
            if (eventSystems.Length != 1)
            {
                Debug.LogError($"ActiveCombatUI: Expected exactly one active EventSystem, found {eventSystems.Length}.", this);
            }

            var canvas = GetComponentInParent<Canvas>();
            if (canvas == null || canvas.GetComponent<GraphicRaycaster>() == null)
            {
                Debug.LogError("ActiveCombatUI: Missing GraphicRaycaster on the parent Canvas.", this);
            }

            WarnIfRaycastBlocker("[SCREENS] ScreenContainer");
            WarnIfRaycastBlocker("[POOL] FloatingCombatTextRoot");
        }

        private void ValidateReference(Object reference, string fieldName)
        {
            if (reference == null)
            {
                Debug.LogError($"ActiveCombatUI: Missing {fieldName} reference on {GetHierarchyPath(transform)}.", this);
            }
        }

        private void ValidateButtonBinding(Button button, string fieldName)
        {
            if (button == null)
            {
                return;
            }

            if (button.targetGraphic == null)
            {
                Debug.LogError($"ActiveCombatUI: {fieldName} has no targetGraphic at {GetHierarchyPath(button.transform)}.", button);
            }
        }

        private void ValidateToggleBinding(Toggle toggle, string fieldName)
        {
            if (toggle == null)
            {
                return;
            }

            if (toggle.targetGraphic == null)
            {
                Debug.LogError($"ActiveCombatUI: {fieldName} has no checkbox targetGraphic at {GetHierarchyPath(toggle.transform)}.", toggle);
            }

            if (toggle.graphic == null)
            {
                Debug.LogError($"ActiveCombatUI: {fieldName} has no checkmark graphic at {GetHierarchyPath(toggle.transform)}.", toggle);
            }
        }

        private void WarnIfRaycastBlocker(string objectName)
        {
            var target = FindNamed(objectName);
            if (target == null)
            {
                return;
            }

            foreach (var graphic in target.GetComponents<Graphic>())
            {
                if (graphic != null && graphic.raycastTarget)
                {
                    Debug.LogWarning($"ActiveCombatUI: Decorative object '{GetHierarchyPath(target)}' has Raycast Target enabled.", graphic);
                }
            }
        }

        private static string GetHierarchyPath(Transform target)
        {
            if (target == null)
            {
                return "<missing>";
            }

            var names = new Stack<string>();
            var current = target;
            while (current != null)
            {
                names.Push(current.name);
                current = current.parent;
            }

            return string.Join("/", names);
        }

        private void TickVisualFeedback(float delta)
        {
            if (delta <= 0f)
            {
                return;
            }

            UpdateImageFlashes(delta);
            for (var i = 0; i < floatingCombatTexts.Count; i++)
            {
                floatingCombatTexts[i].Tick(delta);
            }
        }

        private void UpdateImageFlashes(float delta)
        {
            if (flashTimers.Count == 0)
            {
                return;
            }

            flashScratch.Clear();
            flashScratch.AddRange(flashTimers.Keys);
            flashCompleted.Clear();
            foreach (var image in flashScratch)
            {
                if (image == null)
                {
                    flashCompleted.Add(image);
                    continue;
                }

                var remaining = Mathf.Max(0f, flashTimers[image] - delta);
                if (!baseImageColors.TryGetValue(image, out var baseColor))
                {
                    baseColor = image.color;
                }

                var flashColor = flashColors.TryGetValue(image, out var storedFlash) ? storedFlash : Color.white;
                image.color = Color.Lerp(baseColor, flashColor, remaining / 0.22f);
                if (remaining <= 0f)
                {
                    image.color = baseColor;
                    flashCompleted.Add(image);
                }
                else
                {
                    flashTimers[image] = remaining;
                }
            }

            foreach (var image in flashCompleted)
            {
                flashTimers.Remove(image);
                flashColors.Remove(image);
            }
        }

        private void Flash(Image image, Color flashColor)
        {
            if (image == null)
            {
                return;
            }

            if (!baseImageColors.ContainsKey(image))
            {
                baseImageColors[image] = image.color;
            }

            image.color = flashColor;
            flashColors[image] = flashColor;
            flashTimers[image] = 0.22f;
        }

        private void SpawnFloatingText(string value, Transform target, Color color, bool critical = false)
        {
            if (floatingCombatTextRoot == null || string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            var entry = GetFloatingText();
            if (entry == null)
            {
                return;
            }

            var rootRect = floatingCombatTextRoot as RectTransform;
            var targetRect = target as RectTransform;
            var localPosition = Vector2.zero;
            if (rootRect != null && targetRect != null)
            {
                var world = targetRect.TransformPoint(targetRect.rect.center);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(rootRect, RectTransformUtility.WorldToScreenPoint(null, world), null, out localPosition);
            }

            entry.Play(value, localPosition, color, critical);
        }

        private FloatingCombatText GetFloatingText()
        {
            InitializeFeedbackPools();
            var entry = floatingCombatTexts.FirstOrDefault(item => !item.IsActive);
            if (entry != null)
            {
                return entry;
            }

            if (floatingCombatTextRoot == null || floatingCombatTextTemplate == null)
            {
                return null;
            }

            var text = Instantiate(floatingCombatTextTemplate, floatingCombatTextRoot);
            text.name = "[TEXT] FloatingCombatText";
            text.raycastTarget = false;
            entry = new FloatingCombatText(text);
            floatingCombatTexts.Add(entry);
            return entry;
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

        private void UnhookButtons()
        {
            if (startCombatButton != null)
            {
                startCombatButton.onClick.RemoveListener(StartCombat);
            }

            if (heavyStrikeButton != null)
            {
                heavyStrikeButton.onClick.RemoveListener(QueueHeavyStrike);
            }

            if (potionButton != null)
            {
                potionButton.onClick.RemoveListener(UseMinorPotion);
            }

            if (clearCombatLogButton != null)
            {
                clearCombatLogButton.onClick.RemoveListener(ClearVisibleCombatLog);
            }

            if (pauseScrollButton != null)
            {
                pauseScrollButton.onClick.RemoveListener(ToggleLogAutoScroll);
            }

            if (newLogEventsButton != null)
            {
                newLogEventsButton.onClick.RemoveListener(ResumeLogAutoScroll);
            }

            if (autoRepeatToggle != null)
            {
                autoRepeatToggle.onValueChanged.RemoveListener(SetAutoRepeat);
            }

            if (heavyStrikeAutoToggle != null)
            {
                heavyStrikeAutoToggle.onValueChanged.RemoveListener(SetHeavyStrikeAuto);
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
            AddSelectorRow(
                regionContainer,
                regionTemplate,
                SelectorKind.Region,
                "REG",
                "Region",
                combatSystem.SelectedRegionId == catalog.RegionId ? catalog.RegionDisplayName : "Select a Region",
                combatSystem.SelectedRegionId == catalog.RegionId,
                true,
                "Available combat regions.");

            AddSelectorRow(
                activityTypeContainer,
                activityTypeTemplate,
                SelectorKind.ActivityType,
                "TYPE",
                "Type",
                combatSystem.SelectedActivityTypeId == catalog.ActivityTypeId ? catalog.ActivityTypeDisplayName : "Select a Type",
                combatSystem.SelectedActivityTypeId == catalog.ActivityTypeId,
                combatSystem.SelectedRegionId == catalog.RegionId,
                "Available encounter categories for this region.");

            AddSelectorRow(
                locationContainer,
                locationTemplate,
                SelectorKind.Location,
                "LOC",
                "Location",
                combatSystem.SelectedLocationId == catalog.LocationId ? catalog.LocationDisplayName : "Select a Location",
                combatSystem.SelectedLocationId == catalog.LocationId,
                combatSystem.SelectedActivityTypeId == catalog.ActivityTypeId,
                "Available combat locations for this type.");

            if (combatSystem.SelectedLocationId == catalog.LocationId && catalog.Enemies.Any(enemy => enemy != null))
            {
                foreach (var enemy in catalog.Enemies.Where(enemy => enemy != null))
                {
                    AddEnemyEntry(enemyContainer, enemyTemplate, enemy, combatSystem.SelectedEnemy == enemy);
                }
            }
            else
            {
                AddEmptySelectionState(enemyContainer, "No enemies available in this location.");
            }

            if (breadcrumbText != null)
            {
                var parts = new List<string>();
                if (combatSystem.SelectedRegionId == catalog.RegionId) parts.Add(catalog.RegionDisplayName);
                if (combatSystem.SelectedActivityTypeId == catalog.ActivityTypeId) parts.Add(catalog.ActivityTypeDisplayName);
                if (combatSystem.SelectedLocationId == catalog.LocationId) parts.Add(catalog.LocationDisplayName);
                if (combatSystem.SelectedEnemy != null) parts.Add(combatSystem.SelectedEnemy.DisplayName);
                breadcrumbText.text = parts.Count > 0 ? string.Join(" > ", parts) : "Select a Region";
                breadcrumbText.textWrappingMode = TextWrappingModes.NoWrap;
                breadcrumbText.overflowMode = TextOverflowModes.Ellipsis;
            }

            var enemyHeader = HierarchySearch.FindText(transform, "[HEADER] ENEMY");
            if (enemyHeader != null)
            {
                enemyHeader.text = combatSystem.SelectedLocationId == catalog.LocationId
                    ? $"Enemies in {catalog.LocationDisplayName}"
                    : "Enemies";
                enemyHeader.color = new Color(0.86f, 0.69f, 0.35f, 1f);
                enemyHeader.fontSize = 15f;
                enemyHeader.textWrappingMode = TextWrappingModes.NoWrap;
                enemyHeader.overflowMode = TextOverflowModes.Ellipsis;
            }
        }

        private void RefreshDetails()
        {
            ClearGenerated(generatedDetails);

            var enemy = combatSystem.SelectedEnemy;
            if (enemy == null)
            {
                if (detailsNameText != null) detailsNameText.text = "Select an enemy";
                if (detailsDescriptionText != null) detailsDescriptionText.text = "No enemies available in this location.";
                if (detailsRequirementsText != null) detailsRequirementsText.text = "Select an enemy to view requirements.";
                SetActive(detailsStatsText, false);
                SetActive(detailsLootText, false);
                if (startCombatButton != null) startCombatButton.interactable = false;
                if (startCombatButtonText != null) startCombatButtonText.text = "Start Combat";
                if (notificationText != null) notificationText.text = "Select an enemy to begin combat.";
                return;
            }

            if (detailsNameText != null) detailsNameText.text = enemy.DisplayName;
            if (detailsDescriptionText != null)
            {
                detailsDescriptionText.text = enemy.Description;
                detailsDescriptionText.gameObject.SetActive(!string.IsNullOrWhiteSpace(enemy.Description));
            }

            SetActive(detailsStatsText, false);
            SetActive(detailsLootText, false);

            var unlocked = combatSystem.IsEnemyUnlocked(enemy);
            if (detailsRequirementsText != null)
            {
                detailsRequirementsText.text = unlocked
                    ? "AVAILABLE\nYou meet all requirements."
                    : $"LOCKED\n{BuildRequirementText(enemy)}";
            }

            ConfigureEnemyPortraitTooltip();
            AddStatsSection(enemy);
            AddAbilitiesSection();
            AddRewardsSection(enemy);

            if (startCombatButton != null) startCombatButton.interactable = unlocked;
            if (startCombatButtonText != null)
            {
                startCombatButtonText.text = combatSystem.IsStartConfirmationPending ? "Confirm Start" : "Start Combat";
            }

            if (notificationText != null)
            {
                notificationText.text = unlocked ? string.Empty : BuildRequirementText(enemy);
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
                var tick = Mathf.FloorToInt(Time.unscaledTime * 10f);
                if (tick != lastHeavyStrikeTextTick || heavyStatus != lastHeavyStrikeStatus)
                {
                    lastHeavyStrikeTextTick = tick;
                    lastHeavyStrikeStatus = heavyStatus;
                    heavyStrikeText.text = $"Heavy Strike\n{heavyStatus}";
                }
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

            UpdateHeavyStrikeCooldownBar();

            var potionItem = GetItem(CombatConstants.MinorHealingPotionItemId);
            var potionSprite = potionItem != null && potionItem.Icon != null ? potionItem.Icon : potionIconSprite;
            SetIcon(potionIconImage, potionSprite, potionIconPlaceholderText, "POT");
            RefreshDisabledConsumableSlots();
            if (potionText != null)
            {
                potionText.text = "Potion";
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

        private void UpdateHeavyStrikeCooldownBar()
        {
            if (heavyStrikeCooldownBar == null || combatSystem == null)
            {
                return;
            }

            var duration = Mathf.Max(0.01f, combatSystem.HeavyStrikeCooldownDurationSeconds);
            var progress = combatSystem.HeavyStrikeCooldownRemaining > 0f
                ? 1f - Mathf.Clamp01(combatSystem.HeavyStrikeCooldownRemaining / duration)
                : combatSystem.CanQueueHeavyStrike || combatSystem.IsHeavyStrikeQueued || combatSystem.IsPerformingHeavyStrike
                    ? 1f
                    : 0f;
            heavyStrikeCooldownBar.SetValue(progress, string.Empty);
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
                sessionDamageDealtText.text = combatSystem.SessionDamageDealt.ToString("N0");
            }

            if (sessionCompanionDamageText != null)
            {
                sessionCompanionDamageText.text = "—";
            }

            if (sessionDamageTakenText != null)
            {
                sessionDamageTakenText.text = combatSystem.SessionDamageTaken.ToString("N0");
            }

            if (sessionHealingText != null)
            {
                sessionHealingText.text = combatSystem.SessionHealingReceived.ToString("N0");
            }

            if (sessionDefeatedText != null)
            {
                sessionDefeatedText.text = combatSystem.SessionKillCount.ToString("N0");
            }

            if (sessionDpsText != null)
            {
                sessionDpsText.text = combatSystem.SessionDamagePerSecond.ToString("0.0");
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

        private void AddSelectorRow(
            Transform container,
            Button template,
            SelectorKind kind,
            string iconLabel,
            string label,
            string value,
            bool selected,
            bool interactable,
            string tooltip)
        {
            if (container == null || template == null)
            {
                return;
            }

            var button = Instantiate(template, container);
            button.name = $"[SELECTOR] {label}Selector";
            button.gameObject.SetActive(true);
            button.interactable = interactable;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => ToggleDropdown(kind, button.transform as RectTransform));

            ApplySelectorVisual(button, kind, selected);

            ConfigureHorizontal(button.gameObject, 8, 10, 10, 6, 6, TextAnchor.MiddleLeft);
            ConfigureLayout(button.gameObject, 46f, 46f, 0f);

            var iconText = EnsureText(button.transform, "[TEXT] SelectorIcon", iconLabel, 12f, new Color(0.86f, 0.69f, 0.35f, 1f), TextAlignmentOptions.Center);
            ConfigureLayout(iconText.gameObject, 34f, 34f, 0f, 34f);

            var textGroup = EnsureChild(button.transform, "[GROUP] SelectorText");
            ConfigureVertical(textGroup, 0, 0, 0, 0, 0, TextAnchor.MiddleLeft);
            ConfigureLayout(textGroup, 0f, 0f, 1f);

            var labelText = EnsureText(textGroup.transform, "[TEXT] SelectorLabel", $"{label}:", 11f, new Color(0.58f, 0.65f, 0.72f, 1f), TextAlignmentOptions.Left);
            labelText.textWrappingMode = TextWrappingModes.NoWrap;
            var valueText = EnsureText(textGroup.transform, "[TEXT] SelectorValue", value, 15f, new Color(0.91f, 0.85f, 0.74f, 1f), TextAlignmentOptions.Left);
            valueText.textWrappingMode = TextWrappingModes.NoWrap;
            valueText.overflowMode = TextOverflowModes.Ellipsis;

            var arrowText = EnsureText(button.transform, "[TEXT] DropdownArrow", "v", 16f, new Color(0.86f, 0.69f, 0.35f, 1f), TextAlignmentOptions.Center);
            ConfigureLayout(arrowText.gameObject, 28f, 28f, 0f, 28f);

            ConfigureStaticTooltip(iconText.gameObject, label, "Combat Selector", tooltip);
            generatedSelection.Add(button.gameObject);
        }

        private void ToggleDropdown(SelectorKind kind, RectTransform source)
        {
            if (openSelector == kind)
            {
                CloseDropdown();
                return;
            }

            OpenDropdown(kind, source);
        }

        private void OpenDropdown(SelectorKind kind, RectTransform source)
        {
            CloseDropdown();
            if (dropdownLayer == null || source == null || combatSystem?.Catalog == null)
            {
                return;
            }

            openSelector = kind;

            var blocker = new GameObject("[BLOCKER] CombatSelectorDropdownBlocker", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
            blocker.transform.SetParent(dropdownLayer, false);
            var blockerRect = blocker.transform as RectTransform;
            blockerRect.anchorMin = Vector2.zero;
            blockerRect.anchorMax = Vector2.one;
            blockerRect.offsetMin = Vector2.zero;
            blockerRect.offsetMax = Vector2.zero;
            var blockerImage = blocker.GetComponent<Image>();
            blockerImage.color = new Color(0f, 0f, 0f, 0f);
            blockerImage.raycastTarget = true;
            var blockerButton = blocker.GetComponent<Button>();
            blockerButton.targetGraphic = blockerImage;
            blockerButton.onClick.AddListener(CloseDropdown);
            generatedDropdown.Add(blocker);

            var options = GetSelectorOptions(kind).ToList();
            var panel = new GameObject("[DROPDOWN] CombatSelectorDropdown", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Outline), typeof(VerticalLayoutGroup), typeof(LayoutElement));
            panel.transform.SetParent(dropdownLayer, false);
            panel.transform.SetAsLastSibling();
            ConfigureVertical(panel, 4, 6, 6, 6, 6, TextAnchor.UpperLeft);
            ConfigureLayout(panel, 46f * Mathf.Max(1, options.Count) + 12f, 46f * Mathf.Max(1, options.Count) + 12f, 0f);
            var panelImage = panel.GetComponent<Image>();
            panelImage.color = new Color(0.05f, 0.07f, 0.10f, 0.98f);
            panelImage.raycastTarget = true;
            var panelOutline = panel.GetComponent<Outline>();
            panelOutline.effectColor = new Color(0.34f, 0.56f, 0.82f, 1f);
            panelOutline.effectDistance = new Vector2(1f, -1f);

            PositionDropdownPanel(panel.transform as RectTransform, source, options.Count);
            generatedDropdown.Add(panel);

            if (options.Count == 0)
            {
                AddDropdownLabel(panel.transform, "No valid options");
                return;
            }

            foreach (var option in options)
            {
                AddDropdownOption(panel.transform, option);
            }
        }

        private IEnumerable<SelectorOption> GetSelectorOptions(SelectorKind kind)
        {
            var catalog = combatSystem.Catalog;
            switch (kind)
            {
                case SelectorKind.Region:
                    yield return new SelectorOption(catalog.RegionDisplayName, catalog.RegionId, combatSystem.SelectedRegionId == catalog.RegionId, () => combatSystem.SelectRegion(catalog.RegionId));
                    break;
                case SelectorKind.ActivityType:
                    if (combatSystem.SelectedRegionId == catalog.RegionId)
                    {
                        yield return new SelectorOption(catalog.ActivityTypeDisplayName, catalog.ActivityTypeId, combatSystem.SelectedActivityTypeId == catalog.ActivityTypeId, () => combatSystem.SelectActivityType(catalog.ActivityTypeId));
                    }

                    break;
                case SelectorKind.Location:
                    if (combatSystem.SelectedActivityTypeId == catalog.ActivityTypeId)
                    {
                        yield return new SelectorOption(catalog.LocationDisplayName, catalog.LocationId, combatSystem.SelectedLocationId == catalog.LocationId, () => combatSystem.SelectLocation(catalog.LocationId));
                    }

                    break;
            }
        }

        private void AddDropdownOption(Transform parent, SelectorOption option)
        {
            var row = new GameObject("[OPTION] CombatSelectorOption", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button), typeof(LayoutElement));
            row.transform.SetParent(parent, false);
            ConfigureLayout(row, 40f, 40f, 0f);
            var image = row.GetComponent<Image>();
            image.color = option.Selected ? new Color(0.12f, 0.18f, 0.25f, 1f) : new Color(0.08f, 0.11f, 0.15f, 1f);
            image.raycastTarget = true;
            var button = row.GetComponent<Button>();
            button.targetGraphic = image;
            button.onClick.AddListener(() =>
            {
                CloseDropdown();
                option.Select();
            });

            var text = EnsureText(row.transform, "[TEXT] Label", option.Selected ? $"> {option.DisplayName}" : option.DisplayName, 14f, new Color(0.91f, 0.85f, 0.74f, 1f), TextAlignmentOptions.Left);
            Stretch(text.transform as RectTransform, 10f, 0f, -10f, 0f);
            text.textWrappingMode = TextWrappingModes.NoWrap;
            text.overflowMode = TextOverflowModes.Ellipsis;
        }

        private void AddDropdownLabel(Transform parent, string value)
        {
            var text = EnsureText(parent, "[TEXT] EmptyDropdown", value, 13f, new Color(0.58f, 0.65f, 0.72f, 1f), TextAlignmentOptions.Center);
            ConfigureLayout(text.gameObject, 40f, 40f, 0f);
        }

        private void PositionDropdownPanel(RectTransform panel, RectTransform source, int optionCount)
        {
            if (panel == null || source == null)
            {
                return;
            }

            var parentRect = dropdownLayer as RectTransform;
            var corners = new Vector3[4];
            source.GetWorldCorners(corners);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, corners[0], null, out var bottomLeft);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, corners[2], null, out var topRight);

            panel.anchorMin = new Vector2(0.5f, 0.5f);
            panel.anchorMax = new Vector2(0.5f, 0.5f);
            panel.pivot = new Vector2(0f, 1f);
            panel.sizeDelta = new Vector2(Mathf.Max(260f, topRight.x - bottomLeft.x), 46f * Mathf.Max(1, optionCount) + 12f);
            panel.anchoredPosition = new Vector2(bottomLeft.x, bottomLeft.y - 4f);
        }

        private void CloseDropdown()
        {
            openSelector = SelectorKind.None;
            TooltipManager.HideGlobal();
            ClearGenerated(generatedDropdown);
            UpdateGeneratedSelectorVisuals();
        }

        private void UpdateGeneratedSelectorVisuals()
        {
            if (combatSystem?.Catalog == null)
            {
                return;
            }

            var catalog = combatSystem.Catalog;
            foreach (var item in generatedSelection.Where(item => item != null))
            {
                var button = item.GetComponent<Button>();
                if (button == null)
                {
                    continue;
                }

                if (item.name.Contains("RegionSelector"))
                {
                    ApplySelectorVisual(button, SelectorKind.Region, combatSystem.SelectedRegionId == catalog.RegionId);
                }
                else if (item.name.Contains("TypeSelector"))
                {
                    ApplySelectorVisual(button, SelectorKind.ActivityType, combatSystem.SelectedActivityTypeId == catalog.ActivityTypeId);
                }
                else if (item.name.Contains("LocationSelector"))
                {
                    ApplySelectorVisual(button, SelectorKind.Location, combatSystem.SelectedLocationId == catalog.LocationId);
                }
            }
        }

        private void ApplySelectorVisual(Button button, SelectorKind kind, bool selected)
        {
            if (button == null)
            {
                return;
            }

            var image = button.GetComponent<Image>();
            if (image != null)
            {
                image.color = selected || openSelector == kind ? new Color(0.12f, 0.18f, 0.25f, 1f) : new Color(0.08f, 0.11f, 0.15f, 1f);
                image.raycastTarget = true;
            }

            var outline = button.GetComponent<Outline>();
            if (outline != null)
            {
                outline.effectColor = openSelector == kind
                    ? new Color(0.34f, 0.56f, 0.82f, 1f)
                    : selected ? new Color(0.27f, 0.42f, 0.58f, 1f) : new Color(0.20f, 0.27f, 0.33f, 1f);
            }
        }

        private void AddEnemyEntry(Transform container, Button template, CombatEnemyDefinition enemy, bool selected)
        {
            if (container == null || template == null || enemy == null)
            {
                return;
            }

            var unlocked = combatSystem.IsEnemyUnlocked(enemy);
            var button = Instantiate(template, container);
            button.name = "[CARD] EnemyEntry";
            button.gameObject.SetActive(true);
            button.interactable = true;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => combatSystem.SelectEnemy(enemy.EnemyId));
            ConfigureHorizontal(button.gameObject, 10, 10, 10, 6, 6, TextAnchor.MiddleLeft);
            ConfigureLayout(button.gameObject, 66f, 66f, 0f);

            var image = button.GetComponent<Image>();
            if (image != null)
            {
                image.color = selected ? new Color(0.12f, 0.18f, 0.25f, 1f) : unlocked ? new Color(0.08f, 0.11f, 0.15f, 1f) : new Color(0.08f, 0.08f, 0.09f, 0.88f);
                image.raycastTarget = true;
            }

            var outline = button.GetComponent<Outline>();
            if (outline != null)
            {
                outline.effectColor = selected ? new Color(0.34f, 0.56f, 0.82f, 1f) : new Color(0.20f, 0.27f, 0.33f, 1f);
            }

            var portrait = EnsureChild(button.transform, "[FRAME] EnemyEntryPortrait");
            ConfigureLayout(portrait, 46f, 46f, 0f, 46f);
            var portraitImage = portrait.GetComponent<Image>() ?? portrait.AddComponent<Image>();
            SetIcon(portraitImage, null, EnsureText(portrait.transform, "[TEXT] EnemyEntryPortraitPlaceholder", Abbreviate(enemy.DisplayName), 10f, new Color(0.86f, 0.69f, 0.35f, 1f), TextAlignmentOptions.Center), Abbreviate(enemy.DisplayName));
            ConfigureStaticTooltip(portrait, enemy.DisplayName, $"Level {enemy.RequiredWarriorLevel} Enemy", enemy.Description);

            var textGroup = EnsureChild(button.transform, "[GROUP] EnemyEntryText");
            ConfigureVertical(textGroup, 1, 0, 0, 0, 0, TextAnchor.MiddleLeft);
            ConfigureLayout(textGroup, 0f, 0f, 1f);
            var nameText = EnsureText(textGroup.transform, "[TEXT] EnemyEntryName", enemy.DisplayName, 15f, selected ? new Color(1f, 0.88f, 0.48f, 1f) : new Color(0.91f, 0.85f, 0.74f, 1f), TextAlignmentOptions.Left);
            nameText.textWrappingMode = TextWrappingModes.NoWrap;
            nameText.overflowMode = TextOverflowModes.Ellipsis;
            var stateText = EnsureText(textGroup.transform, "[TEXT] EnemyEntryState", unlocked ? "Available" : BuildRequirementText(enemy), 12f, unlocked ? new Color(0.55f, 0.90f, 0.68f, 1f) : new Color(1f, 0.52f, 0.45f, 1f), TextAlignmentOptions.Left);
            stateText.textWrappingMode = TextWrappingModes.NoWrap;
            stateText.overflowMode = TextOverflowModes.Ellipsis;

            var levelText = EnsureText(button.transform, "[TEXT] EnemyEntryLevel", $"Lv. {enemy.RequiredWarriorLevel}", 13f, new Color(0.68f, 0.78f, 0.86f, 1f), TextAlignmentOptions.Right);
            ConfigureLayout(levelText.gameObject, 64f, 24f, 0f, 64f);
            generatedSelection.Add(button.gameObject);
        }

        private void AddEmptySelectionState(Transform container, string message)
        {
            if (container == null)
            {
                return;
            }

            var item = new GameObject("[STATE] EmptyEnemyList", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI), typeof(LayoutElement));
            item.transform.SetParent(container, false);
            ConfigureLayout(item, 44f, 44f, 0f);
            var text = item.GetComponent<TMP_Text>();
            text.text = message;
            text.fontSize = 13f;
            text.color = new Color(0.58f, 0.65f, 0.72f, 1f);
            text.alignment = TextAlignmentOptions.Center;
            text.raycastTarget = false;
            generatedSelection.Add(item);
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

        private void ConfigureEnemyPortraitTooltip()
        {
            var enemy = combatSystem.SelectedEnemy;
            if (enemy == null)
            {
                return;
            }

            var target = enemyPortraitFrameImage != null ? enemyPortraitFrameImage.gameObject : enemyPortraitImage != null ? enemyPortraitImage.gameObject : null;
            ConfigureStaticTooltip(target, enemy.DisplayName, $"Level {enemy.RequiredWarriorLevel} Enemy", enemy.Description);
        }

        private void AddStatsSection(CombatEnemyDefinition enemy)
        {
            var section = AddDetailsSection("[SECTION] CombatStatistics", "Combat Statistics");
            if (section == null)
            {
                return;
            }

            AddStatRow(section.transform, "Health", enemy.MaximumHealth.ToString());
            AddStatRow(section.transform, "Damage", $"{enemy.MinDamage}-{enemy.MaxDamage}");
            AddStatRow(section.transform, "Attack Speed", $"{enemy.AttackInterval:0.0}s");
            AddStatRow(section.transform, "Accuracy", enemy.Accuracy.ToString());
            AddStatRow(section.transform, "Defence", enemy.Defense.ToString());
            AddStatRow(section.transform, "Critical Chance", enemy.CriticalChance.ToString("P0"));
        }

        private void AddAbilitiesSection()
        {
            var section = AddDetailsSection("[SECTION] SpecialAbilities", "Special Abilities");
            if (section == null)
            {
                return;
            }

            var text = EnsureText(section.transform, "[TEXT] AbilityNone", "Special Abilities: None", 13f, new Color(0.58f, 0.65f, 0.72f, 1f), TextAlignmentOptions.Left);
            ConfigureLayout(text.gameObject, 24f, 24f, 0f);
            text.raycastTarget = false;
        }

        private void AddRewardsSection(CombatEnemyDefinition enemy)
        {
            var section = AddDetailsSection("[SECTION] RewardPreview", "Possible Rewards");
            if (section == null)
            {
                return;
            }

            var strip = EnsureChild(section.transform, "[STRIP] RewardIcons");
            ConfigureHorizontal(strip, 8, 0, 0, 0, 0, TextAnchor.MiddleLeft);
            ConfigureLayout(strip, 70f, 70f, 0f);

            if (enemy.MaxGold > 0)
            {
                AddRewardCard(strip.transform, "Gold", "Gold", $"{enemy.MinGold}-{enemy.MaxGold}", "Guaranteed", null, "Currency", "Gold awarded when this enemy is defeated.");
            }

            foreach (var loot in enemy.Loot.Where(loot => loot != null && loot.IsValid))
            {
                var item = GetItem(loot.itemId);
                AddRewardCard(
                    strip.transform,
                    item != null ? item.DisplayName : loot.itemId,
                    Abbreviate(item != null ? item.DisplayName : loot.itemId),
                    FormatQuantity(loot.minQuantity, loot.maxQuantity),
                    loot.chance >= 1f ? "Guaranteed" : loot.chance.ToString("P0"),
                    item,
                    "Loot",
                    item != null ? item.Description : loot.itemId);
            }

            foreach (var reward in enemy.FirstClearRewards.Where(reward => reward != null && reward.IsValid))
            {
                var item = GetItem(reward.itemId);
                AddRewardCard(
                    strip.transform,
                    item != null ? item.DisplayName : reward.itemId,
                    Abbreviate(item != null ? item.DisplayName : reward.itemId),
                    $"First {FormatQuantity(reward.minQuantity, reward.maxQuantity)}",
                    "First Clear",
                    item,
                    "First Clear",
                    item != null ? item.Description : reward.itemId);
            }

            if (strip.transform.childCount == 0)
            {
                var empty = EnsureText(strip.transform, "[TEXT] NoRewards", "No reward data.", 13f, new Color(0.58f, 0.65f, 0.72f, 1f), TextAlignmentOptions.Left);
                ConfigureLayout(empty.gameObject, 36f, 36f, 0f);
            }
        }

        private GameObject AddDetailsSection(string name, string heading)
        {
            var parent = GetDetailsParent();
            if (parent == null)
            {
                return null;
            }

            var section = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(VerticalLayoutGroup), typeof(LayoutElement));
            section.transform.SetParent(parent, false);
            if (startCombatButton != null)
            {
                section.transform.SetSiblingIndex(startCombatButton.transform.GetSiblingIndex());
            }

            ConfigureVertical(section, 5, 8, 8, 6, 6, TextAnchor.UpperLeft);
            ConfigureLayout(section, 0f, -1f, 0f);
            var image = section.GetComponent<Image>();
            image.color = new Color(0f, 0f, 0f, 0.10f);
            image.raycastTarget = false;

            var title = EnsureText(section.transform, "[HEADER] SectionHeading", heading, 14f, new Color(0.86f, 0.69f, 0.35f, 1f), TextAlignmentOptions.Left);
            ConfigureLayout(title.gameObject, 22f, 22f, 0f);
            title.raycastTarget = false;

            generatedDetails.Add(section);
            return section;
        }

        private Transform GetDetailsParent()
        {
            if (detailsNameText != null)
            {
                return detailsNameText.transform.parent;
            }

            return startCombatButton != null ? startCombatButton.transform.parent : transform;
        }

        private void AddStatRow(Transform parent, string label, string value)
        {
            var row = new GameObject($"[ROW] {label}Stat", typeof(RectTransform), typeof(HorizontalLayoutGroup), typeof(LayoutElement));
            row.transform.SetParent(parent, false);
            ConfigureHorizontal(row, 4, 0, 0, 0, 0, TextAnchor.MiddleLeft);
            ConfigureLayout(row, 22f, 22f, 0f);

            var labelText = EnsureText(row.transform, "[TEXT] StatLabel", label, 13f, new Color(0.68f, 0.78f, 0.86f, 1f), TextAlignmentOptions.Left);
            ConfigureLayout(labelText.gameObject, 0f, 20f, 1f);
            labelText.raycastTarget = false;

            var valueText = EnsureText(row.transform, "[TEXT] StatValue", value, 13f, new Color(0.91f, 0.85f, 0.74f, 1f), TextAlignmentOptions.Right);
            ConfigureLayout(valueText.gameObject, 110f, 20f, 0f, 110f);
            valueText.raycastTarget = false;
        }

        private void AddRewardCard(Transform parent, string title, string placeholder, string quantity, string chance, ItemDefinition item, string category, string description)
        {
            var card = new GameObject("[CARD] RewardIcon", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(VerticalLayoutGroup), typeof(LayoutElement));
            card.transform.SetParent(parent, false);
            ConfigureVertical(card, 2, 4, 4, 4, 4, TextAnchor.UpperCenter);
            ConfigureLayout(card, 68f, 68f, 0f, 74f);
            var cardImage = card.GetComponent<Image>();
            cardImage.color = new Color(0.08f, 0.11f, 0.15f, 1f);
            cardImage.raycastTarget = true;

            var icon = new GameObject("[IMAGE] RewardIcon", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(LayoutElement));
            icon.transform.SetParent(card.transform, false);
            ConfigureLayout(icon, 34f, 34f, 0f, 34f);
            var iconImage = icon.GetComponent<Image>();
            var placeholderText = EnsureText(icon.transform, "[TEXT] RewardIconPlaceholder", placeholder, 9f, new Color(0.86f, 0.69f, 0.35f, 1f), TextAlignmentOptions.Center);
            SetIcon(iconImage, item != null ? item.Icon : null, placeholderText, placeholder);

            var label = EnsureText(card.transform, "[TEXT] RewardChance", string.IsNullOrWhiteSpace(quantity) ? chance : $"{quantity} {chance}", 10f, new Color(0.91f, 0.85f, 0.74f, 1f), TextAlignmentOptions.Center);
            ConfigureLayout(label.gameObject, 20f, 20f, 0f);
            label.textWrappingMode = TextWrappingModes.NoWrap;
            label.overflowMode = TextOverflowModes.Ellipsis;

            if (item != null)
            {
                ConfigureItemTooltip(card, item.ItemId, item, false);
            }
            else
            {
                ConfigureStaticTooltip(card, title, category, description);
            }
        }

        private static string BuildRequirementText(CombatEnemyDefinition enemy)
        {
            return enemy == null ? string.Empty : $"Requires Warrior Level {enemy.RequiredWarriorLevel}";
        }

        private static string FormatQuantity(int min, int max)
        {
            return min == max ? $"x{min}" : $"x{min}-{max}";
        }

        private static string Abbreviate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "?";
            }

            var words = value.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
            if (words.Length == 1)
            {
                return words[0].Length <= 4 ? words[0].ToUpperInvariant() : words[0][..4].ToUpperInvariant();
            }

            return string.Concat(words.Select(word => char.ToUpperInvariant(word[0]))).Substring(0, Mathf.Min(4, words.Length));
        }

        private static GameObject EnsureChild(Transform parent, string name)
        {
            var existing = HierarchySearch.FindDeep(parent, name);
            if (existing != null && existing.parent == parent)
            {
                return existing.gameObject;
            }

            var child = new GameObject(name, typeof(RectTransform));
            child.transform.SetParent(parent, false);
            return child;
        }

        private static TMP_Text EnsureText(Transform parent, string name, string value, float size, Color color, TextAlignmentOptions alignment)
        {
            var existing = HierarchySearch.FindText(parent, name);
            if (existing == null || existing.transform.parent != parent)
            {
                var child = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
                child.transform.SetParent(parent, false);
                existing = child.GetComponent<TMP_Text>();
            }

            existing.text = value;
            existing.fontSize = size;
            existing.color = color;
            existing.alignment = alignment;
            existing.raycastTarget = false;
            return existing;
        }

        private static void ConfigureHorizontal(GameObject target, float spacing, int left, int right, int top, int bottom, TextAnchor alignment)
        {
            var layout = target.GetComponent<HorizontalLayoutGroup>() ?? target.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = spacing;
            layout.padding = new RectOffset(left, right, top, bottom);
            layout.childAlignment = alignment;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = false;
            layout.childForceExpandHeight = false;
        }

        private static void ConfigureVertical(GameObject target, float spacing, int left, int right, int top, int bottom, TextAnchor alignment)
        {
            var layout = target.GetComponent<VerticalLayoutGroup>() ?? target.AddComponent<VerticalLayoutGroup>();
            layout.spacing = spacing;
            layout.padding = new RectOffset(left, right, top, bottom);
            layout.childAlignment = alignment;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = false;
            layout.childForceExpandHeight = false;
        }

        private static void ConfigureLayout(GameObject target, float preferredHeight, float minHeight, float flexibleHeight, float preferredWidth = -1f)
        {
            var layout = target.GetComponent<LayoutElement>() ?? target.AddComponent<LayoutElement>();
            layout.preferredHeight = preferredHeight;
            layout.minHeight = minHeight;
            layout.flexibleHeight = flexibleHeight;
            layout.preferredWidth = preferredWidth;
        }

        private static void Stretch(RectTransform rect, float left, float bottom, float right, float top)
        {
            if (rect == null)
            {
                return;
            }

            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = new Vector2(left, bottom);
            rect.offsetMax = new Vector2(right, top);
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
            CloseDropdown();
            combatSystem?.StartCombat();
            Refresh();
        }

        private void QueueHeavyStrike()
        {
            combatSystem?.QueueHeavyStrike();
            Refresh();
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

        private void OnCombatFeedback(CombatFeedbackEvent feedback)
        {
            switch (feedback.Kind)
            {
                case CombatFeedbackKind.PlayerDamaged:
                    Flash(playerPortraitFrameImage, logEnemyHitColor);
                    SpawnFloatingText(feedback.Critical ? $"CRIT {feedback.Amount}" : feedback.Amount.ToString(), playerPortraitFrameImage != null ? playerPortraitFrameImage.transform : transform, logEnemyHitColor, feedback.Critical);
                    break;
                case CombatFeedbackKind.EnemyDamaged:
                    Flash(enemyPortraitFrameImage, feedback.Critical ? logCriticalColor : logPlayerHitColor);
                    SpawnFloatingText(feedback.Critical ? $"CRIT {feedback.Amount}" : feedback.Amount.ToString(), enemyPortraitFrameImage != null ? enemyPortraitFrameImage.transform : transform, feedback.Critical ? logCriticalColor : logPlayerHitColor, feedback.Critical);
                    break;
                case CombatFeedbackKind.Healing:
                    Flash(playerPortraitFrameImage, logPotionColor);
                    Flash(potionSlotImage, logPotionColor);
                    SpawnFloatingText($"+{feedback.Amount}", playerPortraitFrameImage != null ? playerPortraitFrameImage.transform : transform, logPotionColor);
                    break;
                case CombatFeedbackKind.HeavyStrikeStarted:
                    Flash(heavyStrikeCardImage, logCriticalColor);
                    break;
                case CombatFeedbackKind.EnemyDefeated:
                    Flash(enemyPortraitFrameImage, logVictoryColor);
                    SpawnFloatingText("Defeated", enemyPortraitFrameImage != null ? enemyPortraitFrameImage.transform : transform, logVictoryColor, true);
                    break;
                case CombatFeedbackKind.EnemyRespawnStarted:
                    Flash(enemyPortraitFrameImage, logWarningColor);
                    break;
                case CombatFeedbackKind.EnemyRespawned:
                    Flash(enemyPortraitFrameImage, logPlayerHitColor);
                    break;
            }
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

        private enum SelectorKind
        {
            None = 0,
            Region = 1,
            ActivityType = 2,
            Location = 3
        }

        private readonly struct SelectorOption
        {
            public SelectorOption(string displayName, string id, bool selected, UnityEngine.Events.UnityAction select)
            {
                DisplayName = displayName;
                Id = id;
                Selected = selected;
                Select = select;
            }

            public string DisplayName { get; }
            public string Id { get; }
            public bool Selected { get; }
            public UnityEngine.Events.UnityAction Select { get; }
        }

        private sealed class FloatingCombatText
        {
            private const float Lifetime = 0.85f;
            private readonly TMP_Text text;
            private readonly RectTransform rect;
            private Vector2 startPosition;
            private Color startColor;
            private float remaining;

            public FloatingCombatText(TMP_Text text)
            {
                this.text = text;
                rect = text != null ? text.transform as RectTransform : null;
                if (this.text != null)
                {
                    this.text.gameObject.SetActive(false);
                }
            }

            public TMP_Text Text => text;
            public bool IsActive => text != null && text.gameObject.activeSelf;

            public void Play(string value, Vector2 position, Color color, bool critical)
            {
                if (text == null || rect == null)
                {
                    return;
                }

                startPosition = position;
                startColor = color;
                remaining = Lifetime;
                text.text = value;
                text.color = color;
                text.fontSize = critical ? 17f : 14f;
                text.alignment = TextAlignmentOptions.Center;
                text.gameObject.SetActive(true);
                rect.anchoredPosition = position;
            }

            public void Tick(float delta)
            {
                if (!IsActive || rect == null)
                {
                    return;
                }

                remaining = Mathf.Max(0f, remaining - delta);
                var progress = 1f - remaining / Lifetime;
                rect.anchoredPosition = startPosition + new Vector2(0f, Mathf.Lerp(0f, 34f, progress));
                text.color = new Color(startColor.r, startColor.g, startColor.b, Mathf.Lerp(startColor.a, 0f, progress));
                if (remaining <= 0f)
                {
                    text.gameObject.SetActive(false);
                }
            }
        }
    }
}
