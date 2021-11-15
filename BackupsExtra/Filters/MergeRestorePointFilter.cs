using System.Collections.Generic;
using System.Linq;
using Backups.Entities;
using Backups.JobObjects;
using Backups.Models;
using Backups.Packers;
using Backups.Repositories;
using Backups.RestorePointFilters;
using Backups.RestorePointMatchers;
using Backups.StorageAlgorithms;
using Backups.Storages;
using Backups.Tools;
using Utility.Extensions;

namespace BackupsExtra.Filters
{
    public class MergeRestorePointFilter : IRestorePointFilter
    {
        public void Filter(
            Backup backup, IRestorePointMatcher matcher, IStorageAlgorithm algorithm, IPacker packer, Repository repository, ILogger? logger = null)
        {
            backup.ThrowIfNull(nameof(backup));
            matcher.ThrowIfNull(nameof(matcher));
            algorithm.ThrowIfNull(nameof(algorithm));
            packer.ThrowIfNull(nameof(packer));
            repository.ThrowIfNull(nameof(repository));

            RestorePointsMatchingResult result = matcher.Match(backup.RestorePoints);
            logger?.OnComment($"{nameof(MergeRestorePointFilter)} received {nameof(RestorePointsMatchingResult)}");

            if (!result.Matched.Any())
            {
                logger?.OnComment($"{nameof(MergeRestorePointFilter)} received no matching points");
                return;
            }

            RestorePoint latestPoint;
            IReadOnlyCollection<RestorePoint> mergingPoints;

            if (result.NotMatched.Any())
            {
                latestPoint = result.NotMatched.Last();
                mergingPoints = new[] { latestPoint }.Concat(result.Matched).ToList();
            }
            else
            {
                latestPoint = result.Matched.First();
                mergingPoints = result.Matched;
            }

            var objects = mergingPoints
                .SelectMany(p => p.Objects)
                .Distinct()
                .ToList();

            var packages = new List<Package>();
            var storages = new List<IStorage>();
            var resultPoint = new RestorePoint(latestPoint.Repository, latestPoint.CreatedDateTime, objects);

            foreach (RestorePoint point in mergingPoints)
            {
                IStorage storage = algorithm.Load(point, logger);
                storages.Add(storage);

                foreach (IJobObject obj in point.Objects)
                {
                    if (!objects.Contains(obj))
                        continue;

                    packages.Add(algorithm.Extract(storage, obj, packer, logger));
                    objects.Remove(obj);
                }
            }

            var model = new RestorePointModel(latestPoint.ToString(), packages);
            using IStorage newStorage = algorithm.Pack(model, packer, logger);

            foreach (RestorePoint point in mergingPoints)
            {
                repository.Delete(point.ToString());
                logger?.OnComment($"{nameof(MergeRestorePointFilter)} deleted {point}");
            }

            newStorage.WriteTo(latestPoint.Repository, logger);
            logger?.OnComment($"{nameof(MergeRestorePointFilter)} written new storage");

            backup.RemovePoints(mergingPoints.ToArray());
            backup.AddPoints(resultPoint);

            logger?.OnComment($"{nameof(MergeRestorePointFilter)} merged {mergingPoints.Count} points");

            packages.ForEach(p => p.Dispose());
            storages.ForEach(s => s.Dispose());
        }
    }
}