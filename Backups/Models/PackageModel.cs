using Utility.Extensions;

namespace Backups.Models
{
    public class PackageModel
    {
        public PackageModel(string name, byte[] data)
        {
            Name = name.ThrowIfNull(nameof(name));
            Data = data.ThrowIfNull(nameof(data));
        }

        public string Name { get; }
        public byte[] Data { get; }
    }
}