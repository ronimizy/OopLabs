using Banks.Plans;
using Banks.Tools;
using Utility.Extensions;

namespace Banks.Builders.CreditAccountPlanBuilder
{
    internal class CreditAccountPlanBuilder : ICreditLimitSelector, ICreditPercentageSelector, IBuilder<CreditAccountPlan>
    {
        private decimal? _limit;
        private decimal? _percentage;

        public ICreditPercentageSelector LimitedTo(decimal limit)
        {
            _limit = limit;
            return this;
        }

        public IBuilder<CreditAccountPlan> WithCommissionPercent(decimal percentage)
        {
            _percentage = percentage;
            return this;
        }

        public CreditAccountPlan Build()
        {
            return new CreditAccountPlan(
                _limit.ThrowIfNull(nameof(_limit)),
                _percentage.ThrowIfNull(nameof(_percentage)));
        }
    }
}