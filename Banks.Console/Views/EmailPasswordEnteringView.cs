using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Banks.Console.ViewModels;
using Spectre.Mvvm.Components;
using Spectre.Mvvm.Views;
using Utility.Extensions;

namespace Banks.Console.Views
{
    public class EmailPasswordEnteringView : View
    {
        private readonly EmailPasswordEnteringViewModel _viewModel;

        public EmailPasswordEnteringView(EmailPasswordEnteringViewModel viewModel)
        {
            _viewModel = viewModel.ThrowIfNull(nameof(viewModel));
        }

        public override string Title => _viewModel.Title;

        protected override IReadOnlyCollection<Component> GetComponents()
        {
            var emailComponent = new InputComponent<string>("Email: ", s => new EmailAddressAttribute().IsValid(s));
            var passwordComponent = new InputComponent<string>("Password: ", s => s.Length > 5)
            {
                IsSecret = true,
            };
            var loginButton = new ButtonComponent(Title, _viewModel.ButtonPressed);

            emailComponent.ValueSubmitted += _viewModel.SubmitEmail;
            passwordComponent.ValueSubmitted += _viewModel.SubmitPassword;

            return new Component[]
            {
                emailComponent,
                passwordComponent,
                loginButton,
            };
        }
    }
}