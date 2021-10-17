using System;
using System.Text.Json.Serialization;

namespace Backups.Tcp.Models
{
    [Serializable]
    public class Response
    {
        [JsonConstructor]
        public Response(Status status, object? responseValue)
        {
            Status = status;
            ResponseValue = responseValue;
        }

        public Status Status { get; }
        public object? ResponseValue { get; }
    }
}