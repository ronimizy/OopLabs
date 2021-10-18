using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Backups.Repositories;
using Backups.RepositoryActions;
using Backups.Tools;
using Utility.Extensions;

namespace Backups.Tcp.RepositoryActions
{
    [Serializable]
    public class GetContentsOfRepositoryAction : IRepositoryAction<IReadOnlyCollection<string>>
    {
        [JsonConstructor]
        public GetContentsOfRepositoryAction(string path)
        {
            Path = path.ThrowIfNull(nameof(path));
        }

        public string Path { get; }

        public IReadOnlyCollection<string> Execute(Repository repository, ILogger? logger)
        {
            repository.ThrowIfNull(nameof(repository));

            return repository.GetContentsOf(Path);
        }
    }
}