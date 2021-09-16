using Shops.Console.Base.Delegates;
using Shops.Console.Base.Models;
using Shops.Console.Base.Views;
using Shops.Console.Delegates;

namespace Shops.Console.Views
{
    public class BuyProductView : View
    {
        public BuyProductView(SelectProductDelegate selectProductDelegate, IInputFieldDelegate<int> inputDelegate)
        {
            AddSubview(new SelectorView<SelectorAction>(selectProductDelegate));
            AddSubview(new InputFieldView<int>("Amount: ", inputDelegate));
        }
    }
}