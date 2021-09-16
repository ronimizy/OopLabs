using System;
using Shops.Console.Base.Presenters;
using Shops.Console.Delegates;
using Shops.Console.Views;
using Shops.Entities;
using Shops.Services;

namespace Shops.Console.Presenters
{
    public class SignUpPresenter : Presenter
    {
        private readonly ShopService _service;
        private string? _username;
        private int? _balance;

        public SignUpPresenter(ShopService service)
        {
            _service = service;

            var usernameDelegate = new StrategyInputFieldDelegate<string>(s => _username = s);
            var balanceDelegate = new StrategyInputFieldDelegate<int>(v => _balance = v, validator: v => v >= 0);

            View = new SignUpView(usernameDelegate, balanceDelegate)
            {
                Presenter = this,
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

            Parent?.AddChild(new MenuPresenter(_service, new Person(_username, _balance.Value)));
        }
    }
}