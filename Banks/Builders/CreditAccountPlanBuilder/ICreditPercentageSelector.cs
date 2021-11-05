using Banks.Plans;
using Banks.Tools;

namespace Banks.Builders.CreditAccountPlanBuilder
{
    public interface ICreditPercentageSelector
    {
        IBuilder<CreditAccountPlan> WithCommissionPercent(decimal percentage);
    }
}