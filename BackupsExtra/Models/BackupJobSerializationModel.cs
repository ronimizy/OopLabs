using System.Collections.Generic;
using Backups.Chronometers;
using Backups.Models;
using Backups.Packers;
using Backups.Repositories;
using Backups.RestorePointFilters;
using Backups.RestorePointMatchers;
using Backups.StorageAlgorithms;
using Backups.Tools;
using Newtonsoft.Json;
using Utility.Extensions;

namespace BackupsExtra.Models
{
    public class BackupJobSerializationModel
    {
        public BackupJobSerializationModel(
            BackupJobConfiguration configuration,
            IReadOnlyCollection<JobObjectConfiguration> jobObjectConfigurations,
            IReadOnlyCollection<RestorePointSerializationModel> restorePoints)
        {
            configuration.ThrowIfNull(nameof(configuration));

            Name = configuration.Name;
            JobObjectConfigurations = jobObjectConfigurations.ThrowIfNull(nameof(jobObjectConfigurations));
            Packer = new SerializedConfiguration<IPacker>(configuration.Packer);
            PackingAlgorithm = new SerializedConfiguration<IStorageAlgorithm>(configuration.StorageAlgorithm);
            WritingRepository = new SerializedConfiguration<Repository>(configuration.WritingRepository);
            RestorePointFilter = configuration.RestorePointFilter is null
                ? null
                : new SerializedConfiguration<IRestorePointFilter>(configuration.RestorePointFilter);
            RestorePointMatcher = configuration.RestorePointMatcher is null
                ? null
                : new SerializedConfiguration<IRestorePointMatcher>(configuration.RestorePointMatcher);
            RestorePoints = restorePoints;
        }

        [JsonConstructor]
        private BackupJobSerializationModel(
            string name,
            SerializedConfiguration<IPacker> packer,
            SerializedConfiguration<IStorageAlgorithm> packingAlgorithm,
            SerializedConfiguration<Repository> writingRepository,
            SerializedConfiguration<IRestorePointFilter>? restorePointFilter,
            SerializedConfiguration<IRestorePointMatcher>? restorePointMatcher,
            IReadOnlyCollection<JobObjectConfiguration> jobObjectConfigurations,
            IReadOnlyCollection<RestorePointSerializationModel> restorePoints)
        {
            Name = name.ThrowIfNull(nameof(name));
            Packer = packer.ThrowIfNull(nameof(packer));
            PackingAlgorithm = packingAlgorithm.ThrowIfNull(nameof(packingAlgorithm));
            WritingRepository = writingRepository.ThrowIfNull(nameof(writingRepository));
            RestorePointFilter = restorePointFilter.ThrowIfNull(nameof(restorePointFilter));
            RestorePointMatcher = restorePointMatcher.ThrowIfNull(nameof(restorePointMatcher));
            JobObjectConfigurations = jobObjectConfigurations.ThrowIfNull(nameof(jobObjectConfigurations));
            RestorePoints = restorePoints.ThrowIfNull(nameof(restorePoints));
        }

        public string Name { get; }
        public SerializedConfiguration<IPacker> Packer { get; }
        public SerializedConfiguration<IStorageAlgorithm> PackingAlgorithm { get; }
        public SerializedConfiguration<Repository> WritingRepository { get; }
        public SerializedConfiguration<IRestorePointFilter>? RestorePointFilter { get; }
        public SerializedConfiguration<IRestorePointMatcher>? RestorePointMatcher { get; }
        public IReadOnlyCollection<JobObjectConfiguration> JobObjectConfigurations { get; }
        public IReadOnlyCollection<RestorePointSerializationModel> RestorePoints { get; }

        public BackupJobConfiguration ToConfiguration(IChronometer chronometer, ILogger? logger)
        {
            chronometer.ThrowIfNull(nameof(chronometer));

            return new BackupJobConfiguration(
                Name,
                Packer.Deserialize(),
                PackingAlgorithm.Deserialize(),
                WritingRepository.Deserialize(),
                RestorePointFilter?.Deserialize(),
                RestorePointMatcher?.Deserialize(),
                chronometer.ThrowIfNull(nameof(chronometer)),
                logger);
        }
    }
}