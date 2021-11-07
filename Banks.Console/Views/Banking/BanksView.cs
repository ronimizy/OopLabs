using System.Collections.Generic;
using System.Linq;
using Banks.Console.ViewModels.Banking;
using Spectre.Mvvm.Components;
using Spectre.Mvvm.Models;
using Spectre.Mvvm.Views;

namespace Banks.Console.Views.Banking
{
    public class BanksView : View
    {
        private readonly BanksViewModel _viewModel;

        public BanksView(BanksViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override string Title => "Banks";

        protected override IReadOnlyCollection<Component> GetComponents()
        {
            NavigationElement[] elements = _viewModel.BankViewModels
                .Select(b => new NavigationElement(b.BankName, n => n.PushView(new BankView(b))))
                .ToArray();

            return new[] { new NavigationComponent(_viewModel.Navigator, elements) };
        }
    }
}