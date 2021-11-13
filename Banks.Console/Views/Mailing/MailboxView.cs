using System.Collections.Generic;
using System.Linq;
using Banks.Console.ViewModels.Mailing;
using Banks.Models;
using Spectre.Console;
using Spectre.Mvvm.Components;
using Spectre.Mvvm.Models;
using Spectre.Mvvm.Views;

namespace Banks.Console.Views.Mailing
{
    public class MailboxView : View
    {
        private readonly MailboxViewModel _viewModel;

        public MailboxView(MailboxViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override string Title => "Mailbox";

        protected override IReadOnlyCollection<Component> GetComponents()
        {
            NavigationElement[] elements = _viewModel.Previews.Select(CreateNavigationElement).ToArray();
            return new[] { new NavigationComponent(_viewModel.Navigator, elements) };
        }

        private static string FormatPreviewTitle(EmailPreview preview)
            => $"{(preview.Viewed ? string.Empty : "(*)")} {preview.Sender} - {preview.Title}".EscapeMarkup();

        private NavigationElement CreateNavigationElement(EmailPreview preview)
            => new NavigationElement(
                FormatPreviewTitle(preview),
                n => n.PushView(new EmailView(_viewModel.GetEmailViewModel(preview))));
    }
}