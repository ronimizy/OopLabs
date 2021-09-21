using System.Collections.Generic;
using Shops.Console.Base.Components;
using Shops.Console.Base.Views;
using Shops.Console.Components;
using Shops.Console.ViewModels;

namespace Shops.Console.Views
{
    public class FindCheapestView : View
    {
        private readonly FindCheapestViewModel _viewModel;

        public FindCheapestView(FindCheapestViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override string Title => "Find Cheapest";

        protected override IReadOnlyCollection<Component> GetComponents()
        {
            var productSelector = new ProductSelectorComponent(_viewModel.Products);
            productSelector.ValueChanged += _viewModel.OnProductSelected;

            var amountInput = new InputComponent<int>("Amount: ", v => v > 0);
            amountInput.ValueSubmitted += _viewModel.OnAmountEntered;

            var findButton = new ButtonComponent("Find", _viewModel.OnOperationConfirmed);

            return new Component[]
            {
                productSelector,
                amountInput,
                findButton,
            };
        }
    }
}