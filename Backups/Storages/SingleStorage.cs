using Backups.Models;
using Backups.Repositories;
using Backups.Tools;
using Utility.Extensions;

namespace Backups.Storages
{
    public sealed class SingleStorage : IStorage
    {
        public SingleStorage(Package package)
        {
            Package = package;
        }

        public Package Package { get; }

        public void Dispose()
            => Package.Dispose();

        public void WriteTo(Repository repository, ILogger? logger)
        {
            repository.ThrowIfNull(nameof(repository));

            logger?.OnMessage($"Writing single storage to {repository}");
            repository.Write(Package.Name, Package.Stream);
        }
    }
}