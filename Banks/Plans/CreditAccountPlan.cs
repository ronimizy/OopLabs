using System.ComponentModel.DataAnnotations.Schema;
using Banks.Builders.CreditAccountPlanBuilder;
using Banks.ExceptionFactories;
using Banks.Models;

namespace Banks.Plans
{
    public sealed class CreditAccountPlan : AccountPlan
    {
        private decimal _percentage;

        public CreditAccountPlan(decimal limit, decimal percentage)
        {
            if (percentage < 0)
                throw AccountPlanExceptionFactory.NegativePercentageException(percentage);

            Limit = limit;
            _percentage = percentage;
        }

#pragma warning disable 8618
        private CreditAccountPlan() { }
#pragma warning restore 8618

        public static ICreditLimitSelector BuildPlan => new CreditAccountPlanBuilder();

        public decimal Limit { get; internal set; }

        public decimal Percentage
        {
            get => _percentage;
            set
            {
                if (value < 0)
                    throw AccountPlanExceptionFactory.NegativePercentageException(value);

                _percentage = value;
            }
        }

        [NotMapped]
        public override Info Info => new Info(
            "Credit Bank Account",
            $"Credit commission is calculated based of dept, it is a {Percentage}% of dept per every account operation");

        public decimal CalculateAccrualCommission(decimal balance, decimal accrualAmount)
            => CalculateByDept(balance);

        public decimal CalculateWithdrawalCommission(decimal balance, decimal withdrawalAmount)
            => CalculateByDept(balance);

        public override bool Equals(AccountPlan? other)
            => other is CreditAccountPlan && other.Id.Equals(Id);

        private decimal CalculateByDept(decimal balance)
            => balance < 0 ? -balance * Percentage : 0;
    }
}