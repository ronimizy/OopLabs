using Shops.Console.Base.Views;

namespace Shops.Console.Base.Interfaces
{
    public interface INavigator
    {
        void PushView(View view);
        void PopView();
    }
}