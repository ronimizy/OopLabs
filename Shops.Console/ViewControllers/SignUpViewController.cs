using Shops.Console.Base.ViewControllers;
using Shops.Console.Base.Views;
using Shops.Console.Delegates;
using Shops.Entities;
using Shops.Services;
using Spectre.Console;
using Utility.Extensions;

namespace Shops.Console.ViewControllers
{
    public class SignUpViewController : ViewController
    {
        private readonly ShopService _service;
        private string? _username;
        private int? _balance;

        public SignUpViewController(ShopService service)
        {
            _service = service;

            var usernameDelegate = new StrategyInputFieldDelegate<string>(s => _username = s);
            var balanceDelegate = new StrategyInputFieldDelegate<int>(
                v => _balance = v,
                validator: v => v >= 0 ? ValidationResult.Success() : ValidationResult.Error());

            AddView(new InputFieldView<string>("Username: ", usernameDelegate));
            AddView(new InputFieldView<int>("Balance: ", balanceDelegate));
        }

        public override string Title => "Sign Up";

        public override void DrawContent()
        {
            base.DrawContent();

            OnPushController(new MenuViewController(
                                 _service,
                                 new Person(
                                     _username.ThrowIfNull(nameof(_username)),
                                     _balance.ThrowIfNull(nameof(_balance)))));
        }
    }
}