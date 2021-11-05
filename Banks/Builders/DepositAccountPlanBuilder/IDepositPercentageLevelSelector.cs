using Banks.Models;
using Banks.Plans;
using Banks.Tools;

namespace Banks.Builders.DepositAccountPlanBuilder
{
    public interface IDepositPercentageLevelSelector : IBuilder<DepositAccountPlan>
    {
        IBuilder<DepositAccountPlan> Builder { get; }
        IDepositPercentageLevelSelector WithLevel(DepositPercentLevel level);
    }
}