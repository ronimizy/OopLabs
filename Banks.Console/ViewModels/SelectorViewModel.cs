using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Mvvm.Interfaces;
using Spectre.Mvvm.Models;

namespace Banks.Console.ViewModels
{
    public class SelectorViewModel<T>
    {
        private readonly Func<T, NavigationElement> _elementFactory;
        private readonly IReadOnlyCollection<T> _collection;

        public SelectorViewModel(
            INavigator navigator,
            IReadOnlyCollection<T> collection,
            string title,
            Func<T, NavigationElement> elementFactory)
        {
            Navigator = navigator;
            _elementFactory = elementFactory;
            _collection = collection;
            Title = title;
        }

        public string Title { get; }
        public INavigator Navigator { get; }

        public NavigationElement[] Elements => _collection
            .Select(p => _elementFactory.Invoke(p))
            .ToArray();
    }
}