using System;

namespace Utility.Extensions
{
    public static class GenericExtension
    {
        public static TValue ThrowIfNull<TValue, TException>(this TValue? value, TException exception)
            where TException : Exception
        {
            if (value is null)
                throw exception;

            return value;
        }

        public static TValue ThrowIfNull<TValue>(this TValue? value, string argumentName)
            => value.ThrowIfNull(new ArgumentNullException(argumentName));

        public static TValue ThrowIfNull<TValue, TException>(this TValue? value)
            where TException : Exception, new()
            => value.ThrowIfNull(new TException());
    }
}