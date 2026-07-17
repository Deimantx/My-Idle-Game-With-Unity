using IdleGame.Activities;
using IdleGame.Professions.Woodcutting;
using IdleGame.UI.Shared;
using UnityEngine;
using UnityEngine.UI;

namespace IdleGame.UI.Woodcutting
{
    public sealed class ActiveActivityBarController : MonoBehaviour
    {
        [SerializeField] private ActiveActivityService activeActivityService;
        [SerializeField] private WoodcuttingSystem woodcuttingSystem;
        [SerializeField] private ScreenManager screenManager;
        [SerializeField] private Transform noActivityState;
        [SerializeField] private Transform professionActivityState;
        [SerializeField] private Transform combatActivityState;
        [SerializeField] private TMPro.TMP_Text professionNameText;
        [SerializeField] private RuntimeFillBar professionProgressBar;
        [SerializeField] private Button openProfessionButton;
        [SerializeField] private Button stopProfessionButton;

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
        }

        public void ConfigureForEditor(ActiveActivityService activeActivity, WoodcuttingSystem woodcutting, ScreenManager screens)
        {
            activeActivityService = activeActivity;
            woodcuttingSystem = woodcutting;
            screenManager = screens;
            AutoBind();
        }

        public void AutoBind()
        {
            noActivityState ??= HierarchySearch.FindDeep(transform, "[STATE] NoActivityState");
            professionActivityState ??= HierarchySearch.FindDeep(transform, "[STATE] ProfessionActivityState");
            combatActivityState ??= HierarchySearch.FindDeep(transform, "[STATE] CombatActivityState");
            professionNameText ??= HierarchySearch.FindText(transform, "[TEXT] ProfessionNameText");
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
                professionNameText.text = $"Woodcutting - {tree.DisplayName}";
            }

            var durability01 = tree.MaximumDurability <= 0f ? 0f : woodcuttingSystem.CurrentDurability / tree.MaximumDurability;
            professionProgressBar?.SetValue(durability01, $"{Mathf.CeilToInt(woodcuttingSystem.CurrentDurability)} / {Mathf.CeilToInt(tree.MaximumDurability)}");
        }

        private void OpenWoodcutting()
        {
            screenManager?.OpenScreen(ScreenIds.Woodcutting);
        }

        private void StopWoodcutting()
        {
            woodcuttingSystem?.StopWoodcutting();
        }
    }
}
