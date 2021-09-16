using Shops.Console.Base.Delegates;
using Shops.Console.Base.Models;
using Shops.Console.Base.Views;

namespace Shops.Console.Views
{
    public class SetPriceView : View
    {
        public SetPriceView(ISelectorViewDelegate<SelectorAction> productSelectorDelegate, IInputFieldDelegate<double> priceInputFieldDelegate)
        {
            AddSubview(new SelectorView<SelectorAction>(productSelectorDelegate));
            AddSubview(new InputFieldView<double>("New Price: ", priceInputFieldDelegate));
        }
    }
}