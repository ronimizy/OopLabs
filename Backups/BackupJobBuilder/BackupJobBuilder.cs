using System;
using Backups.Chronometers;
using Backups.Entities;
using Backups.JobObjects;
using Backups.Models;
using Backups.Packers;
using Backups.Repositories;
using Backups.RestorePointFilters;
using Backups.RestorePointMatchers;
using Backups.StorageAlgorithms;
using Backups.Tools;
using Utility.Extensions;

namespace Backups.BackupJobBuilder
{
    internal class BackupJobBuilder : IJobNamePicker,
                                      IJobPackerPicker,
                                      IJobPackingAlgorithmPicker,
                                      IJobChronometerPicker,
                                      IJobWritingRepositoryPicker,
                                      IJobOptionalParameterPicker
    {
        private string? _name;
        private IPacker? _packer;
        private IStorageAlgorithm? _packingAlgorithm;
        private IChronometer? _chronometer;
        private Repository? _writingRepository;
        private IRestorePointFilter? _restorePointFilter;
        private IRestorePointMatcher? _restorePointMatcher;
        private ILogger? _logger;

        public IJobPackerPicker Called(string name)
        {
            _name = name.ThrowIfNull(nameof(name));
            return this;
        }

        public IJobPackingAlgorithmPicker PackingWith(IPacker packer)
        {
            _packer = packer.ThrowIfNull(nameof(packer));
            return this;
        }

        public IJobChronometerPicker UsingAlgorithm(IStorageAlgorithm algorithm)
        {
            _packingAlgorithm = algorithm.ThrowIfNull(nameof(algorithm));
            return this;
        }

        public IJobWritingRepositoryPicker TrackingTimeWith(IChronometer chronometer)
        {
            _chronometer = chronometer.ThrowIfNull(nameof(chronometer));
            return this;
        }

        public IJobOptionalParameterPicker WritingTo(Repository repository)
        {
            _writingRepository = repository.ThrowIfNull(nameof(repository));
            return this;
        }

        public IJobOptionalParameterPicker WithRestorePointFilteringPolicy(IRestorePointFilter filter, IRestorePointMatcher matcher)
        {
            _restorePointFilter = filter.ThrowIfNull(nameof(filter));
            _restorePointMatcher = matcher.ThrowIfNull(nameof(matcher));
            return this;
        }

        public IJobOptionalParameterPicker LoggingWith(ILogger logger)
        {
            _logger = logger.ThrowIfNull(nameof(logger));
            return this;
        }

        public BackupJob Build()
        {
            var configuration = new BackupJobConfiguration(
                _name.ThrowIfNull(nameof(_name)),
                _packer.ThrowIfNull(nameof(_packer)),
                _packingAlgorithm.ThrowIfNull(nameof(_packingAlgorithm)),
                _writingRepository.ThrowIfNull(nameof(_writingRepository)),
                _restorePointFilter,
                _restorePointMatcher,
                _chronometer.ThrowIfNull(nameof(_chronometer)),
                _logger);

            return new BackupJob(configuration, Array.Empty<IJobObject>(), Array.Empty<RestorePoint>());
        }
    }
}