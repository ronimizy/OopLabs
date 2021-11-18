using System;
using System.Text;
using Utility.Extensions;

namespace Backups.Tcp.Tools
{
    public static class TcpCommunicationConfiguration
    {
        public static readonly Encoding Encoding = Encoding.UTF8;

        public static int DataChunkSize => 64;

        public static string GetTypeKey(Type type)
        {
            type.ThrowIfNull(nameof(type));

            return type.FullName.ThrowIfNull(nameof(Type.FullName));
        }

        public static byte[] EncodeString(string value)
        {
            value.ThrowIfNull(nameof(value));
            return Encoding.GetBytes(value);
        }
    }
}