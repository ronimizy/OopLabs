using System.Linq;
using Banks.Console.ViewModels.Banking.RegisterAccountPlans;
using Banks.Console.Views.Banking.Accounts;
using Banks.Console.Views.Banking.RegisterAccountPlans;
using Banks.Entities;
using Banks.Models;
using Banks.Plans;
using Spectre.Mvvm.Interfaces;
using Spectre.Mvvm.Models;

namespace Banks.Console.ViewModels.Banking.AccountPlans
{
    public class DepositAccountPlanViewModel
    {
        private readonly Client _client;
        private readonly IBank _bank;
        private readonly DepositAccountPlan _plan;

        public DepositAccountPlanViewModel(Client client, IBank bank, DepositAccountPlan plan, INavigator navigator)
        {
            _client = client;
            _bank = bank;
            _plan = plan;
            Navigator = navigator;
        }

        public INavigator Navigator { get; }
        public Info Info => _plan.Info;

        public NavigationElement AddOrUpdateDepositPercentLevelElement => new NavigationElement("Add Or Update Deposit Percent Level", n =>
        {
            var viewModel = new EnterDepositPercentLevelViewModel(AddOrUpdateDepositPercentLevel);
            n.PushView(new EnterDepositPercentLevelView(viewModel));
        });

        public NavigationElement RemoveDepositAccountPlanLevelElement => new NavigationElement("Remove Deposit Percent Level", n =>
        {
            var viewModel = new SelectorViewModel<DepositPercentLevel>(
                Navigator,
                _plan.Levels.OrderBy(l => l.Amount).ToArray(),
                "Select level",
                l => new NavigationElement(l.ToString(), navigator =>
                {
                    _bank.RemoveDepositAccountPlanLevel(_client, _plan, l);
                    navigator.PopView();
                }));

            n.PushView(new SelectorView<DepositPercentLevel>(viewModel));
        });

        private void AddOrUpdateDepositPercentLevel(EnterDepositPercentLevelViewModel viewModel)
        {
            _bank.AddOrUpdateDepositAccountPlanLevel(_client, _plan, new DepositPercentLevel(viewModel.Amount, viewModel.Percent));
            Navigator.PopView();
        }
    }
}