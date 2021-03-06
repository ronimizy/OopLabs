using System;
using System.Diagnostics.CodeAnalysis;
using IsuExtra.Models;
using IsuExtra.Tools;
using Utility.Extensions;

namespace IsuExtra.Entities
{
    public class Lesson : IEquatable<Lesson>
    {
        public Lesson(DayOfWeek dayOfWeek, LessonFrequency frequency, TimeSpan begin, TimeSpan end, Mentor mentor, string roomName)
        {
            mentor.ThrowIfNull(nameof(mentor));
            roomName.ThrowIfNull(nameof(roomName));

            if (begin < TimeSpan.Zero || end < TimeSpan.Zero)
                throw InvalidLessonTimeException(begin, end, "Time cannot be negative");

            if (begin >= TimeSpan.FromHours(24) || end >= TimeSpan.FromHours(24))
                throw InvalidLessonTimeException(begin, end, "Time cannot exceed 24 hours");

            if (begin > end)
                throw InvalidLessonTimeException(begin, end, "Begin time cannot be greater than end time");

            Id = Guid.NewGuid();
            DayOfWeek = dayOfWeek;
            Frequency = frequency;
            Begin = begin;
            End = end;
            Mentor = mentor;
            RoomName = roomName;
        }

        public Guid Id { get; }
        public DayOfWeek DayOfWeek { get; }
        public LessonFrequency Frequency { get; }
        public TimeSpan Begin { get; }
        public TimeSpan End { get; }
        public Mentor Mentor { get; }
        public string RoomName { get; }

        public bool IsIntersectsWith(Lesson other)
        {
            if ((Frequency & other.Frequency) == 0)
                return false;

            if (!other.DayOfWeek.Equals(DayOfWeek))
                return false;

            return (other.Begin <= Begin && Begin <= other.End) ||
                   (Begin <= other.Begin && other.Begin <= End);
        }

        public bool Equals(Lesson? other)
            => other is not null && other.Id.Equals(Id);

        [ExcludeFromCodeCoverage]
        public override bool Equals(object? obj)
            => Equals(obj as Lesson);

        public override int GetHashCode()
            => Id.GetHashCode();

        private static ScheduleServiceException InvalidLessonTimeException(TimeSpan begin, TimeSpan end, string description)
            => new ScheduleServiceException($"Provided time for lesson is invalid. Begin: {begin}, end: {end}. {description}.");
    }
}