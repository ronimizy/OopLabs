using System.ComponentModel.DataAnnotations.Schema;
using Banks.AccountInterfaces;
using Banks.Chronometers;
using Banks.Commands.BankAccountCommands;
using Banks.ExceptionFactories;
using Banks.Plans;
using Banks.Tools;
using Utility.Extensions;

namespace Banks.Accounts.Wrappers
{
    internal class CreditAccountDecorator : AccountWrapperBase, IAccrualAccount, IWithdrawingAccount
    {
        private readonly CreditAccountPlan _plan;

        public CreditAccountDecorator(CreditAccountPlan plan, Account wrapped, IChronometer chronometer)
            : base(wrapped, chronometer)
        {
            _plan = plan.ThrowIfNull(nameof(plan));
        }

#pragma warning disable 8618
        private CreditAccountDecorator(BanksDatabaseContext context)
#pragma warning restore 8618
            : base(context) { }

        [NotMapped]
        public override AccountPlan Plan => _plan;

        public void AccrueFunds(decimal amount)
        {
            decimal commission = _plan.CalculateAccrualCommission(Balance, amount);

            if (Balance + amount - commission < _plan.Limit)
                throw AccountExceptionFactory.BalanceLimitBreakException(Balance, _plan.Limit, -amount + commission);

            if (Wrapped.TryExecuteCommand(new AccrualAccountCommand(amount)) && commission is not 0)
                Wrapped.TryExecuteCommand(new WithdrawalAccountCommand(amount, "Credit account commission withdrawal."));
        }

        public void WithdrawFunds(decimal amount)
        {
            decimal commission = _plan.CalculateAccrualCommission(Balance, amount);

            if (Balance - amount - commission < _plan.Limit)
                throw AccountExceptionFactory.BalanceLimitBreakException(Balance, _plan.Limit, amount + commission);

            if (Wrapped.TryExecuteCommand(new WithdrawalAccountCommand(amount)) && commission is not 0)
                Wrapped.TryExecuteCommand(new WithdrawalAccountCommand(amount, "Credit account commission withdrawal."));
        }

        public override bool WithdrawalAllowed(decimal amount = 0)
            => Balance - _plan.CalculateWithdrawalCommission(Balance, amount) >= _plan.Limit;
    }
}