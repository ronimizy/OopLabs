using System;
using System.ComponentModel.DataAnnotations;
using Banks.Models;
using Utility.Extensions;

namespace Banks.Entities
{
    public class EmailUser
    {
        internal EmailUser(EmailAddress address, string password)
        {
            address.ThrowIfNull(nameof(address));
            if (address.IsEmpty)
                throw new ArgumentException("Email address cannot be empty", nameof(address));

            Address = address;
            Password = password.ThrowIfNull(nameof(password));
        }

#pragma warning disable 8618
        private EmailUser() { }
#pragma warning restore 8618

        [Key]
        public Guid? Id { get; private init; }

        public EmailAddress Address { get; private init; }
        public string Password { get; private init; }

        public override string ToString()
            => Address.Value;
    }
}