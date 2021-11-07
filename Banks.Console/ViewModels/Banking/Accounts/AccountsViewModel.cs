using System.Linq;
using Banks.Entities;
using Spectre.Mvvm.Interfaces;
using Utility.Extensions;

namespace Banks.Console.ViewModels.Banking.Accounts
{
    public class AccountsViewModel
    {
        private readonly Client _client;
        private readonly Bank _bank;

        public AccountsViewModel(Client client, Bank bank, INavigator navigator)
        {
            Navigator = navigator;
            _client = client.ThrowIfNull(nameof(client));
            _bank = bank.ThrowIfNull(nameof(bank));
        }

        public INavigator Navigator { get; }

        public AccountViewModel[] AccountViewModels => _bank.AccountsForClient(_client)
            .Select(a => new AccountViewModel(_client, _bank, a, Navigator))
            .ToArray();
    }
}