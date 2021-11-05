using System;
using System.Linq;
using Banks.Accounts;
using Banks.Commands.BankAccountCommands;
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
    public class AccountTests : IDisposable
    {
        private const decimal Percent = 0.10m;
        private const string BanksDatabaseName = "Banks";
        private const string MailingDatabaseName = "Mailing";

        private ChronometerMock _chronometer = null!;
        private BanksDatabaseContext _banksContext = null!;
        private MailingDatabaseContext _mailingDatabaseContext = null!;
        private Account _account = null!;

        [SetUp]
        public void Setup()
        {
            DbContextOptions banksContextOptions = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(BanksDatabaseName)
                .EnableSensitiveDataLogging()
                .Options;
            DbContextOptions mailingContextOptions = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(MailingDatabaseName)
                .EnableSensitiveDataLogging()
                .Options;

            var plan = new DebitAccountPlan(Percent);

            Client client = Client.BuildClient
                .Called("Me", "eM")
                .WithPassword("2281337")
                .WithEmailAddress(new EmailAddress($"me{DateTime.UtcNow}@myemail.com"))
                .WithAddress("address")
                .WithPassportData(new PassportData("21231212", "444444"))
                .Build();
            
            _chronometer = new ChronometerMock();
            _mailingDatabaseContext = new MailingDatabaseContext(mailingContextOptions);
            _banksContext = new BanksDatabaseContext(banksContextOptions, _chronometer, new MailingService(_mailingDatabaseContext));
            _account = new AccountFactory(_banksContext).CreateDebitAccount(client, plan, new SuspiciousLimitPolicy(decimal.MaxValue));
        }

        [TearDown]
        public void Teardown()
            => _banksContext.Dispose();

        public void Dispose() { }

        [Test]
        public void RevertCommandTest_AccrualCommandExecuted_AccrualCommandReverted_OperationCanceled_RevertOperationLogged_CancellationLogged()
        {
            const decimal amount = 1000m;

            var date = new DateTime(2020, 1, 1);

            _chronometer.CurrentDateTime = date;
            _account.TryExecuteCommand(new AccrualAccountCommand(amount));

            ReadOnlyAccountHistoryEntry entry = _account.History.Single();

            Assert.NotNull(entry.Id);
            Assert.IsTrue(_account.TryCancelOperation(entry.Id!.Value));

            Assert.AreEqual(3, _account.History.Count);
            Assert.AreEqual(OperationState.Canceled, _account.History.ElementAt(0).State);
            Assert.AreEqual(nameof(WithdrawalAccountCommand), _account.History.ElementAt(1).Info.Title);
            Assert.AreEqual(nameof(CancelEntryAccountCommand), _account.History.ElementAt(2).Info.Title);
        }
    }
}