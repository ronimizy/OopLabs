using Backups.Packers;

namespace Backups.BackupJobBuilder
{
    public interface IJobPackerPicker
    {
        IJobPackingAlgorithmPicker PackingWith(IPacker packer);
    }
}