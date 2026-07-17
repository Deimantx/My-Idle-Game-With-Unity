using System;
using System.Collections.Generic;
using System.Linq;
using IdleGame.Core.Identifiers;
using UnityEngine;

namespace IdleGame.Professions.Woodcutting
{
    [CreateAssetMenu(menuName = "Idle Game/Woodcutting/Woodcutting Catalog", fileName = "WoodcuttingCatalog")]
    public sealed class WoodcuttingCatalog : ScriptableObject
    {
        [SerializeField] private List<WoodcuttingTreeDefinition> trees = new();

        private Dictionary<string, WoodcuttingTreeDefinition> lookup;

        public IReadOnlyList<WoodcuttingTreeDefinition> Trees => trees;

        public bool TryGetTree(string treeId, out WoodcuttingTreeDefinition tree)
        {
            BuildLookupIfNeeded();
            return lookup.TryGetValue(treeId, out tree);
        }

        public WoodcuttingTreeDefinition GetFirstUnlockedOrFirst(int level)
        {
            return trees.FirstOrDefault(tree => tree != null && tree.RequiredLevel <= level) ?? trees.FirstOrDefault(tree => tree != null);
        }

        public void ConfigureForEditor(IEnumerable<WoodcuttingTreeDefinition> treeDefinitions)
        {
            trees = treeDefinitions?.Where(tree => tree != null).ToList() ?? new List<WoodcuttingTreeDefinition>();
            lookup = null;
        }

        private void BuildLookupIfNeeded()
        {
            if (lookup != null)
            {
                return;
            }

            lookup = new Dictionary<string, WoodcuttingTreeDefinition>(StringComparer.Ordinal);
            foreach (var tree in trees.Where(tree => tree != null))
            {
                StableId.ThrowIfInvalid(tree.TreeId, nameof(tree.TreeId));
                lookup.TryAdd(tree.TreeId, tree);
            }
        }
    }
}
