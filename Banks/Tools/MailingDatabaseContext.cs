using Banks.Entities;
using Banks.Models;
using Microsoft.EntityFrameworkCore;

namespace Banks.Tools
{
    public sealed class MailingDatabaseContext : DbContext
    {
        public MailingDatabaseContext(DbContextOptions options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        internal DbSet<EmailUser> Users { get; private set; } = null!;
        internal DbSet<Email> Emails { get; private set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Email>()
                .Property(c => c.Receiver)
                .HasConversion(e => e.Value, s => new EmailAddress(s));
            modelBuilder.Entity<Email>().OwnsOne(e => e.Message);

            modelBuilder.Entity<EmailUser>()
                .Property(c => c.Address)
                .HasConversion(e => e.Value, s => new EmailAddress(s));
        }
    }
}