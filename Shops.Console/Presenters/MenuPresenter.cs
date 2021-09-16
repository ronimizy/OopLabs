using System.Collections.Generic;
using Shops.Console.Base.Presenters;
using Shops.Console.Views;
using Shops.Entities;
using Shops.Services;

namespace Shops.Console.Presenters
{
    public class MenuPresenter : NavigatedPresenter
    {
        private readonly ShopService _service;
        private readonly Person _user;

        public MenuPresenter(ShopService service, Person user)
        {
            _service = service;
            _user = user;

            View = new UserDetailsView(user);
        }

        public override string Title => "Menu";

        public override IReadOnlyList<Presenter> NavigationLinks
        {
            get
            {
                var links = new Presenter[]
                {
                    new ShopListPresenter(_user, _service.Shops, _service.Products),
                    new ProductListPresenter(_service.Products),
                    new RegisterShopPresenter(s => _service.RegisterShop(s)),
                    new RegisterProductPresenter(p => _service.RegisterProduct(p)),
                };

                return links;
            }
        }
    }
}