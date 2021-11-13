using System.Collections.Generic;
using Banks.Console.ViewModels.Banking;
using Banks.Console.Views.Banking.Accounts;
using Banks.Console.Views.Banking.RegisterAccountPlans;
using Banks.Plans;
using Spectre.Mvvm.Components;
using Spectre.Mvvm.Views;
using Utility.Extensions;

namespace Banks.Console.Views.Banking
{
    public class BankView : View
    {
        private readonly BankViewModel _viewModel;

        public BankView(BankViewModel viewModel)
        {
            _viewModel = viewModel.ThrowIfNull(nameof(viewModel));
        }

        public override string Title => _viewModel.BankName;

        protected override IReadOnlyCollection<Component> GetComponents()
        {
            var views = new List<View>
            {
                new AccountsView(_viewModel.AccountsViewModel),
                new SelectorView<DebitAccountPlan>(_viewModel.EnrollDebitAccountSelectorViewModel),
                new SelectorView<DepositAccountPlan>(_viewModel.EnrollDepositAccountSelectorViewModel),
                new SelectorView<CreditAccountPlan>(_viewModel.EnrollCreditAccountSelectorViewModel),
            };

            if (_viewModel.ClientIsOwner)
            {
                views.AddRange(new View[]
                {
                    new RegisterDebitAccountPlanView(_viewModel.RegisterDebitAccountPlanViewModel),
                    new RegisterDepositAccountPlanView(_viewModel.RegisterDepositAccountPlanViewModel),
                    new RegisterCreditAccountPlanView(_viewModel.RegisterCreditAccountPlanViewModel),
                    new SelectorView<DebitAccountPlan>(_viewModel.EditDebitAccountPlanViewModel),
                    new SelectorView<DepositAccountPlan>(_viewModel.EditDepositAccountPlanViewModel),
                    new SelectorView<CreditAccountPlan>(_viewModel.EditCreditAccountPlanViewModel),
                });
            }

            return new[] { new NavigationComponent(_viewModel.Navigator, true, views.ToArray()) };
        }
    }
}