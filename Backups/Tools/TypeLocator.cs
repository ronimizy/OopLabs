using System;
using System.Collections.Concurrent;
using Utility.Extensions;

namespace Backups.Tools
{
    public class TypeLocator
    {
        private static readonly ConcurrentDictionary<string, Type> Types = new ConcurrentDictionary<string, Type>();
        private static readonly object Lock = new object();
        private static TypeLocator? _instance;

        private TypeLocator() { }

        public static TypeLocator Instance
        {
            get
            {
                if (_instance is not null)
                    return _instance;

                lock (Lock)
                {
                    if (_instance is not null)
                        return _instance;

                    _instance = new TypeLocator();
                    return _instance;
                }
            }
        }

        public TypeLocator Add(Type type)
        {
            type.ThrowIfNull(nameof(type));

            string fullName = BackupConfiguration.GetTypeKey(type);
            Types[fullName] = type;
            return this;
        }

        public Type Get(string name)
            => Types[name.ThrowIfNull(nameof(name))];
    }
}