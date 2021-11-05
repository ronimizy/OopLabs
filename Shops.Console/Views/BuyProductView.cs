using System.Collections.Generic;
using Shops.Console.Components;
using Shops.Console.ViewModels;
using Spectre.Mvvm.Components;
using Spectre.Mvvm.Views;

namespace Shops.Console.Views
{
    public class BuyProductView : View
    {
        private readonly BuyProductViewModel _viewModel;

        public BuyProductView(BuyProductViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override string Title => "Buy Product";

        protected override IReadOnlyCollection<Component> GetComponents()
        {
            var productSelector = new ProductSelectorComponent(_viewModel.Products);
            productSelector.ValueChanged += _viewModel.OnProductSelected;

            var amountInput = new InputComponent<int>("Amount: ", v => v >= 0);
            amountInput.ValueSubmitted += _viewModel.OnAmountEntered;

            var submitSelector = new ConfirmationComponent(_viewModel.OnOperationConfirmed, _viewModel.OnOperationRejected);
            submitSelector.ValueChanged += _viewModel.OnConfirmationChoiceReceived;

            return new Component[]
            {
                new UserComponent(_viewModel.User),
                productSelector,
                amountInput,
                submitSelector,
            };
        }
    }
}