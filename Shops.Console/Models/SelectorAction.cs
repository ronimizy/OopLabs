using System;

namespace Shops.Console.Models
{
    public record SelectorAction(string Title, Action Action)
    {
        public override string ToString()
            => Title;
    }
}