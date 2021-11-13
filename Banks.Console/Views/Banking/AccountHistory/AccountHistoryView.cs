using System.Collections.Generic;
using System.Linq;
using Banks.Console.ViewModels.Banking.AccountHistory;
using Spectre.Mvvm.Components;
using Spectre.Mvvm.Models;
using Spectre.Mvvm.Views;

namespace Banks.Console.Views.Banking.AccountHistory
{
    public class AccountHistoryView : View
    {
        private readonly AccountHistoryViewModel _viewModel;

        public AccountHistoryView(AccountHistoryViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override string Title => "History";

        protected override IReadOnlyCollection<Component> GetComponents()
        {
            NavigationElement[] elements = _viewModel.HistoryEntryViewModels
                .Select(e => new NavigationElement(e.Title, n => n.PushView(new AccountHistoryEntryView(e))))
                .ToArray();

            return new[] { new NavigationComponent(_viewModel.Navigator, elements) };
        }
    }
}