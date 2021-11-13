using System;
using System.Linq;
using Banks.Accounts;
using Banks.Commands.BankAccountCommands;
using Banks.Entities;
using Banks.Models;
using Banks.Notification;
using Banks.Plans;
using Banks.Tests.Mocks;
using Banks.Tools;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Banks.Tests
{
    [TestFixture]
    public class BankTests : IDisposable
    {
        private const string BankName = "My Bank";
        private ChronometerMock _chronometer = null!;

        private BanksDatabaseContext _banksDatabaseContext = null!;
        private MailingDatabaseContext _mailingDatabaseContext = null!;

        private CentralBank _centralBank = null!;
        private MailingService _mailingService = null!;

        private Client _client = null!;
        private IBank _bank = null!;

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
            _bank = _centralBank.RegisterBank(BankName, _client, new SuspiciousLimitPolicy(decimal.MaxValue));
        }

        [TearDown]
        public void Teardown()
        {
            _banksDatabaseContext.Database.EnsureDeleted();
            _mailingDatabaseContext.Database.EnsureDeleted();
            _banksDatabaseContext.Dispose();
            _mailingDatabaseContext.Dispose();
        }

        public void Dispose() { }

        [Test]
        public void AccountsTest_AllTypesOfAccountsCreated_AllTypesOfOperationsExecuted()
        {
            const decimal baseDeposit = 1000;
            const decimal creditLimit = -2000;

            _chronometer.CurrentDateTime = new DateTime(2020, 9, 2);
            var unlockDateTime = new DateTime(2020, 10, 2);
            
            IBuilder<DebitAccountPlan> debitPlanBuilder = DebitAccountPlan.BuildPlan.WithDebitPercentage(0.1m);
            IBuilder<DepositAccountPlan> depositPlanBuilder = DepositAccountPlan.BuildPlan
                .WithLevel(new DepositPercentLevel(baseDeposit, 0.05m))
                .Builder;
            IBuilder<CreditAccountPlan> creditPlanBuilder = CreditAccountPlan.BuildPlan
                .LimitedTo(creditLimit)
                .WithCommissionPercent(0.05m);

            _bank.RegisterDebitAccountPlan(_client, debitPlanBuilder);
            _bank.RegisterDepositAccountPlan(_client, depositPlanBuilder);
            _bank.RegisterCreditAccountPlan(_client, creditPlanBuilder);

            DebitAccountPlan debitPlan = _bank.DebitAccountPlans.Single();
            DepositAccountPlan depositAccountPlan = _bank.DepositAccountPlans.Single();
            CreditAccountPlan creditAccountPlan = _bank.CreditAccountPlans.Single();

            Account debitAccount = _bank.EnrollDebitAccount(_client, debitPlan);
            Account depositAccount = _bank.EnrollDepositAccount(_client, depositAccountPlan, unlockDateTime, baseDeposit);
            Account creditAccount = _bank.EnrollCreditAccount(_client, creditAccountPlan);
            
            Assert.DoesNotThrow(() => _bank.AccrueFunds(debitAccount, baseDeposit));
            Assert.DoesNotThrow(() => _bank.AccrueFunds(depositAccount, baseDeposit));
            Assert.DoesNotThrow(() => _bank.AccrueFunds(creditAccount, baseDeposit));
            
            Assert.DoesNotThrow(() => _bank.WithdrawFunds(debitAccount, baseDeposit));
            Assert.Throws<BanksException>(() => _bank.WithdrawFunds(depositAccount, baseDeposit));
            Assert.DoesNotThrow(() => _bank.WithdrawFunds(creditAccount, baseDeposit));

            _chronometer.CurrentDateTime = unlockDateTime;
            
            Assert.DoesNotThrow(() => _bank.WithdrawFunds(depositAccount, 2 * baseDeposit));

            Assert.Throws<BanksException>(() => _bank.WithdrawFunds(debitAccount, baseDeposit));
            Assert.Throws<BanksException>(() => _bank.WithdrawFunds(depositAccount, baseDeposit));
            Assert.DoesNotThrow(() => _bank.WithdrawFunds(creditAccount, -creditLimit / 10));
            
            Assert.Throws<BanksException>(() => _bank.WithdrawFunds(creditAccount, -creditLimit));
            
            Assert.DoesNotThrow(() => _bank.TransferFunds(creditAccount, debitAccount, -creditLimit / 2));
        }

        [Test]
        public void RevertCommandTest_AccrualCommandExecuted_AccrualCommandReverted_OperationCanceled_RevertOperationLogged_CancellationLogged()
        {
            const decimal amount = 1000m;
            const decimal percent = 0.10m;

            var date = new DateTime(2020, 1, 1);
            
            DebitAccountPlan plan = _bank.RegisterDebitAccountPlan(_client, DebitAccountPlan.BuildPlan.WithDebitPercentage(percent));
            Account account = _bank.EnrollDebitAccount(_client, plan);

            _chronometer.CurrentDateTime = date;
            _bank.AccrueFunds(account, amount);

            ReadOnlyAccountHistoryEntry entry = account.History.Single();

            Assert.NotNull(entry.Id);
            Assert.DoesNotThrow(() => _bank.CancelOperation(_client, account, entry));

            Assert.AreEqual(3, account.History.Count);
            Assert.AreEqual(OperationState.Canceled, account.History.ElementAt(0).State);
            Assert.AreEqual(nameof(WithdrawalAccountCommand), account.History.ElementAt(1).Info.Title);
            Assert.AreEqual(nameof(CancelEntryAccountCommand), account.History.ElementAt(2).Info.Title);
        }
    }
}