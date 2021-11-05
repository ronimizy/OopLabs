using System.Collections.Generic;
using System.Linq;
using Spectre.Console;
using Spectre.Mvvm.Interfaces;
using Spectre.Mvvm.Models;
using Spectre.Mvvm.Views;

namespace Spectre.Mvvm.Components
{
    public class NavigationComponent : Component
    {
        private readonly IReadOnlyCollection<SelectorAction> _actions;

        public NavigationComponent(INavigator navigator, bool needBackButton, params View[] views)
        {
            var actions = new List<SelectorAction>();

            if (needBackButton)
                actions.Add(new SelectorAction("Back", navigator.PopView));

            actions.AddRange(
                views.Select(v => new SelectorAction(v.Title, () => navigator.PushView(v))));

            _actions = actions;
        }

        public override void Draw()
        {
            var prompt = new SelectionPrompt<SelectorAction>
            {
                Title = string.Empty,
            };
            prompt.AddChoices(_actions);

            SelectorAction choice = AnsiConsole.Prompt(prompt);
            choice.Action();
        }
    }
}