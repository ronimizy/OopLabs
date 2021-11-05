using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Console;

namespace Spectre.Mvvm.Components
{
    public class SelectorComponent<T> : Component
        where T : notnull
    {
        private readonly string _title;
        private readonly IReadOnlyCollection<T> _choices;
        private readonly Func<T, string>? _converter;

        public SelectorComponent(string title, IReadOnlyCollection<T> choices, Func<T, string>? converter = null)
        {
            _title = title;
            _choices = choices;
            _converter = converter;
        }

        public delegate void ValueChangeHandler(T newValue);

        public event ValueChangeHandler? ValueChanged;

        public override void Draw()
        {
            if (!_choices.Any())
                return;

            SelectionPrompt<T> prompt = new SelectionPrompt<T>
                {
                    Title = _title,
                    Converter = _converter,
                }
                .AddChoices(_choices);

            T value = AnsiConsole.Prompt(prompt);
            OnValueChanged(value);
        }

        protected virtual void OnValueChanged(T newValue)
        {
            ValueChanged?.Invoke(newValue);
        }
    }
}