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

        public Shop FindCheapest(Person person, Product product, int amount)
        {
            person.ThrowIfNull(nameof(person));
            product.ThrowIfNull(nameof(product));

            if (amount < 0)
                throw ShopsExceptionFactory.NegativeAmountException(amount);

            Shop? shop = _shops
                .Where(s => s.GetProductAmount(product) >= amount)
                .OrderBy(s => s.GetProductPrice(product))
                .FirstOrDefault();

            if (shop is null)
                throw ShopsExceptionFactory.NoShopFoundException(product, amount);

            return shop;
        }
    }
}