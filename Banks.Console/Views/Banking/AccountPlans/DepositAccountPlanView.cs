using System.Collections.Generic;
using Banks.Console.ViewModels.Banking.AccountPlans;
using Spectre.Console;
using Spectre.Mvvm.Components;
using Spectre.Mvvm.Models;
using Spectre.Mvvm.Views;

namespace Banks.Console.Views.Banking.AccountPlans
{
    public class DepositAccountPlanView : View
    {
        private readonly DepositAccountPlanViewModel _viewModel;

        public DepositAccountPlanView(DepositAccountPlanViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override string Title => _viewModel.Info.Title;

        protected override IReadOnlyCollection<Component> GetComponents()
        {
            NavigationElement[] elements =
            {
                _viewModel.AddOrUpdateDepositPercentLevelElement,
                _viewModel.RemoveDepositAccountPlanLevelElement,
            };

            return new Component[]
            {
                new MarkupComponent(new Markup(_viewModel.Info.Description.EscapeMarkup())),
                new NavigationComponent(_viewModel.Navigator, elements),
            };
        }
    }
}