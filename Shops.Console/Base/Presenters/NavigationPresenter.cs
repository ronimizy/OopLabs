using System;
using System.Collections.Generic;
using Shops.Console.Base.Delegates;
using Shops.Console.Base.Models;
using Shops.Console.Base.Views;

namespace Shops.Console.Base.Presenters
{
    public sealed class NavigationPresenter : Presenter, ISelectorViewDelegate<SelectorAction>
    {
        public NavigationPresenter(Presenter initialPresenter)
        {
            AddChild(initialPresenter);
        }

        public override string Title => string.Empty;

        public IReadOnlyList<SelectorAction> GetChoices()
        {
            Presenter current = GetCurrentPresenter();
            var links = new List<SelectorAction>();

            if (current is not NavigatedPresenter navigatedPresenter)
                return links;

            if (Children.Count > 1)
            {
                var action = new SelectorAction($"Back to {GetPreviousPresenter().Title}", () => RemoveChild(GetCurrentPresenter()));
                links.Add(action);
            }

            foreach (Presenter link in navigatedPresenter.NavigationLinks)
            {
                links.Add(new SelectorAction(link.Title, () => AddChild(link)));
            }

            return links;
        }

        public void ProcessInput(SelectorAction value)
            => value.Action();

        public override void OnError(Presenter sender, Exception exception)
        {
            RemoveChild(sender);
            AddChild(new ErrorPresenter(exception.Message));
        }

        public override void AddChild(Presenter child)
        {
            base.AddChild(child);
            UpdateView();
        }

        public override void RemoveChild(Presenter child)
        {
            base.RemoveChild(child);
            UpdateView();
        }

        private void UpdateView()
        {
            View = new NavigationView(GetCurrentPresenter(), this);
        }

        private Presenter GetCurrentPresenter()
            => Children[^1];

        private Presenter GetPreviousPresenter()
            => Children[^2];
    }
}