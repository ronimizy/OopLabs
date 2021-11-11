using System.Linq;
using Banks.Entities;
using Spectre.Mvvm.Interfaces;
using Utility.Extensions;

namespace Banks.Console.ViewModels.Banking.Accounts
{
    public class AccountsViewModel
    {
        private readonly Client _client;
        private readonly IBank _bank;
        private readonly CentralBank _centralBank;

        public AccountsViewModel(Client client, IBank bank, INavigator navigator, CentralBank centralBank)
        {
            Navigator = navigator;
            _centralBank = centralBank;
            _client = client.ThrowIfNull(nameof(client));
            _bank = bank.ThrowIfNull(nameof(bank));
        }

        public INavigator Navigator { get; }

        public AccountViewModel[] AccountViewModels => _bank.AccountsForClient(_client)
            .Select(a => new AccountViewModel(_client, _bank, a, Navigator, _centralBank))
            .ToArray();
    }
}