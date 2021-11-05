using System.Collections.Generic;
using Shops.Entities;
using Spectre.Mvvm.Interfaces;
using Spectre.Mvvm.Models;
using Utility.Extensions;

namespace Shops.Console.ViewModels
{
    public class SetProductPriceViewModel
    {
        private readonly Shop _shop;
        private readonly INavigator _navigator;

        private Product? _product;
        private double? _price;

        public SetProductPriceViewModel(Shop shop, INavigator navigator)
        {
            _shop = shop;
            _navigator = navigator;
        }

        public IReadOnlyCollection<Product> Products => _shop.Products;

        public void OnProductSelected(Product value)
            => _product = value;

        public void OnPriceEntered(double value)
            => _price = value;

        public void OnOperationConfirmed()
        {
            _product.ThrowIfNull(nameof(_product));
            _price.ThrowIfNull(nameof(_price));

            _shop.SetProductPrice(_product!, _price!.Value);
            _navigator.PopView();
        }

        public void OnOperationRejected()
            => _navigator.PopView();

        public void OnConfirmationChoiceReceived(SelectorAction choice)
            => choice.Action();
    }
}