using Backups.Entities;

namespace Backups.BackupJobBuilder
{
    public interface IBackupJobBuilder
    {
        BackupJob Build();
    }
}