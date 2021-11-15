using Backups.Entities;
using Backups.JobObjects;
using Backups.Models;
using Backups.Storages;
using Backups.Tools;
using Utility.Extensions;

namespace BackupsExtra.Entities
{
    public abstract class RestoreJob
    {
        private readonly BackupJobConfiguration _configuration;
        private readonly RestorePoint _restorePoint;
        private readonly ILogger? _logger;

        internal RestoreJob(BackupJobConfiguration configuration, RestorePoint restorePoint, ILogger? logger)
        {
            _configuration = configuration.ThrowIfNull(nameof(configuration));
            _restorePoint = restorePoint.ThrowIfNull(nameof(_restorePoint));
            _logger = logger;
        }

        public void Execute()
        {
            using IStorage storage = _configuration.StorageAlgorithm.Load(_restorePoint, _logger);

            foreach (IJobObject obj in _restorePoint.Objects)
            {
                using Package package = _configuration.StorageAlgorithm.Extract(storage, obj, _configuration.Packer, _logger);
                Write(obj, package);
            }
        }

        protected abstract void Write(IJobObject obj, Package package);
    }
}