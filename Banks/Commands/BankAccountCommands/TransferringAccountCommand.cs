using System.ComponentModel.DataAnnotations.Schema;
using Banks.AccountInterfaces;
using Banks.Accounts;
using Banks.ExceptionFactories;
using Banks.Models;
using Utility.Extensions;

namespace Banks.Commands.BankAccountCommands
{
    public class TransferringAccountCommand : GenericAccountCommand<IWithdrawingAccount>
    {
        internal TransferringAccountCommand(decimal amount, Account originAccount, Account destinationAccount)
            : base(new Info(nameof(TransferringAccountCommand), $"Transferring {amount}$ from {originAccount} to {destinationAccount}"))
        {
            Amount = amount;
            OriginAccount = originAccount.ThrowIfNull(nameof(originAccount));
            DestinationAccount = destinationAccount.ThrowIfNull(nameof(destinationAccount));
        }

#pragma warning disable 8618
        private TransferringAccountCommand() { }
#pragma warning restore 8618

        public decimal Amount { get; private init; }
        public Account OriginAccount { get; private init; }
        public Account DestinationAccount { get; private init; }

        [NotMapped]
        public override AccountCommand RevertCommand => new TransferringAccountCommand(
            Amount, DestinationAccount, OriginAccount);

        protected override void Execute(IWithdrawingAccount account)
        {
            account.WithdrawFunds(Amount);

            if (!DestinationAccount.TryExecuteCommand(new AccrualAccountCommand(Amount, $"Transfer from {OriginAccount}")) &&
                OriginAccount.TryExecuteCommand(new AccrualAccountCommand(Amount, $"Money return because of failed transfer to {DestinationAccount}")))
                throw AccountExceptionFactory.FailedTransferException();
        }
    }
}