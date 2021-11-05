using System;

namespace Spectre.Mvvm.Models
{
    public record SelectorAction(string Title, Action Action)
    {
        public override string ToString()
            => Title;
    }
}