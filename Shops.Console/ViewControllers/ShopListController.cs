using System.Collections.Generic;
using System.Linq;
using Shops.Console.Base.ViewControllers;
using Shops.Console.Views;
using Shops.Entities;

namespace Shops.Console.ViewControllers
{
    public class ShopListController : NavigatedController
    {
        private readonly Person _user;
        private readonly IReadOnlyCollection<Shop> _shops;
        private readonly IReadOnlyList<Product> _products;

        public ShopListController(Person user, IReadOnlyCollection<Shop> shops, IReadOnlyList<Product> products)
        {
            _user = user;
            _shops = shops;
            _products = products;

            View = new UserDetailsView(user);
        }

        public override string Title => "Shop List";

        public override IReadOnlyList<Controller> NavigationLinks => _shops
            .Select(s => new ShopController(_user, s, _products))
            .ToList();
    }
}