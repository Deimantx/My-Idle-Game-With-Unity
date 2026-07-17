using IdleGame.Professions.Woodcutting;
using IdleGame.UI.Shared;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IdleGame.UI.Woodcutting
{
    public sealed class TreeActivityCard : MonoBehaviour
    {
        [SerializeField] private TMP_Text treeNameText;
        [SerializeField] private TMP_Text requiredLevelText;
        [SerializeField] private TMP_Text stateText;
        [SerializeField] private TMP_Text glyphText;
        [SerializeField] private Button button;

        private WoodcuttingTreeDefinition tree;
        private WoodcuttingScreenController owner;

        public void Bind(WoodcuttingTreeDefinition treeDefinition, WoodcuttingScreenController screenController, bool unlocked, bool selected)
        {
            tree = treeDefinition;
            owner = screenController;
            AutoBind();

            if (treeNameText != null)
            {
                treeNameText.text = treeDefinition.DisplayName;
            }

            if (requiredLevelText != null)
            {
                requiredLevelText.text = $"Level {treeDefinition.RequiredLevel}";
            }

            if (stateText != null)
            {
                stateText.text = selected ? "SELECTED" : unlocked ? "READY" : $"LOCKED - LEVEL {treeDefinition.RequiredLevel}";
            }

            if (glyphText != null)
            {
                glyphText.text = "TREE";
            }

            if (button != null)
            {
                button.interactable = true;
                button.onClick.RemoveListener(OnClicked);
                button.onClick.AddListener(OnClicked);
            }
        }

        public void AutoBind()
        {
            treeNameText ??= HierarchySearch.FindText(transform, "[TEXT] TreeName");
            requiredLevelText ??= HierarchySearch.FindText(transform, "[TEXT] RequiredLevel");
            stateText ??= HierarchySearch.FindText(transform, "[STATE] TreeCardState");
            glyphText ??= HierarchySearch.FindText(transform, "[TEXT] TreeGlyph");
            button ??= GetComponent<Button>();
        }

        private void OnClicked()
        {
            if (tree != null && owner != null)
            {
                owner.SelectTree(tree.TreeId);
            }
        }
    }
}
