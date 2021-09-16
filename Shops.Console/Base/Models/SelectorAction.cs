using System;

namespace Shops.Console.Base.Models
{
    public record SelectorAction(string Title, Action Action)
    {
        public override string ToString()
            => Title;
    }
}