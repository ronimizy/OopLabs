using System;
using System.IO;
using System.Text.Json.Serialization;
using Backups.Models;
using Backups.Repositories;
using Backups.RepositoryActions;
using Backups.Tools;
using Utility.Extensions;

namespace Backups.Tcp.RepositoryActions
{
    [Serializable]
    public class WriteRepositoryAction : IRepositoryAction<object?>
    {
        [JsonConstructor]
        public WriteRepositoryAction(string path, Stream data)
        {
            Package = new Package(
                path.ThrowIfNull(nameof(path)),
                data.ThrowIfNull(nameof(data)));
        }

        public Package Package { get; }

        public object? Execute(Repository repository, ILogger? logger)
        {
            repository.ThrowIfNull(nameof(repository));

            repository.Write(Package.Name, Package.Stream);
            return null;
        }
    }
}