using System;
using System.Collections.Generic;
using System.Linq;
using Shops.Console.Base.Delegates;
using Shops.Console.Base.Views;
using Shops.Console.Models;
using Spectre.Console;

namespace Shops.Console.Base.ViewControllers
{
    public class NavigationViewController : ViewController, ISelectorViewDelegate<SelectorAction>
    {
        private readonly List<ViewController> _viewControllers = new();

        public NavigationViewController(ViewController initialViewController)
        {
            _viewControllers.Add(initialViewController);
            SubscribeToViewController(initialViewController);
        }

        public override string Title => string.Empty;

        public override void DrawContent()
        {
            ViewController viewController = GetCurrentViewController();
            var title = new Text(viewController.Title + "\n") { Alignment = Justify.Left };
            AnsiConsole.Render(title);

            viewController.DrawContent();

            if (viewController is not NavigatedViewController)
                return;

            var selector = new SelectorView<SelectorAction>(this);
            selector.DrawBody();
        }

        public IReadOnlyList<SelectorAction> GetChoices()
        {
            var links = new List<SelectorAction>();
            ViewController current = GetCurrentViewController();

            if (_viewControllers.Count > 1)
            {
                var action = new SelectorAction(
                    $"Back to {GetPreviousViewController().Title}",
                    () => DismissViewController(current));
                links.Add(action);
            }

            if (current is not NavigatedViewController navigatedViewController)
                return links;

            foreach (ViewController link in navigatedViewController.NavigationLinks)
            {
                links.Add(new SelectorAction(link.Title, () => PushViewController(link)));
            }

            return links;
        }

        public void ProcessInput(SelectorAction value)
            => value.Action();

        private void PushViewController(ViewController target)
        {
            ViewController current = GetCurrentViewController();
            UnsubscribeFromViewController(current);

            _viewControllers.Add(target);
            SubscribeToViewController(target);

            OnRedraw(this);
        }

        private void DismissViewController(ViewController target)
        {
            if (!target.Equals(GetCurrentViewController()))
                throw new InvalidOperationException("Cannot dismiss not presented view controller");

            UnsubscribeFromViewController(target);
            _viewControllers.Remove(target);
            SubscribeToViewController(GetCurrentViewController());

            OnRedraw(this);
        }

        private void DisplayError(ViewController target, Exception exception)
        {
            DismissViewController(target);
            PushViewController(new ErrorViewController(exception.Message));
        }

        private void SubscribeToViewController(ViewController target)
        {
            target.Redraw += OnRedraw;
            target.Dismiss += DismissViewController;
            target.Error += DisplayError;
            target.PushController += PushViewController;
        }

        private void UnsubscribeFromViewController(ViewController target)
        {
            target.Redraw -= OnRedraw;
            target.Dismiss -= DismissViewController;
            target.Error -= DisplayError;
            target.PushController -= PushViewController;
        }

        private ViewController GetCurrentViewController()
            => _viewControllers.Last();

        private ViewController GetPreviousViewController()
            => _viewControllers[^2];
    }
}