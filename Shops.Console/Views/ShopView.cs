using System.Collections.Generic;
using Shops.Console.Base.Components;
using Shops.Console.Base.Views;
using Shops.Console.Components;
using Shops.Console.ViewModels;
using Spectre.Console;

namespace Shops.Console.Views
{
    public class ShopView : View
    {
        private readonly ShopViewModel _viewModel;

        public ShopView(ShopViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override string Title => _viewModel.ShopName;

        protected override IReadOnlyCollection<Component> GetComponents()
            => new Component[]
            {
                new MarkupComponent(new Markup($"[italic]{_viewModel.ShopLocation.EscapeMarkup()}[/]\n")),
                new UserComponent(_viewModel.User),
                new TableComponent("Products", _viewModel.Headers, _viewModel.Data),
                new NavigationComponent(_viewModel.Navigator, true, _viewModel.NavigationLinks),
            };
    }
}