using System;
using Spectre.Mvvm.Interfaces;

namespace Spectre.Mvvm.Models
{
    public class NavigationElement
    {
        private readonly Action<INavigator> _navigationAction;

        public NavigationElement(string title, Action<INavigator> navigationAction)
        {
            Title = title;
            _navigationAction = navigationAction;
        }

        public string Title { get; }

        public override string ToString()
            => Title;

        internal void Execute(INavigator navigator)
            => _navigationAction.Invoke(navigator);
    }
}