using System.Collections.Generic;
using System.Linq;
using Backups.Entities;
using Backups.Models;
using Backups.RestorePointMatchers;
using BackupsExtra.JsonConverters;
using Newtonsoft.Json;

namespace BackupsExtra.Matchers
{
    [JsonConverter(typeof(AnyOfRestorePointMatcherJsonConverter))]
    public class AnyOfRestorePointMatcher : HybridRestorePointMatcher
    {
        public AnyOfRestorePointMatcher(params IRestorePointMatcher[] matchers)
            : base(matchers) { }

        protected override bool Condition(RestorePoint restorePoint, IReadOnlyCollection<RestorePointsMatchingResult> results)
            => results.Any(r => r.Matched.Contains(restorePoint));
    }
}