using Banks.Plans;
using Banks.Tools;
using Utility.Extensions;

namespace Banks.Builders.DebitAccountPlanBuilder
{
    internal class DebitAccountPlanBuilder : IDebitPercentageSelector, IBuilder<DebitAccountPlan>
    {
        private decimal? _percentage;

        public IBuilder<DebitAccountPlan> WithDebitPercentage(decimal percentage)
        {
            _percentage = percentage;
            return this;
        }

        public DebitAccountPlan Build()
            => new DebitAccountPlan(_percentage.ThrowIfNull(nameof(_percentage)));
    }
}