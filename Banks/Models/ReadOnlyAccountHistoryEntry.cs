using System;
using Banks.Commands;

namespace Banks.Models
{
    public abstract class ReadOnlyAccountHistoryEntry : IEquatable<ReadOnlyAccountHistoryEntry>
    {
        public abstract Guid? Id { get; protected init; }
        public abstract DateTime ExecutedTime { get; protected init; }
        public abstract decimal RemainingBalance { get; protected init; }
        public abstract OperationState State { get; protected set; }
        public abstract Info Info { get; protected init; }
        public abstract AccountCommand? RevertCommand { get; protected set; }
        public abstract bool Equals(ReadOnlyAccountHistoryEntry? other);
        public abstract override string ToString();
    }
}