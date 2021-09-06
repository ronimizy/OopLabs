using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Shops.Entities;
using Shops.Models;
using Shops.Tools;
using Utility.Extensions;

namespace Shops.Services
{
    public class ShopService
    {
        private static int _currentServiceId;

        private readonly List<Shop> _shops;
        private int _currentShopId;
        private int _currentProductId;

        public ShopService()
        {
            Id = Interlocked.Increment(ref _currentServiceId);
            _shops = new List<Shop>();
        }

        public int Id { get; }

        public Shop CreateShop(string name, string location)
        {
            var shop = new Shop(GetNewShopId(), name, location, Id);
            _shops.Add(shop);
            return shop;
        }

        public Product RegisterProduct(string name, string description)
        {
            return new Product(GetNewProductId(), name, description, Id);
        }

        public void BuyCheapest(Person person, Product product, int amount)
        {
            if (amount < 0)
                throw ShopsExceptionFactory.NegativeAmountException(amount);

            person.ThrowIfNull(nameof(person));
            product.ThrowIfNull(nameof(product));

            int amountLeft = amount;
            var packages = _shops
                .Where(s => s.Lots.Any(l => l.Product.Equals(product) && l.Amount != 0))
                .Select(s => new BulkPurchaseEntry(s, s.Lots.Single(l => l.Product.Equals(product))))
                .OrderBy(e => e.Lot.Price)
                .TakeWhile(e => ProcessEntry(ref amountLeft, e))
                .ToList();

            if (amountLeft != 0)
                throw ShopsExceptionFactory.InsufficientProductAmountException(product, amount, amount - amountLeft);

            double totalPrice = packages.Aggregate(0d, (p, e) => p + (e.Lot.Price * e.AmountToBuy));

            if (person.Balance < totalPrice)
                throw ShopsExceptionFactory.InsufficientFundsException(totalPrice, person.Balance);

            foreach (BulkPurchaseEntry package in packages)
            {
                package.Shop.Buy(person, product, package.AmountToBuy);
            }
        }

        private bool ProcessEntry(ref int amountLeft, BulkPurchaseEntry entry)
        {
            if (amountLeft == 0)
                return false;

            if (amountLeft < entry.Lot.Amount)
            {
                entry.AmountToBuy = amountLeft;
                amountLeft = 0;
                return true;
            }

            entry.AmountToBuy = entry.Lot.Amount;
            amountLeft -= entry.AmountToBuy;
            return true;
        }

        private int GetNewShopId()
        {
            return Interlocked.Increment(ref _currentShopId);
        }

        private int GetNewProductId()
        {
            return Interlocked.Increment(ref _currentProductId);
        }
    }
}