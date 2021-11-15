using System;
using System.Linq;
using Backups.RestorePointMatchers;
using BackupsExtra.Matchers;
using BackupsExtra.Models;
using Newtonsoft.Json;
using Utility.Extensions;

namespace BackupsExtra.JsonConverters
{
    public class AnyOfRestorePointMatcherJsonConverter : JsonConverter<AnyOfRestorePointMatcher>
    {
        public override void WriteJson(JsonWriter writer, AnyOfRestorePointMatcher? value, JsonSerializer serializer)
        {
            if (value is null)
                return;

            var model = new HybridRestorePointMatcherModel(value);
            serializer.Serialize(writer, model);
        }

        public override AnyOfRestorePointMatcher ReadJson(
            JsonReader reader, Type objectType, AnyOfRestorePointMatcher? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var model = serializer
                .Deserialize<HybridRestorePointMatcherModel>(reader)
                .ThrowIfNull(nameof(HybridRestorePointMatcherModel));

            IRestorePointMatcher[] matchers = model.Matchers
                .Select(m => m.Deserialize())
                .ToArray();

            return new AnyOfRestorePointMatcher(matchers);
        }
    }
}