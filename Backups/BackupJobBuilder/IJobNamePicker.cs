namespace Backups.BackupJobBuilder
{
    public interface IJobNamePicker
    {
        IJobPackingAlgorithmPicker Called(string name);
    }
}