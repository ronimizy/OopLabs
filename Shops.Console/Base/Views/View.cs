using System.Collections.Generic;
using Shops.Console.Base.Presenters;

namespace Shops.Console.Base.Views
{
    public abstract class View
    {
        private readonly List<View> _subviews = new();

        public Presenter? Presenter { get; set; }

        public void Render()
        {
            RenderBody();

            foreach (View subview in _subviews)
            {
                subview.Render();
            }

            Presenter?.OnViewRendered();
        }

        protected virtual void RenderBody() { }

        protected void AddSubview(View subview)
            => _subviews.Add(subview);
    }
}