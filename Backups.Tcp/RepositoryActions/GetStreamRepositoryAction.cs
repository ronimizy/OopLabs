using System;
using System.IO;
using Backups.Repositories;
using Backups.RepositoryActions;
using Backups.Tools;
using Newtonsoft.Json;
using Utility.Extensions;

namespace Backups.Tcp.RepositoryActions
{
    [Serializable]
    public class GetStreamRepositoryAction : IRepositoryAction<Stream>
    {
        [JsonConstructor]
        public GetStreamRepositoryAction(string path)
        {
            Path = path.ThrowIfNull(nameof(path));
        }

        public string Path { get; }

        public Stream Execute(Repository repository, ILogger? logger)
        {
            repository.ThrowIfNull(nameof(repository));

            return repository.GetStream(Path);
        }
    }
}