using Shops.Console.Base.Views;
using Shops.Entities;
using Spectre.Console;

namespace Shops.Console.Views
{
    public class UserDetailsView : View
    {
        public UserDetailsView(Person user)
        {
            string message = $"{user.Name} - {user.Balance}$\n";
            var markup = new Markup(message)
            {
                Alignment = Justify.Left,
            };

            AddSubview(new MarkupView(markup));
        }
    }
}