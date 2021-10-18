using System.Collections.Generic;
using Backups.Entities;
using Backups.PackingAlgorithm;
using Backups.Repositories;
using Backups.RestorePointMatchers;
using Backups.Tools;

namespace Backups.RestorePointFilters
{
    public interface IRestorePointFilter
    {
        IReadOnlyCollection<RestorePoint> Filter(
            Backup backup,
            IRestorePointMatcher matcher,
            IPackingAlgorithm algorithm,
            Repository repository,
            ILogger? logger = null);
    }
}