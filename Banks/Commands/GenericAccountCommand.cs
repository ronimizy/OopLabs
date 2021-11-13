using System;
using Banks.Accounts;
using Banks.Chronometers;
using Banks.Models;

namespace Banks.Commands
{
    public abstract class GenericAccountCommand<TAccount> : AccountCommand
    {
        protected GenericAccountCommand(Info info)
            : base(info) { }

#pragma warning disable 8618
        protected GenericAccountCommand() { }
#pragma warning restore 8618

        public override bool TryExecute(Account account, IChronometer chronometer, out DateTime? executedDateTime)
        {
            if (account is not TAccount conformingAccount || chronometer is null)
            {
                executedDateTime = null;
                return false;
            }

            Execute(conformingAccount);
            executedDateTime = chronometer.CurrentDateTime;
            return true;
        }

        protected abstract void Execute(TAccount account);
    }
}