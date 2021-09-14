using System;
using Utility.Extensions;

namespace Shops.Entities
{
    public class Product : IEquatable<Product>
    {
        public Product(string name, string description)
        {
            Id = Guid.NewGuid();
            Name = name.ThrowIfNull(nameof(name));
            Description = description.ThrowIfNull(nameof(description));
        }

        public Guid Id { get; }
        public string Name { get; }
        public string Description { get; }

        public bool Equals(Product? other)
            => other is not null && Id == other.Id;

        public override bool Equals(object? obj)
            => obj is Product other && Equals(other);

        public override int GetHashCode()
            => Id.GetHashCode();

        public override string ToString()
            => $"[{Id}] {Name}";
    }
}