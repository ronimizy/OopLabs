using System;
using System.ComponentModel.DataAnnotations;

namespace Banks.Models
{
    public class DepositPercentLevel
    {
        public DepositPercentLevel(decimal amount, decimal percent)
        {
            Amount = amount;
            Percent = percent;
        }

        private DepositPercentLevel() { }

        [Key]
        public Guid? Id { get; private init; }

        public decimal Amount { get; private init; }
        public decimal Percent { get; private init; }

        public override string ToString()
            => $"{Amount}$ -> {Percent}%";
    }
}