using Shops.Console.Base.Views;

namespace Shops.Console.Base.Presenters
{
    public class ErrorPresenter : NavigatedPresenter
    {
        public ErrorPresenter(string message)
        {
            View = new ErrorView(message);
        }

        public override string Title => "Error";
    }
}