using System;
using Backups.Repositories;
using Backups.RepositoryActions;
using Backups.Tools;
using Newtonsoft.Json;
using Utility.Extensions;

namespace Backups.Tcp.RepositoryActions
{
    [Serializable]
    public class ExistsRepositoryAction : IRepositoryAction<bool>
    {
        [JsonConstructor]
        public ExistsRepositoryAction(string path)
        {
            Path = path.ThrowIfNull(nameof(path));
        }

        public string Path { get; }

        public bool Execute(Repository repository, ILogger? logger)
        {
            repository.ThrowIfNull(nameof(repository));

            return repository.Exists(Path);
        }
    }
}