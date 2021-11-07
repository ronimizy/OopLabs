using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Banks.Commands;
using Banks.Entities;
using Banks.Models;
using Banks.Plans;

namespace Banks.Accounts
{
    public abstract class Account : IEquatable<Account>
    {
        [Key]
        public Guid? Id { get; private init; }

        [NotMapped]
        public abstract Client Owner { get; }

        [NotMapped]
        public abstract AccountPlan? Plan { get; }

        [NotMapped]
        public abstract decimal Balance { get; }

        [NotMapped]
        public abstract IReadOnlyCollection<ReadOnlyAccountHistoryEntry> History { get; }

        public abstract bool WithdrawalAllowed(decimal amount = 0);

        public bool Equals(Account? other)
            => other is not null && other.Id.Equals(Id);

        public override bool Equals(object? obj)
            => Equals(obj as Account);

        public override int GetHashCode()
            => Id.GetHashCode();

        public override string ToString()
            => $"{Id} - {Balance}$";

        internal bool TryExecuteCommand(AccountCommand command)
            => TryExecuteCommand(command, out _);

        internal abstract bool TryExecuteCommand(AccountCommand command, out DateTime? executedDateTime);
        internal abstract bool TryCancelOperation(Guid operationId);
    }
}