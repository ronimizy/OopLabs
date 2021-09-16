using Shops.Console.Base;
using Shops.Console.Base.Presenters;
using Shops.Console.Presenters;
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

            var signUpPresenter = new SignUpPresenter(service);
            var navigationPresenter = new NavigationPresenter(signUpPresenter);
            var window = new Window(navigationPresenter);

            window.Run(10);
        }
    }
}