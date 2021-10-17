using Backups.Chronometers;
using Backups.Tools;

namespace Backups.BackupJobBuilder
{
    public interface IJobChronometerPicker
    {
        IJobWritingRepositoryPicker TrackingTimeWith(IChronometer chronometer);
    }
}