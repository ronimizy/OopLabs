using Banks.AccountInterfaces;
using Banks.Models;
using Utility.Extensions;

namespace Banks.Commands.BankAccountCommands
{
    public class CancelEntryAccountCommand : GenericAccountCommand<IHistoryEditingAccount>
    {
        internal CancelEntryAccountCommand(ReadOnlyAccountHistoryEntry entry)
            : base(new Info(nameof(CancelEntryAccountCommand), $"Cancellation of {entry.Info}"))
        {
            entry.ThrowIfNull(nameof(entry));
            entry.Id.ThrowIfNull(nameof(entry.Id));

            Entry = entry;
        }

#pragma warning disable 8618
        private CancelEntryAccountCommand() { }
#pragma warning restore 8618

        public ReadOnlyAccountHistoryEntry Entry { get; }

        public override AccountCommand? RevertCommand => null;

        protected override void Execute(IHistoryEditingAccount account)
            => account.CancelEntry(Entry);
    }
}