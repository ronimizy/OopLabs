using System.Collections.Generic;
using Shops.Entities;
using Shops.Services;
using Spectre.Mvvm.Interfaces;
using Spectre.Mvvm.Models;
using Utility.Extensions;

namespace Shops.Console.ViewModels
{
    public class SupplyProductViewModel
    {
        private readonly ShopService _service;
        private readonly INavigator _navigator;
        private readonly Shop _shop;

        private Product? _product;
        private double? _price;
        private int? _amount;

        public SupplyProductViewModel(ShopService service, Shop shop, INavigator navigator)
        {
            _service = service;
            _shop = shop;
            _navigator = navigator;
        }

        public IReadOnlyCollection<Product> Products => _service.Products;

        public void OnProductSelected(Product value)
            => _product = value;

        public void OnPriceEntered(double value)
            => _price = value;

        public void OnAmountEntered(int value)
            => _amount = value;

        public void OnOperationConfirmed()
        {
            _product.ThrowIfNull(nameof(_product));
            _price.ThrowIfNull(nameof(_price));
            _amount.ThrowIfNull(nameof(_amount));

            _shop.SupplyProduct(_product!, _price!.Value, _amount!.Value);
            _navigator.PopView();
        }

        public void OnOperationRejected()
            => _navigator.PopView();

        public void OnConfirmationChoiceReceived(SelectorAction choice)
            => choice.Action();
    }
}