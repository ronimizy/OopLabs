using System;
using Utility.Extensions;

namespace Isu.Entities
{
    public sealed class Student : IEquatable<Student>
    {
        internal Student(int id, string name, Group group)
        {
            Id = id;
            Name = name.ThrowIfNull(nameof(name));
            Group = group.ThrowIfNull(nameof(group));
        }

        public int Id { get; init; }
        public string Name { get; set; }
        public Group Group { get; set; }

        public bool Equals(Student? other)
            => other is not null && Id == other.Id;

        public override bool Equals(object? obj)
            => obj is Student other && Equals(other);

        public override int GetHashCode()
            => Id.GetHashCode();
    }
}