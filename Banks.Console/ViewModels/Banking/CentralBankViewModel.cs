using Banks.Console.Tools;
using Banks.Console.Views;
using Banks.Console.Views.Banking;
using Banks.Entities;
using Banks.Models;
using Banks.Tools;
using Spectre.Mvvm.Interfaces;
using Spectre.Mvvm.Models;
using Utility.Extensions;

namespace Banks.Console.ViewModels.Banking
{
    public class CentralBankViewModel
    {
        private readonly CentralBank _centralBank;
        private readonly SettableChronometer _chronometer;

        public CentralBankViewModel(CentralBank centralBank, INavigator navigator, SettableChronometer chronometer)
        {
            _centralBank = centralBank;
            Navigator = navigator;
            _chronometer = chronometer;
            Client = null;
        }

        public Client? Client { get; private set; }
        public INavigator Navigator { get; }

        public NavigationElement LoginElement => new NavigationElement("Login", n =>
        {
            var viewModel = new EmailPasswordEnteringViewModel("Login", Login);
            n.PushView(new EmailPasswordEnteringView(viewModel));
        });

        public NavigationElement LogoutElement => new NavigationElement("Logout", _ => Client = null);

        public NavigationElement RegisterClientElement => new NavigationElement("Register", n =>
        {
            var viewModel = new RegisterClientViewModel(n, RegisterClient);
            n.PushView(new RegisterClientView(viewModel));
        });

        public NavigationElement RegisterBankElement => new NavigationElement("Register Bank", n =>
        {
            var viewModel = new RegisterBankViewModel(RegisterBank);
            n.PushView(new RegisterBankView(viewModel));
        });

        public NavigationElement RewindTimeElement => new NavigationElement("Rewind Time", n =>
        {
            var viewModel = new TimeRewindViewModel(_chronometer, Navigator);
            n.PushView(new TimeRewindView(viewModel));
        });

        public NavigationElement AccruePercentsElement => new NavigationElement("Accrue Percents", _ => _centralBank.AccruePercents());

        public NavigationElement GetBanksElement(Client client)
        {
            return new NavigationElement("Banks", n =>
            {
                var viewModel = new BanksViewModel(_centralBank, client, n);
                n.PushView(new BanksView(viewModel));
            });
        }

        private void RegisterClient(RegisterClientViewModel viewModel)
        {
            IBuilder<Client> builder = Client.BuildClient
                .Called(viewModel.Name.ThrowIfNull(nameof(viewModel.Name)), viewModel.Surname.ThrowIfNull(nameof(viewModel.Surname)))
                .WithPassword(viewModel.Password.ThrowIfNull(nameof(viewModel.Password)))
                .WithEmailAddress(viewModel.EmailAddress.ThrowIfNull(nameof(viewModel.EmailAddress)))
                .WithAddress(viewModel.Address)
                .WithPassportData(viewModel.PassportData)
                .Builder;

            Client = _centralBank.RegisterClient(builder);
            Navigator.PopView();
        }

        private void Login(EmailPasswordEnteringViewModel viewModel)
        {
            var email = new EmailAddress(viewModel.Email);
            Client = _centralBank.Login(email, viewModel.Password);
            Navigator.PopView();
        }

        private void RegisterBank(RegisterBankViewModel viewModel)
        {
            Client = Client.ThrowIfNull(nameof(Client));
            var limit = new SuspiciousLimitPolicy(viewModel.Limit);
            _centralBank.RegisterBank(viewModel.Name, Client, limit);
            Navigator.PopView();
        }
    }
}