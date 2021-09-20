using System;
using System.Collections.Generic;
using Shops.Console.Base.Interfaces;
using Shops.Console.Base.Models;
using Shops.Console.Base.Views;

namespace Shops.Console.Base.ViewModels
{
    public class NavigationViewModel : INavigator
    {
        private readonly List<View> _views = new();
        private readonly List<ViewSequenceAction> _sequenceActions = new();

        public NavigationViewModel(Func<INavigator, View> initialViewFactory)
        {
            PushView(initialViewFactory(this));
        }

        public string Title => CurrentView.Title;
        public View CurrentView => _views[^1];

        public void OnError(Exception error)
        {
            RemoveView(CurrentView);
            PushView(new ErrorView(error, this));
        }

        public void PushView(View view)
            => _sequenceActions.Add(ViewSequenceAction.Add(view));

        public void PopView()
            => _sequenceActions.Add(ViewSequenceAction.Pop());

        public void ApplyViewSequenceActions()
        {
            foreach (var action in _sequenceActions)
            {
                action.Execute(_views);
            }

            _sequenceActions.Clear();
        }

        private void RemoveView(View view)
        {
            if (!_views.Remove(view))
                throw new InvalidOperationException("Cannot remove not contained view");
        }
    }
}