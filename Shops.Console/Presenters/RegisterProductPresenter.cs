using System;
using System.Collections.Generic;
using Shops.Console.Base.Delegates;
using Shops.Console.Base.Models;
using Shops.Console.Base.Presenters;
using Shops.Console.Delegates;
using Shops.Console.Views;
using Shops.Entities;
using Utility.Extensions;

namespace Shops.Console.Presenters
{
    public class RegisterProductPresenter : Presenter, ISelectorViewDelegate<SelectorAction>
    {
        private readonly Action<Product> _completion;
        private string? _productName;
        private string? _productDescription;

        public RegisterProductPresenter(Action<Product> completion)
        {
            _completion = completion;

            var nameFieldDelegate = new StrategyInputFieldDelegate<string>(s => _productName = s);
            var descriptionFieldDelegate = new StrategyInputFieldDelegate<string>(s => _productDescription = s, true);

            View = new RegisterProductView(nameFieldDelegate, descriptionFieldDelegate, this);
        }

        public override string Title => "Product Registration";

        public IReadOnlyList<SelectorAction> GetChoices()
        {
            SelectorAction[] actions =
            {
                new SelectorAction("Register", () =>
                {
                    _completion(new Product(
                                    _productName.ThrowIfNull(nameof(_productName)),
                                    _productDescription ?? string.Empty));
                    Parent?.RemoveChild(this);
                }),
                new SelectorAction("Discard", () => Parent?.RemoveChild(this)),
            };

            return actions;
        }

        public void ProcessInput(SelectorAction value)
            => value.Action();
    }
}