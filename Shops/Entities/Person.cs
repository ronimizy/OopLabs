using Shops.Tools;
using Utility.Extensions;

namespace Shops.Entities
{
    public class Person
    {
        public Person(string name, double balance)
        {
            Name = name.ThrowIfNull(nameof(name));
            Balance = balance;
        }

        public string Name { get; }
        public double Balance { get; private set; }

        public void SendBill(double amount)
        {
            if (Balance < amount)
                throw ShopsExceptionFactory.InsufficientFundsException(amount, Balance);

            Balance -= amount;
        }
    }
}