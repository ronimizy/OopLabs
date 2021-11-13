using Banks.AccountInterfaces;
using Banks.Models;

namespace Banks.Commands.BankAccountCommands
{
    public class AccrualAccountCommand : GenericAccountCommand<IAccrualAccount>
    {
        internal AccrualAccountCommand(decimal amount, string additionalInfo = "")
            : base(new Info(nameof(AccrualAccountCommand), $"Accrual of {amount}$. {additionalInfo}"))
        {
            Amount = amount;
        }

#pragma warning disable 8618
        private AccrualAccountCommand() { }
#pragma warning restore 8618

        public decimal Amount { get; }
        public override AccountCommand RevertCommand => new WithdrawalAccountCommand(Amount);

        protected override void Execute(IAccrualAccount account)
            => account.AccrueFunds(Amount);
    }
}