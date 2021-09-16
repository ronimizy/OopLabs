using System;
using Shops.Console.Base.ViewControllers;
using Shops.Console.Delegates;
using Shops.Console.Views;
using Shops.Entities;
using Shops.Services;

namespace Shops.Console.ViewControllers
{
    public class SignUpController : Controller
    {
        private readonly ShopService _service;
        private string? _username;
        private int? _balance;

        public SignUpController(ShopService service)
        {
            _service = service;

            var usernameDelegate = new StrategyInputFieldDelegate<string>(s => _username = s);
            var balanceDelegate = new StrategyInputFieldDelegate<int>(v => _balance = v, validator: v => v >= 0);

            View = new SignUpView(usernameDelegate, balanceDelegate)
            {
                Controller = this,
            };
        }

        public override string Title => "Sign Up";

        public override void OnViewRendered()
        {
            if (_username is null)
            {
                Parent?.OnError(this, new ArgumentNullException(nameof(_username)));
                return;
            }

            if (_balance is null)
            {
                Parent?.OnError(this, new ArgumentNullException(nameof(_balance)));
                return;
            }

            Parent?.AddChild(new MenuController(_service, new Person(_username, _balance.Value)));
        }
    }
}