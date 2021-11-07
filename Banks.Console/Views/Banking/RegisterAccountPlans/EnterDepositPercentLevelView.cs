using System.Collections.Generic;
using Banks.Console.ViewModels.Banking.RegisterAccountPlans;
using Spectre.Mvvm.Components;
using Spectre.Mvvm.Views;

namespace Banks.Console.Views.Banking.RegisterAccountPlans
{
    public class EnterDepositPercentLevelView : View
    {
        private readonly EnterDepositPercentLevelViewModel _viewModel;

        public EnterDepositPercentLevelView(EnterDepositPercentLevelViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override string Title => "Add percent level";

        protected override IReadOnlyCollection<Component> GetComponents()
        {
            var amountInput = new InputComponent<decimal>("Amount: ", d => d >= 0);
            var percentInput = new InputComponent<decimal>("Percent: ", d => d > 0);
            var button = new ButtonComponent("Add", _viewModel.LevelSubmitted);

            amountInput.ValueSubmitted += _viewModel.AmountSubmitted;
            percentInput.ValueSubmitted += _viewModel.PercentSubmitted;

            return new Component[]
            {
                amountInput,
                percentInput,
                button,
            };
        }
    }
}