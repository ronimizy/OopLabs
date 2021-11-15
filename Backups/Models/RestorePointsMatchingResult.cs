using System.Collections.Generic;
using System.Linq;
using Backups.Entities;
using Utility.Extensions;

namespace Backups.Models
{
    public class RestorePointsMatchingResult
    {
        public RestorePointsMatchingResult(IReadOnlyCollection<RestorePoint> matched, IReadOnlyCollection<RestorePoint> notMatched)
        {
            Matched = matched.ThrowIfNull(nameof(matched)).ToList();
            NotMatched = notMatched.ThrowIfNull(nameof(notMatched)).ToList();
        }

        public IReadOnlyCollection<RestorePoint> Matched { get; }
        public IReadOnlyCollection<RestorePoint> NotMatched { get; }
    }
}