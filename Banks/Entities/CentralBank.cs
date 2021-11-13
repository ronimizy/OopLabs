using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Accounts;
using Banks.Chronometers;
using Banks.DatabaseWrappers;
using Banks.ExceptionFactories;
using Banks.Models;
using Banks.Tools;
using Utility.Extensions;

namespace Banks.Entities
{
    public class CentralBank
    {
        private readonly BanksDatabaseContext _databaseContext;

        public CentralBank(BanksDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext.ThrowIfNull(nameof(databaseContext));
        }

        public IReadOnlyCollection<IBank> Banks => _databaseContext.Banks.Select(b => new BankDatabaseWrapper(_databaseContext, b)).ToList();
        public IChronometer Chronometer => _databaseContext.Chronometer;

        public Client RegisterClient(IBuilder<Client> clientBuilder)
        {
            clientBuilder.ThrowIfNull(nameof(clientBuilder));
            Client client = clientBuilder.Build().ThrowIfNull(nameof(Client));

            if (client.EmailAddress.IsEmpty)
                throw BankExceptionFactory.EmptyEmailException(client);

            if (_databaseContext.Clients.AsEnumerable().Any(c => c.EmailAddress.Equals(client.EmailAddress)))
                throw BankExceptionFactory.ExisingClientException(client.EmailAddress);

            _databaseContext.Clients.Add(client);
            _databaseContext.SaveChanges();
            return client;
        }

        public Client Login(EmailAddress emailAddress, string password)
        {
            emailAddress.ThrowIfNull(nameof(emailAddress));
            password.ThrowIfNull(nameof(password));

            Client client = _databaseContext.Clients.AsEnumerable()
                .SingleOrDefault(c => c.EmailAddress.Equals(emailAddress))
                .ThrowIfNull(BankExceptionFactory.NonExistingClientException(emailAddress));

            if (client.Password != PasswordHasher.Hash(password))
                throw BankExceptionFactory.InvalidPasswordException();

            return client;
        }

        public void SpecifyAddress(Client client, string address)
        {
            client.ThrowIfNull(nameof(client));
            address.ThrowIfNull(nameof(address));

            if (!_databaseContext.Clients.Contains(client))
                throw BankExceptionFactory.ForeignClientException(client);

            client.Address = address;
            _databaseContext.Clients.Update(client);
            _databaseContext.SaveChanges();
        }

        public void SpecifyPassportData(Client client, PassportData passportData)
        {
            client.ThrowIfNull(nameof(client));
            passportData.ThrowIfNull(nameof(passportData));

            if (!_databaseContext.Clients.Contains(client))
                throw BankExceptionFactory.ForeignClientException(client);

            client.PassportData = passportData;
            _databaseContext.Clients.Update(client);
            _databaseContext.SaveChanges();
        }

        public IBank RegisterBank(string name, Client owner, SuspiciousLimitPolicy limitPolicy)
        {
            name.ThrowIfNull(nameof(name));
            owner.ThrowIfNull(nameof(owner));
            limitPolicy.ThrowIfNull(nameof(limitPolicy));

            if (!_databaseContext.Clients.Contains(owner))
                throw BankExceptionFactory.ForeignClientException(owner);

            if (_databaseContext.Banks.ToList().Any(b => b.Name.Equals(name)))
                throw BankExceptionFactory.ExistingNameBankException(name);

            var bank = new Bank(
                name,
                owner,
                limitPolicy,
                _databaseContext.AccountFactory,
                _databaseContext.NotificationService,
                _databaseContext.OperationCancellationService);

            _databaseContext.Banks.Add(bank);
            _databaseContext.SaveChanges();

            return new BankDatabaseWrapper(_databaseContext, bank);
        }

        public void AccruePercents()
        {
            foreach (Bank bank in _databaseContext.Banks)
            {
                bank.AccruePercents();
                _databaseContext.Banks.Update(bank);
            }

            _databaseContext.SaveChanges();
        }

        public Account GetAccount(Guid id)
            => _databaseContext.Accounts.Find(id).ThrowIfNull(nameof(Account));
    }
}