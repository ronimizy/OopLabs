using System.Collections.Generic;
using Banks.Console.Views.Banking.RegisterAccountPlans;
using Banks.Models;
using Spectre.Mvvm.Components;
using Spectre.Mvvm.Interfaces;
using Spectre.Mvvm.Models;

namespace Banks.Console.ViewModels.Banking.RegisterAccountPlans
{
    public class RegisterDepositAccountPlanViewModel : EnteringViewModel<RegisterDepositAccountPlanViewModel>
    {
        private readonly INavigator _navigator;
        private readonly List<DepositPercentLevel> _levels = new List<DepositPercentLevel>();

        public RegisterDepositAccountPlanViewModel(INavigator navigator, EnteringCompletedHandler? defaultHandler = null)
            : base(defaultHandler)
        {
            _navigator = navigator;
        }

        public IReadOnlyCollection<DepositPercentLevel> Levels => _levels;

        public NavigationComponent GetNavigationComponent()
        {
            var addLevelViewModel = new EnterDepositPercentLevelViewModel(AddLevel);

            NavigationElement[] elements =
            {
                new NavigationElement("Add Level", n => n.PushView(new EnterDepositPercentLevelView(addLevelViewModel))),
                new NavigationElement("Register", _ => OnEnteringCompleted(this)),
            };

            return new NavigationComponent(_navigator, elements)
            {
                NeedBackButton = false,
            };
        }

        private void AddLevel(EnterDepositPercentLevelViewModel viewModel)
        {
            _levels.Add(new DepositPercentLevel(viewModel.Amount, viewModel.Percent));
            _navigator.PopView();
        }
    }
}