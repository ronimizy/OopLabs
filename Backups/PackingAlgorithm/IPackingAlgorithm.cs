using System.Collections.Generic;
using Backups.JobObjects;
using Backups.Models;
using Backups.Packers;
using Backups.Tools;

namespace Backups.PackingAlgorithm
{
    public interface IPackingAlgorithm
    {
        IPacker Packer { get; }
        IReadOnlyCollection<Package> Pack(string name, IReadOnlyCollection<IJobObject> objects, ILogger? logger);
    }
}