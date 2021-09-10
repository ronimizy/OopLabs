using Shops.Tools;
using Utility.Extensions;

namespace Shops.Entities
{
    public class Lot
    {
        private double _price;
        private int _amount;

        public Lot(double price, int amount)
        {
            Price = price;
            Amount = amount;
        }

        public double Price
        {
            get => _price;
            set
            {
                ValidatePrice(value);
                _price = value;
            }
        }

        public int Amount
        {
            get => _amount;
            set
            {
                ValidateAmount(value);
                _amount = value;
            }
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