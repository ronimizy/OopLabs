using System.Collections.Generic;
using System.Linq;
using Banks.Console.ViewModels.Banking.RegisterAccountPlans;
using Spectre.Console;
using Spectre.Mvvm.Components;
using Spectre.Mvvm.Views;

namespace Banks.Console.Views.Banking.RegisterAccountPlans
{
    public class RegisterDepositAccountPlanView : View
    {
        private readonly RegisterDepositAccountPlanViewModel _viewModel;

        public RegisterDepositAccountPlanView(RegisterDepositAccountPlanViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override string Title => "Register Deposit Account Plan";

        protected override IReadOnlyCollection<Component> GetComponents()
        {
            return _viewModel.Levels
                .OrderBy(l => l.Amount)
                .Select(l => (Component)new MarkupComponent(new Markup($"{l}\n")))
                .Append(_viewModel.GetNavigationComponent())
                .ToList();
        }
    }
}