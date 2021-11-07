using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Banks.Accounts;
using Banks.Commands.BankAccountCommands;
using Banks.ExceptionFactories;
using Banks.Models;
using Banks.Plans;
using Banks.Services;
using Banks.Tools;
using Utility.Extensions;

namespace Banks.Entities
{
    public class Bank
    {
        private readonly BanksDatabaseContext _databaseContext;
        private readonly AccountFactory _accountFactory;
        private readonly IClientNotificationService _notificationService;

        private readonly List<Account> _operatedAccounts;

        private readonly List<DebitAccountPlan> _debitAccountPlans;
        private readonly List<DepositAccountPlan> _depositAccountPlans;
        private readonly List<CreditAccountPlan> _creditAccountPlans;

        public Bank(string name, Client owner, SuspiciousLimitPolicy limitPolicy, BanksDatabaseContext databaseContext)
        {
            Name = name.ThrowIfNull(nameof(name));
            Owner = owner.ThrowIfNull(nameof(owner));
            SuspiciousLimitPolicy = limitPolicy.ThrowIfNull(nameof(limitPolicy));
            _databaseContext = databaseContext.ThrowIfNull(nameof(databaseContext));
            _accountFactory = databaseContext.AccountFactory.ThrowIfNull(nameof(databaseContext.AccountFactory));
            _notificationService = databaseContext.NotificationService.ThrowIfNull(nameof(databaseContext.NotificationService));
            _operatedAccounts = new List<Account>();
            _debitAccountPlans = new List<DebitAccountPlan>();
            _depositAccountPlans = new List<DepositAccountPlan>();
            _creditAccountPlans = new List<CreditAccountPlan>();
        }

#pragma warning disable 8618
        private Bank(BanksDatabaseContext context)
#pragma warning restore 8618
        {
            _databaseContext = context;
            _accountFactory = context.AccountFactory;
            _notificationService = context.NotificationService;
        }

        [Key]
        public Guid? Id { get; private init; }

        public string Name { get; private init; }

        public Client Owner { get; private init; }

        public SuspiciousLimitPolicy SuspiciousLimitPolicy { get; private init; }

        [NotMapped]
        public IReadOnlyCollection<Account> OperatedAccounts => _operatedAccounts;

        [NotMapped]
        public IReadOnlyCollection<DebitAccountPlan> DebitAccountPlans => _debitAccountPlans;

        [NotMapped]
        public IReadOnlyCollection<DepositAccountPlan> DepositAccountPlans => _depositAccountPlans;

        [NotMapped]
        public IReadOnlyCollection<CreditAccountPlan> CreditAccountPlans => _creditAccountPlans;

        public void RegisterDebitAccountPlan(Client client, IBuilder<DebitAccountPlan> builder)
        {
            ThrowIfNotOwner(client);
            builder.ThrowIfNull(nameof(builder));
            _debitAccountPlans.Add(builder.Build());
            UpdateBank();
        }

        public void RegisterDepositAccountPlan(Client client, IBuilder<DepositAccountPlan> builder)
        {
            ThrowIfNotOwner(client);
            builder.ThrowIfNull(nameof(builder));
            _depositAccountPlans.Add(builder.Build());
            UpdateBank();
        }

        public void RegisterCreditAccountPlan(Client client, IBuilder<CreditAccountPlan> builder)
        {
            ThrowIfNotOwner(client);
            builder.ThrowIfNull(nameof(builder));
            _creditAccountPlans.Add(builder.Build());
            UpdateBank();
        }

        public Account EnrollDebitAccount(Client client, DebitAccountPlan plan)
        {
            client.ThrowIfNull(nameof(client));
            plan.ThrowIfNull(nameof(plan));

            plan.Id.ThrowIfNull(BankExceptionFactory.UnregisteredPlanException());
            if (!_debitAccountPlans.Contains(plan))
                throw BankExceptionFactory.ForeignPlanException(this, plan.Id!.Value);

            Account account = _accountFactory.CreateDebitAccount(client, plan, SuspiciousLimitPolicy);

            _operatedAccounts.Add(account);
            _databaseContext.Accounts.Add(account);
            _databaseContext.AccountPlans.Update(plan);
            _databaseContext.Banks.Update(this);
            _databaseContext.SaveChanges();
            return account;
        }

        public Account EnrollDepositAccount(Client client, DepositAccountPlan plan, DateTime unlockDate, decimal deposit)
        {
            client.ThrowIfNull(nameof(client));
            plan.ThrowIfNull(nameof(plan));

            plan.Id.ThrowIfNull(BankExceptionFactory.UnregisteredPlanException());
            if (!_depositAccountPlans.Contains(plan))
                throw BankExceptionFactory.ForeignPlanException(this, plan.Id!.Value);

            Account account = _accountFactory.CreateDepositAccount(client, plan, unlockDate, deposit, SuspiciousLimitPolicy);

            _operatedAccounts.Add(account);
            _databaseContext.Accounts.Add(account);
            _databaseContext.Banks.Update(this);
            _databaseContext.SaveChanges();
            return account;
        }

