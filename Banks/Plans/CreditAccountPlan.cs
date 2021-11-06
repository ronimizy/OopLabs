using System.ComponentModel.DataAnnotations.Schema;
using Banks.Builders.CreditAccountPlanBuilder;
using Banks.ExceptionFactories;
using Banks.Models;
using Banks.Tools;

namespace Banks.Plans
{
    public sealed class CreditAccountPlan : AccountPlan
    {
        private decimal _percentage;
        private decimal _limit;

        public CreditAccountPlan(decimal limit, decimal percentage, BanksDatabaseContext databaseContext)
            : base(databaseContext)
        {
            if (percentage < 0)
                throw AccountPlanExceptionFactory.NegativePercentageException(percentage);

            _limit = limit;
            _percentage = percentage;
        }

#pragma warning disable 8618
        private CreditAccountPlan(BanksDatabaseContext databaseContext)
#pragma warning restore 8618
            : base(databaseContext) { }

        public decimal Limit
        {
            get => _limit;
            internal set
            {
                _limit = value;
                DatabaseContext.AccountPlans.Update(this);
                DatabaseContext.SaveChanges();
            }
        }

        public decimal Percentage
        {
            get => _percentage;
            internal set
            {
                if (value < 0)
                    throw AccountPlanExceptionFactory.NegativePercentageException(value);

                _percentage = value;
                DatabaseContext.AccountPlans.Update(this);
                DatabaseContext.SaveChanges();
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