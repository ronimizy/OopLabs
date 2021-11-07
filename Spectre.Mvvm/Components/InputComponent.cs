using System;
using Spectre.Console;

namespace Spectre.Mvvm.Components
{
    public class InputComponent<T> : Component
    {
        private readonly string _title;
        private readonly Func<T, ValidationResult> _validator;
        private readonly bool _optional;

        public InputComponent(
            string title,
            Func<T, bool>? validator = null,
            bool optional = false,
            ValueSubmittedHandler? defaultHandler = null)
        {
            _title = title;
            _optional = optional;
            _validator = v => validator?.Invoke(v) ?? true
                ? ValidationResult.Success()
                : ValidationResult.Error();
            ValueSubmitted += defaultHandler;
        }

        public delegate void ValueSubmittedHandler(T value);

        public event ValueSubmittedHandler? ValueSubmitted;

        public bool IsSecret { get; init; }

        public override void Draw()
        {
            var input = new TextPrompt<T>(_title)
            {
                IsSecret = IsSecret,
                Validator = _validator,
                AllowEmpty = _optional,
            };

            T value = AnsiConsole.Prompt(input);
            OnValueSubmitted(value);
        }

        protected virtual void OnValueSubmitted(T value)
        {
            ValueSubmitted?.Invoke(value);
        }
    }
}