using System.Linq;
using Banks.Accounts;
using Banks.Console.ViewModels.Banking.AccountHistory;
using Banks.Console.Views;
using Banks.Console.Views.Banking;
using Banks.Entities;
using Banks.Plans;
using Spectre.Mvvm.Interfaces;
using Spectre.Mvvm.Models;
using Utility.Extensions;

namespace Banks.Console.ViewModels.Banking.Accounts
{
    public class AccountViewModel
    {
        private readonly Client _client;
        private readonly IBank _bank;
        private readonly CentralBank _centralBank;
        private readonly Account _account;

        public AccountViewModel(Client client, IBank bank, Account account, INavigator navigator, CentralBank centralBank)
        {
            _centralBank = centralBank;
            _client = client.ThrowIfNull(nameof(client));
            _bank = bank.ThrowIfNull(nameof(bank));
            _account = account.ThrowIfNull(nameof(account));
            Navigator = navigator.ThrowIfNull(nameof(navigator));
        }

        public INavigator Navigator { get; }
        public string Title => _account.Id.ThrowIfNull(nameof(_account.Id)).ToString();
        public decimal Balance => _account.Balance;
        public AccountPlan Plan => _account.Plan.ThrowIfNull(nameof(Plan));

        public AccountHistoryViewModel AccountHistoryViewModel => new AccountHistoryViewModel(_account, _bank, _client, Navigator);

        public NavigationElement AccrualElement => new NavigationElement("Accrue Funds", n =>
        {
            n.PushView(new PromptView<decimal>("Amount: ", validator: PositiveNumberValidator, callback: d =>
            {
                _bank.AccrueFunds(_account, d);
                n.PopView();
            }));
        });

        public NavigationElement WithdrawalElement => new NavigationElement("Withdraw Funds", n =>
        {
            n.PushView(new PromptView<decimal>("Amount: ", validator: PositiveNumberValidator, callback: d =>
            {
                _bank.WithdrawFunds(_account, d);
                n.PopView();
            }));
        });

        public NavigationElement TransferElement => new NavigationElement("Transfer Funds", n =>
        {
            var viewModel = new TransferFundsViewModel(TransferFunds);
            n.PushView(new TransferFundsView(viewModel));
        });

        public NavigationElement GetSubscriptionManagingElement()
        {
            AccountPlan plan = _account.Plan.ThrowIfNull(nameof(_account.Plan));

            return plan.Subscribers.Contains(_client)
                ? new NavigationElement("Unsubscribe", _ => _bank.UnsubscribeFromPlanUpdates(_client, _account.Plan!))
                : new NavigationElement("Subscribe", _ => _bank.SubscribeToPlanUpdates(_client, _account.Plan!));
        }

        private static bool PositiveNumberValidator(decimal value)
            => value > 0;

        private void TransferFunds(TransferFundsViewModel viewModel)
        {
            Account receiver = _centralBank.GetAccount(viewModel.ReceiverId);
            _bank.TransferFunds(_account, receiver, viewModel.Amount);
            Navigator.PopView();
        }
    }
}