using System.Collections.Generic;
using Banks.Console.ViewModels.Banking;
using Spectre.Mvvm.Components;
using Spectre.Mvvm.Views;

namespace Banks.Console.Views.Banking
{
    public class RegisterBankView : View
    {
        private readonly RegisterBankViewModel _viewModel;

        public RegisterBankView(RegisterBankViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override string Title => "Register Bank";

        protected override IReadOnlyCollection<Component> GetComponents()
        {
            var nameComponent = new InputComponent<string>("Bank name: ");
            var limitComponent = new InputComponent<decimal>("Suspicious account limit: ");
            var registerButton = new ButtonComponent("Register", _viewModel.OperationSubmitted);

            nameComponent.ValueSubmitted += _viewModel.NameSubmitted;
            limitComponent.ValueSubmitted += _viewModel.LimitSubmitted;

            return new Component[]
            {
                nameComponent,
                limitComponent,
                registerButton,
            };
        }
    }
}