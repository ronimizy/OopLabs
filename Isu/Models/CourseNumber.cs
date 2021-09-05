using System;
using Isu.Tools;

namespace Isu.Models
{
    public readonly struct CourseNumber : IEquatable<CourseNumber>
    {
        private const int MaxCourse = 8;

        public CourseNumber(int value)
        {
            Number = Validate(value);
        }

        public int Number { get; }

        public static bool operator ==(CourseNumber left, CourseNumber right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CourseNumber left, CourseNumber right)
        {
            return !(left == right);
        }

        public bool Equals(CourseNumber other)
            => Number == other.Number;

        public override bool Equals(object? obj)
            => obj is CourseNumber && Equals(obj);

        public override int GetHashCode()
            => Number.GetHashCode();

        private static int Validate(int value)
        {
            if (value is <= 0 or > MaxCourse)
                throw IsuExceptionFactory.InvalidCourseNumberException(value);

            return value;
        }
    }
}