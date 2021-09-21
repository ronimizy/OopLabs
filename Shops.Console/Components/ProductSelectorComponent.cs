using System.Collections.Generic;
using Shops.Console.Base.Components;
using Shops.Entities;
using Spectre.Console;

namespace Shops.Console.Components
{
    public class ProductSelectorComponent : SelectorComponent<Product>
    {
        public ProductSelectorComponent(IReadOnlyCollection<Product> choices)
            : base("Select a product", choices, p => p.Name.EscapeMarkup()) { }
    }
}