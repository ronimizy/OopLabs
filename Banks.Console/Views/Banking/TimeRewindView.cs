using System.Collections.Generic;
using Banks.Console.ViewModels.Banking;
using Spectre.Console;
using Spectre.Mvvm.Components;
using Spectre.Mvvm.Models;
using Spectre.Mvvm.Views;

namespace Banks.Console.Views.Banking
{
    public class TimeRewindView : View
    {
        private readonly TimeRewindViewModel _viewModel;

        public TimeRewindView(TimeRewindViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override string Title => "Rewind Time";

        protected override IReadOnlyCollection<Component> GetComponents()
        {
            NavigationElement[] elements =
            {
                _viewModel.IncreaseYearElement,
                _viewModel.IncreaseMonthElement,
                _viewModel.IncreaseDayElement,
            };

            return new Component[]
            {
                new MarkupComponent(new Markup($"{_viewModel.CurrentDateTime}")),
                new NavigationComponent(_viewModel.Navigator, elements),
            };
        }
    }
}