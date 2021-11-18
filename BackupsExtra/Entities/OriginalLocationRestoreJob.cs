using System.IO;
using Backups.Entities;
using Backups.JobObjects;
using Backups.Models;
using Backups.Tools;

namespace BackupsExtra.Entities
{
    public class OriginalLocationRestoreJob : RestoreJob
    {
        public OriginalLocationRestoreJob(BackupJobConfiguration configuration, RestorePoint restorePoint, ILogger? logger = null)
            : base(configuration, restorePoint, logger) { }

        protected override void Write(IJobObject obj, Package package)
        {
            string path = $"{Path.GetDirectoryName(obj.FullPath)}{BackupConfiguration.PathDelimiter}{package.Name}";
            obj.Repository.Write(path, package.Stream);
        }
    }
}