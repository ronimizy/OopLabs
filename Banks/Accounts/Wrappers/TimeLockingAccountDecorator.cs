using System;
using Banks.AccountInterfaces;
using Banks.Chronometers;
using Banks.Commands.BankAccountCommands;
using Banks.ExceptionFactories;
using Banks.Tools;

namespace Banks.Accounts.Wrappers
{
    internal class TimeLockingAccountDecorator : AccountWrapperBase, IWithdrawingAccount
    {
        public TimeLockingAccountDecorator(DateTime unlockDateTime, Account wrapped, IChronometer chronometer)
            : base(wrapped, chronometer)
        {
            UnlockDateTime = unlockDateTime;
        }

        private TimeLockingAccountDecorator(BanksDatabaseContext context)
            : base(context) { }

        public DateTime UnlockDateTime { get; init; }

        public void WithdrawFunds(decimal amount)
        {
            if (Chronometer.CurrentDateTime < UnlockDateTime)
                throw AccountExceptionFactory.AccountLockedForWithdrawingOperations(UnlockDateTime);

            Wrapped.TryExecuteCommand(new WithdrawalAccountCommand(amount));
        }

        public override bool WithdrawalAllowed(decimal amount = 0)
            => Chronometer.CurrentDateTime >= UnlockDateTime;
    }
}