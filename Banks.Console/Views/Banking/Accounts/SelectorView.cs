using System.Collections.Generic;
using Banks.Console.ViewModels;
using Spectre.Mvvm.Components;
using Spectre.Mvvm.Views;

namespace Banks.Console.Views.Banking.Accounts
{
    public class SelectorView<T> : View
    {
        private readonly SelectorViewModel<T> _viewModel;

        public SelectorView(SelectorViewModel<T> viewModel)
        {
            _viewModel = viewModel;
        }

        public override string Title => _viewModel.Title;

        protected override IReadOnlyCollection<Component> GetComponents()
            => new[] { new NavigationComponent(_viewModel.Navigator, _viewModel.Elements) };
    }
}