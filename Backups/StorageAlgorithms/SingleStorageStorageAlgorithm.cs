using System.IO;
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
    public class SingleStorageStorageAlgorithm : IStorageAlgorithm
    {
        public IStorage Pack(RestorePointModel point, IPacker packer, ILogger? logger)
        {
            point.ThrowIfNull(nameof(point));
            packer.ThrowIfNull(nameof(packer));

            Package package = packer.Pack(point.Name, logger, point.Packages.ToArray());
            logger?.OnMessage($"{nameof(SingleStorageStorageAlgorithm)} created a single package for given objects");

            return new SingleStorage(package);
        }

        public IStorage Load(RestorePoint restorePoint, ILogger? logger)
        {
            restorePoint.ThrowIfNull(nameof(restorePoint));

            Stream stream = restorePoint.Repository.GetStream(restorePoint.ToString());
            logger?.OnMessage($"{nameof(SingleStorageStorageAlgorithm)} loaded given restore point");
            return new SingleStorage(new Package(restorePoint.ToString(), stream));
        }

        public Package Extract(IStorage storage, IJobObject obj, IPacker packer, ILogger? logger)
        {
            storage.ThrowIfNull(nameof(storage));
            obj.ThrowIfNull(nameof(obj));
            packer.ThrowIfNull(nameof(packer));

            if (storage is not SingleStorage singleStorage)
                throw BackupsExceptionFactory.InvalidStorageType(typeof(SingleStorage), storage);

            return packer.Extract(singleStorage.Package, obj.Name, logger);
        }
    }
}