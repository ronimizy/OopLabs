using System;
using Backups.JobObjects;
using Utility.Extensions;

namespace Backups.Models
{
    public class JobObjectBuilder
    {
        private readonly Func<BackupJobConfiguration, IJobObject> _builder;

        internal JobObjectBuilder(Func<BackupJobConfiguration, IJobObject> builder)
        {
            _builder = builder.ThrowIfNull(nameof(builder));
        }

        public IJobObject Build(BackupJobConfiguration configuration)
            => _builder(configuration);
    }
}