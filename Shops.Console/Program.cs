using Shops.Console.ViewModels;
using Shops.Console.Views;
using Shops.Entities;
using Shops.Services;
using Spectre.Mvvm;
using Spectre.Mvvm.Views;

namespace Shops.Console
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var service = new ShopService();

            var shop = new Shop("SPAR", "Saint-Petersburg");
            var product = new Product("Pizza Piece", "One piece of extra delicious pizza");

            service.RegisterShop(shop);
            service.RegisterProduct(product);

            shop.SupplyProduct(product, 2, 100);

            var navigationView = new NavigationView(n => new SignUpView(new SignUpViewModel(service, n)));
            var window = new Window(navigationView);

            window.Run(10);
        }
    }
}