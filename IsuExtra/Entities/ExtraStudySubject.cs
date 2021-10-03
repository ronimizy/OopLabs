using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Isu.Entities;
using Utility.Extensions;

namespace IsuExtra.Entities
{
    public class ExtraStudySubject : IEquatable<ExtraStudySubject>
    {
        private readonly List<ExtraStudyStream> _streams = new List<ExtraStudyStream>();

        public ExtraStudySubject(string name, Faculty faculty)
        {
            Id = Guid.NewGuid();
            Name = name.ThrowIfNull(nameof(name));
            Faculty = faculty.ThrowIfNull(nameof(faculty));
        }

        public Guid Id { get; }
        public string Name { get; }
        public Faculty Faculty { get; }
        public IReadOnlyCollection<ExtraStudyStream> Streams => _streams;

        public override string ToString()
            => $"[{Id}] - {Name} (faculty: {Faculty})";

        public bool Equals(ExtraStudySubject? other)
            => other is not null && other.Id.Equals(Id);

        [ExcludeFromCodeCoverage]
        public override bool Equals(object? obj)
            => Equals(obj as ExtraStudySubject);

        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
            => Id.GetHashCode();

        internal void AddStream(ExtraStudyStream stream)
            => _streams.Add(stream);

        internal bool HasStudent(Student student)
            => Streams.Any(s => s.Contains(student));
    }
}