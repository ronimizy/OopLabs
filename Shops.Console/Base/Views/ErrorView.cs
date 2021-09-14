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

        public override void DrawBody()
        {
            AnsiConsole.Markup("[bold red]{0}[/]", Markup.Escape(_message));
        }
    }
}