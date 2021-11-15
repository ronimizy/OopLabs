using System.Linq;
using Backups.Entities;
using Backups.Models;
using Backups.Packers;
using Backups.Repositories;
using Backups.RestorePointFilters;
using Backups.RestorePointMatchers;
using Backups.StorageAlgorithms;
using Backups.Tools;
using BackupsExtra.Tools;
using Utility.Extensions;

namespace BackupsExtra.Filters
{
    public class DeleteRestorePointFilter : IRestorePointFilter
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
            logger?.OnComment($"{nameof(DeleteRestorePointFilter)} received {nameof(RestorePointsMatchingResult)}");

            if (!result.Matched.Any())
            {
                logger?.OnComment($"{nameof(DeleteRestorePointFilter)} received no matching points");
                return;
            }

            if (!result.NotMatched.Any())
            {
                var exception = new BackupsExtraException("Cannot delete all restore points");
                logger?.OnException(exception, $"{nameof(DeleteRestorePointFilter)} cannot delete all points");
                throw exception;
            }

            foreach (RestorePoint point in result.Matched)
            {
                repository.Delete(point.ToString());
                logger?.OnComment($"{nameof(DeleteRestorePointFilter)} deleted {point}");
            }

            logger?.OnMessage($"{nameof(DeleteRestorePointFilter)} deleted {result.Matched.Count} point");
            backup.RemovePoints(result.Matched.ToArray());
        }
    }
}