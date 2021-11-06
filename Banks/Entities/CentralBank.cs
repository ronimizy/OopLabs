using System.Linq;
using Banks.Builders.CreditAccountPlanBuilder;
using Banks.Builders.DebitAccountPlanBuilder;
using Banks.Builders.DepositAccountPlanBuilder;
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

        public ICreditLimitSelector BuildCreditPlan => new CreditAccountPlanBuilder(_databaseContext);
        public IDebitPercentageSelector BuildDebitPlan => new DebitAccountPlanBuilder(_databaseContext);
        public IDepositPercentageLevelSelector BuildDepositPlan => new DepositAccountPlanBuilder(_databaseContext);

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

        public Bank RegisterBank(string name, Client owner, SuspiciousLimitPolicy limitPolicy)
        {
            name.ThrowIfNull(nameof(name));
            owner.ThrowIfNull(nameof(owner));
            limitPolicy.ThrowIfNull(nameof(limitPolicy));

            if (!_databaseContext.Clients.Contains(owner))
                throw BankExceptionFactory.ForeignClientException(owner);

            if (_databaseContext.Banks.ToList().Any(b => b.Name.Equals(name)))
                throw BankExceptionFactory.ExistingNameBankException(name);

            var bank = new Bank(name, owner, limitPolicy, _databaseContext);
            _databaseContext.Banks.Add(bank);
            _databaseContext.SaveChanges();

            return bank;
        }

        public void AccruePercents()
        {
            foreach (Bank bank in _databaseContext.Banks)
            {
                bank.AccruePercents();
            }
        }
    }
}