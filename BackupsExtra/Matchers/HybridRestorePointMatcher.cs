using System.Collections.Generic;
using System.Linq;
using Backups.Entities;
using Backups.Models;
using Backups.RestorePointMatchers;
using Utility.Extensions;

namespace BackupsExtra.Matchers
{
    public abstract class HybridRestorePointMatcher : IRestorePointMatcher
    {
        protected HybridRestorePointMatcher(params IRestorePointMatcher[] matchers)
        {
            Matchers = matchers.ThrowIfNull(nameof(matchers));
        }

        public IReadOnlyCollection<IRestorePointMatcher> Matchers { get; }

        public RestorePointsMatchingResult Match(IReadOnlyCollection<RestorePoint> originalPoints)
        {
            originalPoints.ThrowIfNull(nameof(originalPoints));

            var results = Matchers.Select(m => m.Match(originalPoints)).ToList();
            var points = originalPoints
                .OrderByDescending(p => p.CreatedDateTime)
                .ToList();
            var matched = points
                .Where(p => Condition(p, results))
                .ToList();

            var notMatched = points
                .Where(p => !matched.Contains(p))
                .ToList();

            return new RestorePointsMatchingResult(matched, notMatched);
        }

        protected abstract bool Condition(RestorePoint restorePoint, IReadOnlyCollection<RestorePointsMatchingResult> results);
    }
}