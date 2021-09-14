using System;
using System.Collections.Generic;
using Shops.Console.Base.ViewControllers;
using Shops.Console.Base.Views;
using Shops.Console.Delegates;
using Shops.Console.Models;
using Shops.Entities;
using Spectre.Console;
using Utility.Extensions;

namespace Shops.Console.ViewControllers
{
    public class SupplyProductViewController : ViewController
    {
        private readonly Shop _shop;
        private Product? _product;
        private double? _price;
        private int? _amount;

        public SupplyProductViewController(Shop shop, IReadOnlyList<Product> products)
        {
            _shop = shop;

            var productViewDelegate = new SelectProductViewDelegate(products, p => _product = p);
            var priceInputDelegate = new StrategyInputFieldDelegate<double>(
                v => _price = v,
                validator: v => v >= 0 ? ValidationResult.Success() : ValidationResult.Error());
            var amountInputDelegate = new StrategyInputFieldDelegate<int>(
                v => _amount = v,
                validator: v => v >= 0 ? ValidationResult.Success() : ValidationResult.Error());

            AddView(new SelectorView<SelectorAction>(productViewDelegate));
            AddView(new InputFieldView<double>("Price: ", priceInputDelegate));
            AddView(new InputFieldView<int>("Amount: ", amountInputDelegate));
        }

        public override string Title => "Product supply";

        public override void DrawContent()
        {
            base.DrawContent();

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

            OnRedraw(this);
            OnDismiss(this);
        }
    }
}