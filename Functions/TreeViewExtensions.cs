using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ArtCore_Editor.Functions;

/// <summary>
/// Struct to remember tree view expand state
/// </summary>
public static class TreeViewExtensions
{
    public static List<string> GetExpansionState(this TreeNodeCollection nodes)
    {
        return nodes.Descendants()
            .Where(n => n.IsExpanded)
            .Select(n => n.FullPath)
            .ToList();
    }

    public static void SetExpansionState(this TreeNodeCollection nodes, List<string> savedExpansionState)
    {
        foreach (var node in nodes.Descendants()
                     .Where(n => savedExpansionState.Contains(n.FullPath)))
        {
            node.Expand();
        }
    }

    private static IEnumerable<TreeNode> Descendants(this TreeNodeCollection c)
    {
        foreach (var node in c.OfType<TreeNode>())
        {
            yield return node;

            foreach (var child in node.Nodes.Descendants())
            {
                yield return child;
            }
        }
    }

    public static void PopulateTreeView(this TreeView treeView, IEnumerable<string> paths, char pathSeparator)
    {
        TreeNode lastNode = null;
        foreach (string path in paths)
        {
            string subPathAgg = string.Empty;
            foreach (string subPath in path.Split(pathSeparator))
            {
                subPathAgg += subPath + pathSeparator;
                TreeNode[] nodes = treeView.Nodes.Find(subPathAgg, true);
                if (nodes.Length == 0)
                    if (lastNode == null)
                        lastNode = treeView.Nodes.Add(subPathAgg, subPath);
                    else
                        lastNode = lastNode.Nodes.Add(subPathAgg, subPath);
                else
                    lastNode = nodes[0];
            }
            lastNode = null;

        }
    }
}