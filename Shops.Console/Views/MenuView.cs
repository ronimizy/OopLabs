using System.Collections.Generic;
using Shops.Console.Base.Components;
using Shops.Console.Base.Views;
using Shops.Console.Components;
using Shops.Console.ViewModels;

namespace Shops.Console.Views
{
    public class MenuView : View
    {
        private readonly MenuViewModel _viewModel;

        public MenuView(MenuViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override string Title => "Menu";

        protected override IReadOnlyCollection<Component> GetComponents()
            => new Component[]
            {
                new UserComponent(_viewModel.User),
                new NavigationComponent(_viewModel.Navigator, false, _viewModel.NavigationLinks),
            };
    }
}