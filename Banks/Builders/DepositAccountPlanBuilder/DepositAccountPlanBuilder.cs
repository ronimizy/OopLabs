using System.Collections.Generic;
using Banks.Models;
using Banks.Plans;
using Banks.Tools;

namespace Banks.Builders.DepositAccountPlanBuilder
{
    internal class DepositAccountPlanBuilder : IDepositPercentageLevelSelector, IEqualityComparer<DepositPercentLevel>
    {
        private readonly HashSet<DepositPercentLevel> _levels;

        public DepositAccountPlanBuilder()
        {
            _levels = new HashSet<DepositPercentLevel>(this);
        }

        public IBuilder<DepositAccountPlan> Builder => this;

        public IDepositPercentageLevelSelector WithLevel(DepositPercentLevel level)
        {
            if (_levels.Contains(level))
                _levels.Remove(level);

            _levels.Add(level);
            return this;
        }

        public DepositAccountPlan Build()
            => new DepositAccountPlan(_levels);

        public bool Equals(DepositPercentLevel? x, DepositPercentLevel? y)
            => x is not null && y is not null && x.Amount.Equals(y.Amount);

        public int GetHashCode(DepositPercentLevel obj)
            => obj.Amount.GetHashCode();
    }
}