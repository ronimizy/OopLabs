using Banks.Accounts;
using Banks.Accounts.Wrappers;
using Banks.Chronometers;
using Banks.Commands;
using Banks.Commands.BankAccountCommands;
using Banks.Entities;
using Banks.Models;
using Banks.Notification;
using Banks.OperationCancellation;
using Banks.Plans;
using Microsoft.EntityFrameworkCore;
using Utility.Extensions;

namespace Banks.Tools
{
    public sealed class BanksDatabaseContext : DbContext
    {
        public BanksDatabaseContext(DbContextOptions options, IChronometer chronometer, IClientNotificationService notificationService)
            : base(options)
        {
            NotificationService = notificationService.ThrowIfNull(nameof(notificationService));
            OperationCancellationService = new OperationCancellationService(this);
            Chronometer = chronometer.ThrowIfNull(nameof(chronometer));
            AccountFactory = new AccountFactory(Chronometer);

            Database.EnsureCreated();
        }

        internal DbSet<Bank> Banks { get; private set; } = null!;
        internal DbSet<Client> Clients { get; private set; } = null!;
        internal DbSet<Account> Accounts { get; private set; } = null!;

        internal DbSet<AccountPlan> AccountPlans { get; private set; } = null!;

        internal DbSet<ReadOnlyAccountHistoryEntry> HistoryEntries { get; private set; } = null!;
        internal DbSet<AccountCommand> BankAccountCommands { get; private set; } = null!;
        internal DbSet<SuspiciousLimitPolicy> SuspiciousLimitPolicies { get; private set; } = null!;
        internal DbSet<PassportData> PassportData { get; private set; } = null!;
        internal DbSet<OperationCancellationEntry> OperationCancellationEntries { get; private set; } = null!;

        internal DbSet<Info> Infos { get; private set; } = null!;

        internal IChronometer Chronometer { get; }
        internal AccountFactory AccountFactory { get; }
        internal IClientNotificationService NotificationService { get; }
        internal OperationCancellationService OperationCancellationService { get; }

        public void Load()
        {
            Infos.Load();
            SuspiciousLimitPolicies.Load();
            Clients.Load();
            AccountPlans.Load();
            Accounts.Include("_history").Load();
            BankAccountCommands.Load();
            HistoryEntries.Load();
            Banks
                .Include("_debitAccountPlans")
                .Include("_depositAccountPlans")
                .Include("_creditAccountPlans")
                .Include("_operatedAccounts")
                .Load();

            OperationCancellationEntries.Include(e => e.Entry).Load();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureAccounts(modelBuilder);

            ConfigureCommands(modelBuilder);

            ConfigurePlans(modelBuilder);

            ConfigureClient(modelBuilder);

            ConfigureBank(modelBuilder);

            ConfigureHistoryEntries(modelBuilder);
        }

        private static void ConfigureHistoryEntries(ModelBuilder modelBuilder)
            => modelBuilder.Entity<AccountHistoryEntry>();

        private static void ConfigureBank(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bank>().HasMany<Account>("_operatedAccounts");
            modelBuilder.Entity<Bank>().HasMany<DebitAccountPlan>("_debitAccountPlans");
            modelBuilder.Entity<Bank>().HasMany<DepositAccountPlan>("_depositAccountPlans");
            modelBuilder.Entity<Bank>().HasMany<CreditAccountPlan>("_creditAccountPlans");
        }

        private static void ConfigureClient(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>()
                .Property(c => c.EmailAddress)
                .HasConversion(e => e.Value, s => new EmailAddress(s));
        }

        private static void ConfigurePlans(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountPlan>().HasMany<Client>("_subscribers");

            modelBuilder.Entity<DebitAccountPlan>().Property(p => p.Percentage).HasField("_percentage");

            modelBuilder.Entity<DepositAccountPlan>().HasMany("_levels");

            modelBuilder.Entity<CreditAccountPlan>().Property(p => p.Percentage).HasField("_percentage");
            modelBuilder.Entity<CreditAccountPlan>().Property(p => p.Limit);
        }

        private static void ConfigureCommands(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccrualAccountCommand>().Property(c => c.Amount);
            modelBuilder.Entity<CancelEntryAccountCommand>().HasOne(c => c.Entry);
            modelBuilder.Entity<PercentsProcessingAccountCommand>();
            modelBuilder.Entity<TransferringAccountCommand>().Property(c => c.Amount);
            modelBuilder.Entity<TransferringAccountCommand>().HasOne(c => c.OriginAccount);
            modelBuilder.Entity<TransferringAccountCommand>().HasOne(c => c.DestinationAccount);
            modelBuilder.Entity<WithdrawalAccountCommand>().Property(c => c.Amount);
        }

        private static void ConfigureAccounts(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<CommandLoggingAccountProxy>(nameof(CommandLoggingAccountProxy));

            modelBuilder.Entity<BaseAccount>().Property(a => a.Balance).HasField("_balance");
            modelBuilder.Entity<BaseAccount>().HasOne<Client>("_owner");
            modelBuilder.Entity<BaseAccount>().HasMany<AccountHistoryEntry>("_history");

            modelBuilder.Entity<AccountWrapperBase>().HasOne<Account>("Wrapped");
            modelBuilder.Entity<PositiveBalanceAccountDecorator>();
            modelBuilder.Entity<DebitAccountDecorator>().HasOne<DebitAccountPlan>("_plan");
            modelBuilder.Entity<DepositAccountDecorator>().HasOne<DepositAccountPlan>("_plan");
            modelBuilder.Entity<TimeLockingAccountDecorator>();
            modelBuilder.Entity<CreditAccountDecorator>().HasOne<CreditAccountPlan>("_plan");
            modelBuilder.Entity<SuspiciousLimitingAccountDecorator>();
        }
    }
}