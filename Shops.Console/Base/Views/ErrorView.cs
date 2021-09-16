using Spectre.Console;

namespace Shops.Console.Base.Views
{
    public class ErrorView : View
    {
        private readonly string _message;

        public ErrorView(string message)
        {
            _message = message;
        }

        protected override void RenderBody()
        {
            AnsiConsole.Markup("[bold red]{0}[/]\n", Markup.Escape(_message));
        }
    }
}