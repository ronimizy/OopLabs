namespace Shops.Models
{
    public struct Lot
    {
        public Lot(Product product, double price, int amount)
        {
            Product = product;
            Price = price;
            Amount = amount;
        }

        public Product Product { get; }
        public double Price { get; set; }
        public int Amount { get; set; }
    }
}