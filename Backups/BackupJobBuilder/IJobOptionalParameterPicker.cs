using Backups.RestorePointFilters;
using Backups.RestorePointMatchers;
using Backups.Tools;

namespace Backups.BackupJobBuilder
{
    public interface IJobOptionalParameterPicker : IBackupJobBuilder
    {
        IJobOptionalParameterPicker WithRestorePointFilteringPolicy(IRestorePointFilter filter, IRestorePointMatcher matcher);
        IJobOptionalParameterPicker LoggingWith(ILogger logger);
    }
}