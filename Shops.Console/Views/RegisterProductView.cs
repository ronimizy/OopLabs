using System.Collections.Generic;
using Shops.Console.Components;
using Shops.Console.ViewModels;
using Spectre.Mvvm.Components;
using Spectre.Mvvm.Views;

namespace Shops.Console.Views
{
    public class RegisterProductView : View
    {
        private readonly RegisterProductViewModel _viewModel;

        public RegisterProductView(RegisterProductViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override string Title => "Register Product";

        protected override IReadOnlyCollection<Component> GetComponents()
        {
            var nameInput = new InputComponent<string>("Name: ");
            nameInput.ValueSubmitted += _viewModel.OnNameEntered;

            var descriptionInput = new InputComponent<string>("Description: ", optional: true);
            descriptionInput.ValueSubmitted += _viewModel.OnDescriptionEntered;

            var submitSelector = new ConfirmationComponent(_viewModel.OnOperationConfirmed, _viewModel.OnOperationRejected);
            submitSelector.ValueChanged += _viewModel.OnConfirmationChoiceReceived;

            return new Component[]
            {
                nameInput,
                descriptionInput,
                submitSelector,
            };
        }
    }
}