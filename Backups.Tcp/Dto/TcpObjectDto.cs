using System;
using Newtonsoft.Json;
using Utility.Extensions;

namespace Backups.Tcp.Dto
{
    [Serializable]
    public class TcpObjectDto
    {
        [JsonConstructor]
        public TcpObjectDto(string type, string data)
        {
            Type = type.ThrowIfNull(nameof(type));
            Data = data.ThrowIfNull(nameof(data));
        }

        public string Type { get; }
        public string Data { get; }
    }
}