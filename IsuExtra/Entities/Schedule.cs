using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using IsuExtra.Tools;
using Utility.Extensions;

namespace IsuExtra.Entities
{
    public class Schedule : IReadOnlyCollection<Lesson>
    {
        private readonly List<Lesson> _lessons;

        public Schedule(params Lesson[] lessons)
        {
            _lessons = lessons
                .ThrowIfNull(nameof(lessons))
                .Distinct()
                .ToList();

            if (IsIntersectsWith(this, (first, second) => !ReferenceEquals(first, second)))
                throw ScheduleServiceExceptionFactory.OverlappingSchedule();
        }

        internal Schedule(IEnumerable<Schedule> schedules)
            : this(schedules.SelectMany(s => s).ToArray()) { }

        public int Count => _lessons.Count;

        public bool IsIntersectsWith(Schedule other)
            => IsIntersectsWith(other, (_, _) => true);

        public IEnumerator<Lesson> GetEnumerator()
            => _lessons.GetEnumerator();

        [ExcludeFromCodeCoverage]
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        private bool IsIntersectsWith(Schedule other, Func<Lesson, Lesson, bool> condition)
        {
            IEnumerable<ValueTuple<Lesson, Lesson>> pairs =
                from first in this
                from second in other
                where condition(first, second)
                select (first, second);

            return pairs.Any(p => p.Item1.IsIntersectsWith(p.Item2));
        }
    }
}