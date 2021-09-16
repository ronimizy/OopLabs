using System;
using Shops.Console.Base.ViewControllers;
using Shops.Console.Delegates;
using Shops.Console.Views;
using Shops.Entities;
using Utility.Extensions;

namespace Shops.Console.ViewControllers
{
    public class SetPriceController : Controller
    {
        private readonly Shop _shop;
        private Product? _product;
        private double _price;

        public SetPriceController(Shop shop)
        {
            _shop = shop;

            var productSelectorDelegate = new SelectProductDelegate(shop.Products, p => _product = p);
            var priceInputFieldDelegate = new StrategyInputFieldDelegate<double>(v => _price = v, validator: v => v >= 0);

            View = new SetPriceView(productSelectorDelegate, priceInputFieldDelegate)
            {
                Controller = this,
            };
        }

        public override string Title => "Set Price";

        public override void OnViewRendered()
        {
            try
            {
                _shop.SetProductPrice(
                    _product.ThrowIfNull(nameof(_product)),
                    _price.ThrowIfNull(nameof(_price)));
            }
            catch (Exception e)
            {
                OnError(this, e);
            }

            Parent?.RemoveChild(this);
        }
    }
}