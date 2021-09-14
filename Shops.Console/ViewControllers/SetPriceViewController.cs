using System;
using Shops.Console.Base.ViewControllers;
using Shops.Console.Base.Views;
using Shops.Console.Delegates;
using Shops.Console.Models;
using Shops.Entities;
using Spectre.Console;
using Utility.Extensions;

namespace Shops.Console.ViewControllers
{
    public class SetPriceViewController : ViewController
    {
        private readonly Shop _shop;
        private Product? _product;
        private double _price;

        public SetPriceViewController(Shop shop)
        {
            _shop = shop;

            var productViewDelegate = new SelectProductViewDelegate(shop.Products, p => _product = p);
            var inputDelegate = new StrategyInputFieldDelegate<double>(
                v => _price = v,
                validator: v => v >= 0 ? ValidationResult.Success() : ValidationResult.Error());

            AddView(new SelectorView<SelectorAction>(productViewDelegate));
            AddView(new InputFieldView<double>("New Price: ", inputDelegate));
        }

        public override string Title => "Set Price";

        public override void DrawContent()
        {
            base.DrawContent();

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

            OnDismiss(this);
        }
    }
}