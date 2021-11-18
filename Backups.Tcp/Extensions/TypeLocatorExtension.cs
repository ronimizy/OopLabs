using Backups.Tcp.RepositoryActions;
using Backups.Tools;
using Utility.Extensions;

namespace Backups.Tcp.Extensions
{
    public static class TypeLocatorExtension
    {
        public static TypeLocator AddRepositoryActions(this TypeLocator locator)
        {
            locator.ThrowIfNull(nameof(locator));

            locator.Add(typeof(DeleteRepositoryAction));
            locator.Add(typeof(ExistsRepositoryAction));
            locator.Add(typeof(GetContentsOfRepositoryAction));
            locator.Add(typeof(GetStreamRepositoryAction));
            locator.Add(typeof(IsFolderRepositoryAction));
            locator.Add(typeof(WriteRepositoryAction));

            return locator;
        }
    }
}