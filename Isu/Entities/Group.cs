using System.Collections.Generic;
using Isu.Models;
using Utility.Extensions;

namespace Isu.Entities
{
    public sealed class Group
    {
        private readonly List<Student> _students = new List<Student>();

        internal Group(GroupName name)
        {
            Name = name.ThrowIfNull(nameof(name));
        }

        public GroupName Name { get; }

        public CourseNumber CourseNumber => Name.CourseNumber;

        public IReadOnlyList<Student> Students => _students;

        internal void AddStudent(Student student)
            => _students.Add(student);

        internal void RemoveStudent(Student student)
            => _students.Remove(student);
    }
}