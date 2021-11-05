using Banks.AccountInterfaces;
using Banks.Models;

namespace Banks.Commands.BankAccountCommands
{
    public class PercentsProcessingAccountCommand : GenericAccountCommand<IPercentAccrualAccount>
    {
        internal PercentsProcessingAccountCommand()
            : base(new Info(nameof(PercentsProcessingAccountCommand), "Processing percents")) { }

        public override AccountCommand? RevertCommand => null;

        protected override void Execute(IPercentAccrualAccount account)
            => account.ProcessPercentsAccrual();
    }
}