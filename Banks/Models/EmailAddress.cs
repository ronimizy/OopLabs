using System;
using Microsoft.EntityFrameworkCore;

namespace Banks.Models
{
    [Keyless]
    public class EmailAddress : IEquatable<EmailAddress>
    {
        public EmailAddress(string value = "")
        {
            Value = value;
        }

        public string Value { get; set; }

        public bool IsEmpty => string.IsNullOrEmpty(Value);

        public bool Equals(EmailAddress? other)
            => other is not null && other.Value.Equals(Value);

        public override string ToString()
            => Value;
    }
}