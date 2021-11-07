using System;
using System.Linq;
using Banks.Accounts;
using Banks.Chronometers;
using Banks.Entities;
using Banks.Models;
using Banks.Plans;
using Banks.Services;
using Banks.Tools;
using Microsoft.EntityFrameworkCore;
using Utility.Extensions;

const decimal baseDeposit = 1000;
const decimal creditLimit = -2000;
var chronometer = new UtcChronometer();

DbContextOptions mailingContextOptions = new DbContextOptionsBuilder()
    .UseSqlite("Filename=Mailing.db")
    .Options;
DbContextOptions banksContextOptions = new DbContextOptionsBuilder()
    .UseSqlite("Filename=Banks.db")
    .Options;

var mailingContext = new MailingDatabaseContext(mailingContextOptions);
var mailingService = new MailingService(mailingContext);

var banksContext = new BanksDatabaseContext(banksContextOptions, chronometer, mailingService);
var centralBank = new CentralBank(banksContext);

banksContext.Load();

Console.WriteLine();

IBuilder<Client> clientBuilder = Client.BuildClient
    .Called("Me", "eM")
    .WithPassword("2281337")
    .WithEmailAddress(new EmailAddress($"me{DateTime.UtcNow}@myemail.com"))
    .WithAddress("My Home")
    .Builder;

Client client = centralBank.RegisterClient(clientBuilder);

Bank bank = centralBank.RegisterBank($"My bank {DateTime.UtcNow}", client, new SuspiciousLimitPolicy(decimal.MaxValue));
IBuilder<DebitAccountPlan> debitPlanBuilder = DebitAccountPlan.BuildPlan.WithDebitPercentage(0.1m);
IBuilder<DepositAccountPlan> depositPlanBuilder = DepositAccountPlan.BuildPlan
    .WithLevel(new DepositPercentLevel(baseDeposit, 0.05m))
    .Builder;
IBuilder<CreditAccountPlan> creditPlanBuilder = CreditAccountPlan.BuildPlan
    .LimitedTo(creditLimit)
    .WithCommissionPercent(0.05m);

bank.RegisterDebitAccountPlan(client, debitPlanBuilder);
bank.RegisterDepositAccountPlan(client, depositPlanBuilder);
bank.RegisterCreditAccountPlan(client, creditPlanBuilder);

DebitAccountPlan debitPlan = bank.DebitAccountPlans.Single();
DepositAccountPlan depositAccountPlan = bank.DepositAccountPlans.Single();
CreditAccountPlan creditAccountPlan = bank.CreditAccountPlans.Single();

Account debitAccount = bank.EnrollDebitAccount(client, debitPlan);
Account depositAccount = bank.EnrollDepositAccount(client, depositAccountPlan, DateTime.MaxValue, baseDeposit);
Account creditAccount = bank.EnrollCreditAccount(client, creditAccountPlan);

bank.AccrueFunds(debitAccount, baseDeposit);
bank.AccrueFunds(depositAccount, baseDeposit);
bank.AccrueFunds(creditAccount, baseDeposit);
bank.WithdrawFunds(debitAccount, baseDeposit);
bank.WithdrawFunds(creditAccount, baseDeposit);
bank.WithdrawFunds(creditAccount, -creditLimit / 10);
bank.TransferFunds(creditAccount, debitAccount.Id.ThrowIfNull(nameof(debitAccount.Id)), -creditLimit / 2);