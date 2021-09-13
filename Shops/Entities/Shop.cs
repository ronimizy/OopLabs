using System;
using System.Collections.Generic;
using Shops.Tools;
using Utility.Extensions;

namespace Shops.Entities
{
    public sealed class Shop
    {
        private readonly Dictionary<Product, Lot> _lots;

        internal Shop(string name, string location)
        {
            Id = Guid.NewGuid();
            Name = name.ThrowIfNull(nameof(name));
            Location = location.ThrowIfNull(nameof(location));
            _lots = new Dictionary<Product, Lot>();
        }

        public Guid Id { get; }
        public string Name { get; }
        public string Location { get; }
        public IReadOnlyCollection<Product> Products => _lots.Keys;

        public void AddProduct(Product product, double price, int amount)
        {
            product.ThrowIfNull(nameof(product));

            Lot? lot = GetProductLotOrDefault(product);
            if (lot is null)
            {
                _lots[product] = new Lot(price, amount);
            }
            else
            {
                lot.Amount += amount;
                lot.Price = Math.Max(lot.Price, price);
            }
        }

        public void Buy(Person person, Product product, int amount)
        {
            person.ThrowIfNull(nameof(person));
            product.ThrowIfNull(nameof(product));

            if (amount < 0)
                throw ShopsExceptionFactory.NegativeAmountException(amount);

            Lot? lot = GetProductLotOrDefault(product);
            if (lot is null)
                throw ShopsExceptionFactory.NonExisingProductException(this, product);

            if (lot.Amount < amount)
                throw ShopsExceptionFactory.InsufficientProductAmountException(product, amount, lot.Amount);

            double totalPrice = lot.Price * amount;

            person.SendBill(totalPrice);
            lot.Amount -= amount;
        }

        public void SetProductPrice(Product product, double price)
        {
            product.ThrowIfNull(nameof(product));

            if (price < 0)
                throw ShopsExceptionFactory.NegativePriceException(price);

            Lot? lot = GetProductLotOrDefault(product);
            if (lot is null)
                _lots.Add(product, new Lot(price, 0));
            else
                lot.Price = price;
        }

        public double GetProductPrice(Product product)
        {
            Lot? lot = GetProductLotOrDefault(product);
            if (lot is null)
                throw ShopsExceptionFactory.NonExisingProductException(this, product);

            return lot.Price;
        }

        public int GetProductAmount(Product product)
        {
            Lot? lot = GetProductLotOrDefault(product);
            return lot?.Amount ?? 0;
        }

        public override string ToString()
            => $"[{Id}] {Name}";

        private Lot? GetProductLotOrDefault(Product product)
            => _lots.ContainsKey(product) ? _lots[product] : null;
    }
}