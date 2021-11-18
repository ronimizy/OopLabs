using Backups.Entities;
using Backups.JobObjects;
using Backups.Models;
using Backups.Repositories;
using Backups.Tools;
using Utility.Extensions;

namespace BackupsExtra.Entities
{
    public class CustomLocationRestoreJob : RestoreJob
    {
        private readonly Repository _repository;

        public CustomLocationRestoreJob(
            BackupJobConfiguration configuration,
            RestorePoint restorePoint,
            Repository repository,
            ILogger? logger = null)
            : base(configuration, restorePoint, logger)
        {
            _repository = repository.ThrowIfNull(nameof(repository));
        }

        protected override void Write(IJobObject obj, Package package)
            => _repository.Write(obj.Name, package.Stream);
    }
}