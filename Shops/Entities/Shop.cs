using System;
using System.Collections.Generic;
using Shops.Models;
using Shops.Tools;

namespace Shops.Entities
{
    public sealed class Shop
    {
        private readonly List<Lot> _lots;

        public Shop(int id, string name, string location)
        {
            Id = id;
            Name = name;
            Location = location;
            _lots = new List<Lot>();
        }

        public int Id { get; }
        public string Name { get; }
        public string Location { get; }
        public double Balance { get; private set; }
        public IReadOnlyList<Lot> Lots => _lots;

        public Shop AddProducts(params Lot[] shipment)
        {
            foreach (Lot lot in shipment)
            {
                int index = _lots.FindIndex(l => l.Product.Equals(lot.Product));
                if (index == -1)
                {
                    _lots.Add(lot);
                }
                else
                {
                    Lot existingLot = _lots[index];
                    existingLot.Amount += lot.Amount;
                    if (Math.Abs(existingLot.Price - lot.Price) > 0.01)
                        existingLot.Price = ResolveNewPrice(existingLot.Price, lot.Price);

                    _lots[index] = existingLot;
                }
            }

            return this;
        }

        public Shop Buy(Person person, Product product, int amount)
        {
            int index = _lots.FindIndex(l => l.Product.Equals(product));
            if (index == -1)
                throw ShopsExceptionFactory.NonExisingProductException(this, product);

            Lot lot = _lots[index];

            if (lot.Amount < amount)
                throw ShopsExceptionFactory.InsufficientProductAmountException(this, product, amount, lot.Amount);

            double price = lot.Price * amount;

            if (person.Balance < price)
                throw ShopsExceptionFactory.InsufficientFundsException(price, person.Balance);

            person.WriteMoneyOff(price);
            Balance += price;

            person.Ship(new Lot(lot.Product, lot.Price, amount));

            lot.Amount -= amount;
            _lots[index] = lot;

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