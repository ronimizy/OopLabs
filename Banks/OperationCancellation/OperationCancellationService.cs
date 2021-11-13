using System.Linq;
using Banks.Commands;
using Banks.ExceptionFactories;
using Banks.Models;
using Banks.Tools;
using Utility.Extensions;

namespace Banks.OperationCancellation
{
    public class OperationCancellationService
    {
        private readonly BanksDatabaseContext _databaseContext;

        public OperationCancellationService(BanksDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public void AddCancellationEntry(OperationCancellationEntry entry)
        {
            if (_databaseContext.OperationCancellationEntries.AsQueryable().Any(e => e.Entry.Equals(entry.Entry)))
                throw OperationCancellationExceptionFactory.AlreadyLoggedEntryException(entry);

            _databaseContext.OperationCancellationEntries.Add(entry);
            _databaseContext.SaveChanges();
        }

        public void RemoveCancellationEntry(ReadOnlyAccountHistoryEntry historyEntry)
        {
            OperationCancellationEntry cancellationEntry = _databaseContext.OperationCancellationEntries
                .AsEnumerable()
                .SingleOrDefault(e => e.Entry.Equals(historyEntry))
                .ThrowIfNull(OperationCancellationExceptionFactory.NotLoggedEntryException(historyEntry));

            _databaseContext.OperationCancellationEntries.Remove(cancellationEntry);
            _databaseContext.SaveChanges();
        }

        public AccountCommand? GetCancellationCommand(ReadOnlyAccountHistoryEntry historyEntry)
        {
            OperationCancellationEntry? cancellationEntry = _databaseContext.OperationCancellationEntries
                .AsEnumerable()
                .SingleOrDefault(e => e.Entry.Equals(historyEntry));

            return cancellationEntry?.RevertCommand;
        }
    }
}