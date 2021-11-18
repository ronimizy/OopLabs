using System.Collections.Generic;
using System.Linq;
using Backups.Entities;
using Utility.Extensions;

namespace Backups.Models
{
    public class RestorePointModel
    {
        public RestorePointModel(RestorePoint point)
        {
            point.ThrowIfNull(nameof(point));

            Name = point.ToString();
            Packages = point.Objects
                .Select(o => new Package(o.Name, o.GetStream()))
                .ToList();
        }

        public RestorePointModel(string name, IReadOnlyCollection<Package> packages)
        {
            Name = name.ThrowIfNull(nameof(name));
            Packages = packages.ThrowIfNull(nameof(packages));
        }

        public string Name { get; }
        public IReadOnlyCollection<Package> Packages { get; }
    }
}