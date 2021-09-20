using Shops.Console.Base.Interfaces;
using Shops.Console.Base.Views;
using Shops.Console.Views;
using Shops.Entities;
using Shops.Services;

namespace Shops.Console.ViewModels
{
    public class MenuViewModel
    {
        private readonly ShopService _service;

        public MenuViewModel(ShopService service, Person user, INavigator navigator)
        {
            _service = service;
            User = user;
            Navigator = navigator;
        }

        public Person User { get; }
        public INavigator Navigator { get; }

        public View[] NavigationLinks => new View[]
        {
            new ShopListView(new ShopListViewModel(_service, User, Navigator)),
            new ProductListView(new ProductListViewModel(_service, Navigator)),
            new RegisterShopView(new RegisterShopViewModel(_service, Navigator)),
            new RegisterProductView(new RegisterProductViewModel(_service, Navigator)),
            new FindCheapestView(new FindCheapestViewModel(_service, Navigator, User)),
        };
    }
}