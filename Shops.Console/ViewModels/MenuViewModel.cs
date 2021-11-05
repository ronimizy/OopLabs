using Shops.Console.Views;
using Shops.Entities;
using Shops.Services;
using Spectre.Mvvm.Interfaces;
using Spectre.Mvvm.Views;

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