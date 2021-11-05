using Banks.Plans;
using Banks.Tools;

namespace Banks.Builders.DebitAccountPlanBuilder
{
    public interface IDebitPercentageSelector
    {
        IBuilder<DebitAccountPlan> WithDebitPercentage(decimal percentage);
    }
}