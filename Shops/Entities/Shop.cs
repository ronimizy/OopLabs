using System.Collections.Generic;
using System.Linq;
using Shops.Models;
using Shops.Tools;
using Utility.Extensions;

namespace Shops.Entities
{
    public sealed class Shop
    {
        private static readonly IdGenerator IdGenerator = new ();
        private readonly List<Lot> _lots;

        internal Shop(string name, string location)
        {
            Id = IdGenerator.Next();
            Name = name.ThrowIfNull(nameof(name));
            Location = location.ThrowIfNull(nameof(location));
            _lots = new List<Lot>();
        }

        public int Id { get; }
        public string Name { get; }
        public string Location { get; }

        public Shop AddProducts(params Lot[] shipment)
        {
            shipment.ThrowIfNull(nameof(shipment));

            foreach (Lot lot in shipment)
            {
                Lot? exisingLot = ProductLotOrDefault(lot.Product);
                if (exisingLot is null)
                {
                    _lots.Add(lot);
                }
                else
                {
                    exisingLot.ChangeAmountBy(lot.Amount);
                    exisingLot.ProposeNewPrice(lot.Price);
                }
            }

            return this;
        }

        public Shop Buy(Person person, Product product, int amount)
        {
            if (amount < 0)
                throw ShopsExceptionFactory.NegativeAmountException(amount);

            person.ThrowIfNull(nameof(person));
            product.ThrowIfNull(nameof(product));

            Lot? lot = ProductLotOrDefault(product);
            if (lot is null)
                throw ShopsExceptionFactory.NonExisingProductException(this, product);

            if (lot.Amount < amount)
                throw ShopsExceptionFactory.InsufficientProductAmountException(product, amount, lot.Amount);

            double totalPrice = lot.Price * amount;

            person.SendBill(totalPrice);
            lot.ChangeAmountBy(-amount);

            return this;
        }

        public Shop SetPriceFor(Product product, double price)
        {
            if (price < 0)
                throw ShopsExceptionFactory.NegativePriceException(price);

            product.ThrowIfNull(nameof(product));

            Lot? lot = ProductLotOrDefault(product);
            if (lot is null)
                _lots.Add(new Lot(product, price, 0));
            else
                lot.SetNewPrice(price);

            return this;
        }

        public bool ProductsAvailable(Product product, int amount)
        {
            Lot? lot = _lots.SingleOrDefault(l => l.Product.Equals(product) && l.Amount >= amount);
            return lot is not null;
        }

        public Lot? ProductLotOrDefault(Product product)
            => _lots.SingleOrDefault(l => l.Product.Equals(product));

        public Lot ProductLot(Product product)
        {
            Lot? lot = ProductLotOrDefault(product);
            if (lot is null)
                throw ShopsExceptionFactory.NonExisingProductException(this, product);

            return lot;
        }

        public override string ToString()
            => $"[{Id}] {Name}";
    }
}