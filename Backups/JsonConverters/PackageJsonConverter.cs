using System;
using System.IO;
using Backups.Models;
using Newtonsoft.Json;
using Utility.Extensions;

namespace Backups.JsonConverters
{
    public class PackageJsonConverter : JsonConverter<Package>
    {
        public override void WriteJson(JsonWriter writer, Package? value, JsonSerializer serializer)
        {
            if (value is null)
                return;

            var ms = new MemoryStream();
            value.Stream.Position = 0;
            value.Stream.CopyTo(ms);

            var model = new PackageModel(value.Name, ms.ToArray());
            serializer.Serialize(writer, model);
        }

        public override Package ReadJson(JsonReader reader, Type objectType, Package? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            PackageModel deserialized = serializer
                .Deserialize<PackageModel>(reader)
                .ThrowIfNull(nameof(PackageModel));

            var ms = new MemoryStream(deserialized.Data);
            return new Package(deserialized.Name, ms);
        }
    }
}