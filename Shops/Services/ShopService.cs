using System.Collections.Generic;
using System.Linq;
using Shops.Entities;
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
            if (amount < 0)
                throw ShopsExceptionFactory.NegativeAmountException(amount);

            person.ThrowIfNull(nameof(person));
            product.ThrowIfNull(nameof(product));

            Shop? shop = _shops
                .Where(s => s.ProductAvailable(product, 1))
                .OrderBy(s => s.ProductLot(product).Price)
                .FirstOrDefault(s => s.ProductLot(product).Amount >= amount);

            if (shop is null)
                throw ShopsExceptionFactory.NoShopFoundException(product, amount);

            shop.Buy(person, product, amount);
        }
    }
}