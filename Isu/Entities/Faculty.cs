using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Isu.Models;

namespace Isu.Entities
{
    public class Faculty : IEquatable<Faculty>
    {
        private readonly List<Course> _courses = new List<Course>();

        internal Faculty(string name, char letter)
        {
            Id = Guid.NewGuid();
            Name = name;
            Letter = letter;
        }

        public Guid Id { get; }
        public string Name { get; }
        public char Letter { get; }
        public IReadOnlyList<Course> Courses => _courses;

        public bool Equals(Faculty? other)
            => other is not null && other.Id.Equals(Id);

        [ExcludeFromCodeCoverage]
        public override bool Equals(object? obj)
            => Equals(obj as Faculty);

        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
            => Id.GetHashCode();

        public override string ToString()
            => $"[{Letter}] {Name}";

        internal void AddCourse(Course course)
            => _courses.Add(course);

        internal Group AddGroup(GroupName name)
        {
            Course? course = _courses.SingleOrDefault(c => c.Number.Equals(name.CourseNumber));
            if (course is null)
            {
                course = new Course(name.CourseNumber, this);
                AddCourse(course);
            }

            var group = new Group(name, course);
            course.AddGroup(group);

            return group;
        }
    }
}