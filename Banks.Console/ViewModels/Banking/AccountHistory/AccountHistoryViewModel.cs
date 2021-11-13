using System.Linq;
using Banks.Accounts;
using Banks.Entities;
using Spectre.Mvvm.Interfaces;
using Utility.Extensions;

namespace Banks.Console.ViewModels.Banking.AccountHistory
{
    public class AccountHistoryViewModel
    {
        private readonly Account _account;
        private readonly IBank _bank;
        private readonly Client _client;

        public AccountHistoryViewModel(Account account, IBank bank, Client client, INavigator navigator)
        {
            _account = account.ThrowIfNull(nameof(account));
            _bank = bank.ThrowIfNull(nameof(bank));
            _client = client.ThrowIfNull(nameof(client));
            Navigator = navigator.ThrowIfNull(nameof(navigator));
        }

        public INavigator Navigator { get; }

        public AccountHistoryEntryViewModel[] HistoryEntryViewModels => _account.History
            .OrderByDescending(e => e.ExecutedTime)
            .Select(e => new AccountHistoryEntryViewModel(_client, _bank, e, Navigator, _account))
            .ToArray();
    }
}