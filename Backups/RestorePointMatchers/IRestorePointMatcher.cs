using System.Collections.Generic;
using Backups.Entities;

namespace Backups.RestorePointMatchers
{
    public interface IRestorePointMatcher
    {
        IReadOnlyCollection<RestorePoint> GetRestorePointsToRemove(IReadOnlyCollection<RestorePoint> originalPoints);
    }
}