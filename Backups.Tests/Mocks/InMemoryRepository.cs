using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Backups.Repositories;
using Backups.Tools;
using Utility.Extensions;

namespace Backups.Tests.Mocks
{
    public class InMemoryRepository : Repository
    {
        public InMemoryRepository(string id = "")
            : base(id) { }

        public interface INode
        {
            public string Name { get; }
        }

        public static FolderNode Root { get; private set; } = new FolderNode("Memory");

        public static void Clear()
        {
            Root.Dispose();
            Root = new FolderNode("Memory");
        }

        public override Repository GetSubRepositoryAt(string path)
            => new InMemoryRepository($"{Id}{BackupConfiguration.PathDelimiter}{path}");

        public override bool Exists(string path)
            => Find(ParsePath(path)).Node is not null;

        public override bool IsFolder(string path)
            => Exists(path) && IsDirectory(path);

        public override void Delete(string path)
        {
            (INode? node, FolderNode? parent) = Find(ParsePath(path));

            if (node is null || parent is null)
                throw new InvalidOperationException("Node or parent is missing");

            if (!parent.Children.Remove(node.Name))
                throw new InvalidOperationException("Unexpected behavior");

            if (node is IDisposable disposable)
                disposable.Dispose();
        }

        public override void Write(string path, Stream data)
        {
            (INode? node, FolderNode? parent) = Find(ParsePath(path), true);

            if (node is null || parent is null)
                throw new InvalidOperationException("Node or parent is missing");

            if (node is not DataNode dataNode)
                throw new InvalidOperationException("Node is not a data node");

            dataNode.Stream.SetLength(0);
            data.Position = 0;
            data.CopyTo(dataNode.Stream);
            dataNode.Stream.Position = 0;
        }

        public override IReadOnlyCollection<string> GetContentsOf(string folderPath)
        {
            (INode? node, FolderNode? _) = Find(ParsePath(folderPath), true);

            if (node is not FolderNode folderNode)
                throw new InvalidOperationException("Node is not a folder node");

            return folderNode.Children.Keys;
        }

        public override Stream GetStream(string path)
        {
            (INode? node, FolderNode? _) = Find(ParsePath(path), true);

            if (node is not DataNode dataNode)
                throw new InvalidOperationException("Node is not a data node");

            var ms = new MemoryStream();
            dataNode.Stream.Position = 0;
            dataNode.Stream.CopyTo(ms);
            ms.Position = 0;

            return ms;
        }

        public override bool Equals(Repository? other)
            => other is InMemoryRepository && other.Id.Equals(Id);

        private IReadOnlyCollection<string> ParsePath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return Array.Empty<string>();

            string[] split = $"{Id}{BackupConfiguration.PathDelimiter}{path}"
                .Split(BackupConfiguration.PathDelimiter)
                .Where(s => !string.IsNullOrEmpty(s))
                .ToArray();
            bool fileFound = false;

            foreach (var str in split)
            {
                if (fileFound)
                    throw new InvalidOperationException("File must not contain anything");

                if (!IsDirectory(str))
                    fileFound = true;
            }

            return split;
        }

        private static bool IsDirectory(ReadOnlySpan<char> s)
        {
            for (int i = s.Length - 1; i >= 0; i--)
            {
                if (s[i].Equals(BackupConfiguration.ExtensionDelimiter))
                    return false;
                if (s[i].Equals(BackupConfiguration.PathDelimiter))
                    return true;
            }

            return true;
        }

        private static INode CreateNode(string name)
            => IsDirectory(name) ? new FolderNode(name) : new DataNode(name);

        private static (INode? Node, FolderNode? Parent) Find(IReadOnlyCollection<string> path, bool creating = false)
        {
            FolderNode? parent = null;
            INode node = Root;

            foreach (string s in path)
            {
                if (node is not FolderNode folderNode)
                    return (null, null);

                if (!folderNode.Children.ContainsKey(s))
                {
                    if (creating)
                        folderNode.Children[s] = CreateNode(s);
                    else
                        return (null, null);
                }

                parent = folderNode;
                node = folderNode.Children[s];
            }

            return (node, parent);
        }

        public sealed class DataNode : INode, IDisposable
        {
            public DataNode(string name)
            {
                name.ThrowIfNull(nameof(name));

                Name = name;
                Stream = new MemoryStream();
            }

            public string Name { get; }
            public MemoryStream Stream { get; }

            public void Dispose()
                => Stream.Dispose();
        }

        public sealed class FolderNode : INode, IDisposable
        {
            public FolderNode(string name)
            {
                Name = name;
                Children = new Dictionary<string, INode>();
            }

            public string Name { get; }
            public Dictionary<string, INode> Children { get; }

            public void Dispose()
            {
                foreach (var (_, value) in Children)
                {
                    if (value is IDisposable disposable)
                        disposable.Dispose();
                }
            }
        }
    }
}