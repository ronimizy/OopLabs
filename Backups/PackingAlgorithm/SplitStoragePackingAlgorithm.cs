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
    public class SplitStoragePackingAlgorithm : IPackingAlgorithm
    {
        [JsonConstructor]
        public SplitStoragePackingAlgorithm(IPacker packer)
        {
            Packer = packer;
        }

        public IPacker Packer { get; }

        public IReadOnlyCollection<Package> Pack(string name, IReadOnlyCollection<IJobObject> objects, ILogger? logger)
        {
            Package[] packages = objects
                .Select(o => o.GetPackage())
                .Select(p => Packer.Pack(p.Name, logger, p))
                .ToArray();
            logger?.OnMessage($"{nameof(SingleStoragePackingAlgorithm)} packed given objects");

            return packages;
        }
    }
}