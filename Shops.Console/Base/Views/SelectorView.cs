using System.Collections.Generic;
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

        protected override void RenderBody()
        {
            IReadOnlyList<T> choices = _delegate.GetChoices();
            if (choices.Count == 0)
                return;

            SelectionPrompt<T> prompt = new SelectionPrompt<T>()
                .AddChoices(choices);

            T input = AnsiConsole.Prompt(prompt);
            _delegate.ProcessInput(input);
        }
    }
}