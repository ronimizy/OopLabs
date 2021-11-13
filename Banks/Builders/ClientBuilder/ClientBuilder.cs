using Banks.Entities;
using Banks.Models;
using Banks.Tools;
using Utility.Extensions;

namespace Banks.Builders.ClientBuilder
{
    internal class ClientBuilder : IClientFullNameSelector, IClientPasswordSelector, IClientEmailAddressSelector, IClientOptionalInfoSelector
    {
        private string? _name;
        private string? _surname;
        private string? _password;
        private string? _address;
        private EmailAddress? _emailAddress;
        private PassportData? _passportData;

        public IBuilder<Client> Builder => this;

        public IClientPasswordSelector Called(string name, string surname)
        {
            _name = name.ThrowIfNull(nameof(name));
            _surname = surname.ThrowIfNull(nameof(surname));

            return this;
        }

        public IClientEmailAddressSelector WithPassword(string password)
        {
            _password = PasswordHasher.Hash(password);
            return this;
        }

        public IClientOptionalInfoSelector WithEmailAddress(EmailAddress emailAddress)
        {
            _emailAddress = emailAddress.ThrowIfNull(nameof(emailAddress));
            return this;
        }

        public IClientOptionalInfoSelector WithAddress(string? address)
        {
            _address = address;
            return this;
        }

        public IClientOptionalInfoSelector WithPassportData(PassportData? passportData)
        {
            _passportData = passportData;
            return this;
        }

        public Client Build()
        {
            return new Client(
                _name.ThrowIfNull(nameof(_name)),
                _surname.ThrowIfNull(nameof(_surname)),
                _password.ThrowIfNull(nameof(_password)),
                _emailAddress.ThrowIfNull(nameof(_emailAddress)),
                _address,
                _passportData);
        }
    }
}