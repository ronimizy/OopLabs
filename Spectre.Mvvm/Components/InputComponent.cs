using System;
using Spectre.Console;

namespace Spectre.Mvvm.Components
{
    public class InputComponent<T> : Component
    {
        private readonly string _title;
        private readonly Func<T, ValidationResult> _validator;
        private readonly bool _optional;

        public InputComponent(string title, Func<T, bool>? validator = null, bool optional = false)
        {
            _title = title;
            _optional = optional;
            _validator = v => validator?.Invoke(v) ?? true
                                  ? ValidationResult.Success()
                                  : ValidationResult.Error();
        }

        public delegate void ValueSubmittedHandler(T value);

        public event ValueSubmittedHandler? ValueSubmitted;

        public override void Draw()
        {
            TextPrompt<T> input = new TextPrompt<T>(_title)
                .Validate(_validator);

            input.AllowEmpty = _optional;

            T value = AnsiConsole.Prompt(input);
            OnValueSubmitted(value);
        }

        protected virtual void OnValueSubmitted(T value)
        {
            ValueSubmitted?.Invoke(value);
        }
    }
}