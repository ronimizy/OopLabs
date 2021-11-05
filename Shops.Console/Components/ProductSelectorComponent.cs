using System.Collections.Generic;
using Shops.Entities;
using Spectre.Console;
using Spectre.Mvvm.Components;

namespace Shops.Console.Components
{
    public class ProductSelectorComponent : SelectorComponent<Product>
    {
        public ProductSelectorComponent(IReadOnlyCollection<Product> choices)
            : base("Select a product", choices, p => p.Name.EscapeMarkup()) { }
    }
}