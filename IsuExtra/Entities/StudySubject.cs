using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Isu.Entities;
using Utility.Extensions;

namespace IsuExtra.Entities
{
    public class StudySubject : IEquatable<StudySubject>
    {
        private readonly List<GroupStudySchedule> _groupSchedules = new();

        public StudySubject(string subjectName, Course course)
        {
            Id = Guid.NewGuid();
            Name = subjectName.ThrowIfNull(nameof(subjectName));
            Course = course.ThrowIfNull(nameof(course));
        }

        public Guid Id { get; }
        public string Name { get; }
        public Course Course { get; }
        public IReadOnlyList<GroupStudySchedule> GroupSchedules => _groupSchedules;

        public bool Equals(StudySubject? other)
            => other is not null && other.Id.Equals(Id);

        [ExcludeFromCodeCoverage]
        public override bool Equals(object? obj)
            => Equals(obj as StudySubject);

        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
            => Id.GetHashCode();

        public override string ToString()
            => $"[{Id}] - {Name} ({Course})";

        internal void AddGroupSchedule(GroupStudySchedule schedule)
            => _groupSchedules.Add(schedule);
    }
}