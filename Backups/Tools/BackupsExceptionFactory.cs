using Backups.Entities;
using Backups.JobObjects;
using Backups.Repositories;
using Backups.RestorePointFilters;
using Backups.RestorePointMatchers;

namespace Backups.Tools
{
    internal static class BackupsExceptionFactory
    {
        public static BackupsException InvalidFilterMatcherNullability(IRestorePointFilter? filter, IRestorePointMatcher? matcher)
            => new BackupsException(
                $"{nameof(IRestorePointFilter)} is null ({filter is null}) != {nameof(IRestorePointMatcher)} is null ({matcher is null})");

        public static BackupsException ObjectIsNotBeingTracked(BackupJob job, IJobObject jobObject)
            => new BackupsException($"Item {jobObject} is not being tracked by BackupJob {job}");

        public static BackupsException RestorePointIsNotBeingTracked(RestorePoint point)
            => new BackupsException($"RestorePoint: {point} is not being tracked by backup");

        public static BackupsException AlreadyTrackingRestorePoint(RestorePoint point)
            => new BackupsException($"RestorePoint: {point} is already being tracked by backup");

        public static BackupsException RepositoryDoesNotContainRequestedPath(Repository repository, string path)
            => new BackupsException($"Repository: {repository} does not contain an object at path: {path}");
    }
}