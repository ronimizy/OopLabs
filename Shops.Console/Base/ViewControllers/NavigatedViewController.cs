using System;
using System.Collections.Generic;

namespace Shops.Console.Base.ViewControllers
{
    public abstract class NavigatedViewController : ViewController
    {
        public virtual IReadOnlyList<ViewController> NavigationLinks => Array.Empty<ViewController>();
    }
}