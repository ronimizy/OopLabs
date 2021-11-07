using System.Collections.Generic;
using Banks.Console.ViewModels;
using Spectre.Mvvm.Components;
using Spectre.Mvvm.Views;

namespace Banks.Console.Views
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
            => new[] { _viewModel.NavigationComponent };
    }
}