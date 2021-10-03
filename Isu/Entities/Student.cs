using System;
using System.Diagnostics.CodeAnalysis;
using Utility.Extensions;

namespace Isu.Entities
{
    public sealed class Student : IEquatable<Student>
    {
        internal Student(string name, Group group)
        {
            Id = Guid.NewGuid();
            Name = name.ThrowIfNull(nameof(name));
            Group = group.ThrowIfNull(nameof(group));
        }

        public Guid Id { get; init; }
        public string Name { get; set; }
        public Group Group { get; set; }

        public override string ToString()
            => $"[{Id}] - {Name} ({Group})";

        public bool Equals(Student? other)
            => other is not null && other.Id.Equals(Id);

        [ExcludeFromCodeCoverage]
        public override bool Equals(object? obj)
            => Equals(obj as Student);

        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
            => Id.GetHashCode();
    }
}