using IdleGame.Activities;
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

        private float activeSeconds;
        private bool wasWoodcuttingActive;

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

            if (progressionSystem != null)
            {
                progressionSystem.ProgressChanged += OnProgressChanged;
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

            if (progressionSystem != null)
            {
                progressionSystem.ProgressChanged -= OnProgressChanged;
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
        }

        private void Update()
        {
            var isWoodcutting = woodcuttingSystem != null && woodcuttingSystem.IsActive;
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

            if (noActivityState != null)
            {
                noActivityState.gameObject.SetActive(!isWoodcutting);
            }

            if (professionActivityState != null)
            {
                professionActivityState.gameObject.SetActive(isWoodcutting);
            }

            if (combatActivityState != null)
            {
                combatActivityState.gameObject.SetActive(false);
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
