using Backups.Packers;
using Backups.PackingAlgorithm;
using Backups.Repositories;
using Backups.RepositoryActions;
using Backups.Tcp.RepositoryActions;
using Backups.Tcp.Tools;
using Utility.Extensions;

namespace Backups.Tcp.Extensions
{
    public static class TypeLocatorExtension
    {
        public static TypeLocator AddRepositoryActions(this TypeLocator locator)
        {
            locator.ThrowIfNull(nameof(locator));

            locator.Add(typeof(SendPackagesAction));
            locator.Add(typeof(DeleteRepositoryAction));
            locator.Add(typeof(ExistsRepositoryAction));
            locator.Add(typeof(GetContentsOfRepositoryAction));
            locator.Add(typeof(GetStreamRepositoryAction));
            locator.Add(typeof(IsFolderRepositoryAction));
            locator.Add(typeof(WriteRepositoryAction));

            return locator;
        }

        public static TypeLocator AddPackingAlgorithms(this TypeLocator locator)
        {
            locator.ThrowIfNull(nameof(locator));

            locator.Add(typeof(SplitStoragePackingAlgorithm));
            locator.Add(typeof(SingleStoragePackingAlgorithm));

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