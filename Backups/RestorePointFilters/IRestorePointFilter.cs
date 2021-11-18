using Backups.Entities;
using Backups.Packers;
using Backups.Repositories;
using Backups.RestorePointMatchers;
using Backups.StorageAlgorithms;
using Backups.Tools;

namespace Backups.RestorePointFilters
{
    public interface IRestorePointFilter
    {
        void Filter(
            Backup backup,
            IRestorePointMatcher matcher,
            IStorageAlgorithm algorithm,
            IPacker packer,
            Repository repository,
            ILogger? logger = null);
    }
}