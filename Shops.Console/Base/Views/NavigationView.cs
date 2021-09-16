using Shops.Console.Base.Delegates;
using Shops.Console.Base.Models;
using Shops.Console.Base.ViewControllers;
using Spectre.Console;

namespace Shops.Console.Base.Views
{
    public class NavigationView : View
    {
        public NavigationView(Controller controller, ISelectorViewDelegate<SelectorAction> selectorViewDelegate)
        {
            var title = new Markup($"[bold]{controller.Title}[/]\n") { Alignment = Justify.Left };
            AddSubview(new MarkupView(title));

            if (controller.View != null)
                AddSubview(controller.View);

            if (controller is not NavigatedController)
                return;

            AddSubview(new MarkupView(new Markup("\n")));
            AddSubview(new SelectorView<SelectorAction>(selectorViewDelegate));
        }
    }
}