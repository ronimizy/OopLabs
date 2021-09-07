using System;
using NUnit.Framework;
using Shops.Entities;
using Shops.Services;
using Shops.Tools;

namespace Shops.Tests
{
    [TestFixture]
    public class ShopTests
    {
        private const string ShopName = "Shop Name";
        private const string ShopLocation = "Shop Location";

        private const string ProductName = "Product Name";
        private const string ProductDescription = "Product Description";

        private ShopService _service = null!;
        private Shop _shop = null!;
        private Product _product = null!;

        [SetUp]
        public void Setup()
        {
            _service = new ShopService();
            _shop = _service.CreateShop(ShopName, ShopLocation);
            _product = _service.RegisterProduct(ProductName, ProductDescription);
        }

        [Test]
        public void SupplyTest_LotAdded_LotsAvailable()
        {
            const int price = 20;
            const int amount = 20;

            Assert.NotNull(_shop);
            Assert.NotNull(_product);

            _shop.AddProducts(new Lot(_product, price, amount));

            Assert.IsTrue(_shop.ProductAvailable(_product, amount));

            Lot lot = _shop.ProductLot(_product);

            Assert.AreSame(_product, lot.Product);
            Assert.AreEqual(price, lot.Price);
            Assert.AreEqual(amount, lot.Amount);
        }

        [Test]
        public void AddExisingProductTest_ProductAdded_PriceChanged()
        {
            const double firstPrice = 10;
            const double secondPrice = 2 * firstPrice;
            const int amount = 10;

            _shop.AddProducts(new Lot(_product, firstPrice, amount))
                .AddProducts(new Lot(_product, secondPrice, amount));

            Lot lot = _shop.ProductLot(_product);
            
            Assert.AreEqual(2 * amount, lot.Amount);
            Assert.AreEqual(secondPrice, lot.Price);
        }

        [Test]
        public void ChangePriceTest_LotAdded_PriceChanged()
        {
            const int oldPrice = 20;
            const int newPrice = 30;

            _shop.AddProducts(new Lot(_product, oldPrice, 0))
                .SetPriceFor(_product, newPrice);

            Lot lot = _shop.ProductLot(_product);

            Assert.AreEqual(newPrice, lot.Price);
        }

        [Test]
        public void SetPriceTest_PriceSet()
        {
            const int newPrice = 30;

            _shop.SetPriceFor(_product, newPrice);
            Lot lot = _shop.ProductLot(_product);

            Assert.AreEqual(newPrice, lot.Price);
        }

        [Test]
        public void AcquisitionTest_ProductsCreated_LotsAdded_PurchasesMade()
        {
            const double poorBalance = 0.5;
            const double richBalance = double.MaxValue;
            const double largePrice = double.MaxValue;
            const double smallPrice = 20;
            const int largeAmount = 5;
            const int smallAmount = 1;
            
            var poorPerson = new Person("Poor", poorBalance);
            var richPerson = new Person("Rich", richBalance);
            
            Product expensiveProduct = _service.RegisterProduct("Expensive", "Really expensive");

            _shop.AddProducts(new Lot(_product, smallPrice, largeAmount),
                              new Lot(expensiveProduct, largePrice, smallAmount));

            Assert.Throws<ShopException>(() => _shop.Buy(poorPerson, expensiveProduct, smallAmount));
            Assert.Throws<ShopException>(() => _shop.Buy(richPerson, _product, largeAmount + 1));
            Assert.DoesNotThrow(() => _shop.Buy(richPerson, _product, smallAmount));
        }

        [Test]
        public void BuyCheapestTest_BoughtNegativeAmount_BoughtMoreItemsThanExists_NotEnoughMoney_ValidPurchase()
        {
            const int balance = 100;

            const int firstPrice = 10;
            const int secondPrice = 20;
            
            const int firstCount = 20;
            const int secondCount = 10;
            
            Shop anotherShop = _service.CreateShop("Another Shop", "Another Location");
            var person = new Person("Name", balance);

            _shop.AddProducts(new Lot(_product, firstPrice, firstCount));
            anotherShop.AddProducts(new Lot(_product, secondPrice, secondCount));

            Assert.Throws<ShopException>(() => _service.BuyCheapest(person, _product, firstCount + secondCount + 1));
            Assert.Throws<ShopException>(() => _service.BuyCheapest(person, _product, firstCount + secondCount));
            Assert.Throws<ShopException>(() => _service.BuyCheapest(person, _product, -1));
            _service.BuyCheapest(person, _product, Math.Min(firstCount, secondCount));
        }
    }
}