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
    public class BuyProductViewController : ViewController
    {
        private readonly Person _user;
        private readonly Shop _shop;
        private Product? _product;
        private int? _amount;

        public BuyProductViewController(Person user, Shop shop)
        {
            _user = user;
            _shop = shop;

            var productViewDelegate = new SelectProductViewDelegate(shop.Products, p => _product = p);
            var inputDelegate = new StrategyInputFieldDelegate<int>(
                v => _amount = v,
                validator: v => v >= 0 ? ValidationResult.Success() : ValidationResult.Error());

            AddView(new SelectorView<SelectorAction>(productViewDelegate));
            AddView(new InputFieldView<int>("Amount: ", inputDelegate));
        }

        public override string Title => "Buy Product";

        public override void DrawContent()
        {
            base.DrawContent();

            try
            {
                _shop.ArrangePurchase(
                    _user,
                    _product.ThrowIfNull(nameof(_product)),
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