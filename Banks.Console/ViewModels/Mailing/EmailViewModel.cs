using Banks.Entities;
using Banks.Models;
using Banks.Services;
using Spectre.Console;
using Spectre.Mvvm.Interfaces;
using Utility.Extensions;

namespace Banks.Console.ViewModels.Mailing
{
    public class EmailViewModel
    {
        private readonly EmailUser _user;
        private readonly EmailPreview _preview;
        private readonly MailingService _mailingService;

        public EmailViewModel(EmailPreview preview, MailingService mailingService, EmailUser user, INavigator navigator)
        {
            Navigator = navigator;
            _preview = preview.ThrowIfNull(nameof(_preview));
            _mailingService = mailingService.ThrowIfNull(nameof(mailingService));
            _user = user.ThrowIfNull(nameof(user));
        }

        public INavigator Navigator { get; }

        public string Title => _preview.Title.EscapeMarkup();

        public Email Email => _mailingService.GetEmail(_user, _preview.Id);
    }
}