using System.ComponentModel.DataAnnotations.Schema;
using Banks.Builders.DebitAccountPlanBuilder;
using Banks.ExceptionFactories;
using Banks.Models;
using Banks.Tools;

namespace Banks.Plans
{
    public class DebitAccountPlan : AccountPlan
    {
        private decimal _percentage;

        public DebitAccountPlan(decimal percentage, BanksDatabaseContext databaseContext)
            : base(databaseContext)
        {
            if (percentage < 0)
                throw AccountPlanExceptionFactory.NegativePercentageException(percentage);

            _percentage = percentage;
        }

#pragma warning disable 8618
        private DebitAccountPlan(BanksDatabaseContext databaseContext)
#pragma warning restore 8618
            : base(databaseContext) { }

        public decimal Percentage
        {
            get => _percentage;
            set
            {
                if (value < 0)
                    throw AccountPlanExceptionFactory.NegativePercentageException(value);

                _percentage = value;
                DatabaseContext.AccountPlans.Update(this);
                DatabaseContext.SaveChanges();
            }
        }

        [NotMapped]
        public override Info Info => new Info("Debit Bank Account", $"Account that has a {Percentage}% yearly, that accrue daily");

        public override bool Equals(AccountPlan? other)
            => other is DebitAccountPlan && other.Id.Equals(Id);
    }
}