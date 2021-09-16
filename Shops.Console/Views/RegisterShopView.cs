using Shops.Console.Base.Delegates;
using Shops.Console.Base.Models;
using Shops.Console.Base.Views;

namespace Shops.Console.Views
{
    public class RegisterShopView : View
    {
        public RegisterShopView(
            IInputFieldDelegate<string> nameInputFieldDelegate,
            IInputFieldDelegate<string> locationInputFieldDelegate,
            ISelectorViewDelegate<SelectorAction> selectorViewDelegate)
        {
            AddSubview(new InputFieldView<string>("Name: ", nameInputFieldDelegate));
            AddSubview(new InputFieldView<string>("Location: ", locationInputFieldDelegate));
            AddSubview(new SelectorView<SelectorAction>(selectorViewDelegate));
        }
    }
}