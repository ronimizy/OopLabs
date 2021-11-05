using System.Collections.Generic;
using Shops.Console.Components;
using Shops.Console.ViewModels;
using Spectre.Mvvm.Components;
using Spectre.Mvvm.Views;

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