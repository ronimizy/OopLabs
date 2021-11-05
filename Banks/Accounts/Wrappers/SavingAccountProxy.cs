using System;
using Banks.Commands;
using Banks.Tools;
using Utility.Extensions;

namespace Banks.Accounts.Wrappers
{
    internal class SavingAccountProxy : AccountWrapperBase
    {
        private readonly BanksDatabaseContext _context;

        public SavingAccountProxy(Account wrapped, BanksDatabaseContext context)
            : base(wrapped, context.Chronometer)
        {
            _context = context.ThrowIfNull(nameof(context));
        }

#pragma warning disable 8618
        private SavingAccountProxy(BanksDatabaseContext context)
#pragma warning restore 8618
            : base(context)
        {
            _context = context;
        }

        internal override bool TryExecuteCommand(AccountCommand command, out DateTime? executedDateTime)
        {
            command.ThrowIfNull(nameof(command));

            executedDateTime = null;
            if (!Wrapped.TryExecuteCommand(command, out executedDateTime))
                return false;

            _context.Accounts.Update(this);
            _context.SaveChanges();
            return true;
        }

        internal override bool TryCancelOperation(Guid operationId)
        {
            if (!Wrapped.TryCancelOperation(operationId))
                return false;

            _context.Accounts.Update(this);
            _context.SaveChanges();
            return true;
        }
    }
}