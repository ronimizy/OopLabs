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

            if (IsIntersectsWithConditional(this, (first, second) => !ReferenceEquals(first, second)))
                throw new ScheduleServiceException("Provided lessons are overlapping.");
        }

        internal Schedule(IEnumerable<Schedule> schedules)
            : this(schedules.SelectMany(s => s).ToArray()) { }

        public int Count => _lessons.Count;

        public bool IsIntersectsWith(Schedule other)
            => IsIntersectsWithConditional(other);

        public IEnumerator<Lesson> GetEnumerator()
            => _lessons.GetEnumerator();

        [ExcludeFromCodeCoverage]
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        private bool IsIntersectsWithConditional(Schedule other, Func<Lesson, Lesson, bool>? predicate = null)
        {
            IEnumerable<ValueTuple<Lesson, Lesson>> pairs = this
                .SelectMany(_ => other, (first, second) => (first, second))
                .Where(p => predicate?.Invoke(p.first, p.second) ?? true);

            return pairs.Any(p => p.Item1.IsIntersectsWith(p.Item2));
        }
    }
}