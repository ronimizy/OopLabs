using System;
using Spectre.Console;

namespace Spectre.Mvvm.Components
{
    public class ButtonComponent : Component
    {
        private readonly string _title;
        private readonly Action _action;

        public ButtonComponent(string title, Action action)
        {
            _title = title;
            _action = action;
        }

        public override void Draw()
        {
            var prompt = new SelectionPrompt<string>
            {
                Title = string.Empty,
            };

            prompt.AddChoice(_title);

            AnsiConsole.Prompt(prompt);
            _action();
        }
    }
}