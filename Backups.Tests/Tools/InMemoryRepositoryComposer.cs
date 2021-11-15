using Backups.Tests.Mocks;
using Spectre.Console;

namespace Backups.Tests.Tools
{
    public static class InMemoryRepositoryComposer
    {
        public static Tree Compose()
        {
            Tree tree = new Tree(string.Empty);
            tree.AddNode(CreateNode(InMemoryRepository.Root));

            return tree;
        }

        private static TreeNode CreateNode(InMemoryRepository.INode node)
        {
            if (node is not InMemoryRepository.FolderNode folderNode)
                return new TreeNode(new Text(node.Name));

            var treeNode = new TreeNode(new Text(node.Name));
            foreach (InMemoryRepository.INode childrenNode in folderNode.Children.Values)
            {
                treeNode.AddNode(CreateNode(childrenNode));
            }

            return treeNode;
        }
    }
}