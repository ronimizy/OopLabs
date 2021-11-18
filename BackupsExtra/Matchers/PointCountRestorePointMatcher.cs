using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Entities;
using Backups.Models;
using Backups.RestorePointMatchers;
using Utility.Extensions;

namespace BackupsExtra.Matchers
{
    public class PointCountRestorePointMatcher : IRestorePointMatcher
    {
        public PointCountRestorePointMatcher(int count)
        {
            Count = count;
        }

        public int Count { get; }

        public RestorePointsMatchingResult Match(IReadOnlyCollection<RestorePoint> originalPoints)
        {
            originalPoints.ThrowIfNull(nameof(originalPoints));

            if (originalPoints.Count <= Count)
                return new RestorePointsMatchingResult(Array.Empty<RestorePoint>(), originalPoints);

            var points = originalPoints
                .OrderByDescending(p => p.CreatedDateTime)
                .ToList();

            return new RestorePointsMatchingResult(points.TakeLast(points.Count - Count).ToList(), points.Take(Count).ToList());
        }
    }
}