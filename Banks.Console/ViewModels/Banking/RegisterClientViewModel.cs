using System.Collections.Generic;
using Banks.Console.Views.Banking;
using Banks.Models;
using Spectre.Mvvm.Components;
using Spectre.Mvvm.Interfaces;
using Spectre.Mvvm.Models;
using Utility.Extensions;

namespace Banks.Console.ViewModels.Banking
{
    public class RegisterClientViewModel : EnteringViewModel<RegisterClientViewModel>
    {
        private readonly INavigator _navigator;

        public RegisterClientViewModel(INavigator navigator, EnteringCompletedHandler? defaultHandler = null)
            : base(defaultHandler)
        {
            _navigator = navigator;
        }

        public string? Name { get; private set; }
        public string? Surname { get; private set; }
        public string? Password { get; private set; }
        public EmailAddress? EmailAddress { get; private set; }
        public string? Address { get; private set; }
        public PassportData? PassportData { get; private set; }

        public void NameSubmitted(string name)
            => Name = name.ThrowIfNull(nameof(name));

        public void SurnameSubmitted(string surname)
            => Surname = surname.ThrowIfNull(nameof(surname));

        public void PasswordSubmitted(string password)
            => Password = password.ThrowIfNull(nameof(password));

        public void EmailSubmitted(string email)
            => EmailAddress = new EmailAddress(email.ThrowIfNull(nameof(email)));

        public void AddressSubmitted(string address)
            => Address = address;

        public void PassportDataSubmitted(PassportData passportData)
            => PassportData = passportData;

        public NavigationComponent GetNavigationComponent()
        {
            var elements = new List<NavigationElement>();

            if (PassportData is null)
            {
                elements.Add(new NavigationElement("Enter passport info (optional): ", n =>
                {
                    var viewModel = new PassportDataEnterViewModel(this, n);
                    n.PushView(new PassportDataEnterView(viewModel));
                }));
            }

            elements.Add(new NavigationElement("Register", _ => OnEnteringCompleted(this)));

            return new NavigationComponent(_navigator, elements.ToArray())
            {
                NeedBackButton = false,
            };
        }
    }
}