using System.Collections.Generic;
using Banks.Console.ViewModels.Banking.RegisterAccountPlans;
using Spectre.Mvvm.Components;
using Spectre.Mvvm.Views;

namespace Banks.Console.Views.Banking.RegisterAccountPlans
{
    public class RegisterDebitAccountPlanView : View
    {
        private readonly RegisterDebitAccountPlanViewModel _viewModel;

        public RegisterDebitAccountPlanView(RegisterDebitAccountPlanViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override string Title => "Register Debit Account Plan";

        protected override IReadOnlyCollection<Component> GetComponents()
        {
            var percentageInput = new InputComponent<decimal>("Percentage: ", d => d >= 0);
            var button = new ButtonComponent("Register", _viewModel.RegistrationSubmitted);

            percentageInput.ValueSubmitted += _viewModel.PercentageSubmitted;

            return new Component[]
            {
                percentageInput,
                button,
            };
        }
    }
}