using System.Collections.Generic;
using Banks.Console.ViewModels.Mailing;
using Spectre.Console;
using Spectre.Mvvm.Components;
using Spectre.Mvvm.Models;
using Spectre.Mvvm.Views;

namespace Banks.Console.Views.Mailing
{
    public class MailingServiceView : View
    {
        private readonly MailingServiceViewModel _viewModel;

        public MailingServiceView(MailingServiceViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override string Title => "Mailing Service";

        protected override IReadOnlyCollection<Component> GetComponents()
        {
            NavigationElement[] elements = _viewModel.User switch
            {
                null => new[]
                {
                    _viewModel.LoginElement,
                    _viewModel.RegisterElement,
                },
                not null => new[]
                {
                    _viewModel.LogoutElement,
                    _viewModel.GetMailboxElement(_viewModel.User),
                }
            };

            return new Component[]
            {
                new MarkupComponent(new Markup($"{_viewModel.User?.Address}")),
                new NavigationComponent(_viewModel.Navigator, elements),
            };
        }
    }
}