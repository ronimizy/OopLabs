using System.Collections.Generic;
using Shops.Console.Base.Interfaces;
using Shops.Console.Base.Models;
using Shops.Entities;
using Utility.Extensions;

namespace Shops.Console.ViewModels
{
    public class BuyProductViewModel
    {
        private readonly Shop _shop;
        private readonly INavigator _navigator;

        private Product? _product;
        private int? _amount;

        public BuyProductViewModel(Shop shop, Person user, INavigator navigator)
        {
            _shop = shop;
            User = user;
            _navigator = navigator;
        }

        public Person User { get; }

        public IReadOnlyCollection<Product> Products => _shop.Products;

        public void OnProductSelected(Product value)
            => _product = value;

        public void OnAmountEntered(int value)
            => _amount = value;

        public void OnOperationConfirmed()
        {
            _product.ThrowIfNull(nameof(_product));
            _amount.ThrowIfNull(nameof(_amount));

            _shop.ArrangePurchase(User, _product!, _amount!.Value);
            _navigator.PopView();
        }

        public void OnOperationRejected()
            => _navigator.PopView();

        public void OnConfirmationChoiceReceived(SelectorAction choice)
            => choice.Action();
    }
}