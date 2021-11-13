using System.Collections.Generic;
using Banks.Console.ViewModels.Banking.Accounts;
using Banks.Console.Views.Banking.AccountHistory;
using Spectre.Console;
using Spectre.Mvvm.Components;
using Spectre.Mvvm.Models;
using Spectre.Mvvm.Views;

namespace Banks.Console.Views.Banking.Accounts
{
    public class AccountView : View
    {
        private readonly AccountViewModel _viewModel;

        public AccountView(AccountViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override string Title => _viewModel.Title;

        protected override IReadOnlyCollection<Component> GetComponents()
        {
            var titleComponent = new MarkupComponent(new Markup($"[bold]{_viewModel.Plan.Info.Title.EscapeMarkup()}[/]\n"));
            var balanceComponent = new MarkupComponent(new Markup($"{_viewModel.Balance}$\n"));
            var descriptionComponent = new MarkupComponent(new Markup($"{_viewModel.Plan.Info.Description.EscapeMarkup()}\n"));

            NavigationElement[] elements =
            {
                _viewModel.GetSubscriptionManagingElement(),
                _viewModel.AccrualElement,
                _viewModel.WithdrawalElement,
                _viewModel.TransferElement,
                new NavigationElement("History", n => n.PushView(new AccountHistoryView(_viewModel.AccountHistoryViewModel))),
            };

            return new Component[]
            {
                titleComponent,
                balanceComponent,
                descriptionComponent,
                new NavigationComponent(_viewModel.Navigator, elements),
            };
        }
    }
}