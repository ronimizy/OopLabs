using System;
using System.Collections.Generic;
using Shops.Tools;
using Utility.Extensions;

namespace Shops.Entities
{
    public sealed class Shop
    {
        private static readonly IdGenerator IdGenerator = new ();

        private readonly Dictionary<int, Lot> _lots;

        internal Shop(string name, string location)
        {
            Id = IdGenerator.Next();
            Name = name.ThrowIfNull(nameof(name));
            Location = location.ThrowIfNull(nameof(location));
            _lots = new Dictionary<int, Lot>();
        }

        public int Id { get; }
        public string Name { get; }
        public string Location { get; }

        public Shop AddProduct(Product product, double price, int amount)
        {
            product.ThrowIfNull(nameof(product));

            Lot? lot = ProductLotOrDefault(product);
            if (lot is null)
            {
                _lots[product.Id] = new Lot(product, price, amount);
            }
            else
            {
                lot.Amount += amount;
                lot.Price = Math.Max(lot.Price, price);
            }

            return this;
        }

        public Shop Buy(Person person, Product product, int amount)
        {
            person.ThrowIfNull(nameof(person));
            product.ThrowIfNull(nameof(product));

            if (amount < 0)
                throw ShopsExceptionFactory.NegativeAmountException(amount);

            Lot? lot = ProductLotOrDefault(product);
            if (lot is null)
                throw ShopsExceptionFactory.NonExisingProductException(this, product);

            if (lot.Amount < amount)
                throw ShopsExceptionFactory.InsufficientProductAmountException(product, amount, lot.Amount);

            double totalPrice = lot.Price * amount;

            person.SendBill(totalPrice);
            lot.Amount -= amount;

            return this;
        }

        public Shop SetPriceFor(Product product, double price)
        {
            product.ThrowIfNull(nameof(product));

            if (price < 0)
                throw ShopsExceptionFactory.NegativePriceException(price);

            Lot? lot = ProductLotOrDefault(product);
            if (lot is null)
                _lots.Add(product.Id, new Lot(product, price, 0));
            else
                lot.Price = price;

            return this;
        }

        public double ProductPrice(Product product)
        {
            Lot? lot = ProductLotOrDefault(product);
            if (lot is null)
                throw ShopsExceptionFactory.NonExisingProductException(this, product);

            return lot.Price;
        }

        public int ProductAmount(Product product)
        {
            Lot? lot = ProductLotOrDefault(product);
            return lot?.Amount ?? 0;
        }

        public override string ToString()
            => $"[{Id}] {Name}";

        private Lot? ProductLotOrDefault(Product product)
            => _lots.ContainsKey(product.Id) ? _lots[product.Id] : null;
    }
}