using System;
using System.Diagnostics.CodeAnalysis;
using Isu.Tools;
using Utility.Extensions;

namespace Isu.Models
{
    public readonly struct GroupName : IEquatable<GroupName>
    {
        public GroupName(string name)
        {
            Validate(name.ThrowIfNull(nameof(name)));

            Name = name;
            FacultyLetter = GetFacultyLetterFromGroupName(name);
            CourseNumber = new CourseNumber(GetCourseNumberFromGroupName(Name));
        }

        public string Name { get; }
        public char FacultyLetter { get; }
        public CourseNumber CourseNumber { get; }

        public static bool operator ==(GroupName left, GroupName right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GroupName left, GroupName right)
        {
            return !(left == right);
        }

        public bool Equals(GroupName other)
            => Name == other.Name && CourseNumber.Equals(other.CourseNumber);

        [ExcludeFromCodeCoverage]
        public override bool Equals(object? obj)
            => obj is GroupName other && Equals(other);

        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
            => HashCode.Combine(Name, CourseNumber);

        private static void Validate(string value)
        {
            if (value.Length != 5)
                throw IsuExceptionFactory.InvalidGroupNameException(value, $"Invalid character count: {value.Length}");

            if (!char.IsUpper(value[0]))
                throw IsuExceptionFactory.InvalidGroupNameException(value, $"First character must be an uppercase letter. Got '{value[0]}' instead.");

            foreach (char c in value.AsSpan(1))
            {
                if (!char.IsDigit(c))
                    throw IsuExceptionFactory.InvalidGroupNameException(value, "All characters after the index must be digits");
            }
        }

        private static char GetFacultyLetterFromGroupName(string name)
            => name[0];

        private static int GetCourseNumberFromGroupName(string name)
            => int.Parse(name.AsSpan(2, 1));
    }
}