using Banks.Models;
using Banks.Plans;
using Banks.Tools;

namespace Banks.ExceptionFactories
{
    internal static class AccountPlanExceptionFactory
    {
        public static BanksException NegativePercentageException(decimal percentage)
            => new BanksException($"Percent cannot be negative, value: {percentage}");

        public static BanksException ForeignDepositPercentLevelException(DepositAccountPlan plan, DepositPercentLevel level)
            => new BanksException($"DepositAccountPlan {plan}, does not contain a level {level}");
    }
}