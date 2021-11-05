using System.ComponentModel.DataAnnotations.Schema;
using Banks.Builders.DebitAccountPlanBuilder;
using Banks.ExceptionFactories;
using Banks.Models;

namespace Banks.Plans
{
    public class DebitAccountPlan : AccountPlan
    {
        private decimal _percentage;

        public DebitAccountPlan(decimal percentage)
        {
            if (percentage < 0)
                throw AccountPlanExceptionFactory.NegativePercentageException(percentage);

            _percentage = percentage;
        }

#pragma warning disable 8618
        private DebitAccountPlan() { }
#pragma warning restore 8618

        public static IDebitPercentageSelector BuildPlan => new DebitAccountPlanBuilder();

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
        public override Info Info => new Info("Debit Bank Account", $"Account that has a {Percentage}% yearly, that accrue daily");

        public override bool Equals(AccountPlan? other)
            => other is DebitAccountPlan && other.Id.Equals(Id);
    }
}