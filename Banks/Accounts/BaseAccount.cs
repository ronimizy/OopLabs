using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Banks.AccountInterfaces;
using Banks.Chronometers;
using Banks.Commands;
using Banks.Commands.BankAccountCommands;
using Banks.Entities;
using Banks.ExceptionFactories;
using Banks.Models;
using Banks.Plans;
using Banks.Tools;
using Utility.Extensions;

namespace Banks.Accounts
{
    internal sealed class BaseAccount : Account, IAccrualAccount, IWithdrawingAccount, IHistoryEditingAccount
    {
        private readonly List<AccountHistoryEntry> _history;
        private readonly IChronometer _chronometer;
        private readonly Client _owner;
        private decimal _balance;

        public BaseAccount(Client owner, decimal balance, IChronometer chronometer)
        {
            _owner = owner.ThrowIfNull(nameof(owner));
            _balance = balance;
            _chronometer = chronometer.ThrowIfNull(nameof(chronometer));
            _history = new List<AccountHistoryEntry>();
        }

#pragma warning disable 8618
        private BaseAccount(BanksDatabaseContext context)
#pragma warning restore 8618
        {
            _chronometer = context.Chronometer;
        }

        public override Client Owner => _owner;

        [NotMapped]
        public override AccountPlan? Plan => null;

        public override decimal Balance => _balance;

        [NotMapped]
        public override IReadOnlyCollection<ReadOnlyAccountHistoryEntry> History => _history;

        public override bool WithdrawalAllowed(decimal amount = 0)
            => true;

        public void AccrueFunds(decimal amount)
        {
            if (amount < 0)
                throw new InvalidOperationException($"Cannot accrue a negative amount: {amount}");

            _balance += amount;
        }

        public void WithdrawFunds(decimal amount)
        {
            if (amount < 0)
                throw new InvalidOperationException($"Cannot withdraw a negative amount: {amount}");

            _balance -= amount;
        }

        public void LogEntry(AccountHistoryEntry entry)
        {
            entry.ThrowIfNull(nameof(entry));

            if (_history.Contains(entry))
                throw AccountExceptionFactory.AlreadyContainsHistoryEntryException(this, entry);

            _history.Add(entry);
        }

        public void CancelEntry(ReadOnlyAccountHistoryEntry entry)
        {
            entry.ThrowIfNull(nameof(entry));

            AccountHistoryEntry realEntry = _history
                .SingleOrDefault(e => e.Equals(entry))
                .ThrowIfNull(AccountExceptionFactory.NotContainingHistoryEntryException(this, entry));

            realEntry.Cancel();
        }

        internal override bool TryExecuteCommand(AccountCommand command, out DateTime? executedDateTime)
        {
            command.ThrowIfNull(nameof(command));
            return command.TryExecute(this, _chronometer, out executedDateTime);
        }

        internal override bool TryCancelOperation(Guid operationId)
        {
            AccountHistoryEntry? entry = _history.SingleOrDefault(e => e.Id.Equals(operationId));

            if (entry?.State is OperationState.Canceled || entry?.RevertCommand is null)
                return false;

            return TryExecuteCommand(entry.RevertCommand) && TryExecuteCommand(new CancelEntryAccountCommand(entry));
        }
    }
}