using Shops.Entities;
using Spectre.Console;
using Spectre.Mvvm.Components;

namespace Shops.Console.Components
{
    public class UserComponent : Component
    {
        private readonly Person _user;

        public UserComponent(Person user)
        {
            _user = user;
        }

        public override void Draw()
        {
            AnsiConsole.Markup("[bold]{0} - {1}$[/]\n", _user.Name.EscapeMarkup(), _user.Balance);
        }
    }
}