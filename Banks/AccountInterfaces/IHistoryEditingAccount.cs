using Banks.Models;

namespace Banks.AccountInterfaces
{
    public interface IHistoryEditingAccount
    {
        void LogEntry(AccountHistoryEntry entry);
        void CancelEntry(ReadOnlyAccountHistoryEntry entry);
    }
}