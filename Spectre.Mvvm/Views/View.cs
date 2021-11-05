using System;
using System.Collections.Generic;
using Spectre.Mvvm.Components;

namespace Spectre.Mvvm.Views
{
    public abstract class View
    {
        public abstract string Title { get; }

        public virtual void Draw()
        {
            foreach (Component component in GetComponents())
            {
                component.Draw();
            }
        }

        protected virtual IReadOnlyCollection<Component> GetComponents()
            => Array.Empty<Component>();
    }
}