using System.Collections.Generic;
using Shops.Console.Base.ViewControllers;
using Shops.Console.Views;
using Shops.Entities;
using Shops.Services;

namespace Shops.Console.ViewControllers
{
    public class MenuViewController : NavigatedViewController
    {
        private readonly ShopService _service;
        private readonly Person _user;

        public MenuViewController(ShopService service, Person user)
        {
            _service = service;
            _user = user;

            AddView(new UserDetailsView(user));
        }

        public override string Title => "Menu";

        public override IReadOnlyList<ViewController> NavigationLinks
        {
            get
            {
                var links = new ViewController[]
                {
                    new ShopListViewController(_user, _service.Shops, _service.Products),
                    new ProductListViewController(_service.Products),
                    new RegisterShopViewController(s => _service.RegisterShop(s)),
                    new RegisterProductViewController(p => _service.RegisterProduct(p)),
                };

                return links;
            }
        }
    }
}