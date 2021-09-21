using System.Collections.Generic;
using Shops.Console.Base.Components;
using Shops.Console.Base.Views;
using Shops.Console.Components;
using Shops.Console.ViewModels;

namespace Shops.Console.Views
{
    public class RegisterShopView : View
    {
        private readonly RegisterShopViewModel _viewModel;

        public RegisterShopView(RegisterShopViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override string Title => "Register Shop";

        protected override IReadOnlyCollection<Component> GetComponents()
        {
            var nameInput = new InputComponent<string>("Name: ");
            nameInput.ValueSubmitted += _viewModel.OnNameEntered;

            var locationInput = new InputComponent<string>("Location: ");
            locationInput.ValueSubmitted += _viewModel.OnLocationEntered;

            var submitSelector = new ConfirmationComponent(_viewModel.OnOperationConfirmed, _viewModel.OnOperationRejected);
            submitSelector.ValueChanged += _viewModel.OnSubmitChoiceReceived;

            return new Component[]
            {
                nameInput,
                locationInput,
                submitSelector,
            };
        }
    }
}