using System.Collections;
using System.Collections.Generic;

namespace Utility.Extensions
{
    public static class CollectionExtension
    {
        public static bool IsEmpty(this ICollection collection)
            => collection.Count == 0;

        public static bool IsEmpty<T>(this IReadOnlyCollection<T> collection)
            => collection.Count == 0;
    }
}