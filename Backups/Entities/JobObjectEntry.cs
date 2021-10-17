using Backups.JobObjects;
using Utility.Extensions;

namespace Backups.Entities
{
    public class JobObjectEntry
    {
        public JobObjectEntry(IJobObject jobObject)
        {
            jobObject.ThrowIfNull(nameof(jobObject));

            Name = jobObject.ToString() ?? string.Empty;
            Version = jobObject.Version;
        }

        public string Name { get; }
        public int Version { get; }
    }
}