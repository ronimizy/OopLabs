using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Accounts;
using Banks.Entities;
using Banks.Models;
using Banks.Tools;

namespace Banks.ExceptionFactories
{
    internal static class BankExceptionFactory
    {
        public static BanksException ExisingClientException(EmailAddress address)
            => new BanksException($"Client with email address {address} already exists");

        public static BanksException NonExistingClientException(EmailAddress address)
            => new BanksException($"Client with email address {address} is not exists");

        public static BanksException ForeignClientException(Client client)
            => new BanksException($"Client {client} is not being tracked");

        public static BanksException InvalidPasswordException()
            => new BanksException("Provided password is invalid");

        public static BanksException UnregisteredPlanException()
            => new BanksException("Given plan is not registered in the system");

        public static BanksException ForeignPlanException(Bank bank, Guid planId)
            => new BanksException($"Bank {bank}, does not have a plan with id {planId}");

        public static BanksException FailedPercentAccrualException(Bank bank, IReadOnlyCollection<Account> accounts)
            => new BanksException(
                $"Failed to accrue percents in Bank {bank} for accounts:\n{string.Join('\n', accounts.Select(a => a.Plan?.Info.ToString()))}");

        public static BanksException ForeignAccountException(Bank bank, Account account)
            => new BanksException($"Bank {bank} does not operate Account {account}");

        public static BanksException FailedOperationException(Bank bank, Account account, Info operationInfo)
            => new BanksException($"Bank {bank} has failed to execute an operation {operationInfo} on account {account}");

        public static BanksException ExistingNameBankException(string name)
            => new BanksException($"Bank called {name} already exists");
    }
}