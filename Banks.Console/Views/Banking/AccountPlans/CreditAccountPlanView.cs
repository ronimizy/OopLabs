using System.Collections.Generic;
using Banks.Console.ViewModels.Banking.AccountPlans;
using Spectre.Console;
using Spectre.Mvvm.Components;
using Spectre.Mvvm.Models;
using Spectre.Mvvm.Views;

namespace Banks.Console.Views.Banking.AccountPlans
{
    public class CreditAccountPlanView : View
    {
        private readonly CreditAccountPlanViewModel _viewModel;

        public CreditAccountPlanView(CreditAccountPlanViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override string Title => _viewModel.Info.Title;

        protected override IReadOnlyCollection<Component> GetComponents()
        {
            NavigationElement[] elements =
            {
                _viewModel.UpdateLimitElement,
                _viewModel.UpdatePercentageElement,
            };

            return new Component[]
            {
                new MarkupComponent(new Markup(_viewModel.Info.Description.EscapeMarkup())),
                new NavigationComponent(_viewModel.Navigator, elements),
            };
        }
    }
}