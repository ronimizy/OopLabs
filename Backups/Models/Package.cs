using System;
using System.IO;
using Backups.JsonConverters;
using Newtonsoft.Json;
using Utility.Extensions;

namespace Backups.Models
{
    [Serializable]
    [JsonConverter(typeof(PackageJsonConverter))]
    public sealed class Package : IDisposable
    {
        [JsonConstructor]
        public Package(string name, Stream stream)
        {
            Name = name.ThrowIfNull(nameof(name));
            Stream = stream.ThrowIfNull(nameof(stream));
        }

        public string Name { get; }
        public Stream Stream { get; }

        public override string ToString()
            => Name;

        public void Dispose()
            => Stream.Dispose();
    }
}