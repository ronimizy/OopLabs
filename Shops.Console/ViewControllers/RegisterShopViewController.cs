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
    public class RegisterShopViewController : ViewController, ISelectorViewDelegate<SelectorAction>
    {
        private readonly Action<Shop> _completion;
        private string? _shopName;
        private string? _shopLocation;

        public RegisterShopViewController(Action<Shop> completion)
        {
            _completion = completion;

            AddView(new InputFieldView<string>(
                                "Name: ",
                                new StrategyInputFieldDelegate<string>(v => _shopName = v)));
            AddView(new InputFieldView<string>(
                                "Location: ",
                                new StrategyInputFieldDelegate<string>(v => _shopLocation = v)));

            AddView(new SelectorView<SelectorAction>(this));
        }

        public override string Title => "Shop Registration";

        public IReadOnlyList<SelectorAction> GetChoices()
        {
            SelectorAction[] actions =
            {
                new SelectorAction("Create", () =>
                {
                    _completion(new Shop(
                                    _shopName.ThrowIfNull(nameof(_shopName)),
                                    _shopLocation.ThrowIfNull(nameof(_shopLocation))));
                    OnDismiss(this);
                }),
                new SelectorAction("Discard", () => OnDismiss(this)),
            };

            return actions;
        }

        public void ProcessInput(SelectorAction value)
            => value.Action();
    }
}