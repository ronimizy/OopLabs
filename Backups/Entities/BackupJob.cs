using System;
using System.Collections.Generic;
using System.Linq;
using Backups.BackupJobBuilder;
using Backups.JobObjects;
using Backups.Models;
using Backups.Repositories;
using Backups.Storages;
using Backups.Tools;
using Utility.Extensions;

namespace Backups.Entities
{
    public class BackupJob
    {
        private readonly List<IJobObject> _objects;
        private readonly Backup _backup;

        public BackupJob(
            BackupJobConfiguration configuration,
            IReadOnlyCollection<IJobObject> objects,
            IReadOnlyCollection<RestorePoint> restorePoints)
        {
            Configuration = configuration.ThrowIfNull(nameof(configuration));
            _objects = objects.ToList();
            _backup = new Backup(restorePoints.ThrowIfNull(nameof(restorePoints)), configuration.Logger);
        }

        public static IJobNamePicker Build => new BackupJobBuilder.BackupJobBuilder();

        public string Name => Configuration.Name;
        public IReadOnlyCollection<IJobObject> Objects => _objects;
        public IReadOnlyCollection<RestorePoint> Points => _backup.RestorePoints;

        public BackupJobConfiguration Configuration { get; }

        public void AddObjects(params JobObjectBuilder[] jobObjects)
        {
            IJobObject[] createdObjects = jobObjects
                .Select(o => o.Build(Configuration))
                .ToArray();

            _objects.AddRange(createdObjects);

            Configuration.Logger?.OnMessage(
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
                Configuration.Logger?.OnException(exception);
                Configuration.Logger?.OnComment($"The total count of untracked objects is {exceptions.Length}");
                throw exception;
            }

            foreach (IJobObject jobObject in jobObjects)
            {
                _objects.Remove(jobObject);
            }

            Configuration.Logger?.OnMessage(
                $"{jobObjects.Length} job objects has been removed to the BackupJob: {this}.",
                string.Join("\n", jobObjects.Select(o => o.ToString())));
        }

        public void Execute()
        {
            Configuration.Logger?.OnMessage($"BackupJob: {this} started execution.");

            DateTime createdTime = Configuration.Chronometer.GetCurrentTime();
            string packageName = BackupConfiguration.FormatDateTime(createdTime);

            Repository jobRepository = Configuration.WritingRepository.GetSubRepositoryAt($"{Name}");
            Repository pointRepository = jobRepository.GetSubRepositoryAt($"{packageName}");
            var restorePoint = new RestorePoint(pointRepository, createdTime, _objects);
            Configuration.Logger?.OnComment($"BackupJob: {this} created restore point created at {packageName}.");

            using IStorage storage = Configuration.StorageAlgorithm.Pack(new RestorePointModel(restorePoint), Configuration.Packer, Configuration.Logger);
            Configuration.Logger?.OnMessage($"BackupJob: {this} packed its objects.");

            storage.WriteTo(pointRepository, Configuration.Logger);
            Configuration.Logger?.OnMessage($"BackupJob: {this} written to Repository: {Configuration.WritingRepository}.");

            _backup.AddPoints(restorePoint);
            Configuration.Logger?.OnMessage($"BackupJob: {this} added new RestorePoint {restorePoint} to the Backup.");

            if (Configuration.RestorePointFilter is null || Configuration.RestorePointMatcher is null)
                return;

            Configuration.Logger?.OnMessage($"BackupJob: {this} started restore point filtering.");
            Configuration.RestorePointFilter.Filter(
                _backup,
                Configuration.RestorePointMatcher,
                Configuration.StorageAlgorithm,
                Configuration.Packer,
                jobRepository,
                Configuration.Logger);
            Configuration.Logger?.OnMessage($"BackupJob: {this} finished restore point filtering");
        }

        public override string ToString()
            => Name;
    }
}