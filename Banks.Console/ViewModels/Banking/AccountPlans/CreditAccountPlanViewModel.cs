using Banks.Console.Views;
using Banks.Entities;
using Banks.Models;
using Banks.Plans;
using Spectre.Mvvm.Interfaces;
using Spectre.Mvvm.Models;

namespace Banks.Console.ViewModels.Banking.AccountPlans
{
    public class CreditAccountPlanViewModel
    {
        private readonly Client _client;
        private readonly Bank _bank;
        private readonly CreditAccountPlan _plan;

        public CreditAccountPlanViewModel(Client client, Bank bank, CreditAccountPlan plan, INavigator navigator)
        {
            _client = client;
            _bank = bank;
            _plan = plan;
            Navigator = navigator;
        }

        public Info Info => _plan.Info;
        public INavigator Navigator { get; }

        public NavigationElement UpdatePercentageElement => new NavigationElement("Update Percentage", n =>
        {
            n.PushView(new PromptView<decimal>("Percentage: ", validator: d => d > 0, callback: d =>
            {
                _bank.UpdateCreditAccountPlanPercentage(_client, _plan, d);
                n.PopView();
            }));
        });

        public NavigationElement UpdateLimitElement => new NavigationElement("Update Limit", n =>
        {
            n.PushView(new PromptView<decimal>("Percentage: ", callback: d =>
            {
                _bank.UpdateCreditAccountPlanLimit(_client, _plan, d);
                n.PopView();
            }));
        });
    }
}