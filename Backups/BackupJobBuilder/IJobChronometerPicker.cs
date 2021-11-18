using Backups.Chronometers;

namespace Backups.BackupJobBuilder
{
    public interface IJobChronometerPicker
    {
        IJobWritingRepositoryPicker TrackingTimeWith(IChronometer chronometer);
    }
}