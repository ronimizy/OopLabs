using System;
using System.ComponentModel.DataAnnotations.Schema;
using Banks.Accounts;
using Banks.Chronometers;
using Banks.Models;

namespace Banks.Commands
{
    public abstract class AccountCommand
    {
        protected AccountCommand(Info info)
        {
            Info = info;
        }

#pragma warning disable 8618
        protected AccountCommand() { }
#pragma warning restore 8618

        public Guid? Id { get; private init; }
        public Info Info { get; private init; }

        [NotMapped]
        public abstract AccountCommand? RevertCommand { get; }

        public abstract bool TryExecute(Account account, IChronometer chronometer, out DateTime? executedDateTime);
    }
}