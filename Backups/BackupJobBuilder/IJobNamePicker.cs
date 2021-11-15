namespace Backups.BackupJobBuilder
{
    public interface IJobNamePicker
    {
        IJobPackerPicker Called(string name);
    }
}