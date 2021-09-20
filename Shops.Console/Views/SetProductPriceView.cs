using System.Collections.Generic;
using Shops.Console.Base.Components;
using Shops.Console.Base.Views;
using Shops.Console.Components;
using Shops.Console.ViewModels;
using Shops.Entities;

namespace Shops.Console.Views
{
    public class SetProductPriceView : View
    {
        private readonly SetProductPriceViewModel _viewModel;

        public SetProductPriceView(SetProductPriceViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override string Title => "Set Product Price";

        protected override IReadOnlyCollection<Component> GetComponents()
        {
            var productSelector = new SelectorComponent<Product>("Select a product", _viewModel.Products);
            productSelector.ValueChanged += _viewModel.OnProductSelected;

            var amountInput = new InputComponent<double>("Price: ", v => v >= 0);
            amountInput.ValueSubmitted += _viewModel.OnPriceEntered;

            var submitSelector = new ConfirmationComponent(_viewModel.OnOperationConfirmed, _viewModel.OnOperationRejected);
            submitSelector.ValueChanged += _viewModel.OnConfirmationChoiceReceived;

            return new Component[]
            {
                productSelector,
                amountInput,
                submitSelector,
            };
        }
    }
}