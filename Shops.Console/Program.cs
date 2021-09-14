using Shops.Console.Base;
using Shops.Console.Base.ViewControllers;
using Shops.Console.ViewControllers;
using Shops.Services;

namespace Shops.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var service = new ShopService();

            var signupViewController = new SignUpViewController(service);
            var navigationController = new NavigationViewController(signupViewController);
            var window = new Window(navigationController);

            window.Run(10);
        }
    }
}