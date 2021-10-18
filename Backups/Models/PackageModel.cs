namespace Backups.Models
{
    public class PackageModel
    {
        public PackageModel(string name, byte[] data)
        {
            Name = name;
            Data = data;
        }

        public string Name { get; }
        public byte[] Data { get; }
    }
}