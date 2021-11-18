using System.Collections.Generic;
using System.Linq;
using Backups.Models;
using Backups.Repositories;
using Backups.Tools;
using Utility.Extensions;

namespace Backups.Storages
{
    public sealed class SplitStorage : IStorage
    {
        private readonly List<Package> _packages;

        public SplitStorage(IReadOnlyCollection<Package> packages)
        {
            _packages = packages.ThrowIfNull(nameof(packages)).ToList();
        }

        public IReadOnlyCollection<Package> Packages => _packages;

        public void Dispose()
            => _packages.ForEach(p => p.Dispose());

        public void WriteTo(Repository repository, ILogger? logger)
        {
            repository.ThrowIfNull(nameof(repository));

            logger?.OnMessage($"Writing split storage to {repository}");
            _packages.ForEach(p => repository.Write(p.Name, p.Stream));
        }
    }
}