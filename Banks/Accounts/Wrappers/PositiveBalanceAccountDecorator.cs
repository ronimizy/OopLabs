using Banks.AccountInterfaces;
using Banks.Chronometers;
using Banks.Commands.BankAccountCommands;
using Banks.ExceptionFactories;
using Banks.Tools;

namespace Banks.Accounts.Wrappers
{
    internal class PositiveBalanceAccountDecorator : AccountWrapperBase, IWithdrawingAccount
    {
        public PositiveBalanceAccountDecorator(Account wrapped, IChronometer chronometer)
            : base(wrapped, chronometer) { }

#pragma warning disable 8618
        private PositiveBalanceAccountDecorator(BanksDatabaseContext context)
#pragma warning restore 8618
            : base(context) { }

        public void WithdrawFunds(decimal amount)
        {
            if (Balance - amount < 0)
                throw AccountExceptionFactory.BalanceLimitBreakException(Balance, 0, amount);

            Wrapped.TryExecuteCommand(new WithdrawalAccountCommand(amount));
        }

        public override bool WithdrawalAllowed(decimal amount = 0)
            => Balance - amount >= 0;
    }
}