using System;
using System.Collections.Generic;
using Shops.Console.Base.Components;
using Shops.Console.Base.Interfaces;
using Spectre.Console;

namespace Shops.Console.Base.Views
{
    public class ErrorView : View
    {
        private readonly Exception _error;
        private readonly INavigator _navigator;

        public ErrorView(Exception error, INavigator navigator)
        {
            _error = error;
            _navigator = navigator;
        }

        public override string Title => "Error";

        protected override IReadOnlyCollection<Component> GetComponents()
            => new Component[]
            {
                new MarkupComponent(new Markup($"[bold]{_error.Message.EscapeMarkup()}[/]\n")),
                new ButtonComponent("Ok", _navigator.PopView),
            };
    }
}