        public Account EnrollCreditAccount(Client client, CreditAccountPlan plan)
        {
            client.ThrowIfNull(nameof(client));
            plan.ThrowIfNull(nameof(plan));

            plan.Id.ThrowIfNull(BankExceptionFactory.UnregisteredPlanException());
            if (!_creditAccountPlans.Contains(plan))
                throw BankExceptionFactory.ForeignPlanException(this, plan.Id!.Value);

            Account account = _accountFactory.CreateCreditAccount(client, plan, SuspiciousLimitPolicy);

            _operatedAccounts.Add(account);
            _databaseContext.Accounts.Add(account);
            _databaseContext.Banks.Update(this);
            _databaseContext.SaveChanges();
            return account;
        }

        public void UpdateDebitAccountPlanPercentage(Client client, DebitAccountPlan plan, decimal percentage)
        {
            ThrowIfNotOwner(client);
            plan.ThrowIfNull(nameof(plan));
            plan.Id.ThrowIfNull(BankExceptionFactory.UnregisteredPlanException());

            if (!_debitAccountPlans.Contains(plan))
                throw BankExceptionFactory.ForeignPlanException(this, plan.Id!.Value);

            plan.Percentage = percentage;
            _databaseContext.AccountPlans.Update(plan);
            _databaseContext.SaveChanges();

            var message = new Message(
                "Debit account plan update",
                $"We updated the percentage of debit account plan {plan}, not it is {percentage}%");
            NotifyHolders(plan, message);
        }

        public void AddOrUpdateDepositAccountPlanLevel(Client client, DepositAccountPlan plan, DepositPercentLevel level)
        {
            ThrowIfNotOwner(client);
            plan.ThrowIfNull(nameof(plan));
            plan.Id.ThrowIfNull(BankExceptionFactory.UnregisteredPlanException());
            level.ThrowIfNull(nameof(level));

            if (!_depositAccountPlans.Contains(plan))
                throw BankExceptionFactory.ForeignPlanException(this, plan.Id!.Value);

            plan.AddOrUpdateLevel(level);
            _databaseContext.AccountPlans.Update(plan);
            _databaseContext.SaveChanges();

            var message = new Message(
                "Deposit account plan update",
                $"We updated the level of deposit account plan {plan}, it now has {level.Percent}% from the deposit of {level.Amount}.\nThe new levels are:\n{plan}");
            NotifyHolders(plan, message);
        }

        public void RemoveDepositAccountPlanLevel(Client client, DepositAccountPlan plan, DepositPercentLevel level)
        {
            ThrowIfNotOwner(client);
            plan.ThrowIfNull(nameof(plan));
            plan.Id.ThrowIfNull(BankExceptionFactory.UnregisteredPlanException());
            level.ThrowIfNull(nameof(level));

            if (!_depositAccountPlans.Contains(plan))
                throw BankExceptionFactory.ForeignPlanException(this, plan.Id!.Value);

            plan.RemoveLevel(level);
            _databaseContext.AccountPlans.Update(plan);
            _databaseContext.SaveChanges();

            var message = new Message(
                "Deposit account plan update",
                $"We removed the level of deposit account plan {plan}.\nThe new levels are:\n{plan}");
            NotifyHolders(plan, message);
        }

        public void UpdateCreditAccountPlanPercentage(Client client, CreditAccountPlan plan, decimal percentage)
        {
            ThrowIfNotOwner(client);
            plan.ThrowIfNull(nameof(plan));
            plan.Id.ThrowIfNull(BankExceptionFactory.UnregisteredPlanException());

            if (!_creditAccountPlans.Contains(plan))
                throw BankExceptionFactory.ForeignPlanException(this, plan.Id!.Value);

            plan.Percentage = percentage;
            _databaseContext.AccountPlans.Update(plan);
            _databaseContext.SaveChanges();

            var message = new Message(
                "Credit account plan update",
                $"We updated the percentage of credit account plan {plan}, it is now {percentage}%.");
            NotifyHolders(plan, message);
        }

        public void UpdateCreditAccountPlanLimit(Client client, CreditAccountPlan plan, decimal limit)
        {
            ThrowIfNotOwner(client);
            plan.ThrowIfNull(nameof(plan));
            plan.Id.ThrowIfNull(BankExceptionFactory.UnregisteredPlanException());

            if (!_creditAccountPlans.Contains(plan))
                throw BankExceptionFactory.ForeignPlanException(this, plan.Id!.Value);

            plan.Limit = limit;
            _databaseContext.AccountPlans.Update(plan);
            _databaseContext.SaveChanges();

            var message = new Message(
                "Credit account plan update",
                $"We updated the limit of credit account plan {plan}, it is now {limit}$.");
            NotifyHolders(plan, message);
        }

