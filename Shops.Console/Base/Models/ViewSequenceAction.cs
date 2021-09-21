using System;
using System.Collections.Generic;
using Shops.Console.Base.Views;

namespace Shops.Console.Base.Models
{
    public class ViewSequenceAction
    {
        private readonly Action<List<View>> _action;

        private ViewSequenceAction(Action<List<View>> action)
        {
            _action = action;
        }

        public static ViewSequenceAction Add(View view)
            => new ViewSequenceAction(s => s.Add(view));

        public static ViewSequenceAction Pop()
            => new ViewSequenceAction(s => s.Remove(s[^1]));

        public void Execute(List<View> views)
            => _action(views);
    }
}