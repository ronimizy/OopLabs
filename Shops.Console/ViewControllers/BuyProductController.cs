using System;
using Shops.Console.Base.ViewControllers;
using Shops.Console.Delegates;
using Shops.Console.Views;
using Shops.Entities;
using Utility.Extensions;

namespace Shops.Console.ViewControllers
{
    public class BuyProductController : Controller
    {
        private readonly Shop _shop;
        private readonly Person _user;
        private Product? _product;
        private int? _amount;

        public BuyProductController(Person user, Shop shop)
        {
            _user = user;
            _shop = shop;

            var selectProductDelegate = new SelectProductDelegate(shop.Products, p => _product = p);
            var inputDelegate = new StrategyInputFieldDelegate<int>(v => _amount = v, validator: v => v >= 0);

            View = new BuyProductView(selectProductDelegate, inputDelegate)
            {
                Controller = this,
            };
        }

        public override string Title => "Buy Product";

        public override void OnViewRendered()
        {
            try
            {
                _shop.ArrangePurchase(
                    _user,
                    _product.ThrowIfNull(nameof(_product)),
                    _amount.ThrowIfNull(nameof(_amount)));
            }
            catch (Exception e)
            {
                Parent?.OnError(this, e);
            }

            Parent?.RemoveChild(this);
        }
    }
}