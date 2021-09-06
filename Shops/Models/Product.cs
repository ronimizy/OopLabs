using System;
using Utility.Extensions;

namespace Shops.Models
{
    public class Product : IEquatable<Product>
    {
        internal Product(int id, string name, string description, int serviceId)
        {
            Id = id;
            Name = name.ThrowIfNull(nameof(name));
            Description = description.ThrowIfNull(nameof(description));
            ServiceId = serviceId;
        }

        public int Id { get; }
        public string Name { get; }
        public string Description { get; }
        public int ServiceId { get; }

        public bool Equals(Product? other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return Id == other.Id;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;

            return Equals((Product)obj);
        }

        public override int GetHashCode()
            => Id;

        public override string ToString()
            => $"[{Id}] {Name}";
    }
}