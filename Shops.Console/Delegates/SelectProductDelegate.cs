using System;
using System.Collections.Generic;
using System.Linq;
using Shops.Console.Base.Delegates;
using Shops.Console.Base.Models;
using Shops.Entities;

namespace Shops.Console.Delegates
{
    public class SelectProductDelegate : ISelectorViewDelegate<SelectorAction>
    {
        private readonly IReadOnlyList<Product> _products;
        private readonly Action<Product> _completion;

        public SelectProductDelegate(IReadOnlyList<Product> products, Action<Product> completion)
        {
            _products = products;
            _completion = completion;
        }

        public IReadOnlyList<SelectorAction> GetChoices()
            => _products
                .Select(p => new SelectorAction(p.Name, () => _completion(p)))
                .ToList();

        public void ProcessInput(SelectorAction value)
            => value.Action();
    }
}