using Shops.Console.Base.Delegates;
using Shops.Console.Base.Models;
using Shops.Console.Base.Presenters;
using Spectre.Console;

namespace Shops.Console.Base.Views
{
    public class NavigationView : View
    {
        public NavigationView(Presenter presenter, ISelectorViewDelegate<SelectorAction> selectorViewDelegate)
        {
            var title = new Markup($"[bold]{presenter.Title}[/]\n") { Alignment = Justify.Left };
            AddSubview(new MarkupView(title));

            if (presenter.View != null)
                AddSubview(presenter.View);

            if (presenter is not NavigatedPresenter)
                return;

            AddSubview(new MarkupView(new Markup("\n")));
            AddSubview(new SelectorView<SelectorAction>(selectorViewDelegate));
        }
    }
}