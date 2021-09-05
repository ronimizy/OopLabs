using System;
using Isu.Tools;

namespace Isu.Models
{
    public readonly struct GroupName : IEquatable<GroupName>
    {
        public GroupName(string name)
        {
            Name = Validate(name);
            CourseNumber = new CourseNumber(GetCourseNumberFromGroupName(Name));
        }

        public string Name { get; }
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

        public override bool Equals(object? obj)
            => obj is GroupName other && Equals(other);

        public override int GetHashCode()
            => HashCode.Combine(Name, CourseNumber);

        private static string Validate(string value)
        {
            if (value.Length != 5)
                throw IsuExceptionFactory.InvalidGroupNameException(value, $"Invalid character count: {value.Length}");

            if (!value.AsSpan(0, 2).SequenceEqual("M3"))
                throw IsuExceptionFactory.InvalidGroupNameException(value, $"Invalid group index: {value[..1]}");

            foreach (char c in value.AsSpan(2))
            {
                if (!char.IsDigit(c))
                    throw IsuExceptionFactory.InvalidGroupNameException(value, "All characters after the index must be digits");
            }

            return value;
        }

        private static int GetCourseNumberFromGroupName(string name)
            => int.Parse(name.AsSpan(2, 1));
    }
}