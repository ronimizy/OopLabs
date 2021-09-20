using System.Collections.Generic;
using System.Linq;
using Shops.Console.Base.Interfaces;
using Shops.Services;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace Shops.Console.ViewModels
{
    public class ProductListViewModel
    {
        private readonly ShopService _service;

        public ProductListViewModel(ShopService service, INavigator navigator)
        {
            _service = service;
            Navigator = navigator;
        }

        public INavigator Navigator { get; }

        public IReadOnlyCollection<TableColumn> Headers => new[]
        {
            new TableColumn("Name"),
            new TableColumn("Description"),
        };

        public IReadOnlyCollection<IReadOnlyCollection<IRenderable>> Data => _service.Products
            .Select(p => new[]
            {
                new Text(p.Name),
                new Text(p.Description),
            })
            .ToList();
    }
}