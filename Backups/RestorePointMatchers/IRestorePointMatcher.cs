using System.Collections.Generic;
using Backups.Entities;
using Backups.Models;

namespace Backups.RestorePointMatchers
{
    public interface IRestorePointMatcher
    {
        RestorePointsMatchingResult Match(IReadOnlyCollection<RestorePoint> originalPoints);
    }
}