using System.Collections.Generic;
using System.Linq;
using Shops.Console.Base.Interfaces;
using Shops.Console.Base.Views;
using Shops.Console.Views;
using Shops.Entities;
using Shops.Services;
using Spectre.Console;
using Spectre.Console.Rendering;
using Utility.Extensions;

namespace Shops.Console.ViewModels
{
    public class ShopViewModel
    {
        private readonly ShopService _service;
        private readonly Shop _shop;

        public ShopViewModel(ShopService service, Shop shop, Person user, INavigator navigator)
        {
            _service = service;
            _shop = shop;
            User = user;
            Navigator = navigator;
        }

        public Person User { get; }
        public INavigator Navigator { get; }
        public string ShopName => _shop.Name;
        public string ShopLocation => _shop.Location;

        public IReadOnlyCollection<TableColumn> Headers => new[]
        {
            new TableColumn("Name"),
            new TableColumn("Description"),
            new TableColumn("Price"),
            new TableColumn("Amount"),
        };

        public IReadOnlyCollection<IReadOnlyCollection<IRenderable>> Data => _shop.Products
            .Select(p => new[]
            {
                new Text(p.Name),
                new Text(p.Description),
                new Text($"{_shop.GetProductPrice(p)}$"),
                new Text(_shop.GetProductAmount(p).ToString()),
            })
            .ToArray();

        public View[] NavigationLinks
        {
            get
            {
                var links = new List<View>();
                if (!_service.Products.IsEmpty())
                    links.Add(new SupplyProductView(new SupplyProductViewModel(_service, _shop, Navigator)));

                if (!_shop.Products.IsEmpty())
                {
                    links.Add(new BuyProductView(new BuyProductViewModel(_shop, User, Navigator)));
                    links.Add(new SetProductPriceView(new SetProductPriceViewModel(_shop, Navigator)));
                }

                return links.ToArray();
            }
        }
    }
}