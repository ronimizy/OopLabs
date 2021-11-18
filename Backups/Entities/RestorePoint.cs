using System;
using System.Collections.Generic;
using System.Linq;
using Backups.JobObjects;
using Backups.Repositories;
using Backups.Tools;
using Utility.Extensions;

namespace Backups.Entities
{
    public sealed class RestorePoint
    {
        public RestorePoint(Repository repository, DateTime createdDateTime, IReadOnlyCollection<IJobObject> objects)
        {
            Repository = repository.ThrowIfNull(nameof(repository));
            CreatedDateTime = createdDateTime;
            Objects = objects.ThrowIfNull(nameof(objects)).ToList();
        }

        public Repository Repository { get; }
        public DateTime CreatedDateTime { get; }
        public IReadOnlyCollection<IJobObject> Objects { get; }

        public override string ToString()
            => BackupConfiguration.FormatDateTime(CreatedDateTime);
    }
}