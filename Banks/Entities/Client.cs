using System;
using System.ComponentModel.DataAnnotations;
using Banks.Builders.ClientBuilder;
using Banks.Models;
using Utility.Extensions;

namespace Banks.Entities
{
    public class Client : IEquatable<Client>
    {
        public Client(
            string name,
            string surname,
            string password,
            EmailAddress emailAddress,
            string? address = null,
            PassportData? passportData = null)
        {
            Name = name.ThrowIfNull(nameof(name));
            Surname = surname.ThrowIfNull(nameof(surname));
            Password = password.ThrowIfNull(nameof(password));
            EmailAddress = emailAddress.ThrowIfNull(nameof(emailAddress));
            Address = address;
            PassportData = passportData;
        }

#pragma warning disable 8618
        private Client() { }
#pragma warning restore 8618

        public static IClientFullNameSelector BuildClient => new ClientBuilder();

        [Key]
        public Guid? Id { get; private init; }

        public string Name { get; private init; }
        public string Surname { get; private init; }
        public string Password { get; private init; }
        public string? Address { get; set; }
        public EmailAddress EmailAddress { get; private init; }
        public PassportData? PassportData { get; set; }

        public bool IsSuspicious => Address is null || PassportData is null;

        public bool Equals(Client? other)
            => other is not null && other.Id.Equals(Id);

        public override bool Equals(object? obj)
            => Equals(obj as Client);

        public override int GetHashCode()
            => Id.GetHashCode();
    }
}