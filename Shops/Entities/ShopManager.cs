using System.Threading;
using Shops.Models;

namespace Shops.Entities
{
    public class ShopManager
    {
        private static int _currentManagerId;

        private int _currentShopId;
        private int _currentProductId;

        public ShopManager()
        {
            Id = Interlocked.Increment(ref _currentManagerId);
        }

        public int Id { get; }

        public Shop CreateShop(string name, string location)
        {
            return new Shop(GetNewShopId(), name, location, Id);
        }

        public Product RegisterProduct(string name, string description)
        {
            return new Product(GetNewProductId(), name, description, Id);
        }

        private int GetNewShopId()
        {
            return Interlocked.Increment(ref _currentShopId);
        }

        private int GetNewProductId()
        {
            return Interlocked.Increment(ref _currentProductId);
        }
    }
}