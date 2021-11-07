using System.Linq;
using Banks.Entities;
using Spectre.Mvvm.Interfaces;

namespace Banks.Console.ViewModels.Banking
{
    public class BanksViewModel
    {
        private readonly CentralBank _centralBank;
        private readonly Client _client;

        public BanksViewModel(CentralBank centralBank, Client client, INavigator navigator)
        {
            _centralBank = centralBank;
            _client = client;
            Navigator = navigator;
        }

        public INavigator Navigator { get; }

        public BankViewModel[] BankViewModels => _centralBank.Banks
            .Select(b => new BankViewModel(_centralBank, b, _client, Navigator))
            .ToArray();
    }
}