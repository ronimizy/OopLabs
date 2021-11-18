using Backups.Chronometers;
using Backups.Packers;
using Backups.Repositories;
using Backups.RestorePointFilters;
using Backups.RestorePointMatchers;
using Backups.StorageAlgorithms;
using Backups.Tools;
using Utility.Extensions;

namespace Backups.Models
{
    public record BackupJobConfiguration
    {
        public BackupJobConfiguration(
            string name,
            IPacker packer,
            IStorageAlgorithm storageAlgorithm,
            Repository writingRepository,
            IRestorePointFilter? restorePointFilter,
            IRestorePointMatcher? restorePointMatcher,
            IChronometer chronometer,
            ILogger? logger)
        {
            Name = name.ThrowIfNull(nameof(name));
            Packer = packer.ThrowIfNull(nameof(packer));
            StorageAlgorithm = storageAlgorithm.ThrowIfNull(nameof(storageAlgorithm));
            WritingRepository = writingRepository.ThrowIfNull(nameof(writingRepository));
            RestorePointFilter = restorePointFilter;
            RestorePointMatcher = restorePointMatcher;
            Chronometer = chronometer.ThrowIfNull(nameof(chronometer));
            Logger = logger;

            if (restorePointFilter is null == restorePointMatcher is null)
                return;

            BackupsException exception = BackupsExceptionFactory.InvalidFilterMatcherNullability(restorePointFilter, restorePointMatcher);
            logger?.OnException(exception);
            throw exception;
        }

        public string Name { get; }
        public IPacker Packer { get; }
        public IStorageAlgorithm StorageAlgorithm { get; }
        public Repository WritingRepository { get; }
        public IRestorePointFilter? RestorePointFilter { get; }
        public IRestorePointMatcher? RestorePointMatcher { get; }
        public IChronometer Chronometer { get; }
        public ILogger? Logger { get; }
    }
}