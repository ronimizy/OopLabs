using System.Collections.Generic;
using Shops.Console.Views;
using Shops.Entities;
using Shops.Services;
using Spectre.Mvvm.Interfaces;
using Utility.Extensions;

namespace Shops.Console.ViewModels
{
    public class FindCheapestViewModel
    {
        private readonly ShopService _service;
        private readonly Person _user;
        private readonly INavigator _navigator;

        private Product? _product;
        private int? _amount;

        public FindCheapestViewModel(ShopService service, INavigator navigator, Person user)
        {
            _service = service;
            _navigator = navigator;
            _user = user;
        }

        public IReadOnlyCollection<Product> Products => _service.Products;

        public void OnProductSelected(Product value)
            => _product = value;

        public void OnAmountEntered(int value)
            => _amount = value;

        public void OnOperationConfirmed()
        {
            _product.ThrowIfNull(nameof(_product));
            _amount.ThrowIfNull(nameof(_amount));

            Shop shop = _service.FindCheapest(_product!, _amount!.Value);
            _navigator.PopView();
            _navigator.PushView(new ShopView(new ShopViewModel(_service, shop, _user, _navigator)));
        }
    }
}