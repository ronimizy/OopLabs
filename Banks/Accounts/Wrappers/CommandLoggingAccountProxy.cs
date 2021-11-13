using System;
using Banks.Chronometers;
using Banks.Commands;
using Banks.Commands.BankAccountCommands;
using Banks.ExceptionFactories;
using Banks.Models;
using Banks.Tools;
using Utility.Extensions;

namespace Banks.Accounts.Wrappers
{
    internal class CommandLoggingAccountProxy : AccountWrapperBase
    {
        public CommandLoggingAccountProxy(Account wrapped, IChronometer chronometer)
            : base(wrapped, chronometer) { }

        public CommandLoggingAccountProxy(BanksDatabaseContext context)
            : base(context) { }

        internal override bool TryExecuteCommand(AccountCommand command, out DateTime? executedDateTime)
        {
            command.ThrowIfNull(nameof(command));

            if (!Wrapped.TryExecuteCommand(command, out executedDateTime))
                return false;

            executedDateTime = executedDateTime.ThrowIfNull(nameof(executedDateTime));
            var entry = new AccountHistoryEntry(executedDateTime.Value, Balance, command.Info);
            var logCommand = new LogEntryAccountCommand(entry);

            if (!Wrapped.TryExecuteCommand(logCommand))
                throw AccountExceptionFactory.LoggingFailedException(this, command);

            return true;
        }
    }
}