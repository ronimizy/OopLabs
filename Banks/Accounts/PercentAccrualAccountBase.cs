using System;
using System.Collections.Generic;
using System.Linq;
using Banks.AccountInterfaces;
using Banks.Chronometers;
using Banks.Commands;
using Banks.Extensions;
using Banks.Models;
using Banks.Tools;

namespace Banks.Accounts
{
    internal abstract class PercentAccrualAccountBase : AccountWrapperBase, IPercentAccrualAccount
    {
        protected const decimal DayCount = 365;

        protected PercentAccrualAccountBase(Account wrapped, IChronometer chronometer)
            : base(wrapped, chronometer) { }

        protected PercentAccrualAccountBase(BanksDatabaseContext context)
            : base(context) { }

        public DateTime LatestProcessingDateTime { get; private set; }

        public void ProcessPercentsAccrual()
        {
            DateTime accrualDateTime = Chronometer.CurrentDateTime.GetYearMonthAndFirstDay();

            IEnumerable<(DateTime Key, decimal)> percentsGroupedByMonth = History
                .GetSortedEntriesInRange(LatestProcessingDateTime, accrualDateTime)
                .TakeLastOperationsOfDay()
                .FillMissingDays()
                .GroupByMonth()
                .Select(CalculatePercents);

            foreach ((DateTime date, decimal percents) in percentsGroupedByMonth)
            {
                Wrapped.TryExecuteCommand(GetProcessingCommand(date, percents));
            }

            LatestProcessingDateTime = accrualDateTime;
        }

        protected abstract decimal CalculatePercentPerDay(decimal remainingBalance);
        protected abstract AccountCommand GetProcessingCommand(DateTime dateTime, decimal percents);

        private (DateTime Date, decimal percents) CalculatePercents(IGrouping<DateTime, ReadOnlyAccountHistoryEntry> grouping)
            => (grouping.Key, grouping.Sum(e => e.RemainingBalance * CalculatePercentPerDay(e.RemainingBalance)));
    }
}