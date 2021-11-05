using System;
using System.ComponentModel.DataAnnotations;

namespace Banks.Models
{
    public class SuspiciousLimitPolicy
    {
        public SuspiciousLimitPolicy(decimal limit)
        {
            Limit = limit;
        }

        private SuspiciousLimitPolicy() { }

        [Key]
        public Guid? Id { get; private init; }

        public decimal Limit { get; private init; }
    }
}