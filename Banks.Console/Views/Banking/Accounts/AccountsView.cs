using System.Collections.Generic;
using System.Linq;
using Banks.Console.ViewModels.Banking.Accounts;
using Spectre.Mvvm.Components;
using Spectre.Mvvm.Views;

namespace Banks.Console.Views.Banking.Accounts
{
    public class AccountsView : View
    {
        private readonly AccountsViewModel _viewModel;

        public AccountsView(AccountsViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override string Title => "Accounts";

        protected override IReadOnlyCollection<Component> GetComponents()
        {
            View[] views = _viewModel.AccountViewModels
                .Select(a => (View)new AccountView(a))
                .ToArray();

            return new[] { new NavigationComponent(_viewModel.Navigator, true, views) };
        }
    }
}