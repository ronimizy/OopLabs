using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Entities;
using Banks.ExceptionFactories;
using Banks.Models;
using Banks.Tools;
using Utility.Extensions;

namespace Banks.Services
{
    public class MailingService : IClientNotificationService
    {
        private readonly MailingDatabaseContext _databaseContext;

        public MailingService(MailingDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext.ThrowIfNull(nameof(databaseContext));
        }

        public EmailUser Register(EmailAddress address, string password)
        {
            address.ThrowIfNull(nameof(address));
            password.ThrowIfNull(nameof(password));

            if (_databaseContext.Users.AsEnumerable().Any(u => u.Address.Equals(address)))
                throw MailingServiceExceptionFactory.ExisingEmailException(address);

            var user = new EmailUser(address, PasswordHasher.Hash(password));
            _databaseContext.Users.Add(user);
            _databaseContext.SaveChanges();

            return user;
        }

        public EmailUser Login(EmailAddress address, string password)
        {
            address.ThrowIfNull(nameof(address));
            password.ThrowIfNull(nameof(password));

            EmailUser user = _databaseContext.Users.AsEnumerable()
                .SingleOrDefault(u => u.Address.Equals(address))
                .ThrowIfNull(MailingServiceExceptionFactory.NonExistingUserException(address));

            if (user.Password != PasswordHasher.Hash(password))
                throw MailingServiceExceptionFactory.InvalidPasswordException();

            return user;
        }

        public void Notify(Bank bank, Client client, Message message)
        {
            bank.ThrowIfNull(nameof(bank));
            client.ThrowIfNull(nameof(client));
            message.ThrowIfNull(nameof(message));

            if (string.IsNullOrEmpty(client.EmailAddress.Value))
                return;

            var email = new Email(client.EmailAddress, bank.Name, message);
            _databaseContext.Emails.Add(email);
            _databaseContext.SaveChanges();
        }

        public IReadOnlyCollection<EmailPreview> PreviewsForClient(EmailUser client)
        {
            client.ThrowIfNull(nameof(client));

            return _databaseContext.Emails
                .Where(e => e.Receiver.Equals(client.Address))
                .Select(e => e.Preview)
                .ToList();
        }

        public int NotViewedEmailCountForClient(EmailUser client)
        {
            client.ThrowIfNull(nameof(client));

            return _databaseContext.Emails
                .Count(e => e.Receiver.Equals(client.Address) && !e.Viewed);
        }

        public Email GetEmail(EmailUser client, Guid emailId)
        {
            client.ThrowIfNull(nameof(client));

            Email email = _databaseContext.Emails
                .SingleOrDefault(e => e.Receiver.Equals(client.Address) && e.Id.Equals(emailId))
                .ThrowIfNull(MailingServiceExceptionFactory.MissingEmailException(client, emailId));

            email.Viewed = true;
            return email;
        }
    }
}