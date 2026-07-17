using IdleGame.Activities;
using IdleGame.Combat;
using IdleGame.Professions.Woodcutting;
using IdleGame.Progression;
using IdleGame.UI.Shared;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IdleGame.UI.Woodcutting
{
    public sealed class ActiveActivityBarController : MonoBehaviour
    {
        [SerializeField] private ActiveActivityService activeActivityService;
        [SerializeField] private WoodcuttingSystem woodcuttingSystem;
        [SerializeField] private CombatSystem combatSystem;
        [SerializeField] private ProfessionProgressionSystem progressionSystem;
        [SerializeField] private ScreenManager screenManager;
        [SerializeField] private Transform noActivityState;
        [SerializeField] private Transform professionActivityState;
        [SerializeField] private Transform combatActivityState;
        [SerializeField] private TMP_Text professionNameText;
        [SerializeField] private TMP_Text professionTargetText;
        [SerializeField] private TMP_Text activeTimeText;
        [SerializeField] private TMP_Text professionLevelText;
        [SerializeField] private TMP_Text etaText;
        [SerializeField] private RuntimeFillBar professionProgressBar;
        [SerializeField] private Button openProfessionButton;
        [SerializeField] private Button stopProfessionButton;
        [SerializeField] private TMP_Text combatDisciplineText;
        [SerializeField] private TMP_Text combatEnemyText;
        [SerializeField] private RuntimeFillBar compactPlayerHealthBar;
        [SerializeField] private RuntimeFillBar compactDevotionBar;
        [SerializeField] private RuntimeFillBar compactEnemyHealthBar;
        [SerializeField] private Button openCombatButton;
        [SerializeField] private TMP_Text openCombatButtonText;
        [SerializeField] private Image openCombatButtonImage;
        [SerializeField] private Button quitCombatButton;
        [SerializeField] private TMP_Text quitCombatButtonText;

        private float activeSeconds;
        private bool wasWoodcuttingActive;
        private Color openCombatNormalColor = new(0.1f, 0.14f, 0.19f, 1f);

        private void Awake()
        {
            AutoBind();
        }

        private void OnEnable()
        {
            if (activeActivityService != null)
            {
                activeActivityService.ActiveActivityChanged += Refresh;
            }

            if (woodcuttingSystem != null)
            {
                woodcuttingSystem.StateChanged += Refresh;
            }

            if (combatSystem != null)
            {
                combatSystem.StateChanged += Refresh;
            }

            if (progressionSystem != null)
            {
                progressionSystem.ProgressChanged += OnProgressChanged;
            }

            if (screenManager != null)
            {
                screenManager.ScreenChanged += OnScreenChanged;
            }

            Refresh();
        }

        private void OnDisable()
        {
            if (activeActivityService != null)
            {
                activeActivityService.ActiveActivityChanged -= Refresh;
            }

            if (woodcuttingSystem != null)
            {
                woodcuttingSystem.StateChanged -= Refresh;
            }

            if (combatSystem != null)
            {
                combatSystem.StateChanged -= Refresh;
            }

            if (progressionSystem != null)
            {
                progressionSystem.ProgressChanged -= OnProgressChanged;
            }

            if (screenManager != null)
            {
                screenManager.ScreenChanged -= OnScreenChanged;
            }
        }

        public void ConfigureForEditor(ActiveActivityService activeActivity, WoodcuttingSystem woodcutting, ScreenManager screens)
        {
            activeActivityService = activeActivity;
            woodcuttingSystem = woodcutting;
            screenManager = screens;
            AutoBind();
        }

        public void ConfigureProgressionForEditor(ProfessionProgressionSystem progression)
        {
            progressionSystem = progression;
        }

        public void AutoBind()
        {
            noActivityState ??= HierarchySearch.FindDeep(transform, "[STATE] NoActivityState");
            professionActivityState ??= HierarchySearch.FindDeep(transform, "[STATE] ProfessionActivityState");
            combatActivityState ??= HierarchySearch.FindDeep(transform, "[STATE] CombatActivityState");
            professionNameText ??= HierarchySearch.FindText(transform, "[TEXT] ProfessionNameText");
            professionTargetText ??= HierarchySearch.FindText(transform, "[TEXT] ProfessionTargetText");
            activeTimeText ??= HierarchySearch.FindText(transform, "[TEXT] ActiveTimeText");
            professionLevelText ??= HierarchySearch.FindText(transform, "[TEXT] ActivityLevelText");
            etaText ??= HierarchySearch.FindText(transform, "[TEXT] ActivityETAText");
            professionProgressBar ??= HierarchySearch.FindOrAddFillBar(transform, "[BAR] ProfessionProgressBar");
            openProfessionButton ??= HierarchySearch.FindButton(transform, "[BUTTON] OpenProfessionButton");
            stopProfessionButton ??= HierarchySearch.FindButton(transform, "[BUTTON] StopProfessionButton");
            combatDisciplineText ??= HierarchySearch.FindText(transform, "[TEXT] CombatDisciplineText");
            combatEnemyText ??= HierarchySearch.FindText(transform, "[TEXT] CombatEnemyText");
            compactPlayerHealthBar ??= HierarchySearch.FindOrAddFillBar(transform, "[BAR] CompactPlayerHealthBar");
            compactDevotionBar ??= HierarchySearch.FindOrAddFillBar(transform, "[BAR] CompactDevotionBar");
            compactEnemyHealthBar ??= HierarchySearch.FindOrAddFillBar(transform, "[BAR] CompactEnemyHealthBar");
            openCombatButton ??= HierarchySearch.FindButton(transform, "[BUTTON] OpenCombatButton");
            openCombatButtonText ??= HierarchySearch.FindText(openCombatButton != null ? openCombatButton.transform : null, "[TEXT] Label");
            openCombatButtonImage ??= openCombatButton != null ? openCombatButton.GetComponent<Image>() : null;
            quitCombatButton ??= HierarchySearch.FindButton(transform, "[BUTTON] QuitCombatButton");
            quitCombatButtonText ??= HierarchySearch.FindText(quitCombatButton != null ? quitCombatButton.transform : null, "[TEXT] Label");
            if (openCombatButtonImage != null && openCombatButtonImage.color.a > 0f)
            {
                openCombatNormalColor = openCombatButtonImage.color;
            }

            if (openProfessionButton != null)
            {
                openProfessionButton.onClick.RemoveListener(OpenWoodcutting);
                openProfessionButton.onClick.AddListener(OpenWoodcutting);
            }

            if (stopProfessionButton != null)
            {
                stopProfessionButton.onClick.RemoveListener(StopWoodcutting);
                stopProfessionButton.onClick.AddListener(StopWoodcutting);
            }

            if (openCombatButton != null)
            {
                openCombatButton.onClick.RemoveListener(OpenCombat);
                openCombatButton.onClick.AddListener(OpenCombat);
            }

            if (quitCombatButton != null)
            {
                quitCombatButton.onClick.RemoveListener(QuitCombat);
                quitCombatButton.onClick.AddListener(QuitCombat);
            }

            if (quitCombatButtonText != null)
            {
                quitCombatButtonText.text = "Leave Combat";
            }
        }

        private void Update()
        {
            var isWoodcutting = woodcuttingSystem != null && woodcuttingSystem.IsActive;
            var isCombat = combatSystem != null && combatSystem.IsActive;
            if (!isWoodcutting)
            {
                wasWoodcuttingActive = false;
                return;
            }

            if (!wasWoodcuttingActive)
            {
                activeSeconds = 0f;
                wasWoodcuttingActive = true;
            }

            activeSeconds += Time.deltaTime;
            Refresh();
        }

        private void Refresh()
        {
            var isWoodcutting = woodcuttingSystem != null && woodcuttingSystem.IsActive;
            var isCombat = combatSystem != null && combatSystem.IsActive;

            if (noActivityState != null)
            {
                noActivityState.gameObject.SetActive(!isWoodcutting && !isCombat);
            }

            if (professionActivityState != null)
            {
                professionActivityState.gameObject.SetActive(isWoodcutting && !isCombat);
            }

            if (combatActivityState != null)
            {
                combatActivityState.gameObject.SetActive(isCombat);
            }

            if (isCombat)
            {
                RefreshCombat();
                RefreshCombatOpenButton();
                return;
            }

            if (!isWoodcutting || woodcuttingSystem.SelectedTree == null)
            {
                return;
            }

            var tree = woodcuttingSystem.SelectedTree;
            if (professionNameText != null)
            {
                professionNameText.text = "Woodcutting";
            }

            if (professionTargetText != null)
            {
                professionTargetText.text = tree.DisplayName;
            }

            if (activeTimeText != null)
            {
                activeTimeText.text = $"Active Time\n{FormatDuration(activeSeconds)}";
            }

            if (progressionSystem == null)
            {
                var durability01 = tree.MaximumDurability <= 0f ? 0f : woodcuttingSystem.CurrentDurability / tree.MaximumDurability;
                professionProgressBar?.SetValue(durability01, $"{Mathf.CeilToInt(woodcuttingSystem.CurrentDurability)} / {Mathf.CeilToInt(tree.MaximumDurability)}");
                return;
            }

            var level = progressionSystem.GetLevel(WoodcuttingConstants.ProfessionId);
            var xpIntoLevel = progressionSystem.GetXpIntoLevel(WoodcuttingConstants.ProfessionId);
            var xpNeeded = progressionSystem.GetXpNeededForCurrentLevel(WoodcuttingConstants.ProfessionId);

            if (professionLevelText != null)
            {
                professionLevelText.text = $"Level {level}";
            }

            professionProgressBar?.SetValue(
                progressionSystem.GetLevelProgress01(WoodcuttingConstants.ProfessionId),
                xpNeeded > 0 ? $"{xpIntoLevel} / {xpNeeded} XP" : "Max Level");

            if (etaText != null)
            {
                etaText.text = $"ETA to Level {level + 1}\n{EstimateEtaText(tree, xpNeeded - xpIntoLevel)}";
            }
        }

        private void OpenWoodcutting()
        {
            screenManager?.OpenScreen(ScreenIds.Woodcutting);
        }

        private void StopWoodcutting()
        {
            woodcuttingSystem?.StopWoodcutting();
        }

        private void OpenCombat()
        {
            screenManager?.OpenScreen(ScreenIds.Combat);
        }

        private void QuitCombat()
        {
            combatSystem?.QuitCombat();
        }

        public void ConfigureCombatForEditor(CombatSystem combat)
        {
            combatSystem = combat;
            AutoBind();
        }

        private void RefreshCombat()
        {
            if (combatSystem == null || combatSystem.SelectedEnemy == null)
            {
                return;
            }

            var stats = combatSystem.PlayerStats;
            if (combatDisciplineText != null)
            {
                combatDisciplineText.text = "Combat - Warrior";
            }

            if (combatEnemyText != null)
            {
                combatEnemyText.text = combatSystem.SelectedEnemy.DisplayName;
            }

            compactPlayerHealthBar?.SetValue(
                stats.MaxHealth <= 0 ? 0f : (float)combatSystem.PlayerHealth / stats.MaxHealth,
                $"HP {combatSystem.PlayerHealth} / {stats.MaxHealth}");
            compactDevotionBar?.SetValue(
                stats.MaxDevotion <= 0 ? 0f : combatSystem.PlayerDevotion / stats.MaxDevotion,
                $"Devotion {Mathf.FloorToInt(combatSystem.PlayerDevotion)} / {stats.MaxDevotion}");
            compactEnemyHealthBar?.SetValue(
                combatSystem.SelectedEnemy.MaximumHealth <= 0 ? 0f : (float)combatSystem.EnemyHealth / combatSystem.SelectedEnemy.MaximumHealth,
                $"{combatSystem.EnemyHealth} / {combatSystem.SelectedEnemy.MaximumHealth}");
        }

        private void RefreshCombatOpenButton()
        {
            if (openCombatButton == null)
            {
                return;
            }

            var viewingCombat = screenManager != null && screenManager.CurrentScreenId == ScreenIds.Combat;
            openCombatButton.interactable = !viewingCombat;
            if (openCombatButtonText != null)
            {
                openCombatButtonText.text = viewingCombat ? "Viewing Combat" : "Open";
            }

            if (openCombatButtonImage != null)
            {
                openCombatButtonImage.color = viewingCombat
                    ? new Color(0.2f, 0.18f, 0.11f, 1f)
                    : openCombatNormalColor;
            }
        }

        private void OnScreenChanged(string _)
        {
            Refresh();
        }

        private void OnProgressChanged(string professionId)
        {
            if (professionId == WoodcuttingConstants.ProfessionId)
            {
                Refresh();
            }
        }

        private string EstimateEtaText(WoodcuttingTreeDefinition tree, long remainingXp)
        {
            if (tree == null || remainingXp <= 0)
            {
                return "Max Level";
            }

            var modifiers = woodcuttingSystem.CurrentModifiers;
            var power = Mathf.Max(0.01f, modifiers.finalPower);
            var interval = Mathf.Max(0.05f, modifiers.finalActionInterval);
            var actionsPerCycle = Mathf.Ceil(tree.MaximumDurability / power);
            var secondsPerCycle = (actionsPerCycle * interval) + Mathf.Max(0f, tree.RespawnSeconds);
            if (tree.CompletionXp <= 0 || secondsPerCycle <= 0f)
            {
                return "ETA unavailable";
            }

            var cyclesNeeded = Mathf.Ceil((float)remainingXp / tree.CompletionXp);
            return FormatDuration(cyclesNeeded * secondsPerCycle);
        }

        private static string FormatDuration(float seconds)
        {
            seconds = Mathf.Max(0f, seconds);
            var hours = Mathf.FloorToInt(seconds / 3600f);
            var minutes = Mathf.FloorToInt((seconds % 3600f) / 60f);
            var secs = Mathf.FloorToInt(seconds % 60f);
            return hours > 0
                ? $"{hours:00}:{minutes:00}:{secs:00}"
                : $"{minutes:00}:{secs:00}";
        }
    }
}
