using System;
using Banks.Accounts;
using Banks.Commands;
using Banks.Models;
using Banks.Tools;

namespace Banks.ExceptionFactories
{
    internal static class AccountExceptionFactory
    {
        public static BanksException LoggingFailedException(Account account, AccountCommand command)
            => new BanksException($"Failed to log a command {command} at account {account}");

        public static BanksException AlreadyCanceledEntryException(ReadOnlyAccountHistoryEntry entry)
            => new BanksException($"Entry has already been canceled. {entry}");

        public static BanksException BalanceLimitBreakException(decimal balance, decimal limit, decimal requested)
            => new BanksException($"The account is limited to minimum of {limit}$, balance is {balance}$, {requested}$ requested");

        public static BanksException AlreadyContainsHistoryEntryException(Account account, ReadOnlyAccountHistoryEntry entry)
            => new BanksException($"BankAccount: {account} already contains history entry {entry}");

        public static BanksException NotContainingHistoryEntryException(Account account, ReadOnlyAccountHistoryEntry entry)
            => new BanksException($"BankAccount: {account} does not contain a history entry {entry}");

        public static BanksException AccountLockedForWithdrawingOperations(DateTime unlockDateTime)
            => new BanksException($"Withdrawing operations cannot be executed until {unlockDateTime}");

        public static BanksException FailedTransferException()
            => throw new BanksException(
                "Failed to complete transfer operation. Destination account couldn't receive transfer, origin account couldn't receive money back");

        public static BanksException SuspiciousLimitedOperationException(string operationType, decimal amount, decimal limit)
            => new BanksException(
                $"{operationType} operation of {amount}$ cannot be executed because the account is suspicious and limited to {limit}$ operations");
    }
}