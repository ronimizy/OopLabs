using Shops.Console.Base.Views;

namespace Shops.Console.Base.ViewControllers
{
    public class ErrorViewController : NavigatedViewController
    {
        public ErrorViewController(string message)
        {
            AddView(new ErrorView(message));
        }

        public override string Title => "Error";
    }
}