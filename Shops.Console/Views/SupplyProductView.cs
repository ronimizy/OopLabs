using Shops.Console.Base.Delegates;
using Shops.Console.Base.Models;
using Shops.Console.Base.Views;

namespace Shops.Console.Views
{
    public class SupplyProductView : View
    {
        public SupplyProductView(
            ISelectorViewDelegate<SelectorAction> productViewDelegate,
            IInputFieldDelegate<double> priceInputDelegate,
            IInputFieldDelegate<int> amountInputDelegate)
        {
            AddSubview(new SelectorView<SelectorAction>(productViewDelegate));
            AddSubview(new InputFieldView<double>("Price: ", priceInputDelegate));
            AddSubview(new InputFieldView<int>("Amount: ", amountInputDelegate));
        }
    }
}