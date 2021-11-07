using System.Collections.Generic;
using Banks.Console.ViewModels.Banking;
using Spectre.Console;
using Spectre.Mvvm.Components;
using Spectre.Mvvm.Models;
using Spectre.Mvvm.Views;

namespace Banks.Console.Views.Banking
{
    public class CentralBankView : View
    {
        private readonly CentralBankViewModel _viewModel;

        public CentralBankView(CentralBankViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override string Title => "Central Bank";

        protected override IReadOnlyCollection<Component> GetComponents()
        {
            NavigationElement[] elements = _viewModel.Client switch
            {
                null => new[]
                {
                    _viewModel.LoginElement,
                    _viewModel.RegisterClientElement,
                },
                not null => new[]
                {
                    _viewModel.LogoutElement,
                    _viewModel.GetBanksElement(_viewModel.Client),
                    _viewModel.RegisterBankElement,
                    _viewModel.RewindTimeElement,
                    _viewModel.AccruePercentsElement,
                }
            };

            return new Component[]
            {
                new MarkupComponent(new Markup($"{_viewModel.Client?.EmailAddress}")),
                new NavigationComponent(_viewModel.Navigator, elements),
            };
        }
    }
}