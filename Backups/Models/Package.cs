using System;
using System.IO;
using System.Text.Json.Serialization;
using Backups.JsonConverters;
using Utility.Extensions;

namespace Backups.Models
{
    [Serializable]
    [JsonConverter(typeof(PackageJsonConverter))]
    public class Package
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
    }
}