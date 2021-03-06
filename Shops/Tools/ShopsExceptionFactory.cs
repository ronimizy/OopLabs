using System;
using Shops.Entities;

namespace Shops.Tools
{
    internal static class ShopsExceptionFactory
    {
        public static ShopException InsufficientFundsException(double requested, double owned)
            => new ShopException($"Buyer has insufficient funds. {requested} requested, {owned} owned.");

        public static ShopException NegativeAmountException(int amount)
            => new ShopException($"Product amount cannot be negative. Given value: {amount}.");

        public static ShopException NegativePriceException(double price)
            => new ShopException($"Lot price cannot be negative. Given value: {price}.");

        public static ShopException InsufficientProductAmountException(Product product, int requestedAmount, int ownedAmount)
            => new ShopException($"Insufficient amount of {product}. {requestedAmount} requested, {ownedAmount} owned");

        public static ShopException NonExisingProductException(Shop shop, Product product)
            => new ShopException($"{shop} doesn't have a {product}");

        public static ShopException NonExistingShopException(Guid shopId)
            => new ShopException($"Shop with id = {shopId} does not exist");

        public static ShopException NoShopFoundException(Product product, int amount)
            => new ShopException($"No shop was found containing {product} with amount equal {amount}");
    }
}