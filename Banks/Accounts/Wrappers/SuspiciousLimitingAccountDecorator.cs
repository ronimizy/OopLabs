using Banks.AccountInterfaces;
using Banks.Chronometers;
using Banks.Commands.BankAccountCommands;
using Banks.ExceptionFactories;
using Banks.Models;
using Banks.Tools;
using Utility.Extensions;

namespace Banks.Accounts.Wrappers
{
    internal class SuspiciousLimitingAccountDecorator : AccountWrapperBase, IAccrualAccount, IWithdrawingAccount
    {
        public SuspiciousLimitingAccountDecorator(Account wrapped, SuspiciousLimitPolicy limitPolicy, IChronometer chronometer)
            : base(wrapped, chronometer)
        {
            SuspiciousLimitPolicy = limitPolicy.ThrowIfNull(nameof(limitPolicy));
        }

#pragma warning disable 8618
        private SuspiciousLimitingAccountDecorator(BanksDatabaseContext context)
#pragma warning restore 8618
            : base(context) { }

        public SuspiciousLimitPolicy SuspiciousLimitPolicy { get; init; }

        public void AccrueFunds(decimal amount)
        {
            if (Owner.IsSuspicious && amount > SuspiciousLimitPolicy.Limit)
                throw AccountExceptionFactory.SuspiciousLimitedOperationException("Accrual", amount, SuspiciousLimitPolicy.Limit);

            Wrapped.TryExecuteCommand(new AccrualAccountCommand(amount));
        }

        public void WithdrawFunds(decimal amount)
        {
            if (Owner.IsSuspicious && amount > SuspiciousLimitPolicy.Limit)
                throw AccountExceptionFactory.SuspiciousLimitedOperationException("Withdrawal", amount, SuspiciousLimitPolicy.Limit);

            Wrapped.TryExecuteCommand(new WithdrawalAccountCommand(amount));
        }
    }
}