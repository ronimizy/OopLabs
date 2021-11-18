using System;
using System.Linq;
using Backups.Entities;
using Backups.JobObjects;
using Backups.Models;
using Backups.Packers;
using Backups.Storages;
using Backups.Tools;
using Utility.Extensions;

namespace Backups.StorageAlgorithms
{
    [Serializable]
    public class SplitStorageStorageAlgorithm : IStorageAlgorithm
    {
        public IStorage Pack(RestorePointModel point, IPacker packer, ILogger? logger)
        {
            point.ThrowIfNull(nameof(point));
            packer.ThrowIfNull(nameof(packer));

            Package[] packages = point.Packages
                .Select(p => packer.Pack(p.Name, logger, p))
                .ToArray();
            logger?.OnMessage($"{nameof(SplitStorageStorageAlgorithm)} packed given objects");

            return new SplitStorage(packages);
        }

        public IStorage Load(RestorePoint restorePoint, ILogger? logger)
        {
            restorePoint.ThrowIfNull(nameof(restorePoint));

            Package[] packages = restorePoint.Objects
                .Select(o => new Package(o.Name, restorePoint.Repository.GetStream(o.Name)))
                .ToArray();
            logger?.OnMessage($"{nameof(SplitStorageStorageAlgorithm)} loaded given restore point storage");
            return new SplitStorage(packages);
        }

        public Package Extract(IStorage storage, IJobObject obj, IPacker packer, ILogger? logger)
        {
            storage.ThrowIfNull(nameof(storage));
            obj.ThrowIfNull(nameof(obj));
            packer.ThrowIfNull(nameof(packer));

            if (storage is not SplitStorage splitStorage)
                throw BackupsExceptionFactory.InvalidStorageType(typeof(SplitStorage), storage);

            return splitStorage.Packages
                .SingleOrDefault(p => p.Name.Equals(obj.Name))
                .ThrowIfNull(BackupsExceptionFactory.JobObjectNotFoundInStorage(obj));
        }
    }
}