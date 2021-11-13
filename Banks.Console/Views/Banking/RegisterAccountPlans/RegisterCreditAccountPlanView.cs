using System.Collections.Generic;
using Banks.Console.ViewModels.Banking.RegisterAccountPlans;
using Spectre.Mvvm.Components;
using Spectre.Mvvm.Views;

namespace Banks.Console.Views.Banking.RegisterAccountPlans
{
    public class RegisterCreditAccountPlanView : View
    {
        private readonly RegisterCreditAccountPlanViewModel _viewModel;

        public RegisterCreditAccountPlanView(RegisterCreditAccountPlanViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override string Title => "Register Credit Account Plan";

        protected override IReadOnlyCollection<Component> GetComponents()
        {
            var percentComponent = new InputComponent<decimal>("Percent: ", d => d > 0);
            var limitComponent = new InputComponent<decimal>("Limit: ");
            var button = new ButtonComponent("Register", _viewModel.ButtonPressed);

            percentComponent.ValueSubmitted += _viewModel.PercentSubmitted;
            limitComponent.ValueSubmitted += _viewModel.LimitSubmitted;

            return new Component[]
            {
                percentComponent,
                limitComponent,
                button,
            };
        }
    }
}