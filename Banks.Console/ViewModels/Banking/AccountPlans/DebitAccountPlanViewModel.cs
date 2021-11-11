using Banks.Console.Views;
using Banks.Entities;
using Banks.Models;
using Banks.Plans;
using Spectre.Mvvm.Interfaces;
using Spectre.Mvvm.Models;

namespace Banks.Console.ViewModels.Banking.AccountPlans
{
    public class DebitAccountPlanViewModel
    {
        private readonly Client _client;
        private readonly IBank _bank;
        private readonly DebitAccountPlan _plan;

        public DebitAccountPlanViewModel(Client client, IBank bank, DebitAccountPlan plan, INavigator navigator)
        {
            _client = client;
            _bank = bank;
            _plan = plan;
            Navigator = navigator;
        }

        public Info PlanInfo => _plan.Info;
        public INavigator Navigator { get; }

        public NavigationElement UpdatePercentageElement => new NavigationElement("Update Percentage", n =>
        {
            n.PushView(new PromptView<decimal>("Percentage: ", validator: d => d >= 0, callback: d =>
            {
                _bank.UpdateDebitAccountPlanPercentage(_client, _plan, d);
                n.PopView();
            }));
        });
    }
}