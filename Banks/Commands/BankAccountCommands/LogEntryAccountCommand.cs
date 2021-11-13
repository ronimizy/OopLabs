using Banks.AccountInterfaces;
using Banks.Models;

namespace Banks.Commands.BankAccountCommands
{
    public class LogEntryAccountCommand : GenericAccountCommand<IHistoryEditingAccount>
    {
        private readonly AccountHistoryEntry _entry;

        internal LogEntryAccountCommand(AccountHistoryEntry entry)
            : base(new Info(nameof(LogEntryAccountCommand), $"Logging of {entry}"))
        {
            _entry = entry;
        }

        public override AccountCommand? RevertCommand => null;

        protected override void Execute(IHistoryEditingAccount account)
            => account.LogEntry(_entry);
    }
}