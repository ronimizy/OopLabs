using System;
using System.Collections.Generic;
using Banks.Accounts;
using Banks.Entities;
using Banks.Models;
using Banks.Plans;
using Banks.Tools;

namespace Banks.DatabaseWrappers
{
    public class BankDatabaseWrapper : IBank
    {
        private readonly BanksDatabaseContext _databaseContext;
        private readonly Bank _bank;

        public BankDatabaseWrapper(BanksDatabaseContext databaseContext, Bank bank)
        {
            _databaseContext = databaseContext;
            _bank = bank;
        }

        public Guid? Id => _bank.Id;
        public string Name => _bank.Name;
        public Client Owner => _bank.Owner;

        public SuspiciousLimitPolicy SuspiciousLimitPolicy => _bank.SuspiciousLimitPolicy;
        public IReadOnlyCollection<DebitAccountPlan> DebitAccountPlans => _bank.DebitAccountPlans;
        public IReadOnlyCollection<DepositAccountPlan> DepositAccountPlans => _bank.DepositAccountPlans;
        public IReadOnlyCollection<CreditAccountPlan> CreditAccountPlans => _bank.CreditAccountPlans;

        public DebitAccountPlan RegisterDebitAccountPlan(Client client, IBuilder<DebitAccountPlan> builder)
        {
            DebitAccountPlan plan = _bank.RegisterDebitAccountPlan(client, builder);
            _databaseContext.AccountPlans.Add(plan);
            _databaseContext.Banks.Update(_bank);
            _databaseContext.SaveChanges();

            return plan;
        }

        public DepositAccountPlan RegisterDepositAccountPlan(Client client, IBuilder<DepositAccountPlan> builder)
        {
            DepositAccountPlan plan = _bank.RegisterDepositAccountPlan(client, builder);
            _databaseContext.AccountPlans.Add(plan);
            _databaseContext.Banks.Update(_bank);
            _databaseContext.SaveChanges();

            return plan;
        }

        public CreditAccountPlan RegisterCreditAccountPlan(Client client, IBuilder<CreditAccountPlan> builder)
        {
            CreditAccountPlan plan = _bank.RegisterCreditAccountPlan(client, builder);
            _databaseContext.AccountPlans.Add(plan);
            _databaseContext.Banks.Update(_bank);
            _databaseContext.SaveChanges();

            return plan;
        }

        public Account EnrollDebitAccount(Client client, DebitAccountPlan plan)
        {
            Account account = _bank.EnrollDebitAccount(client, plan);
            _databaseContext.Accounts.Add(account);
            _databaseContext.AccountPlans.Update(plan);
            _databaseContext.Banks.Update(_bank);
            _databaseContext.SaveChanges();

            return account;
        }

        public Account EnrollDepositAccount(Client client, DepositAccountPlan plan, DateTime unlockDate, decimal deposit)
        {
            Account account = _bank.EnrollDepositAccount(client, plan, unlockDate, deposit);
            _databaseContext.Accounts.Add(account);
            _databaseContext.Banks.Update(_bank);
            _databaseContext.SaveChanges();

            return account;
        }

        public Account EnrollCreditAccount(Client client, CreditAccountPlan plan)
        {
            Account account = _bank.EnrollCreditAccount(client, plan);
            _databaseContext.Accounts.Add(account);
            _databaseContext.Banks.Update(_bank);
            _databaseContext.SaveChanges();

            return account;
        }

        public void UpdateDebitAccountPlanPercentage(Client client, DebitAccountPlan plan, decimal percentage)
        {
            _bank.UpdateDebitAccountPlanPercentage(client, plan, percentage);
            _databaseContext.AccountPlans.Update(plan);
            _databaseContext.SaveChanges();
        }

        public void AddOrUpdateDepositAccountPlanLevel(Client client, DepositAccountPlan plan, DepositPercentLevel level)
        {
            _bank.AddOrUpdateDepositAccountPlanLevel(client, plan, level);
            _databaseContext.AccountPlans.Update(plan);
            _databaseContext.SaveChanges();
        }

        public void RemoveDepositAccountPlanLevel(Client client, DepositAccountPlan plan, DepositPercentLevel level)
        {
            _bank.RemoveDepositAccountPlanLevel(client, plan, level);
            _databaseContext.AccountPlans.Update(plan);
            _databaseContext.SaveChanges();
        }

        public void UpdateCreditAccountPlanPercentage(Client client, CreditAccountPlan plan, decimal percentage)
        {
            _bank.UpdateCreditAccountPlanPercentage(client, plan, percentage);
            _databaseContext.AccountPlans.Update(plan);
            _databaseContext.SaveChanges();
        }

        public void UpdateCreditAccountPlanLimit(Client client, CreditAccountPlan plan, decimal limit)
        {
            _bank.UpdateCreditAccountPlanLimit(client, plan, limit);
            _databaseContext.AccountPlans.Update(plan);
            _databaseContext.SaveChanges();
        }

        public void AccrueFunds(Account account, decimal amount)
        {
            _bank.AccrueFunds(account, amount);
            _databaseContext.Accounts.Update(account);
            _databaseContext.SaveChanges();
        }

        public void WithdrawFunds(Account account, decimal amount)
        {
            _bank.WithdrawFunds(account, amount);
            _databaseContext.Accounts.Update(account);
            _databaseContext.SaveChanges();
        }

        public void TransferFunds(Account origin, Account destination, decimal amount)
        {
            _bank.TransferFunds(origin, destination, amount);
            _databaseContext.Accounts.Update(origin);
            _databaseContext.Accounts.Update(destination);
            _databaseContext.SaveChanges();
        }

        public void CancelOperation(Client client, Account account, ReadOnlyAccountHistoryEntry entry)
        {
            _bank.CancelOperation(client, account, entry);
            _databaseContext.Accounts.Update(account);
            _databaseContext.SaveChanges();
        }

        public void SubscribeToPlanUpdates(Client client, AccountPlan plan)
        {
            _bank.SubscribeToPlanUpdates(client, plan);
            _databaseContext.AccountPlans.Update(plan);
            _databaseContext.SaveChanges();
        }

        public void UnsubscribeFromPlanUpdates(Client client, AccountPlan plan)
        {
            _bank.UnsubscribeFromPlanUpdates(client, plan);
            _databaseContext.AccountPlans.Update(plan);
            _databaseContext.SaveChanges();
        }

        public IReadOnlyCollection<Account> AccountsForClient(Client client)
            => _bank.AccountsForClient(client);

        public void AccruePercents()
        {
            _bank.AccruePercents();
            _databaseContext.Banks.Update(_bank);
            _databaseContext.SaveChanges();
        }
    }
}