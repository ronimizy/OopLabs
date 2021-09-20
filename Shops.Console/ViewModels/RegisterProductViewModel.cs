using Shops.Console.Base.Interfaces;
using Shops.Console.Base.Models;
using Shops.Entities;
using Shops.Services;
using Utility.Extensions;

namespace Shops.Console.ViewModels
{
    public class RegisterProductViewModel
    {
        private readonly ShopService _service;
        private readonly INavigator _navigator;

        private string? _name;
        private string _description = string.Empty;

        public RegisterProductViewModel(ShopService service, INavigator navigator)
        {
            _service = service;
            _navigator = navigator;
        }

        public void OnNameEntered(string value)
            => _name = value;

        public void OnDescriptionEntered(string value)
            => _description = value;

        public void OnOperationConfirmed()
        {
            _name.ThrowIfNull(nameof(_name));

            _service.RegisterProduct(new Product(_name!, _description));
            _navigator.PopView();
        }

        public void OnOperationRejected()
            => _navigator.PopView();

        public void OnConfirmationChoiceReceived(SelectorAction choice)
            => choice.Action();
    }
}