using System.Collections.Generic;
using System.Linq;
using Shops.Console.Base.Presenters;
using Shops.Console.Views;
using Shops.Entities;

namespace Shops.Console.Presenters
{
    public class ShopListPresenter : NavigatedPresenter
    {
        private readonly Person _user;
        private readonly IReadOnlyCollection<Shop> _shops;
        private readonly IReadOnlyList<Product> _products;

        public ShopListPresenter(Person user, IReadOnlyCollection<Shop> shops, IReadOnlyList<Product> products)
        {
            _user = user;
            _shops = shops;
            _products = products;

            View = new UserDetailsView(user);
        }

        public override string Title => "Shop List";

        public override IReadOnlyList<Presenter> NavigationLinks => _shops
            .Select(s => new ShopPresenter(_user, s, _products))
            .ToList();
    }
}