using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Banks.Entities;
using Banks.ExceptionFactories;
using Banks.Models;
using Banks.Tools;

namespace Banks.Plans
{
    public abstract class AccountPlan : IEquatable<AccountPlan>
    {
        private readonly List<Client> _subscribers;

        protected AccountPlan(BanksDatabaseContext databaseContext)
        {
            DatabaseContext = databaseContext;
            _subscribers = new List<Client>();
        }

        [Key]
        public Guid? Id { get; private init; }

        [NotMapped]
        public abstract Info Info { get; }

        [NotMapped]
        public IReadOnlyCollection<Client> Subscribers => _subscribers;

        protected BanksDatabaseContext DatabaseContext { get; }

        public void Subscribe(Client client)
        {
            if (_subscribers.Contains(client))
                throw AccountPlanExceptionFactory.AlreadySubscribedException(this, client);

            _subscribers.Add(client);
            DatabaseContext.AccountPlans.Update(this);
            DatabaseContext.SaveChanges();
        }

        public void Unsubscribe(Client client)
        {
            if (!_subscribers.Contains(client))
                throw AccountPlanExceptionFactory.NotSubscribedException(this, client);

            _subscribers.Remove(client);
            DatabaseContext.AccountPlans.Update(this);
            DatabaseContext.SaveChanges();
        }

        public abstract bool Equals(AccountPlan? other);

        public override bool Equals(object? obj)
            => Equals(obj as AccountPlan);

        public override int GetHashCode()
            => Id.GetHashCode();
    }
}