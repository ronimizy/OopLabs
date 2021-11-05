using System;
using Spectre.Mvvm.Components;
using Spectre.Mvvm.Models;

namespace Shops.Console.Components
{
    public class ConfirmationComponent : SelectorComponent<SelectorAction>
    {
        public ConfirmationComponent(Action confirmationAction, Action rejectionAction)
            : base("Do you confirm this action?", new[]
            {
                new SelectorAction("Yes", confirmationAction),
                new SelectorAction("No", rejectionAction),
            }) { }
    }
}