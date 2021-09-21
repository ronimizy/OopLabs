using System;
using System.Collections.Generic;
using System.Linq;
using Shops.Entities;
using Shops.Tools;
using Utility.Extensions;

namespace Shops.Services
{
    public class ShopService
    {
        private readonly Dictionary<Guid, Shop> _shops;
        private readonly Dictionary<Guid, Product> _products;

        public ShopService()
        {
            _shops = new Dictionary<Guid, Shop>();
            _products = new Dictionary<Guid, Product>();
        }

        public IReadOnlyList<Shop> Shops => _shops.Values.ToList();
        public IReadOnlyList<Product> Products => _products.Values.ToList();

        public void RegisterShop(Shop shop)
        {
            _shops[shop.Id] = shop;
        }

        public void RegisterProduct(Product product)
        {
            _products[product.Id] = product;
        }

        public Shop FindCheapest(Product product, int amount)
        {
            product.ThrowIfNull(nameof(product));

            if (amount < 0)
                throw ShopsExceptionFactory.NegativeAmountException(amount);

            Shop? shop = _shops.Values
                .Where(s => s.GetProductAmount(product) >= amount)
                .OrderBy(s => s.GetProductPrice(product))
                .FirstOrDefault();

            if (shop is null)
                throw ShopsExceptionFactory.NoShopFoundException(product, amount);

            return shop;
        }
    }
}