using System;
using System.Diagnostics.CodeAnalysis;
using Isu.Tools;

namespace Isu.Models
{
    public readonly struct CourseNumber : IEquatable<CourseNumber>
    {
        private const int MaxCourse = 4;

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

        [ExcludeFromCodeCoverage]
        public override bool Equals(object? obj)
            => obj is CourseNumber && Equals(obj);

        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
            => Number.GetHashCode();

        public override string ToString()
            => Number.ToString();

        private static int Validate(int value)
        {
            if (value is <= 0 or > MaxCourse)
                throw IsuExceptionFactory.InvalidCourseNumberException(value);

            return value;
        }
    }
}