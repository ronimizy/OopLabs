using Backups.Packers;
using Backups.Repositories;
using Backups.StorageAlgorithms;
using Backups.Tools;
using Utility.Extensions;

namespace Backups.Extensions
{
    public static class TypeLocatorExtensions
    {
        public static TypeLocator AddPackingAlgorithms(this TypeLocator locator)
        {
            locator.ThrowIfNull(nameof(locator));

            locator.Add(typeof(SplitStorageStorageAlgorithm));
            locator.Add(typeof(SingleStorageStorageAlgorithm));

            return locator;
        }

        public static TypeLocator AddRepositories(this TypeLocator locator)
        {
            locator.ThrowIfNull(nameof(locator));

            locator.Add(typeof(FileSystemRepository));

            return locator;
        }

        public static TypeLocator AddPackers(this TypeLocator locator)
        {
            locator.ThrowIfNull(nameof(locator));

            locator.Add(typeof(ZipPacker));

            return locator;
        }
    }
}