using System;
using Spectre.Console;
using Spectre.Mvvm.Interfaces;
using Spectre.Mvvm.ViewModels;

namespace Spectre.Mvvm.Views
{
    public class NavigationView : View
    {
        private readonly NavigationViewModel _viewModel;

        public NavigationView(Func<INavigator, View> initialViewFactory)
        {
            _viewModel = new NavigationViewModel(initialViewFactory);
        }

        public override string Title => string.Empty;

        public override void Draw()
        {
            _viewModel.ApplyViewSequenceActions();

            try
            {
                AnsiConsole.Markup($"[bold]{_viewModel.Title.EscapeMarkup()}[/]\n");
                _viewModel.CurrentView.Draw();
            }
            catch (Exception e)
            {
                _viewModel.OnError(e);
            }
        }
    }
}