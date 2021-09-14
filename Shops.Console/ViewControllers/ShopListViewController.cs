using System.Collections.Generic;
using System.Linq;
using Shops.Console.Base.ViewControllers;
using Shops.Console.Views;
using Shops.Entities;

namespace Shops.Console.ViewControllers
{
    public class ShopListViewController : NavigatedViewController
    {
        private readonly Person _user;
        private readonly IReadOnlyCollection<Shop> _shops;
        private readonly IReadOnlyList<Product> _products;

        public ShopListViewController(Person user, IReadOnlyCollection<Shop> shops, IReadOnlyList<Product> products)
        {
            _user = user;
            _shops = shops;
            _products = products;

            AddView(new UserDetailsView(user));
        }

        public override string Title => "Shop List";

        public override IReadOnlyList<ViewController> NavigationLinks => _shops
            .Select(s => new ShopViewController(_user, s, _products))
            .ToList();
    }
}