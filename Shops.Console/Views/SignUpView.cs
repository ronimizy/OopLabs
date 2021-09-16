using Shops.Console.Base.Delegates;
using Shops.Console.Base.Views;

namespace Shops.Console.Views
{
    public class SignUpView : View
    {
        public SignUpView(IInputFieldDelegate<string> usernameDelegate, IInputFieldDelegate<int> balanceDelegate)
        {
            AddSubview(new InputFieldView<string>("Username: ", usernameDelegate));
            AddSubview(new InputFieldView<int>("Balance: ", balanceDelegate));
        }
    }
}