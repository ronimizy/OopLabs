using System;
using Banks.Accounts;
using Banks.Accounts.Wrappers;
using Banks.Chronometers;
using Banks.Entities;
using Banks.Models;
using Banks.Plans;
using Utility.Extensions;

namespace Banks.Tools
{
    public class AccountFactory
    {
        private readonly IChronometer _chronometer;

        public AccountFactory(IChronometer chronometer)
        {
            _chronometer = chronometer.ThrowIfNull(nameof(chronometer));
        }

        public Account CreateDebitAccount(Client client, DebitAccountPlan plan, SuspiciousLimitPolicy limitPolicy)
        {
            client.ThrowIfNull(nameof(client));
            plan.ThrowIfNull(nameof(plan));
            limitPolicy.ThrowIfNull(nameof(limitPolicy));

            Account account = new BaseAccount(client, 0, _chronometer);
            account = new PositiveBalanceAccountDecorator(account, _chronometer);
            account = new DebitAccountDecorator(plan, account, _chronometer);
            account = new SuspiciousLimitingAccountDecorator(account, limitPolicy, _chronometer);
            account = new CommandLoggingAccountProxy(account, _chronometer);

            return account;
        }

        public Account CreateDepositAccount(
            Client client, DepositAccountPlan plan, DateTime unlockDateTime, decimal deposit, SuspiciousLimitPolicy limitPolicy)
        {
            client.ThrowIfNull(nameof(client));
            plan.ThrowIfNull(nameof(plan));
            limitPolicy.ThrowIfNull(nameof(limitPolicy));

            Account account = new BaseAccount(client, deposit, _chronometer);
            account = new PositiveBalanceAccountDecorator(account, _chronometer);
            account = new TimeLockingAccountDecorator(unlockDateTime, account, _chronometer);
            account = new DepositAccountDecorator(deposit, plan, account, _chronometer);
            account = new SuspiciousLimitingAccountDecorator(account, limitPolicy, _chronometer);
            account = new CommandLoggingAccountProxy(account, _chronometer);

            return account;
        }

        public Account CreateCreditAccount(Client client, CreditAccountPlan plan, SuspiciousLimitPolicy limitPolicy)
        {
            client.ThrowIfNull(nameof(client));
            plan.ThrowIfNull(nameof(plan));
            limitPolicy.ThrowIfNull(nameof(limitPolicy));

            Account account = new BaseAccount(client, 0, _chronometer);
            account = new CreditAccountDecorator(plan, account, _chronometer);
            account = new SuspiciousLimitingAccountDecorator(account, limitPolicy, _chronometer);
            account = new CommandLoggingAccountProxy(account, _chronometer);

            return account;
        }
    }
}