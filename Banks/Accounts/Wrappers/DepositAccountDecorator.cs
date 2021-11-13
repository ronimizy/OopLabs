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
    internal class DepositAccountDecorator : PercentAccrualAccountBase
    {
        private readonly DepositAccountPlan _plan;

        public DepositAccountDecorator(decimal initialBalance, DepositAccountPlan plan, Account wrapped, IChronometer chronometer)
            : base(wrapped, chronometer)
        {
            InitialBalance = initialBalance;
            _plan = plan.ThrowIfNull(nameof(plan));
        }

#pragma warning disable 8618
        private DepositAccountDecorator(BanksDatabaseContext context)
#pragma warning restore 8618
            : base(context) { }

        [NotMapped]
        public override AccountPlan Plan => _plan;

        public decimal InitialBalance { get; init; }

        protected override decimal CalculatePercentPerDay(decimal remainingBalance)
            => _plan.RetrievePercent(InitialBalance) / DayCount;

        protected override AccountCommand GetProcessingCommand(DateTime dateTime, decimal percents)
            => new AccrualAccountCommand(percents, $"Deposit percents for {dateTime}");
    }
}