using System.Collections.Generic;
using System.Linq;
using Backups.Entities;
using Backups.Models;
using Backups.RestorePointMatchers;
using BackupsExtra.JsonConverters;
using Newtonsoft.Json;

namespace BackupsExtra.Matchers
{
    [JsonConverter(typeof(AllOfRestorePointMatcherJsonConverter))]
    public class AllOfRestorePointMatcher : HybridRestorePointMatcher
    {
        public AllOfRestorePointMatcher(params IRestorePointMatcher[] matchers)
            : base(matchers) { }

        protected override bool Condition(RestorePoint restorePoint, IReadOnlyCollection<RestorePointsMatchingResult> results)
            => results.All(r => r.Matched.Contains(restorePoint));
    }
}