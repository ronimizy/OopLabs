using Banks.Plans;
using Banks.Tools;
using Utility.Extensions;

namespace Banks.Builders.DebitAccountPlanBuilder
{
    internal class DebitAccountPlanBuilder : IDebitPercentageSelector, IBuilder<DebitAccountPlan>
    {
        private readonly BanksDatabaseContext _databaseContext;
        private decimal? _percentage;

        public DebitAccountPlanBuilder(BanksDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public IBuilder<DebitAccountPlan> WithDebitPercentage(decimal percentage)
        {
            _percentage = percentage;
            return this;
        }

        public DebitAccountPlan Build()
            => new DebitAccountPlan(_percentage.ThrowIfNull(nameof(_percentage)), _databaseContext);
    }
}