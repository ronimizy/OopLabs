using System;
using Banks.Accounts;
using Banks.Entities;
using Banks.Models;
using Spectre.Mvvm.Interfaces;
using Spectre.Mvvm.Models;

namespace Banks.Console.ViewModels.Banking.AccountHistory
{
    public class AccountHistoryEntryViewModel
    {
        private readonly ReadOnlyAccountHistoryEntry _entry;
        private readonly Client _client;
        private readonly Account _account;
        private readonly IBank _bank;

        public AccountHistoryEntryViewModel(Client client, IBank bank, ReadOnlyAccountHistoryEntry entry, INavigator navigator, Account account)
        {
            _client = client;
            _bank = bank;
            _entry = entry;
            Navigator = navigator;
            _account = account;
        }

        public INavigator Navigator { get; }
        public string Title => _entry.Info.Title;
        public DateTime ExecutedDateTime => _entry.ExecutedTime;
        public decimal RemainingBalance => _entry.RemainingBalance;
        public string Description => _entry.Info.Description;
        public bool CanCancel => _client.Equals(_bank.Owner);

        public NavigationElement CancelElement => new NavigationElement("Cancel", n =>
        {
            _bank.CancelOperation(_client, _account, _entry);
            n.PopView();
        });
    }
}