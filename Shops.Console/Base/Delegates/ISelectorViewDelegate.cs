using System.Collections.Generic;

namespace Shops.Console.Base.Delegates
{
    public interface ISelectorViewDelegate<T>
    {
        IReadOnlyList<T> GetChoices();
        void ProcessInput(T value);
    }
}