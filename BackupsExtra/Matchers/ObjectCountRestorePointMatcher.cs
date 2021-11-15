using System.Collections.Generic;
using System.Linq;
using Backups.Entities;
using Backups.JobObjects;
using Backups.Models;
using Backups.RestorePointMatchers;
using Utility.Extensions;

namespace BackupsExtra.Matchers
{
    public class ObjectCountRestorePointMatcher : IRestorePointMatcher
    {
        public ObjectCountRestorePointMatcher(int maxObjectCount)
        {
            MaxObjectCount = maxObjectCount;
        }

        public int MaxObjectCount { get; }

        public RestorePointsMatchingResult Match(IReadOnlyCollection<RestorePoint> originalPoints)
        {
            originalPoints.ThrowIfNull(nameof(originalPoints));

            var matched = new List<RestorePoint>();

            var points = originalPoints
                .OrderByDescending(p => p.CreatedDateTime)
                .ToList();

            var objects = points
                .SelectMany(p => p.Objects)
                .Distinct()
                .ToList();

            foreach (IJobObject obj in objects)
            {
                int totalCount = 0;

                foreach (RestorePoint point in points)
                {
                    if (!point.Objects.Contains(obj))
                        continue;

                    ++totalCount;

                    if (totalCount > MaxObjectCount && !matched.Contains(point))
                        matched.Add(point);
                }
            }

            matched = matched.OrderByDescending(p => p.CreatedDateTime).ToList();

            return new RestorePointsMatchingResult(matched, points.Where(p => !matched.Contains(p)).ToList());
        }
    }
}