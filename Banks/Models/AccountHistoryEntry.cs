using System;
using System.ComponentModel.DataAnnotations;
using Banks.ExceptionFactories;
using Utility.Extensions;

namespace Banks.Models
{
    public sealed class AccountHistoryEntry : ReadOnlyAccountHistoryEntry
    {
        public AccountHistoryEntry(
            DateTime executedTime,
            decimal remainingBalance,
            Info info)
        {
            ExecutedTime = executedTime;
            RemainingBalance = remainingBalance;
            State = OperationState.Valid;
            Info = info.ThrowIfNull(nameof(info));
        }

#pragma warning disable 8618
        private AccountHistoryEntry() { }
#pragma warning restore 8618

        [Key]
        public override Guid? Id { get; protected init; }

        public override DateTime ExecutedTime { get; protected init; }
        public override decimal RemainingBalance { get; protected init; }
        public override OperationState State { get; protected set; }
        public override Info Info { get; protected init; }

        public void Cancel()
        {
            if (State is OperationState.Canceled)
                throw AccountExceptionFactory.AlreadyCanceledEntryException(this);

            State = OperationState.Canceled;
        }

        public override string ToString()
            => $"{Id} - {State} - {Info.Title}";

        public override bool Equals(ReadOnlyAccountHistoryEntry? other)
            => other is not null && Id is not null && other.Id is not null && Id.Equals(other.Id);

        public override bool Equals(object? obj)
            => Equals(obj as ReadOnlyAccountHistoryEntry);

        public override int GetHashCode()
            => Id.GetHashCode();
    }
}