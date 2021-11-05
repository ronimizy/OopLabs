using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Models;
using Utility.Extensions;

namespace Banks.Extensions
{
    internal static class AccountHistoryExtensions
    {
        public static IEnumerable<ReadOnlyAccountHistoryEntry> GetSortedEntriesInRange(
            this IEnumerable<ReadOnlyAccountHistoryEntry> entries, DateTime from, DateTime to)
        {
            return entries
                .ThrowIfNull(nameof(entries))
                .OrderByDescending(e => e.ExecutedTime)
                .SkipWhile(e => e.ExecutedTime >= to)
                .TakeWhile(e => e.ExecutedTime.GetYearMonthAndFirstDay() >= from);
        }

        public static IEnumerable<ReadOnlyAccountHistoryEntry> TakeLastOperationsOfDay(this IEnumerable<ReadOnlyAccountHistoryEntry> entries)
        {
            return entries
                .ThrowIfNull(nameof(entries))
                .GroupBy(e => e.ExecutedTime.Date)
                .Select(g => g.First());
        }

        public static IEnumerable<ReadOnlyAccountHistoryEntry> FillMissingDays(this IEnumerable<ReadOnlyAccountHistoryEntry> entries)
        {
            ReadOnlyAccountHistoryEntry? lastEntry = null;

            foreach (ReadOnlyAccountHistoryEntry entry in entries.ThrowIfNull(nameof(entries)))
            {
                lastEntry ??= entry;

                if (entry.ExecutedTime.Date - lastEntry.ExecutedTime.Date <= TimeSpan.FromDays(1))
                {
                    lastEntry = entry;
                    yield return entry;
                }

                lastEntry = new AccountHistoryEntry(
                    lastEntry.ExecutedTime.Date + TimeSpan.FromDays(1), lastEntry.RemainingBalance, lastEntry.Info, null);
                yield return lastEntry;
            }
        }

        public static IEnumerable<IGrouping<DateTime, ReadOnlyAccountHistoryEntry>> GroupByMonth(
            this IEnumerable<ReadOnlyAccountHistoryEntry> entries)
        {
            return entries
                .ThrowIfNull(nameof(entries))
                .GroupBy(e => e.ExecutedTime.GetYearMonthAndFirstDay());
        }
    }
}