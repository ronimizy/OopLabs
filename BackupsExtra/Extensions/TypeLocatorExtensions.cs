using Backups.Tools;
using BackupsExtra.Filters;
using BackupsExtra.Matchers;

namespace BackupsExtra.Extensions
{
    public static class TypeLocatorExtensions
    {
        public static TypeLocator AddMatchers(this TypeLocator locator)
        {
            return locator
                .Add(typeof(AllOfRestorePointMatcher))
                .Add(typeof(AnyOfRestorePointMatcher))
                .Add(typeof(DateTimeRestorePointMatcher))
                .Add(typeof(HybridRestorePointMatcher))
                .Add(typeof(ObjectCountRestorePointMatcher))
                .Add(typeof(PointCountRestorePointMatcher));
        }

        public static TypeLocator AddFilters(this TypeLocator locator)
        {
            return locator
                .Add(typeof(DeleteRestorePointFilter))
                .Add(typeof(MergeRestorePointFilter));
        }
    }
}