using System;
using Banks.Entities;
using Banks.Models;
using Banks.Tools;

namespace Banks.ExceptionFactories
{
    internal static class MailingServiceExceptionFactory
    {
        public static BanksException MissingEmailException(EmailUser client, Guid emailId)
            => new BanksException($"Client {client} has no email with id {emailId}");

        public static BanksException ExisingEmailException(EmailAddress address)
            => new BanksException($"Email with address {address} already exists");

        public static BanksException NonExistingUserException(EmailAddress address)
            => new BanksException($"Email user with address {address} is not exists");

        public static BanksException InvalidPasswordException()
            => new BanksException($"Provided password is invalid");
    }
}