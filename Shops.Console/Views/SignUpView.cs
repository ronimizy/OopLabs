using System.Collections.Generic;
using Shops.Console.Base.Components;
using Shops.Console.Base.Views;
using Shops.Console.ViewModels;

namespace Shops.Console.Views
{
    public class SignUpView : View
    {
        private readonly SignUpViewModel _viewModel;

        public SignUpView(SignUpViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override string Title => "Sign Up";

        protected override IReadOnlyCollection<Component> GetComponents()
        {
            var usernameInput = new InputComponent<string>("Username: ");
            usernameInput.ValueSubmitted += _viewModel.OnUsernameEntered;

            var balanceInput = new InputComponent<double>("Balance: ", v => v >= 0);
            balanceInput.ValueSubmitted += _viewModel.OnBalanceEntered;

            var buttonComponent = new ButtonComponent("Proceed", _viewModel.OnProceed);

            return new Component[]
            {
                usernameInput,
                balanceInput,
                buttonComponent,
            };
        }
    }
}