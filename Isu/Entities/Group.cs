using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Isu.Models;
using Utility.Extensions;

namespace Isu.Entities
{
    public sealed class Group : IEquatable<Group>
    {
        private readonly List<Student> _students = new List<Student>();

        internal Group(GroupName name, Course course)
        {
            Id = Guid.NewGuid();
            Name = name.ThrowIfNull(nameof(name));
            Course = course.ThrowIfNull(nameof(course));
        }

        public Guid Id { get; }
        public GroupName Name { get; }
        public Course Course { get; }
        public IReadOnlyList<Student> Students => _students;

        public override string ToString()
            => $"[{Id}] - {Name}";

        public bool Equals(Group? other)
            => other is not null && other.Id.Equals(Id);

        [ExcludeFromCodeCoverage]
        public override bool Equals(object? obj)
            => Equals(obj as Group);

        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
            => Id.GetHashCode();

        internal void AddStudent(Student student)
            => _students.Add(student);

        internal void RemoveStudent(Student student)
            => _students.Remove(student);
    }
}