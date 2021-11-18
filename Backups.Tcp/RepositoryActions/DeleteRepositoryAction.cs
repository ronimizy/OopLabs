using System;
using Backups.Repositories;
using Backups.RepositoryActions;
using Backups.Tools;
using Newtonsoft.Json;
using Utility.Extensions;

namespace Backups.Tcp.RepositoryActions
{
    [Serializable]
    public class DeleteRepositoryAction : IRepositoryAction<object?>
    {
        [JsonConstructor]
        public DeleteRepositoryAction(string path)
        {
            Path = path.ThrowIfNull(nameof(path));
        }

        public string Path { get; }

        public object? Execute(Repository repository, ILogger? logger)
        {
            repository.ThrowIfNull(nameof(repository));

            repository.Delete(Path);
            return null;
        }
    }
}