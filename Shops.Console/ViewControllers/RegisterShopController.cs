using System;
using System.Collections.Generic;
using Shops.Console.Base.Delegates;
using Shops.Console.Base.Models;
using Shops.Console.Base.ViewControllers;
using Shops.Console.Delegates;
using Shops.Console.Views;
using Shops.Entities;
using Utility.Extensions;

namespace Shops.Console.ViewControllers
{
    public class RegisterShopController : Controller, ISelectorViewDelegate<SelectorAction>
    {
        private readonly Action<Shop> _completion;
        private string? _shopName;
        private string? _shopLocation;

        public RegisterShopController(Action<Shop> completion)
        {
            _completion = completion;

            var nameInputFieldDelegate = new StrategyInputFieldDelegate<string>(v => _shopName = v);
            var locationInputFieldDelegate = new StrategyInputFieldDelegate<string>(v => _shopLocation = v);

            View = new RegisterShopView(nameInputFieldDelegate, locationInputFieldDelegate, this);
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