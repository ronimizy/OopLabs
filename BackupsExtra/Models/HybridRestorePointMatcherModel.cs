using System.Collections.Generic;
using System.Linq;
using Backups.Models;
using Backups.RestorePointMatchers;
using BackupsExtra.Matchers;
using Newtonsoft.Json;
using Utility.Extensions;

namespace BackupsExtra.Models
{
    public class HybridRestorePointMatcherModel
    {
        public HybridRestorePointMatcherModel(HybridRestorePointMatcher matcher)
        {
            matcher.ThrowIfNull(nameof(matcher));

            Matchers = matcher.Matchers
                .Select(m => new SerializedConfiguration<IRestorePointMatcher>(m))
                .ToList();
        }

        [JsonConstructor]
        private HybridRestorePointMatcherModel(IReadOnlyCollection<SerializedConfiguration<IRestorePointMatcher>> matchers)
        {
            Matchers = matchers.ThrowIfNull(nameof(matchers));
        }

        public IReadOnlyCollection<SerializedConfiguration<IRestorePointMatcher>> Matchers { get; }
    }
}