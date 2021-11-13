using Banks.Models;
using Banks.OperationCancellation;
using Banks.Tools;

namespace Banks.ExceptionFactories
{
    internal static class OperationCancellationExceptionFactory
    {
        public static BanksException AlreadyLoggedEntryException(OperationCancellationEntry entry)
            => new BanksException($"{entry} is already logged into service");

        public static BanksException NotLoggedEntryException(ReadOnlyAccountHistoryEntry historyEntry)
            => new BanksException($"No cancellation entry logged for {historyEntry}");
    }
}