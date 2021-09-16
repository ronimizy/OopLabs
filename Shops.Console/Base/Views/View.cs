using System.Collections.Generic;
using Shops.Console.Base.ViewControllers;

namespace Shops.Console.Base.Views
{
    public abstract class View
    {
        private readonly List<View> _subviews = new();

        public Controller? Controller { get; set; }

        public void Render()
        {
            RenderBody();

            foreach (View subview in _subviews)
            {
                subview.Render();
            }

            Controller?.OnViewRendered();
        }

        protected virtual void RenderBody() { }

        protected void AddSubview(View subview)
            => _subviews.Add(subview);
    }
}