        public void AccrueFunds(Account account, decimal amount)
        {
            account.ThrowIfNull(nameof(account));

            if (!_operatedAccounts.Contains(account))
                throw BankExceptionFactory.ForeignAccountException(this, account);

            var command = new AccrualAccountCommand(amount);
            if (!account.TryExecuteCommand(command))
                throw BankExceptionFactory.FailedOperationException(this, account, command.Info);
        }

        public void WithdrawFunds(Account account, decimal amount)
        {
            account.ThrowIfNull(nameof(account));

            if (!_operatedAccounts.Contains(account))
                throw BankExceptionFactory.ForeignAccountException(this, account);

            var command = new WithdrawalAccountCommand(amount);
            if (!account.TryExecuteCommand(command))
                throw BankExceptionFactory.FailedOperationException(this, account, command.Info);
        }

        public void TransferFunds(Account origin, Guid destinationId, decimal amount)
        {
            origin.ThrowIfNull(nameof(origin));

            if (!_operatedAccounts.Contains(origin))
                throw BankExceptionFactory.ForeignAccountException(this, origin);

            Account destination = _databaseContext.Banks
                .AsEnumerable()
                .SelectMany(b => b.OperatedAccounts)
                .SingleOrDefault(a => destinationId.Equals(a.Id))
                .ThrowIfNull(BankExceptionFactory.UnknownAccountException(destinationId));

            var command = new TransferringAccountCommand(amount, origin, destination);
            if (!origin.TryExecuteCommand(command))
                throw BankExceptionFactory.FailedOperationException(this, origin, command.Info);
        }

        public void CancelOperation(Client client, Account account, Guid operationId)
        {
            client.ThrowIfNull(nameof(client));
            account.ThrowIfNull(nameof(account));
            ThrowIfNotOwner(client);

            if (!_operatedAccounts.Contains(account))
                throw BankExceptionFactory.ForeignAccountException(this, account);

            if (!account.TryCancelOperation(operationId))
                throw BankExceptionFactory.FailedToCancelOperationException(account, operationId);
        }

        public void SubscribeToPlanUpdates(Client client, AccountPlan plan)
        {
            client.ThrowIfNull(nameof(client));
            plan.ThrowIfNull(nameof(plan));
            plan.Id.ThrowIfNull(BankExceptionFactory.UnregisteredPlanException());

            if (!_debitAccountPlans.Contains(plan) && !_creditAccountPlans.Contains(plan) && !_depositAccountPlans.Contains(plan))
                throw BankExceptionFactory.ForeignPlanException(this, plan.Id!.Value);

            if (plan.Subscribers.Contains(client) || !_operatedAccounts.Any(a => a.Owner.Equals(client) && plan.Equals(plan)))
                throw BankExceptionFactory.CannotSubscribeException(plan, client);

            plan.Subscribe(client);
        }

        public void UnsubscribeFromPlanUpdates(Client client, AccountPlan plan)
        {
            client.ThrowIfNull(nameof(client));
            plan.ThrowIfNull(nameof(plan));
            plan.Id.ThrowIfNull(BankExceptionFactory.UnregisteredPlanException());

            if (!_debitAccountPlans.Contains(plan) && !_creditAccountPlans.Contains(plan) && !_depositAccountPlans.Contains(plan))
                throw BankExceptionFactory.ForeignPlanException(this, plan.Id!.Value);

            if (!plan.Subscribers.Contains(client))
                throw BankExceptionFactory.CannotUnsubscribeException(plan, client);

            plan.Unsubscribe(client);
        }

        public IReadOnlyCollection<Account> AccountsForClient(Client client)
            => _operatedAccounts.Where(a => a.Owner.Equals(client)).ToList();

        public void AccruePercents()
        {
            var command = new PercentsProcessingAccountCommand();

            IEnumerable<Account> percentAccrualAccounts = _operatedAccounts
                .Where(a => _debitAccountPlans.Contains(a.Plan) || _depositAccountPlans.Contains(a.Plan));
            List<Account> failedAccounts = percentAccrualAccounts.Where(a => !a.TryExecuteCommand(command)).ToList();

            if (failedAccounts.Any())
                throw BankExceptionFactory.FailedPercentAccrualException(this, failedAccounts);
        }

        private void UpdateBank()
        {
            _databaseContext.Banks.Update(this);
            _databaseContext.SaveChanges();
        }

        private void NotifyHolders(AccountPlan plan, Message message)
        {
            foreach (Client client in plan.Subscribers)
            {
                _notificationService.Notify(this, client, message);
            }
        }

        private void ThrowIfNotOwner(Client client)
        {
            if (!client.Equals(Owner))
                throw BankExceptionFactory.InsufficientPermissionException(client);
        }
    }
}