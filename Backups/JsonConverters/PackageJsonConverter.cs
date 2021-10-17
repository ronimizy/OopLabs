using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Backups.Models;
using Utility.Extensions;

namespace Backups.JsonConverters
{
    public class PackageJsonConverter : JsonConverter<Package>
    {
        public override Package Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            PackageModel deserialized = JsonSerializer
                .Deserialize<PackageModel>(ref reader, options)
                .ThrowIfNull(nameof(PackageModel));

            var ms = new MemoryStream(deserialized.Data);

            return new Package(deserialized.Name, ms);
        }

        public override void Write(Utf8JsonWriter writer, Package value, JsonSerializerOptions options)
        {
            var ms = new MemoryStream();
            value.Stream.Position = 0;
            value.Stream.CopyTo(ms);

            var model = new PackageModel(value.Name, ms.ToArray());
            JsonSerializer.Serialize(writer, model, options);
        }
    }
}