using System;
using System.Collections.Generic;
using System.Linq;
using Backups.BackupJobBuilder;
using Backups.Chronometers;
using Backups.JobObjects;
using Backups.Models;
using Backups.PackingAlgorithm;
using Backups.Repositories;
using Backups.RepositoryActions;
using Backups.RestorePointFilters;
using Backups.RestorePointMatchers;
using Backups.Tools;
using Microsoft.Extensions.DependencyInjection;

namespace Backups.Entities
{
    public class BackupJob
    {
        private readonly List<IJobObject> _objects = new List<IJobObject>();
        private readonly Backup _backup;
        private readonly IPackingAlgorithm _packingAlgorithm;
        private readonly IChronometer _chronometer;
        private readonly Repository _writeRepository;
        private readonly IRestorePointFilter? _restorePointFilter;
        private readonly IRestorePointMatcher? _restorePointMatcher;
        private readonly ILogger? _logger;

        private readonly IServiceProvider _provider;

        internal BackupJob(string name, IServiceProvider provider)
        {
            Name = name;
            _provider = provider;
            _packingAlgorithm = provider.GetRequiredService<IPackingAlgorithm>();
            _chronometer = provider.GetRequiredService<IChronometer>();
            _writeRepository = provider.GetRequiredService<Repository>();
            _restorePointFilter = provider.GetService<IRestorePointFilter>();
            _restorePointMatcher = provider.GetService<IRestorePointMatcher>();
            _logger = provider.GetService<ILogger>();
            _backup = new Backup(_logger);

            if (_restorePointFilter is null == _restorePointMatcher is null)
                return;

            BackupsException exception = BackupsExceptionFactory.InvalidFilterMatcherNullability(_restorePointFilter, _restorePointMatcher);
            _logger?.OnException(exception);
            throw exception;
        }

        public static IJobNamePicker Build => new BackupJobBuilder.BackupJobBuilder();

        public string Name { get; }
        public IReadOnlyCollection<IJobObject> Objects => _objects;
        public IReadOnlyCollection<RestorePoint> Points => _backup.RestorePoints;

        public void AddObjects(params JobObjectBuilder[] jobObjects)
        {
            IJobObject[] createdObjects = jobObjects
                .Select(o => o.Create(_provider))
                .ToArray();

            _objects.AddRange(createdObjects);

            _logger?.OnMessage(
                $"{createdObjects.Length} job objects has been added to the BackupJob: {this}.",
                string.Join("\n", createdObjects.Select(o => o.ToString())));
        }

        public void RemoveObjects(params IJobObject[] jobObjects)
        {
            Exception[] exceptions = jobObjects
                .Where(o => !_objects.Contains(o))
                .Select(o => (Exception)BackupsExceptionFactory.ObjectIsNotBeingTracked(this, o))
                .ToArray();

            if (exceptions.Any())
            {
                var exception = new AggregateException(exceptions);
                _logger?.OnException(exception);
                _logger?.OnComment($"The total count of untracked objects is {exceptions.Length}");
                throw exception;
            }

            foreach (IJobObject jobObject in jobObjects)
            {
                _objects.Remove(jobObject);
            }

            _logger?.OnMessage(
                $"{jobObjects.Length} job objects has been removed to the BackupJob: {this}.",
                string.Join("\n", jobObjects.Select(o => o.ToString())));
        }

        public void Execute()
        {
            _logger?.OnMessage($"BackupJob: {this} started execution.");

            DateTime createdTime = _chronometer.GetCurrentTime();
            string packageName = BackupConfiguration.FormatDateTime(createdTime);
            _logger?.OnComment($"BackupJob: {this} created restore point created at {packageName}.");

            IReadOnlyCollection<Package> packages = _packingAlgorithm.Pack(packageName, _objects, _logger);
            _logger?.OnMessage($"BackupJob: {this} packed its objects.");

            _writeRepository.ExecuteAction(new SendPackagesAction($"{Name}{BackupConfiguration.PathDelimiter}{packageName}", packages));
            _logger?.OnMessage($"BackupJob: {this} written to Repository: {_writeRepository}.");

            var restorePoint = new RestorePoint(createdTime, _objects);
            _backup.AddPoints(restorePoint);
            _logger?.OnMessage($"BackupJob: {this} added new RestorePoint {restorePoint} to the Backup.");

            if (_restorePointFilter is not null)
            {
                _logger?.OnMessage($"BackupJob: {this} started restore point filtering.");
                _restorePointFilter.Filter(_backup, _restorePointMatcher!, _packingAlgorithm, _writeRepository, _logger);
                _logger?.OnMessage($"BackupJob: {this} finished restore point filtering");
            }

            foreach (IJobObject jobObject in _objects)
            {
                jobObject.IncrementVersion();
            }

            _logger?.OnComment($"BackupJob: {this} incremented versions of its objects");
        }

        public override string ToString()
            => Name;
    }
}