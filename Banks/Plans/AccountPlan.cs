using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Banks.Entities;
using Banks.Models;

namespace Banks.Plans
{
    public abstract class AccountPlan : IEquatable<AccountPlan>
    {
        private readonly List<Client> _subscribers;

        protected AccountPlan()
        {
            _subscribers = new List<Client>();
        }

        [Key]
        public Guid? Id { get; private init; }

        [NotMapped]
        public abstract Info Info { get; }

        [NotMapped]
        public IReadOnlyCollection<Client> Subscribers => _subscribers;

        public abstract override string ToString();

        public abstract bool Equals(AccountPlan? other);

        public override bool Equals(object? obj)
            => Equals(obj as AccountPlan);

        public override int GetHashCode()
            => Id.GetHashCode();

        internal void Subscribe(Client client)
        {
            _subscribers.Add(client);
        }

        internal void Unsubscribe(Client client)
        {
            _subscribers.Remove(client);
        }
    }
}