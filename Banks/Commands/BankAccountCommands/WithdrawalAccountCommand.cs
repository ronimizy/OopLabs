using Banks.AccountInterfaces;
using Banks.Models;

namespace Banks.Commands.BankAccountCommands
{
    public class WithdrawalAccountCommand : GenericAccountCommand<IWithdrawingAccount>
    {
        internal WithdrawalAccountCommand(decimal amount, string additionalInfo = "")
            : base(new Info(nameof(WithdrawalAccountCommand), $"Withdrawal of {amount}$. {additionalInfo}"))
        {
            Amount = amount;
        }

#pragma warning disable 8618
        private WithdrawalAccountCommand() { }
#pragma warning restore 8618

        public decimal Amount { get; }
        public override AccountCommand RevertCommand => new AccrualAccountCommand(Amount);

        protected override void Execute(IWithdrawingAccount account)
            => account.WithdrawFunds(Amount);
    }
}