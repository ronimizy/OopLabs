using System;
using System.Linq;
using Banks.Chronometers;
using Banks.Commands.BankAccountCommands;
using Banks.Models;
using Banks.Tools;

namespace Banks.Accounts.Wrappers
{
    internal class CancelingAccountProxy : AccountWrapperBase
    {
        public CancelingAccountProxy(Account wrapped, IChronometer chronometer)
            : base(wrapped, chronometer) { }

        public CancelingAccountProxy(BanksDatabaseContext context)
            : base(context) { }

        internal override bool TryCancelOperation(Guid operationId)
        {
            ReadOnlyAccountHistoryEntry? entry = History.SingleOrDefault(e => operationId.Equals(e.Id));

            if (entry?.State is OperationState.Canceled || entry?.RevertCommand is null)
                return false;

            return Wrapped.TryExecuteCommand(entry.RevertCommand) && Wrapped.TryExecuteCommand(new CancelEntryAccountCommand(entry));
        }
    }
}