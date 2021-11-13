using System;
using System.Collections.Generic;
using Banks.Accounts;
using Banks.Models;
using Banks.Plans;
using Banks.Tools;

namespace Banks.Entities
{
    public interface IBank
    {
        Guid? Id { get; }
        string Name { get; }
        Client Owner { get; }
        SuspiciousLimitPolicy SuspiciousLimitPolicy { get; }
        IReadOnlyCollection<DebitAccountPlan> DebitAccountPlans { get; }
        IReadOnlyCollection<DepositAccountPlan> DepositAccountPlans { get; }
        IReadOnlyCollection<CreditAccountPlan> CreditAccountPlans { get; }

        DebitAccountPlan RegisterDebitAccountPlan(Client client, IBuilder<DebitAccountPlan> builder);
        DepositAccountPlan RegisterDepositAccountPlan(Client client, IBuilder<DepositAccountPlan> builder);
        CreditAccountPlan RegisterCreditAccountPlan(Client client, IBuilder<CreditAccountPlan> builder);

        Account EnrollDebitAccount(Client client, DebitAccountPlan plan);
        Account EnrollDepositAccount(Client client, DepositAccountPlan plan, DateTime unlockDate, decimal deposit);
        Account EnrollCreditAccount(Client client, CreditAccountPlan plan);

        void UpdateDebitAccountPlanPercentage(Client client, DebitAccountPlan plan, decimal percentage);
        void AddOrUpdateDepositAccountPlanLevel(Client client, DepositAccountPlan plan, DepositPercentLevel level);
        void RemoveDepositAccountPlanLevel(Client client, DepositAccountPlan plan, DepositPercentLevel level);
        void UpdateCreditAccountPlanPercentage(Client client, CreditAccountPlan plan, decimal percentage);
        void UpdateCreditAccountPlanLimit(Client client, CreditAccountPlan plan, decimal limit);

        void AccrueFunds(Account account, decimal amount);
        void WithdrawFunds(Account account, decimal amount);
        void TransferFunds(Account origin, Account destination, decimal amount);
        void CancelOperation(Client client, Account account, ReadOnlyAccountHistoryEntry entry);
        void SubscribeToPlanUpdates(Client client, AccountPlan plan);
        void UnsubscribeFromPlanUpdates(Client client, AccountPlan plan);

        IReadOnlyCollection<Account> AccountsForClient(Client client);
        void AccruePercents();
    }
}