using System;
using Banks.Accounts;
using Banks.Chronometers;

namespace Banks.Commands.BankAccountCommands
{
    public class RevertAccountCommand : AccountCommand
    {
        private readonly AccountCommand _revertCommand;

        public RevertAccountCommand(AccountCommand revertCommand)
            : base(revertCommand.Info)
        {
            _revertCommand = revertCommand;
        }

#pragma warning disable 8618
        private RevertAccountCommand() { }
#pragma warning restore 8618

        public override AccountCommand? RevertCommand => null;

        public override bool TryExecute(Account account, IChronometer chronometer, out DateTime? executedDateTime)
            => _revertCommand.TryExecute(account, chronometer, out executedDateTime);
    }
}