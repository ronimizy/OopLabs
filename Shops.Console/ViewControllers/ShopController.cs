using System;
using System.Collections.Generic;
using System.Globalization;
using Shops.Console.Base.Delegates;
using Shops.Console.Base.Models;
using Shops.Console.Base.ViewControllers;
using Shops.Console.Views;
using Shops.Entities;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace Shops.Console.ViewControllers
{
    public class ShopController : NavigatedController, ITableViewDelegate
    {
        private readonly Person _user;
        private readonly Shop _shop;
        private readonly IReadOnlyList<Product> _products;

        public ShopController(Person user, Shop shop, IReadOnlyList<Product> products)
        {
            _shop = shop;
            _products = products;
            _user = user;

            View = new ShopView(user, this);
        }

        public override string Title => _shop.Name;

        public override IReadOnlyList<Controller> NavigationLinks
        {
            get
            {
                var links = new List<Controller>();

                if (_products.Count > 0)
                    links.Add(new SupplyProductController(_shop, _products));

                if (_shop.Products.Count > 0)
                {
                    links.Add(new BuyProductController(_user, _shop));
                    links.Add(new SetPriceController(_shop));
                }

                return links;
            }
        }

        public TableBorder GetBorder()
            => TableBorder.Rounded;

        public int GetColumnCount()
            => 3;

        public int GetRowCount()
            => _shop.Products.Count;

        public TableColumn GetHeaderCellFor(IndexPath indexPath)
        {
            string[] headers = { "Product", "Amount", "Price" };
            string title = headers[indexPath.Column];
            return new TableColumn(title);
        }

        public IRenderable GetCellFor(IndexPath indexPath)
        {
            Product product = _shop.Products[indexPath.Row];

            return indexPath.Column switch
            {
                0 => new Text(product.Name),
                1 => new Text(_shop.GetProductAmount(product).ToString()),
                2 => new Text(_shop.GetProductPrice(product).ToString(CultureInfo.InvariantCulture)),
                _ => throw new InvalidOperationException("Invalid column"),
            };
        }
    }
}