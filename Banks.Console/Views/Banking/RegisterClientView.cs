using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Banks.Console.ViewModels.Banking;
using Spectre.Console;
using Spectre.Mvvm.Components;
using Spectre.Mvvm.Views;

namespace Banks.Console.Views.Banking
{
    public class RegisterClientView : View
    {
        private readonly RegisterClientViewModel _viewModel;

        public RegisterClientView(RegisterClientViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override string Title => "Register";

        protected override IReadOnlyCollection<Component> GetComponents()
        {
            return new[]
            {
                GetNameComponent(),
                GetSurnameComponent(),
                GetEmailComponent(),
                GetPasswordComponent(),
                GetAddressComponent(),
                _viewModel.GetNavigationComponent(),
            };
        }

        private Component GetNameComponent()
            => _viewModel.Name switch
            {
                null => new InputComponent<string>("Name: ", defaultHandler: _viewModel.NameSubmitted),
                not null => new MarkupComponent(new Markup($"Name: {_viewModel.Name}")),
            };

        private Component GetSurnameComponent()
            => _viewModel.Surname switch
            {
                null => new InputComponent<string>("Surname: ", defaultHandler: _viewModel.SurnameSubmitted),
                not null => new MarkupComponent(new Markup($"Surname: {_viewModel.Surname}")),
            };

        private Component GetPasswordComponent()
            => _viewModel.Password switch
            {
                null => new InputComponent<string>("Password: ", s => s.Length > 5, defaultHandler: _viewModel.PasswordSubmitted)
                {
                    IsSecret = true,
                },
                not null => new MarkupComponent(new Markup($"Password: {new string('*', _viewModel.Password.Length)}")),
            };

        private Component GetEmailComponent()
            => _viewModel.EmailAddress switch
            {
                null => new InputComponent<string>(
                    "Email: ", s => new EmailAddressAttribute().IsValid(s), defaultHandler: _viewModel.EmailSubmitted),
                not null => new MarkupComponent(new Markup($"Email: {_viewModel.EmailAddress}")),
            };

        private Component GetAddressComponent()
            => _viewModel.EmailAddress switch
            {
                null => new InputComponent<string>("Address: ", optional: true, defaultHandler: _viewModel.AddressSubmitted),
                not null => new MarkupComponent(new Markup($"Address (optional): {_viewModel.Address}")),
            };
    }
}