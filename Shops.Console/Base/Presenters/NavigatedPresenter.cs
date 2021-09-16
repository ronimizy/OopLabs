using System;
using System.Collections.Generic;

namespace Shops.Console.Base.Presenters
{
    public abstract class NavigatedPresenter : Presenter
    {
        public virtual IReadOnlyList<Presenter> NavigationLinks => Array.Empty<Presenter>();
    }
}