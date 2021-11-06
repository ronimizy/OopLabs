using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Banks.ExceptionFactories;
using Banks.Models;
using Banks.Tools;
using Utility.Extensions;

namespace Banks.Plans
{
    public class DepositAccountPlan : AccountPlan
    {
        private readonly List<DepositPercentLevel> _levels;

        public DepositAccountPlan(IReadOnlyCollection<DepositPercentLevel> levels, BanksDatabaseContext databaseContext)
            : base(databaseContext)
        {
            _levels = levels.ThrowIfNull(nameof(levels)).ToList();
        }

#pragma warning disable 8618
        private DepositAccountPlan(BanksDatabaseContext databaseContext)
#pragma warning restore 8618
            : base(databaseContext) { }

        [NotMapped]
        public IReadOnlyCollection<DepositPercentLevel> Levels => _levels;

        [NotMapped]
        public override Info Info => new Info("Deposit Bank Account", ToString());

        public override bool Equals(AccountPlan? other)
            => other is DepositAccountPlan && other.Id.Equals(Id);

        public override string ToString()
            => string.Join('\n', Levels.OrderBy(l => l.Amount).Select(l => l.ToString()));

        internal void AddOrUpdateLevel(DepositPercentLevel level)
        {
            level.ThrowIfNull(nameof(level));

            DepositPercentLevel? exisingLevel = _levels.SingleOrDefault(l => level.Amount.Equals(l.Amount));

            if (exisingLevel is not null)
                _levels.Remove(exisingLevel);

            _levels.Add(level);
            DatabaseContext.AccountPlans.Update(this);
            DatabaseContext.SaveChanges();
        }

        internal void RemoveLevel(DepositPercentLevel level)
        {
            level.ThrowIfNull(nameof(level));

            DepositPercentLevel? exisingLevel = _levels.SingleOrDefault(l => level.Id.Equals(l.Id));

            exisingLevel.ThrowIfNull(AccountPlanExceptionFactory.ForeignDepositPercentLevelException(this, level));
            _levels.Remove(exisingLevel!);
            DatabaseContext.AccountPlans.Update(this);
            DatabaseContext.SaveChanges();
        }

        internal decimal RetrievePercent(decimal amount)
        {
            return _levels
                .OrderByDescending(l => l.Amount)
                .SkipWhile(l => l.Amount > amount)
                .First()
                .Percent;
        }
    }
}