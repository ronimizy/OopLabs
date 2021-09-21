using System.Collections.Generic;
using Shops.Console.Base.Components;
using Shops.Console.Base.Views;
using Shops.Console.ViewModels;

namespace Shops.Console.Views
{
    public sealed class ShopListView : View
    {
        private readonly ShopListViewModel _viewModel;

        public ShopListView(ShopListViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override string Title => "Shop List";

        protected override IReadOnlyCollection<Component> GetComponents()
            => new[] { new NavigationComponent(_viewModel.Navigator, true, _viewModel.NavigationLinks) };
    }
}