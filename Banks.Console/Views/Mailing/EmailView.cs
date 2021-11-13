using System.Collections.Generic;
using Banks.Console.ViewModels.Mailing;
using Banks.Models;
using Spectre.Console;
using Spectre.Mvvm.Components;
using Spectre.Mvvm.Views;

namespace Banks.Console.Views.Mailing
{
    public class EmailView : View
    {
        private readonly EmailViewModel _viewModel;

        public EmailView(EmailViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override string Title => _viewModel.Title;

        protected override IReadOnlyCollection<Component> GetComponents()
        {
            Email email = _viewModel.Email;

            return new Component[]
            {
                new MarkupComponent(new Markup($"From: {email.Sender}\n")),
                new MarkupComponent(new Markup("Message: \n")),
                new MarkupComponent(new Markup($"{email.Message.Body}")),
                new NavigationComponent(_viewModel.Navigator),
            };
        }
    }
}