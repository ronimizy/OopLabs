using System;
using System.Collections.Generic;
using Shops.Console.Base.Views;

namespace Shops.Console.Base.ViewControllers
{
    public abstract class Controller
    {
        private readonly List<Controller> _children = new();

        public Controller? Parent { get; set; }
        public abstract string Title { get; }
        public View? View { get; set; }
        protected IReadOnlyList<Controller> Children => _children;

        public virtual void OnError(Controller sender, Exception exception)
        {
            Parent?.OnError(sender, exception);
        }

        public virtual void OnViewRendered()
        {
            Parent?.OnViewRendered();
        }

        public virtual void AddChild(Controller child)
        {
            _children.Add(child);
            child.Parent = this;
        }

        public virtual void RemoveChild(Controller child)
        {
            if (_children.Remove(child))
                child.Parent = null;
            else
                throw new InvalidOperationException("Cannot remove non-child controller");
        }
    }
}