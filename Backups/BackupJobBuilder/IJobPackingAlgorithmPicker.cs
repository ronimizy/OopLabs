using Backups.PackingAlgorithm;

namespace Backups.BackupJobBuilder
{
    public interface IJobPackingAlgorithmPicker
    {
        IJobChronometerPicker UsingAlgorithm(IPackingAlgorithm algorithm);
    }
}