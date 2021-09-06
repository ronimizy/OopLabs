using Shops.Entities;

namespace Shops.Models
{
    public class BulkPurchaseEntry
    {
        public BulkPurchaseEntry(Shop shop, Lot lot)
        {
            Shop = shop;
            Lot = lot;
        }

        public Shop Shop { get; }
        public Lot Lot { get; }
        public int AmountToBuy { get; set; }
    }
}