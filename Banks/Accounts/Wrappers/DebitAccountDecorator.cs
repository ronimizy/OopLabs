using System;
using System.ComponentModel.DataAnnotations.Schema;
using Banks.Chronometers;
using Banks.Commands;
using Banks.Commands.BankAccountCommands;
using Banks.Plans;
using Banks.Tools;
using Utility.Extensions;

namespace Banks.Accounts.Wrappers
{
    internal class DebitAccountDecorator : PercentAccrualAccountBase
    {
        private readonly DebitAccountPlan _plan;

        public DebitAccountDecorator(DebitAccountPlan plan, Account wrapped, IChronometer chronometer)
            : base(wrapped, chronometer)
        {
            _plan = plan.ThrowIfNull(nameof(plan));
        }

#pragma warning disable 8618
        private DebitAccountDecorator(BanksDatabaseContext context)
#pragma warning restore 8618
            : base(context) { }

        [NotMapped]
        public override AccountPlan Plan => _plan;

        protected override decimal CalculatePercentPerDay(decimal remainingBalance)
            => _plan.Percentage / DayCount;

        protected override AccountCommand GetProcessingCommand(DateTime dateTime, decimal percents)
            => new AccrualAccountCommand(percents, $"Debit percents for {dateTime}");
    }
}