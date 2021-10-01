using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Isu.Models;
using Utility.Extensions;

namespace Isu.Entities
{
    public class Course : IEquatable<Course>
    {
        private readonly List<Group> _groups = new List<Group>();

        internal Course(CourseNumber number, Faculty faculty)
        {
            Id = Guid.NewGuid();
            Number = number;
            Faculty = faculty.ThrowIfNull(nameof(faculty));
        }

        public Guid Id { get; }
        public CourseNumber Number { get; }
        public Faculty Faculty { get; }
        public IReadOnlyList<Group> Groups => _groups;

        public bool Equals(Course? other)
            => other is not null && other.Id.Equals(Id);

        [ExcludeFromCodeCoverage]
        public override bool Equals(object? obj)
            => Equals(obj as Course);

        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
            => Id.GetHashCode();

        public override string ToString()
            => $"#{Number} F: {Faculty}";

        internal void AddGroup(Group group)
            => _groups.Add(group);
    }
}