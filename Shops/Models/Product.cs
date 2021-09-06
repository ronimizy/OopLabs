using System;
using Shops.Tools;
using Utility.Extensions;

namespace Shops.Models
{
    public class Product : IEquatable<Product>
    {
        private static readonly IdGenerator IdGenerator = new ();

        internal Product(string name, string description)
        {
            Id = IdGenerator.Next();
            Name = name.ThrowIfNull(nameof(name));
            Description = description.ThrowIfNull(nameof(description));
        }

        public int Id { get; }
        public string Name { get; }
        public string Description { get; }

        public bool Equals(Product? other)
            => other is not null && Id == other.Id;

        public override bool Equals(object? obj)
            => obj is Product other && Equals(other);

        public override int GetHashCode()
            => Id;

        public override string ToString()
            => $"[{Id}] {Name}";
    }
}