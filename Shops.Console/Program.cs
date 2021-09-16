using Shops.Console.Base;
using Shops.Console.Base.ViewControllers;
using Shops.Console.ViewControllers;
using Shops.Entities;
using Shops.Services;

namespace Shops.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var service = new ShopService();

            var shop = new Shop("SPAR", "Saint-Petersburg");
            var product = new Product("Pizza Piece", "One piece of extra delicious pizza");

            service.RegisterShop(shop);
            service.RegisterProduct(product);

            shop.SupplyProduct(product, 2, 100);

            var signupViewController = new SignUpController(service);
            var navigationController = new NavigationController(signupViewController);
            var window = new Window(navigationController);

            window.Run(10);
        }
    }
}