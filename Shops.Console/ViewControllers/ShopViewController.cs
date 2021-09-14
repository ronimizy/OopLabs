using System;
using System.Collections.Generic;
using System.Globalization;
using Shops.Console.Base.Delegates;
using Shops.Console.Base.Models;
using Shops.Console.Base.ViewControllers;
using Shops.Console.Base.Views;
using Shops.Console.Views;
using Shops.Entities;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace Shops.Console.ViewControllers
{
    public class ShopViewController : NavigatedViewController, ITableViewDelegate
    {
        private readonly Person _user;
        private readonly Shop _shop;
        private readonly IReadOnlyList<Product> _products;

        public ShopViewController(Person user, Shop shop, IReadOnlyList<Product> products)
        {
            _shop = shop;
            _products = products;
            _user = user;

            AddView(new UserDetailsView(user));
            AddView(new TableView(this));
        }

        public override string Title => _shop.Name;
        public override IReadOnlyList<ViewController> NavigationLinks
        {
            get
            {
                var links = new List<ViewController>();

                if (_products.Count > 0)
                    links.Add(new SupplyProductViewController(_shop, _products));

                if (_shop.Products.Count > 0)
                {
                    links.Add(new BuyProductViewController(_user, _shop));
                    links.Add(new SetPriceViewController(_shop));
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