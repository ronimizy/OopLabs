using System.Collections.Generic;
using Banks.Console.ViewModels.Banking.AccountHistory;
using Spectre.Console;
using Spectre.Mvvm.Components;
using Spectre.Mvvm.Views;

namespace Banks.Console.Views.Banking.AccountHistory
{
    public class AccountHistoryEntryView : View
    {
        private readonly AccountHistoryEntryViewModel _viewModel;

        public AccountHistoryEntryView(AccountHistoryEntryViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override string Title => _viewModel.Title;

        protected override IReadOnlyCollection<Component> GetComponents()
        {
            var dateComponent = new MarkupComponent(new Markup($"[bold]{_viewModel.ExecutedDateTime}[/]\n"));
            var balanceComponent = new MarkupComponent(new Markup($"[bold]{_viewModel.RemainingBalance}[/]\n"));
            var descriptionComponent = new MarkupComponent(new Markup($"[bold]{_viewModel.Description}[/]\n"));

            NavigationComponent navigationComponent = _viewModel.CanCancel
                ? new NavigationComponent(_viewModel.Navigator)
                : new NavigationComponent(_viewModel.Navigator, _viewModel.CancelElement);

            return new Component[]
            {
                dateComponent,
                balanceComponent,
                descriptionComponent,
                navigationComponent,
            };
        }
    }
}