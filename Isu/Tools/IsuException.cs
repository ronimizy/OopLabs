using System;
using System.Diagnostics.CodeAnalysis;

namespace Isu.Tools
{
    public class IsuException : Exception
    {
        [ExcludeFromCodeCoverage]
        public IsuException() { }

        public IsuException(string message)
            : base(message) { }

        [ExcludeFromCodeCoverage]
        public IsuException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}