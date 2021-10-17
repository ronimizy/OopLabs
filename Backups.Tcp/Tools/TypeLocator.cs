using System;
using System.Collections.Concurrent;
using Utility.Extensions;

namespace Backups.Tcp.Tools
{
    public class TypeLocator
    {
        private readonly ConcurrentDictionary<string, Type> _types = new ConcurrentDictionary<string, Type>();

        public void Add(Type type)
        {
            string fullName = TcpCommunicationConfiguration.GetTypeKey(type);
            _types[fullName] = type;
        }

        public Type Get(string name)
            => _types[name.ThrowIfNull(nameof(name))];
    }
}