using System.Collections.Generic;
using System.Linq;
using Shops.Entities;
using Shops.Models;
using Shops.Tools;
using Utility.Extensions;

namespace Shops.Services
{
    public class ShopService
    {
        private readonly List<Shop> _shops;

        public ShopService()
        {
            _shops = new List<Shop>();
        }

        public Shop CreateShop(string name, string location)
        {
            var shop = new Shop(name, location);
            _shops.Add(shop);
            return shop;
        }

        public Product RegisterProduct(string name, string description)
        {
            return new Product(name, description);
        }

        public void BuyCheapest(Person person, Product product, int amount)
        {
            person.ThrowIfNull(nameof(person));
            product.ThrowIfNull(nameof(product));

            if (amount < 0)
                throw ShopsExceptionFactory.NegativeAmountException(amount);

            person.ThrowIfNull(nameof(person));
            product.ThrowIfNull(nameof(product));

            var package = _shops
                .Where(s => s.ProductsAvailable(product, 1))
                .Select(s => new { shop = s, lot = s.ProductLot(product) })
                .OrderBy(p => p.lot.Price)
                .FirstOrDefault(p => p.lot.Amount >= amount);

            if (package is null)
                throw ShopsExceptionFactory.NoShopFoundException(product, amount);

            package.shop.Buy(person, product, amount);
        }
    }
}