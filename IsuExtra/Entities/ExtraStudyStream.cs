using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Isu.Entities;
using Utility.Extensions;

namespace IsuExtra.Entities
{
    public class ExtraStudyStream : IReadOnlyCollection<Student>, IEquatable<ExtraStudyStream>
    {
        private readonly List<Student> _students = new List<Student>();

        public ExtraStudyStream(string name, Schedule schedule, int capacity)
        {
            if (capacity < 0)
                throw new ArgumentException("Capacity cannot be negative");

            Id = Guid.NewGuid();
            Name = name.ThrowIfNull(nameof(name));
            Schedule = schedule.ThrowIfNull(nameof(schedule));
            Capacity = capacity;
        }

        public Guid Id { get; }
        public string Name { get; }
        public Schedule Schedule { get; }
        public int Capacity { get; }
        public int Count => _students.Count;

        public IEnumerator<Student> GetEnumerator()
            => _students.GetEnumerator();

        [ExcludeFromCodeCoverage]
        IEnumerator IEnumerable.GetEnumerator()
            => _students.GetEnumerator();

        public bool Equals(ExtraStudyStream? other)
            => other is not null && other.Id.Equals(Id);

        [ExcludeFromCodeCoverage]
        public override bool Equals(object? obj)
            => Equals(obj as ExtraStudyStream);

        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
            => Id.GetHashCode();

        internal void AddStudent(Student student)
            => _students.Add(student);

        internal bool RemoveStudent(Student student)
            => _students.Remove(student);
    }
}