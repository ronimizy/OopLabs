using System;
using System.Collections.Generic;
using Shops.Models;
using Shops.Tools;
using Utility.Extensions;

namespace Shops.Entities
{
    public sealed class Shop
    {
        private readonly List<Lot> _lots;

        internal Shop(int id, string name, string location, int managerId)
        {
            Id = id;
            Name = name.ThrowIfNull(nameof(name));
            Location = location.ThrowIfNull(nameof(location));
            ManagerId = managerId;
            _lots = new List<Lot>();
        }

        public int Id { get; }
        public int ManagerId { get; }
        public string Name { get; }
        public string Location { get; }
        public double Balance { get; private set; }
        public IReadOnlyList<Lot> Lots => _lots;

        public Shop AddProducts(params Lot[] shipment)
        {
            shipment.ThrowIfNull(nameof(shipment));

            foreach (Lot lot in shipment)
            {
                if (ManagerId != lot.Product.ManagerId)
                    throw ShopsExceptionFactory.AlienProductException(this, lot.Product);

                int index = _lots.FindIndex(l => l.Product.Equals(lot.Product));
                if (index == -1)
                {
                    _lots.Add(lot);
                }
                else
                {
                    Lot existingLot = _lots[index];
                    existingLot.Amount += lot.Amount;
                    if (Math.Abs(existingLot.Price - lot.Price) > 0.0001)
                        existingLot.Price = ResolveNewPrice(existingLot.Price, lot.Price);

                    _lots[index] = existingLot;
                }
            }

            return this;
        }

        public Shop Buy(Person person, Product product, int amount)
        {
            person.ThrowIfNull(nameof(person));
            product.ThrowIfNull(nameof(product));

            if (ManagerId != product.ManagerId)
                throw ShopsExceptionFactory.AlienProductException(this, product);

            int index = _lots.FindIndex(l => l.Product.Equals(product));
            if (index == -1)
                throw ShopsExceptionFactory.NonExisingProductException(this, product);

            Lot lot = _lots[index];

            if (lot.Amount < amount)
                throw ShopsExceptionFactory.InsufficientProductAmountException(this, product, amount, lot.Amount);

            double totalPrice = lot.Price * amount;

            if (person.Balance < totalPrice)
                throw ShopsExceptionFactory.InsufficientFundsException(totalPrice, person.Balance);

            person.SendBill(totalPrice);
            Balance += totalPrice;

            lot.Amount -= amount;
            _lots[index] = lot;

            return this;
        }

        public Shop SetPriceFor(Product product, double price)
        {
            product.ThrowIfNull(nameof(product));

            if (ManagerId != product.ManagerId)
                throw ShopsExceptionFactory.AlienProductException(this, product);

            int index = _lots.FindIndex(l => l.Product.Equals(product));
            if (index == -1)
            {
                _lots.Add(new Lot(product, price, 0));
            }
            else
            {
                Lot lot = _lots[index];
                lot.Price = price;
                _lots[index] = lot;
            }

            return this;
        }

        public override string ToString()
            => $"[{Id}] {Name}";

        private static double ResolveNewPrice(double oldPrice, double newPrice)
        {
            return Math.Max(oldPrice, newPrice);
        }
    }
}