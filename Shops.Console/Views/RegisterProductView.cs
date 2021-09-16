using Shops.Console.Base.Delegates;
using Shops.Console.Base.Models;
using Shops.Console.Base.Views;

namespace Shops.Console.Views
{
    public class RegisterProductView : View
    {
        public RegisterProductView(
            IInputFieldDelegate<string> nameFieldDelegate,
            IInputFieldDelegate<string> descriptionFieldDelegate,
            ISelectorViewDelegate<SelectorAction> selectorViewDelegate)
        {
            AddSubview(new InputFieldView<string>("Name: ", nameFieldDelegate));
            AddSubview(new InputFieldView<string>("Description: ", descriptionFieldDelegate));
            AddSubview(new SelectorView<SelectorAction>(selectorViewDelegate));
        }
    }
}