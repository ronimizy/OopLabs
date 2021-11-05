using System.Collections.Generic;
using Shops.Console.ViewModels;
using Spectre.Mvvm.Components;
using Spectre.Mvvm.Views;

namespace Shops.Console.Views
{
    public class ProductListView : View
    {
        private readonly ProductListViewModel _viewModel;

        public ProductListView(ProductListViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override string Title => "Product List";

        protected override IReadOnlyCollection<Component> GetComponents()
            => new Component[]
            {
                new TableComponent(string.Empty, _viewModel.Headers, _viewModel.Data),
                new NavigationComponent(_viewModel.Navigator, true),
            };
    }
}