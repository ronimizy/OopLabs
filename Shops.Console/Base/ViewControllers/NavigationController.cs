using System;
using System.Collections.Generic;
using Shops.Console.Base.Delegates;
using Shops.Console.Base.Models;
using Shops.Console.Base.Views;

namespace Shops.Console.Base.ViewControllers
{
    public sealed class NavigationController : Controller, ISelectorViewDelegate<SelectorAction>
    {
        public NavigationController(Controller initialController)
        {
            AddChild(initialController);
        }

        public override string Title => string.Empty;

        public IReadOnlyList<SelectorAction> GetChoices()
        {
            Controller current = GetCurrentController();
            var links = new List<SelectorAction>();

            if (current is not NavigatedController navigatedController)
                return links;

            if (Children.Count > 1)
            {
                var action = new SelectorAction($"Back to {GetPreviousController().Title}", () => RemoveChild(GetCurrentController()));
                links.Add(action);
            }

            foreach (Controller link in navigatedController.NavigationLinks)
            {
                links.Add(new SelectorAction(link.Title, () => AddChild(link)));
            }

            return links;
        }

        public void ProcessInput(SelectorAction value)
            => value.Action();

        public override void OnError(Controller sender, Exception exception)
        {
            RemoveChild(sender);
            AddChild(new ErrorController(exception.Message));
        }

        public override void AddChild(Controller child)
        {
            base.AddChild(child);
            UpdateView();
        }

        public override void RemoveChild(Controller child)
        {
            base.RemoveChild(child);
            UpdateView();
        }

        private void UpdateView()
        {
            View = new NavigationView(GetCurrentController(), this);
        }

        private Controller GetCurrentController()
            => Children[^1];

        private Controller GetPreviousController()
            => Children[^2];
    }
}