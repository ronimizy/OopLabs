using System;
using System.Diagnostics.CodeAnalysis;
using Utility.Extensions;

namespace IsuExtra.Entities
{
    public class Mentor : IEquatable<Mentor>
    {
        public Mentor(string name)
        {
            Id = Guid.NewGuid();
            Name = name.ThrowIfNull(nameof(name));
        }

        public Guid Id { get; }
        public string Name { get; }

        public bool Equals(Mentor? other)
            => other is not null && other.Id.Equals(Id);

        [ExcludeFromCodeCoverage]
        public override bool Equals(object? obj)
            => Equals(obj as Mentor);

        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
            => Id.GetHashCode();
    }
}