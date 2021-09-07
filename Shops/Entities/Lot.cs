using System;
using Shops.Tools;
using Utility.Extensions;

namespace Shops.Entities
{
    public class Lot
    {
        public Lot(Product product, double price, int amount)
        {
            Product = product.ThrowIfNull(nameof(product));
            Price = price;
            Amount = amount;
        }

        public Product Product { get; }
        public double Price { get; private set; }
        public int Amount { get; private set; }

        internal void ProposeNewPrice(double price)
        {
            double newPrice = Math.Max(Price, price);
            ValidatePrice(newPrice);
            Price = newPrice;
        }

        internal void SetNewPrice(double price)
        {
            ValidatePrice(price);
            Price = price;
        }

        internal void ChangeAmountBy(int value)
        {
            ValidateAmount(Amount + value);
            Amount += value;
        }

        private static void ValidatePrice(double value)
        {
            if (value < 0)
                throw ShopsExceptionFactory.NegativePriceException(value);
        }

        private static void ValidateAmount(int value)
        {
            if (value < 0)
                throw ShopsExceptionFactory.NegativeAmountException(value);
        }
    }
}