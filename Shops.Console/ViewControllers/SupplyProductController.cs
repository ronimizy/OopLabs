using System;
using System.Collections.Generic;
using Shops.Console.Base.ViewControllers;
using Shops.Console.Delegates;
using Shops.Console.Views;
using Shops.Entities;
using Utility.Extensions;

namespace Shops.Console.ViewControllers
{
    public class SupplyProductController : Controller
    {
        private readonly Shop _shop;
        private Product? _product;
        private double? _price;
        private int? _amount;

        public SupplyProductController(Shop shop, IReadOnlyList<Product> products)
        {
            _shop = shop;

            var productViewDelegate = new SelectProductDelegate(products, p => _product = p);
            var priceInputDelegate = new StrategyInputFieldDelegate<double>(v => _price = v, validator: v => v >= 0);
            var amountInputDelegate = new StrategyInputFieldDelegate<int>(v => _amount = v, validator: v => v >= 0);

            View = new SupplyProductView(productViewDelegate, priceInputDelegate, amountInputDelegate)
            {
                Controller = this,
            };
        }

        public override string Title => "Product supply";

        public override void OnViewRendered()
        {
            try
            {
                _shop.SupplyProduct(
                    _product.ThrowIfNull(nameof(_product)),
                    _price.ThrowIfNull(nameof(_price)),
                    _amount.ThrowIfNull(nameof(_amount)));
            }
            catch (Exception e)
            {
                OnError(this, e);
            }

            Parent?.RemoveChild(this);
        }
    }
}