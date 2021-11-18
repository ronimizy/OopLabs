using System;
using Backups.Tools;
using Newtonsoft.Json;
using Utility.Extensions;

namespace Backups.Models
{
    public class SerializedConfiguration<T>
    {
        public SerializedConfiguration(T obj)
        {
            obj.ThrowIfNull(nameof(obj));

            Type type = obj!.GetType();
            TypeKey = BackupConfiguration.GetTypeKey(type);
            Data = JsonConvert.SerializeObject(obj, type, null);
        }

        [JsonConstructor]
        private SerializedConfiguration(string typeKey, string data)
        {
            TypeKey = typeKey;
            Data = data;
        }

        public string TypeKey { get; init; }
        public string Data { get; init; }

        public T Deserialize()
            => (T)JsonConvert
                .DeserializeObject(Data, TypeLocator.Instance.Get(TypeKey))
                .ThrowIfNull(nameof(T));
    }
}