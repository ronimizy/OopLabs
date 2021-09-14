using System;
using System.Collections.Generic;
using Shops.Console.Base.Delegates;
using Shops.Console.Base.ViewControllers;
using Shops.Console.Base.Views;
using Shops.Console.Delegates;
using Shops.Console.Models;
using Shops.Entities;
using Utility.Extensions;

namespace Shops.Console.ViewControllers
{
    public class RegisterProductViewController : ViewController, ISelectorViewDelegate<SelectorAction>
    {
        private readonly Action<Product> _completion;
        private string? _productName;
        private string? _productDescription;

        public RegisterProductViewController(Action<Product> completion)
        {
            _completion = completion;

            AddView(new InputFieldView<string>(
                                "Name: ",
                                new StrategyInputFieldDelegate<string>(s => _productName = s)));
            AddView(new InputFieldView<string>(
                                "Description: ",
                                new StrategyInputFieldDelegate<string>(
                                    s => _productDescription = s,
                                    true)));

            AddView(new SelectorView<SelectorAction>(this));
        }

        public override string Title => "Product Registration";

        public void ProcessInput(SelectorAction value)
            => value.Action();

        public IReadOnlyList<SelectorAction> GetChoices()
        {
            SelectorAction[] actions =
            {
                new SelectorAction("Register", () =>
                {
                    _completion(new Product(
                                    _productName.ThrowIfNull(nameof(_productName)),
                                    _productDescription ?? string.Empty));
                    OnDismiss(this);
                }),
                new SelectorAction("Discard", () => OnDismiss(this)),
            };

            return actions;
        }
    }
}