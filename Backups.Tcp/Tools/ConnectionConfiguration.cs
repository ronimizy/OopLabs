using Utility.Extensions;

namespace Backups.Tcp.Tools
{
    public class ConnectionConfiguration
    {
        public ConnectionConfiguration(string host, int port)
        {
            Host = host.ThrowIfNull(nameof(host));
            Port = port;
        }

        public string Host { get; }
        public int Port { get; }

        public override string ToString()
            => $"{Host}:{Port}";
    }
}