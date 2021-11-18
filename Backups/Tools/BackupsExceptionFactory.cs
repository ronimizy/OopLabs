using System;
using Backups.Entities;
using Backups.JobObjects;
using Backups.Models;
using Backups.Repositories;
using Backups.RestorePointFilters;
using Backups.RestorePointMatchers;
using Backups.Storages;

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

        public static BackupsException InvalidStorageType(Type expectedType, IStorage received)
            => new BackupsException($"Provided storage type is invalid. Expected type: {expectedType.Name}, received object: {received}");

        public static BackupsException JobObjectNotFoundInStorage(IJobObject obj)
            => new BackupsException($"Job object: {obj} was not found in storage");

        public static BackupsException MissingSubPackageException(Package package, string subPackageName)
            => new BackupsException($"Package {package} does not contain subpackage called {subPackageName}");
    }
}