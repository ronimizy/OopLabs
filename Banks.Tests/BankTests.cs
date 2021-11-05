using System;
using System.Linq;
using Banks.Accounts;
using Banks.Entities;
using Banks.Models;
using Banks.Plans;
using Banks.Services;
using Banks.Tests.Mocks;
using Banks.Tools;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Banks.Tests
{
    [TestFixture]
    public class BankTests : IDisposable
    {
        private ChronometerMock _chronometer = null!;

        private BanksDatabaseContext _banksDatabaseContext = null!;
        private MailingDatabaseContext _mailingDatabaseContext = null!;

        private CentralBank _centralBank = null!;
        private MailingService _mailingService = null!;

        private Client _client = null!;

        [SetUp]
        public void Setup()
        {
            _chronometer = new ChronometerMock();

            DbContextOptions mailingContextOptions = new DbContextOptionsBuilder()
                .UseInMemoryDatabase("Mailing")
                .Options;
            DbContextOptions banksContextOptions = new DbContextOptionsBuilder()
                .UseInMemoryDatabase("Banks")
                .Options;

            _mailingDatabaseContext = new MailingDatabaseContext(mailingContextOptions);
            _mailingService = new MailingService(_mailingDatabaseContext);

            _banksDatabaseContext = new BanksDatabaseContext(banksContextOptions, _chronometer, _mailingService);
            _centralBank = new CentralBank(_banksDatabaseContext);

            Console.WriteLine();

            IBuilder<Client> clientBuilder = Client.BuildClient
                .Called("Me", "eM")
                .WithPassword("2281337")
                .WithEmailAddress(new EmailAddress("me@myemail.com"))
                .WithAddress("My Home")
                .WithPassportData(new PassportData("21231212", "444444"))
                .Builder;

            _client = _centralBank.RegisterClient(clientBuilder);
        }

        [TearDown]
        public void Teardown()
        {
            _banksDatabaseContext.Dispose();
            _mailingDatabaseContext.Dispose();
        }

        public void Dispose() { }

        [Test]
        public void AccountsTest_AllTypesOfAccountsCreated_AllTypesOfOperationsExecuted()
        {
            const string bankName = "My Bank";
            const decimal baseDeposit = 1000;
            const decimal creditLimit = -2000;

            _chronometer.CurrentDateTime = new DateTime(2020, 9, 2);
            var unlockDateTime = new DateTime(2020, 10, 2);

            Bank bank = _centralBank.RegisterBank(bankName, _client, new SuspiciousLimitPolicy(decimal.MaxValue));
            IBuilder<DebitAccountPlan> debitPlanBuilder = DebitAccountPlan.BuildPlan.WithDebitPercentage(0.1m);
            IBuilder<DepositAccountPlan> depositPlanBuilder = DepositAccountPlan.BuildPlan
                .WithLevel(new DepositPercentLevel(baseDeposit, 0.05m))
                .Builder;
            IBuilder<CreditAccountPlan> creditPlanBuilder = CreditAccountPlan.BuildPlan
                .LimitedTo(creditLimit)
                .WithCommissionPercent(0.05m);

            bank.RegisterDebitAccountPlan(debitPlanBuilder);
            bank.RegisterDepositAccountPlan(depositPlanBuilder);
            bank.RegisterCreditAccountPlan(creditPlanBuilder);

            DebitAccountPlan debitPlan = bank.DebitAccountPlans.Single();
            DepositAccountPlan depositAccountPlan = bank.DepositAccountPlans.Single();
            CreditAccountPlan creditAccountPlan = bank.CreditAccountPlans.Single();

            Account debitAccount = bank.EnrollDebitAccount(_client, debitPlan);
            Account depositAccount = bank.EnrollDepositAccount(_client, depositAccountPlan, unlockDateTime, baseDeposit);
            Account creditAccount = bank.EnrollCreditAccount(_client, creditAccountPlan);
            
            Assert.DoesNotThrow(() => bank.AccrueFunds(debitAccount, baseDeposit));
            Assert.DoesNotThrow(() => bank.AccrueFunds(depositAccount, baseDeposit));
            Assert.DoesNotThrow(() => bank.AccrueFunds(creditAccount, baseDeposit));
            
            Assert.DoesNotThrow(() => bank.WithdrawFunds(debitAccount, baseDeposit));
            Assert.Throws<BanksException>(() => bank.WithdrawFunds(depositAccount, baseDeposit));
            Assert.DoesNotThrow(() => bank.WithdrawFunds(creditAccount, baseDeposit));

            _chronometer.CurrentDateTime = unlockDateTime;
            
            Assert.DoesNotThrow(() => bank.WithdrawFunds(depositAccount, 2 * baseDeposit));

            Assert.Throws<BanksException>(() => bank.WithdrawFunds(debitAccount, baseDeposit));
            Assert.Throws<BanksException>(() => bank.WithdrawFunds(depositAccount, baseDeposit));
            Assert.DoesNotThrow(() => bank.WithdrawFunds(creditAccount, -creditLimit / 10));
            
            Assert.Throws<BanksException>(() => bank.WithdrawFunds(creditAccount, -creditLimit));
            
            Assert.DoesNotThrow(() => bank.TransferFunds(creditAccount, debitAccount, -creditLimit / 2));
        }
    }
}