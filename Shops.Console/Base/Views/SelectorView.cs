using Shops.Console.Base.Delegates;
using Spectre.Console;

namespace Shops.Console.Base.Views
{
    public class SelectorView<T> : View
        where T : notnull
    {
        private readonly ISelectorViewDelegate<T> _delegate;

        public SelectorView(ISelectorViewDelegate<T> @delegate)
        {
            _delegate = @delegate;
        }

        public override void DrawBody()
        {
            SelectionPrompt<T> prompt = new SelectionPrompt<T>()
                .AddChoices(_delegate.GetChoices());

            T input = AnsiConsole.Prompt(prompt);
            _delegate.ProcessInput(input);
        }
    }
}