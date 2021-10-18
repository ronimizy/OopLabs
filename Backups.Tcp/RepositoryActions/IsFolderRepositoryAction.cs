using System;
using System.Text.Json.Serialization;
using Backups.Repositories;
using Backups.RepositoryActions;
using Backups.Tools;
using Utility.Extensions;

namespace Backups.Tcp.RepositoryActions
{
    [Serializable]
    public class IsFolderRepositoryAction : IRepositoryAction<bool>
    {
        [JsonConstructor]
        public IsFolderRepositoryAction(string path)
        {
            Path = path.ThrowIfNull(nameof(path));
        }

        public string Path { get; }

        public bool Execute(Repository repository, ILogger? logger)
        {
            repository.ThrowIfNull(nameof(repository));

            return repository.IsFolder(Path);
        }
    }
}