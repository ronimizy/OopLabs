using System;
using Banks.Commands;
using Banks.Models;
using Utility.Extensions;

namespace Banks.OperationCancellation
{
    public class OperationCancellationEntry
    {
        public OperationCancellationEntry(ReadOnlyAccountHistoryEntry entry, AccountCommand revertCommand)
        {
            Entry = entry.ThrowIfNull(nameof(entry));
            RevertCommand = revertCommand.ThrowIfNull(nameof(entry));
        }

#pragma warning disable 8618
        private OperationCancellationEntry() { }
#pragma warning restore 8618

        public Guid? Id { get; private init; }
        public ReadOnlyAccountHistoryEntry Entry { get; private init; }
        public AccountCommand RevertCommand { get; private init; }

        public override string ToString()
            => Entry.ToString();
    }
}