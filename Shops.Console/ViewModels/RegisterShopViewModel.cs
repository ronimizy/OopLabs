using Shops.Entities;
using Shops.Services;
using Spectre.Mvvm.Interfaces;
using Spectre.Mvvm.Models;
using Utility.Extensions;

namespace Shops.Console.ViewModels
{
    public class RegisterShopViewModel
    {
        private readonly ShopService _service;
        private readonly INavigator _navigator;

        private string? _name;
        private string? _location;

        public RegisterShopViewModel(ShopService service, INavigator navigator)
        {
            _service = service;
            _navigator = navigator;
        }

        public void OnNameEntered(string value)
            => _name = value;

        public void OnLocationEntered(string value)
            => _location = value;

        public void OnOperationConfirmed()
        {
            _name.ThrowIfNull(nameof(_name));
            _location.ThrowIfNull(nameof(_location));

            _service.RegisterShop(new Shop(_name!, _location!));
            _navigator.PopView();
        }

        public void OnOperationRejected()
            => _navigator.PopView();

        public void OnSubmitChoiceReceived(SelectorAction choice)
            => choice.Action();
    }
}