using System;
using System.Collections.Generic;
using Shops.Console.Base.Views;

namespace Shops.Console.Base.ViewControllers
{
    public abstract class ViewController
    {
        private readonly List<View> _views = new List<View>();

        public delegate void ViewControllerEventHandler(ViewController target);

        public delegate void ViewControllerErrorHandler(ViewController target, Exception exception);

        public event ViewControllerEventHandler? Redraw;
        public event ViewControllerEventHandler? Dismiss;
        public event ViewControllerEventHandler? PushController;
        public event ViewControllerErrorHandler? Error;

        public abstract string Title { get; }

        public virtual void DrawContent()
        {
            foreach (View subview in _views)
            {
                subview.DrawBody();
            }
        }

        protected void AddView(View view)
        {
            _views.Add(view);
        }

        protected virtual void OnRedraw(ViewController target)
        {
            Redraw?.Invoke(target);
        }

        protected virtual void OnDismiss(ViewController target)
        {
            Dismiss?.Invoke(target);
        }

        protected virtual void OnPushController(ViewController target)
        {
            PushController?.Invoke(target);
        }

        protected virtual void OnError(ViewController target, Exception exception)
        {
            Error?.Invoke(target, exception);
        }
    }
}