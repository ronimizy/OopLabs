using Shops.Console.Base.Delegates;
using Shops.Console.Base.Views;
using Shops.Entities;

namespace Shops.Console.Views
{
    public class ShopView : View
    {
        public ShopView(Person user, ITableViewDelegate shopTableDelegate)
        {
            AddSubview(new UserDetailsView(user));
            AddSubview(new TableView(shopTableDelegate));
        }
    }
}