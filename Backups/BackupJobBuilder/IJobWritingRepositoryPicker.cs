using Backups.Repositories;

namespace Backups.BackupJobBuilder
{
    public interface IJobWritingRepositoryPicker
    {
        IJobOptionalParameterPicker WritingTo(Repository repository);
    }
}