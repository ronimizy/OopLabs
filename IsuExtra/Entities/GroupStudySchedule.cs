using System;
using System.Diagnostics.CodeAnalysis;
using Isu.Entities;
using Utility.Extensions;

namespace IsuExtra.Entities
{
    public class GroupStudySchedule : IEquatable<GroupStudySchedule>
    {
        public GroupStudySchedule(Group group, Schedule schedule)
        {
            Group = group;
            Id = Guid.NewGuid();
            Schedule = schedule.ThrowIfNull(nameof(schedule));
        }

        public Guid Id { get; }
        public Group Group { get; }
        public Schedule Schedule { get; }

        public bool Equals(GroupStudySchedule? other)
            => other is not null && other.Id.Equals(Id);

        [ExcludeFromCodeCoverage]
        public override bool Equals(object? obj)
            => Equals(obj as GroupStudySchedule);

        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
            => Id.GetHashCode();

        public override string ToString()
            => $"[{Id}] - {Group.Name}";
    }
}