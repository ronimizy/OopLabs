using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Banks.Accounts;
using Banks.Commands;
using Banks.Commands.BankAccountCommands;
using Banks.ExceptionFactories;
using Banks.Models;
using Banks.Notification;
using Banks.OperationCancellation;
using Banks.Plans;
using Banks.Tools;
using Utility.Extensions;

namespace Banks.Entities
{
    public class Bank : IBank
    {
        private readonly AccountFactory _accountFactory;
        private readonly IClientNotificationService _notificationService;
        private readonly OperationCancellationService _cancellationService;

        private readonly List<Account> _operatedAccounts;

        private readonly List<DebitAccountPlan> _debitAccountPlans;
        private readonly List<DepositAccountPlan> _depositAccountPlans;
        private readonly List<CreditAccountPlan> _creditAccountPlans;

        public Bank(
            string name,
            Client owner,
            SuspiciousLimitPolicy limitPolicy,
            AccountFactory accountFactory,
            IClientNotificationService notificationService,
            OperationCancellationService cancellationService)
        {
            Name = name.ThrowIfNull(nameof(name));
            Owner = owner.ThrowIfNull(nameof(owner));
            SuspiciousLimitPolicy = limitPolicy.ThrowIfNull(nameof(limitPolicy));
            _accountFactory = accountFactory;
            _notificationService = notificationService;
            _cancellationService = cancellationService;
            _operatedAccounts = new List<Account>();
            _debitAccountPlans = new List<DebitAccountPlan>();
            _depositAccountPlans = new List<DepositAccountPlan>();
            _creditAccountPlans = new List<CreditAccountPlan>();
        }

#pragma warning disable 8618
        private Bank(BanksDatabaseContext context)
#pragma warning restore 8618
        {
            _cancellationService = context.OperationCancellationService;
            _accountFactory = context.AccountFactory;
            _notificationService = context.NotificationService;
        }

        [Key]
        public Guid? Id { get; private init; }

        public string Name { get; private init; }

        public Client Owner { get; private init; }

        public SuspiciousLimitPolicy SuspiciousLimitPolicy { get; private init; }

        [NotMapped]
        public IReadOnlyCollection<DebitAccountPlan> DebitAccountPlans => _debitAccountPlans;

        [NotMapped]
        public IReadOnlyCollection<DepositAccountPlan> DepositAccountPlans => _depositAccountPlans;

        [NotMapped]
        public IReadOnlyCollection<CreditAccountPlan> CreditAccountPlans => _creditAccountPlans;

        [NotMapped]
        internal IReadOnlyCollection<Account> OperatedAccounts => _operatedAccounts;

        public DebitAccountPlan RegisterDebitAccountPlan(Client client, IBuilder<DebitAccountPlan> builder)
        {
            ThrowIfNotOwner(client);
            builder.ThrowIfNull(nameof(builder));

            DebitAccountPlan plan = builder.Build();
            _debitAccountPlans.Add(plan);
            return plan;
        }

        public DepositAccountPlan RegisterDepositAccountPlan(Client client, IBuilder<DepositAccountPlan> builder)
        {
            ThrowIfNotOwner(client);
            builder.ThrowIfNull(nameof(builder));

            DepositAccountPlan plan = builder.Build();
            _depositAccountPlans.Add(plan);
            return plan;
        }

        public CreditAccountPlan RegisterCreditAccountPlan(Client client, IBuilder<CreditAccountPlan> builder)
        {
            ThrowIfNotOwner(client);
            builder.ThrowIfNull(nameof(builder));

            CreditAccountPlan plan = builder.Build();
            _creditAccountPlans.Add(plan);
            return plan;
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
            if (!account.TryExecuteCommand(command, out DateTime? executedDateTime))
                throw BankExceptionFactory.FailedOperationException(this, account, command.Info);

            LogCancellationEntry(account, executedDateTime.ThrowIfNull(nameof(executedDateTime)), command);
        }

        public void WithdrawFunds(Account account, decimal amount)
        {
            account.ThrowIfNull(nameof(account));

            if (!_operatedAccounts.Contains(account))
                throw BankExceptionFactory.ForeignAccountException(this, account);

            var command = new WithdrawalAccountCommand(amount);
            if (!account.TryExecuteCommand(command, out DateTime? executedDateTime))
                throw BankExceptionFactory.FailedOperationException(this, account, command.Info);

            LogCancellationEntry(account, executedDateTime.ThrowIfNull(nameof(executedDateTime)), command);
        }

        public void TransferFunds(Account origin, Account destination, decimal amount)
        {
            origin.ThrowIfNull(nameof(origin));

            if (!_operatedAccounts.Contains(origin))
                throw BankExceptionFactory.ForeignAccountException(this, origin);

            var command = new TransferringAccountCommand(amount, origin, destination);
            if (!origin.TryExecuteCommand(command, out DateTime? executedDateTime))
                throw BankExceptionFactory.FailedOperationException(this, origin, command.Info);

            LogCancellationEntry(origin, executedDateTime.ThrowIfNull(nameof(executedDateTime)), command);
        }

        public void CancelOperation(Client client, Account account, ReadOnlyAccountHistoryEntry entry)
        {
            client.ThrowIfNull(nameof(client));
            account.ThrowIfNull(nameof(account));
            ThrowIfNotOwner(client);

            if (!_operatedAccounts.Contains(account))
                throw BankExceptionFactory.ForeignAccountException(this, account);

            if (!account.History.Any(e => e.Equals(entry)))
                throw AccountExceptionFactory.NotContainingHistoryEntryException(account, entry);

            AccountCommand revertCommand = _cancellationService
                .GetCancellationCommand(entry)
                .ThrowIfNull(BankExceptionFactory.FailedToCancelOperationException(account, entry));

            if (!account.TryExecuteCommand(revertCommand) || !account.TryExecuteCommand(new CancelEntryAccountCommand(entry)))
                throw BankExceptionFactory.FailedToCancelOperationException(account, entry);
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

        private void LogCancellationEntry(Account account, DateTime executedDateTime, AccountCommand command)
        {
            ReadOnlyAccountHistoryEntry historyEntry = account.History.Last(e => e.ExecutedTime.Equals(executedDateTime));
            AccountCommand revertCommand = command.RevertCommand.ThrowIfNull(nameof(command.RevertCommand));
            var cancellationEntry = new OperationCancellationEntry(historyEntry, revertCommand);

            _cancellationService.AddCancellationEntry(cancellationEntry);
        }
    }
}