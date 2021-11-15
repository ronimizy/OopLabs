using System;
using System.IO;
using Backups.Models;
using Backups.Repositories;
using Backups.RepositoryActions;
using Backups.Tools;
using Newtonsoft.Json;
using Utility.Extensions;

namespace Backups.Tcp.RepositoryActions
{
    [Serializable]
    public class WriteRepositoryAction : IRepositoryAction<object?>
    {
        public WriteRepositoryAction(string path, Stream data)
        {
            Package = new Package(
                path.ThrowIfNull(nameof(path)),
                data.ThrowIfNull(nameof(data)));
        }

        [JsonConstructor]
        private WriteRepositoryAction(Package package)
        {
            Package = package;
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