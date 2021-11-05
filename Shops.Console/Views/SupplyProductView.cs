using System.Collections.Generic;
using Shops.Console.Components;
using Shops.Console.ViewModels;
using Spectre.Mvvm.Components;
using Spectre.Mvvm.Views;

namespace Shops.Console.Views
{
    public class SupplyProductView : View
    {
        private readonly SupplyProductViewModel _viewModel;

        public SupplyProductView(SupplyProductViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override string Title => "Supply Products";

        protected override IReadOnlyCollection<Component> GetComponents()
        {
            var productSelector = new ProductSelectorComponent(_viewModel.Products);
            productSelector.ValueChanged += _viewModel.OnProductSelected;

            var priceInput = new InputComponent<double>("Price: ", v => v >= 0);
            priceInput.ValueSubmitted += _viewModel.OnPriceEntered;

            var amountInput = new InputComponent<int>("Amount: ", v => v >= 0);
            amountInput.ValueSubmitted += _viewModel.OnAmountEntered;

            var submitSelector = new ConfirmationComponent(_viewModel.OnOperationConfirmed, _viewModel.OnOperationRejected);
            submitSelector.ValueChanged += _viewModel.OnConfirmationChoiceReceived;

            return new Component[]
            {
                productSelector,
                priceInput,
                amountInput,
                submitSelector,
            };
        }
    }
}