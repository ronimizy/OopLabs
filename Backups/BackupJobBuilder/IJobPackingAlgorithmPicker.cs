using Backups.StorageAlgorithms;

namespace Backups.BackupJobBuilder
{
    public interface IJobPackingAlgorithmPicker
    {
        IJobChronometerPicker UsingAlgorithm(IStorageAlgorithm algorithm);
    }
}