using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Banks.Chronometers;
using Banks.Commands;
using Banks.Entities;
using Banks.Models;
using Banks.Plans;
using Banks.Tools;
using Utility.Extensions;

namespace Banks.Accounts
{
    internal abstract class AccountWrapperBase : Account
    {
        protected AccountWrapperBase(Account wrapped, IChronometer chronometer)
        {
            Wrapped = wrapped.ThrowIfNull(nameof(wrapped));
            Chronometer = chronometer.ThrowIfNull(nameof(wrapped));
        }

#pragma warning disable 8618
        protected AccountWrapperBase(BanksDatabaseContext context)
        {
            Chronometer = context.Chronometer;
        }
#pragma warning restore 8618

        [NotMapped]
        public override Client Owner => Wrapped.Owner;

        [NotMapped]
        public override AccountPlan? Plan => Wrapped.Plan;

        [NotMapped]
        public override decimal Balance => Wrapped.Balance;

        [NotMapped]
        public override IReadOnlyCollection<ReadOnlyAccountHistoryEntry> History => Wrapped.History;

        protected Account Wrapped { get; }
        protected IChronometer Chronometer { get; }

        public override bool WithdrawalAllowed(decimal amount = 0)
            => Wrapped.WithdrawalAllowed(amount);

        internal override bool TryExecuteCommand(AccountCommand command, out DateTime? executedDateTime)
        {
            executedDateTime = null;
            return command.TryExecute(this, Chronometer, out executedDateTime) || Wrapped.TryExecuteCommand(command, out executedDateTime);
        }

        internal override bool TryCancelOperation(Guid operationId)
            => Wrapped.TryCancelOperation(operationId);
    }
}