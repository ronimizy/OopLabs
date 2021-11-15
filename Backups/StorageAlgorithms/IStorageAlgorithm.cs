using Backups.Entities;
using Backups.JobObjects;
using Backups.Models;
using Backups.Packers;
using Backups.Storages;
using Backups.Tools;

namespace Backups.StorageAlgorithms
{
    public interface IStorageAlgorithm
    {
        IStorage Pack(RestorePointModel point, IPacker packer, ILogger? logger);
        IStorage Load(RestorePoint restorePoint, ILogger? logger);
        Package Extract(IStorage storage, IJobObject obj, IPacker packer, ILogger? logger);
    }
}