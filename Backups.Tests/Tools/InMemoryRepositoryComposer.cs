using Backups.Tests.Mocks;
using Spectre.Console;

namespace Backups.Tests.Tools
{
    public class InMemoryRepositoryComposer
    {
        private readonly InMemoryRepository _repository;

        public InMemoryRepositoryComposer(InMemoryRepository repository)
        {
            _repository = repository;
        }

        public Tree Compose()
        {
            Tree tree = new Tree(string.Empty);
            tree.AddNode(CreateNode(_repository.Root));

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