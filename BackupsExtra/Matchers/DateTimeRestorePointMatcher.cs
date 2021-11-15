using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Entities;
using Backups.Models;
using Backups.RestorePointMatchers;
using Utility.Extensions;

namespace BackupsExtra.Matchers
{
    public class DateTimeRestorePointMatcher : IRestorePointMatcher
    {
        public DateTimeRestorePointMatcher(DateTime deadline)
        {
            Deadline = deadline;
        }

        public DateTime Deadline { get; }

        public RestorePointsMatchingResult Match(IReadOnlyCollection<RestorePoint> originalPoints)
        {
            originalPoints.ThrowIfNull(nameof(originalPoints));

            var matched = new List<RestorePoint>();
            var notMatched = new List<RestorePoint>();

            IOrderedEnumerable<RestorePoint> points = originalPoints
                .OrderByDescending(p => p.CreatedDateTime);

            foreach (RestorePoint point in points)
            {
                if (point.CreatedDateTime < Deadline)
                    matched.Add(point);
                else
                    notMatched.Add(point);
            }

            return new RestorePointsMatchingResult(matched, notMatched);
        }
    }
}