using Shops.Console.Views;
using Shops.Entities;
using Shops.Services;
using Spectre.Mvvm.Interfaces;
using Utility.Extensions;

namespace Shops.Console.ViewModels
{
    public class SignUpViewModel
    {
        private readonly ShopService _service;
        private readonly INavigator _navigator;

        private string? _username;
        private double? _balance;

        public SignUpViewModel(ShopService service, INavigator navigator)
        {
            _service = service;
            _navigator = navigator;
        }

        public void OnUsernameEntered(string value)
            => _username = value;

        public void OnBalanceEntered(double value)
        {
            _balance = value;
        }

        public void OnProceed()
        {
            _username.ThrowIfNull(nameof(_username));
            _balance.ThrowIfNull(nameof(_balance));

            var menuViewModel = new MenuViewModel(
                _service,
                new Person(_username!, _balance!.Value),
                _navigator);
            var menuView = new MenuView(menuViewModel);

            _navigator.PushView(menuView);
        }
    }
}