using System;
using Shops.Entities;
using Shops.Models;

namespace Shops.Tools
{
    internal static class ShopsExceptionFactory
    {
        public static InvalidOperationException InsufficientFundsException(double requested, double owned)
            => new InvalidOperationException($"Buyer has insufficient funds. {requested} requested, {owned} owned");

        public static ShopException InsufficientProductAmountException(Shop shop, Product product, int requestedAmount, int ownedAmount)
            => new ShopException($"{shop} doesn't have requested amount of {product}. " +
                                 $"{requestedAmount} requested, {ownedAmount} owned");

        public static ShopException NonExisingProductException(Shop shop, Product product)
            => new ShopException($"{shop} doesn't have a {product}");
    }
}