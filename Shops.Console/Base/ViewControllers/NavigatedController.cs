using System;
using System.Collections.Generic;
using Shops.Console.Base.Views;

namespace Shops.Console.Base.ViewControllers
{
    public abstract class NavigatedController : Controller
    {
        public virtual IReadOnlyList<Controller> NavigationLinks => Array.Empty<Controller>();
    }
}