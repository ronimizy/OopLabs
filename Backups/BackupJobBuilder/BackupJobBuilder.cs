using System;
using Backups.Chronometers;
using Backups.Entities;
using Backups.PackingAlgorithm;
using Backups.Repositories;
using Backups.RestorePointFilters;
using Backups.RestorePointMatchers;
using Backups.Tools;
using Microsoft.Extensions.DependencyInjection;
using Utility.Extensions;

namespace Backups.BackupJobBuilder
{
    internal class BackupJobBuilder : IJobNamePicker,
                                      IJobPackingAlgorithmPicker,
                                      IJobChronometerPicker,
                                      IJobWritingRepositoryPicker,
                                      IJobOptionalParameterPicker
    {
        private readonly IServiceCollection _serviceCollection = new ServiceCollection();
        private string? _name;

        public IJobPackingAlgorithmPicker Called(string name)
        {
            _name = name.ThrowIfNull(nameof(name));
            return this;
        }

        public IJobChronometerPicker UsingAlgorithm(IPackingAlgorithm algorithm)
        {
            algorithm.ThrowIfNull(nameof(algorithm));
            _serviceCollection.AddSingleton(algorithm);
            return this;
        }

        public IJobWritingRepositoryPicker TrackingTimeWith(IChronometer chronometer)
        {
            chronometer.ThrowIfNull(nameof(chronometer));
            _serviceCollection.AddSingleton(chronometer);
            return this;
        }

        public IJobOptionalParameterPicker WritingTo(Repository repository)
        {
            repository.ThrowIfNull(nameof(repository));
            _serviceCollection.AddSingleton(repository);
            return this;
        }

        public IJobOptionalParameterPicker WithRestorePointFilteringPolicy(IRestorePointFilter filter, IRestorePointMatcher matcher)
        {
            filter.ThrowIfNull(nameof(filter));
            matcher.ThrowIfNull(nameof(matcher));

            _serviceCollection.AddSingleton(filter);
            _serviceCollection.AddSingleton(matcher);
            return this;
        }

        public IJobOptionalParameterPicker LoggingWith(ILogger logger)
        {
            logger.ThrowIfNull(nameof(logger));

            _serviceCollection.AddSingleton(logger);
            return this;
        }

        public BackupJob Build()
        {
            IServiceProvider provider = _serviceCollection.BuildServiceProvider();
            return new BackupJob(_name.ThrowIfNull(nameof(_name)), provider);
        }
    }
}