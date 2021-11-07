using System;
using System.Collections.Generic;
using Spectre.Mvvm.Components;
using Spectre.Mvvm.Views;

namespace Banks.Console.Views
{
    public class PromptView<T> : View
    {
        private readonly InputComponent<T>.ValueSubmittedHandler _callback;
        private readonly Func<T, bool>? _validator;

        public PromptView(
            string title,
            InputComponent<T>.ValueSubmittedHandler callback,
            Func<T, bool>? validator = null)
        {
            Title = title;
            _callback = callback;
            _validator = validator;
        }

        public override string Title { get; }

        protected override IReadOnlyCollection<Component> GetComponents()
        {
            var inputComponent = new InputComponent<T>(Title, _validator);
            inputComponent.ValueSubmitted += _callback;

            return new[] { inputComponent };
        }
    }
}