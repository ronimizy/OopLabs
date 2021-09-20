using System.Linq;
using Shops.Console.Base.Interfaces;
using Shops.Console.Base.Views;
using Shops.Console.Views;
using Shops.Entities;
using Shops.Services;

namespace Shops.Console.ViewModels
{
    public class ShopListViewModel
    {
        private readonly ShopService _service;
        private readonly Person _user;

        public ShopListViewModel(ShopService service, Person user, INavigator navigator)
        {
            _service = service;
            _user = user;
            Navigator = navigator;
        }

        public INavigator Navigator { get; }

        public View[] NavigationLinks => _service.Shops
            .Select(s => new ShopViewModel(_service, s, _user, Navigator))
            .Select(vm => (View)new ShopView(vm))
            .ToArray();
    }
}