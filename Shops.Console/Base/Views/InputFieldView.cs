using Shops.Console.Base.Delegates;
using Spectre.Console;

namespace Shops.Console.Base.Views
{
    public class InputFieldView<T> : View
    {
        private readonly string _question;
        private readonly IInputFieldDelegate<T> _delegate;

        public InputFieldView(string question, IInputFieldDelegate<T> @delegate)
        {
            _question = question;
            _delegate = @delegate;
        }

        protected override void RenderBody()
        {
            TextPrompt<T> prompt = new TextPrompt<T>(_question) { AllowEmpty = _delegate.IsOptional() }
                .Validate(_delegate.Validate);

            T input = AnsiConsole.Prompt(prompt);
            _delegate.ProcessInput(input);
        }
    }
}