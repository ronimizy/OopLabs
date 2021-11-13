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
        private readonly INavigator _navigator;
        private readonly IReadOnlyCollection<NavigationElement> _elements;

        public NavigationComponent(INavigator navigator, bool needBackButton, params View[] views)
        {
            _navigator = navigator;
            NeedBackButton = needBackButton;
            _elements = views
                .Select(v => new NavigationElement(v.Title, n => n.PushView(v)))
                .ToArray();
        }

        public NavigationComponent(INavigator navigator, params NavigationElement[] elements)
        {
            _navigator = navigator;
            _elements = elements;
        }

        public bool NeedBackButton { get; init; } = true;

        public override void Draw()
        {
            var prompt = new SelectionPrompt<NavigationElement>
            {
                Title = string.Empty,
            };

            prompt.AddChoices(AddBackButtonIfNeeded(_elements));

            NavigationElement choice = AnsiConsole.Prompt(prompt);
            choice.Execute(_navigator);
        }

        private IReadOnlyCollection<NavigationElement> AddBackButtonIfNeeded(IReadOnlyCollection<NavigationElement> elements)
        {
            if (!NeedBackButton)
                return elements;

            var backElement = new NavigationElement("Back", n => n.PopView());
            return new[] { backElement }.Concat(elements).ToArray();
        }
    }
}