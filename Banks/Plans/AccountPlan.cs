using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Banks.Models;

namespace Banks.Plans
{
    public abstract class AccountPlan : IEquatable<AccountPlan>
    {
        [Key]
        public Guid? Id { get; private init; }

        [NotMapped]
        public abstract Info Info { get; }

        public abstract bool Equals(AccountPlan? other);

        public override bool Equals(object? obj)
            => Equals(obj as AccountPlan);

        public override int GetHashCode()
            => Id.GetHashCode();
    }
}