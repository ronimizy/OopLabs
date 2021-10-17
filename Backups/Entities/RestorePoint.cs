using System;
using System.Collections.Generic;
using System.Linq;
using Backups.JobObjects;
using Backups.Tools;
using Utility.Extensions;

namespace Backups.Entities
{
    public sealed class RestorePoint : IEquatable<RestorePoint>
    {
        public RestorePoint(DateTime createdDateTime, IReadOnlyCollection<IJobObject> objects)
        {
            objects.ThrowIfNull(nameof(objects));

            Id = Guid.NewGuid();
            CreatedDateTime = createdDateTime;
            Entries = objects
                .Select(o => new JobObjectEntry(o))
                .ToList();
        }

        public Guid Id { get; }
        public DateTime CreatedDateTime { get; }
        public IReadOnlyCollection<JobObjectEntry> Entries { get; }

        public bool Equals(RestorePoint? other)
            => other is not null && other.Id.Equals(Id);

        public override string ToString()
            => $"[{Id}] - {BackupConfiguration.FormatDateTime(CreatedDateTime)}";

        public override bool Equals(object? obj)
            => Equals(obj as RestorePoint);

        public override int GetHashCode()
            => Id.GetHashCode();
    }
}