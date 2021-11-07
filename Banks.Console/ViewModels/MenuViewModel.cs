using Banks.Console.Tools;
using Banks.Console.ViewModels.Banking;
using Banks.Console.ViewModels.Mailing;
using Banks.Console.Views.Banking;
using Banks.Console.Views.Mailing;
using Banks.Entities;
using Banks.Services;
using Spectre.Mvvm.Components;
using Spectre.Mvvm.Interfaces;
using Spectre.Mvvm.Views;

namespace Banks.Console.ViewModels
{
    public class MenuViewModel
    {
        private readonly INavigator _navigator;
        private readonly View[] _navigationLinks;

        public MenuViewModel(CentralBank centralBank, MailingService mailingService, INavigator navigator, SettableChronometer chronometer)
        {
            _navigator = navigator;
            _navigationLinks = new View[]
            {
                new MailingServiceView(new MailingServiceViewModel(mailingService, navigator)),
                new CentralBankView(new CentralBankViewModel(centralBank, navigator, chronometer)),
            };
        }

        public Component NavigationComponent => new NavigationComponent(_navigator, false, _navigationLinks);
    }
}