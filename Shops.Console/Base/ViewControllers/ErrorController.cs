using Shops.Console.Base.Views;

namespace Shops.Console.Base.ViewControllers
{
    public class ErrorController : NavigatedController
    {
        public ErrorController(string message)
        {
            View = new ErrorView(message);
        }

        public override string Title => "Error";
    }
}