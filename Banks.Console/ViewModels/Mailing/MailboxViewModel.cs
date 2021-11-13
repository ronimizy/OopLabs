using System.Collections.Generic;
using Banks.Entities;
using Banks.Models;
using Banks.Notification;
using Spectre.Mvvm.Interfaces;
using Utility.Extensions;

namespace Banks.Console.ViewModels.Mailing
{
    public class MailboxViewModel
    {
        private readonly MailingService _mailingService;
        private readonly EmailUser _emailUser;

        public MailboxViewModel(MailingService mailingService, EmailUser emailUser, INavigator navigator)
        {
            Navigator = navigator;
            _mailingService = mailingService.ThrowIfNull(nameof(mailingService));
            _emailUser = emailUser.ThrowIfNull(nameof(emailUser));
        }

        public INavigator Navigator { get; }

        public IReadOnlyCollection<EmailPreview> Previews => _mailingService.PreviewsForUser(_emailUser);

        public EmailViewModel GetEmailViewModel(EmailPreview preview)
            => new EmailViewModel(preview, _mailingService, _emailUser, Navigator);
    }
}