using System.Collections.Generic;
using Shops.Console.Base.ViewControllers;
using Shops.Console.Views;
using Shops.Entities;
using Shops.Services;

namespace Shops.Console.ViewControllers
{
    public class MenuController : NavigatedController
    {
        private readonly ShopService _service;
        private readonly Person _user;

        public MenuController(ShopService service, Person user)
        {
            _service = service;
            _user = user;

            View = new UserDetailsView(user);
        }

        public override string Title => "Menu";

        public override IReadOnlyList<Controller> NavigationLinks
        {
            get
            {
                var links = new Controller[]
                {
                    new ShopListController(_user, _service.Shops, _service.Products),
                    new ProductListController(_service.Products),
                    new RegisterShopController(s => _service.RegisterShop(s)),
                    new RegisterProductController(p => _service.RegisterProduct(p)),
                };

                return links;
            }
        }
    }
}