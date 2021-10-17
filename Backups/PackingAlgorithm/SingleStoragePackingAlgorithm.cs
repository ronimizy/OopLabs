using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Backups.JobObjects;
using Backups.Models;
using Backups.Packers;
using Backups.Tools;

namespace Backups.PackingAlgorithm
{
    [Serializable]
    public class SingleStoragePackingAlgorithm : IPackingAlgorithm
    {
        [JsonConstructor]
        public SingleStoragePackingAlgorithm(IPacker packer)
        {
            Packer = packer;
        }

        public IPacker Packer { get; }

        public IReadOnlyCollection<Package> Pack(string name, IReadOnlyCollection<IJobObject> objects, ILogger? logger)
        {
            Package[] packages = objects
                .Select(o => o.GetPackage())
                .ToArray();
            logger?.OnMessage($"{nameof(SingleStoragePackingAlgorithm)} packed given objects");

            Package package = Packer.Pack(name, logger, packages);
            logger?.OnMessage($"{nameof(SingleStoragePackingAlgorithm)} created a single package for given objects");

            return new[] { package };
        }
    }
}