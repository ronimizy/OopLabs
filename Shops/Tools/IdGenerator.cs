using System.Threading;

namespace Shops.Tools
{
    internal class IdGenerator
    {
        private int _id;

        public int Next()
            => Interlocked.Increment(ref _id);
    }
}