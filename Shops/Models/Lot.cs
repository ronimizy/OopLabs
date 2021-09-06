using Shops.Tools;
using Utility.Extensions;

namespace Shops.Models
{
    public struct Lot
    {
        private double _price;
        private int _amount;

        public Lot(Product product, double price, int amount)
        {
            Product = product.ThrowIfNull(nameof(product));
            _price = price;
            _amount = amount;
        }

        public Product Product { get; }
        public double Price
        {
            get => _price;
            set => _price = ValidatePrice(value);
        }

        public int Amount
        {
            get => _amount;
            set => _amount = ValidateAmount(value);
        }

        private static double ValidatePrice(double value)
        {
            if (value < 0)
                throw ShopsExceptionFactory.NegativePriceException(value);

            return value;
        }

        private static int ValidateAmount(int value)
        {
            if (value < 0)
                throw ShopsExceptionFactory.NegativeAmountException(value);

            return value;
        }
    }
}