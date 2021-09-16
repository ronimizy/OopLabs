using System;
using System.Collections.Generic;
using Shops.Console.Base.Views;

namespace Shops.Console.Base.Presenters
{
    public abstract class Presenter
    {
        private readonly List<Presenter> _children = new();

        public Presenter? Parent { get; set; }
        public abstract string Title { get; }
        public View? View { get; set; }
        protected IReadOnlyList<Presenter> Children => _children;

        public virtual void OnError(Presenter sender, Exception exception)
        {
            Parent?.OnError(sender, exception);
        }

        public virtual void OnViewRendered()
        {
            Parent?.OnViewRendered();
        }

        public virtual void AddChild(Presenter child)
        {
            _children.Add(child);
            child.Parent = this;
        }

        public virtual void RemoveChild(Presenter child)
        {
            if (_children.Remove(child))
                child.Parent = null;
            else
                throw new InvalidOperationException("Cannot remove non-child presenter");
        }
    }
}