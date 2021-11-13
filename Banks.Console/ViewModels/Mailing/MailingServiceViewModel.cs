using Banks.Console.Views;
using Banks.Console.Views.Mailing;
using Banks.Entities;
using Banks.Models;
using Banks.Notification;
using Spectre.Mvvm.Interfaces;
using Spectre.Mvvm.Models;
using Utility.Extensions;

namespace Banks.Console.ViewModels.Mailing
{
    public class MailingServiceViewModel
    {
        private readonly MailingService _mailingService;

        public MailingServiceViewModel(MailingService mailingService, INavigator navigator)
        {
            _mailingService = mailingService;
            Navigator = navigator;
            User = null;
        }

        public EmailUser? User { get; private set; }
        public INavigator Navigator { get; }

        public NavigationElement LoginElement => new NavigationElement("Login", n =>
        {
            var viewModel = new EmailPasswordEnteringViewModel("Login", ProceedLogin);
            n.PushView(new EmailPasswordEnteringView(viewModel));
        });

        public NavigationElement RegisterElement => new NavigationElement("Register", n =>
        {
            var viewModel = new EmailPasswordEnteringViewModel("Register", ProceedRegister);
            n.PushView(new EmailPasswordEnteringView(viewModel));
        });

        public NavigationElement LogoutElement => new NavigationElement("Logout", _ => User = null);

        public NavigationElement GetMailboxElement(EmailUser user)
        {
            return new NavigationElement($"Mailbox ({_mailingService.NotViewedEmailCountForUser(user)} new)", n =>
            {
                var viewModel = new MailboxViewModel(_mailingService, user, Navigator);
                n.PushView(new MailboxView(viewModel));
            });
        }

        private void ProceedLogin(EmailPasswordEnteringViewModel viewModel)
        {
            var email = new EmailAddress(viewModel.Email.ThrowIfNull(nameof(viewModel.Email)));
            string password = viewModel.Password.ThrowIfNull(nameof(viewModel.Password));

            Navigator.PopView();

            User = _mailingService.Login(email, password);
        }

        private void ProceedRegister(EmailPasswordEnteringViewModel viewModel)
        {
            var email = new EmailAddress(viewModel.Email.ThrowIfNull(nameof(viewModel.Email)));
            string password = viewModel.Password.ThrowIfNull(nameof(viewModel.Password));

            Navigator.PopView();

            User = _mailingService.Register(email, password);
        }
    }
}