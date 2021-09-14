using Shops.Console.Base.Views;
using Shops.Entities;
using Spectre.Console;

namespace Shops.Console.Views
{
    public class UserDetailsView : View
    {
        private readonly Person _user;

        public UserDetailsView(Person user)
        {
            _user = user;
        }

        public override void DrawBody()
        {
            string message = $"{_user.Name} - {_user.Balance}$";

            var balanceLabel = new Text(message)
            {
                Alignment = Justify.Left,
            };

            AnsiConsole.Render(balanceLabel);
            AnsiConsole.WriteLine();
        }
    }
}