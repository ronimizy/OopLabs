using System.Collections.Generic;
using Shops.Models;
using Shops.Tools;

namespace Shops.Entities
{
    public class Person
    {
        private readonly List<Lot> _lots;

        public Person(string name, double balance)
        {
            Name = name;
            Balance = balance;
            _lots = new List<Lot>();
        }

        public string Name { get; }
        public double Balance { get; private set; }
        public IReadOnlyList<Lot> Lots => _lots;

        public void WriteMoneyOff(double amount)
        {
            if (Balance < amount)
                throw ShopsExceptionFactory.InsufficientFundsException(amount, Balance);

            Balance -= amount;
        }

        public void Ship(Lot lot)
        {
            _lots.Add(lot);
        }
    }
}