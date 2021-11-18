using System.IO;
using Backups.Models;
using Backups.Tools;

namespace Backups.Packers
{
    public interface IPacker
    {
        Package Pack(string packageName, ILogger? logger, params Package[] packages);
        Package Extract(Package package, string subPackageName, ILogger? logger);
        void AddToPackage(Stream package, ILogger? logger, params Package[] packages);
        void RemoveFromPackage(Stream package, ILogger? logger, params string[] packages);
    }
}