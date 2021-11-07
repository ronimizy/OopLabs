using System.Collections.Generic;
using Banks.Console.ViewModels.Banking;
using Spectre.Mvvm.Components;
using Spectre.Mvvm.Views;

namespace Banks.Console.Views.Banking
{
    public class EnrollDepositAccountView : View
    {
        private readonly EnrollDepositAccountViewModel _viewModel;

        public EnrollDepositAccountView(EnrollDepositAccountViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override string Title => "Enroll Deposit Account";

        protected override IReadOnlyCollection<Component> GetComponents()
        {
            var yearComponent = new InputComponent<int>("Unlock year: ", i => i >= _viewModel.CurrentYear);
            var monthComponent = new InputComponent<int>("Unlock month: ", i => i is >= 1 and <= 12);
            var dayComponent = new InputComponent<int>("Unlock Day: ", i => i is >= 1 and <= 31);
            var depositComponent = new InputComponent<decimal>("Deposit: ", d => d >= 0);
            var button = new ButtonComponent("Enroll", _viewModel.ButtonPressed);

            yearComponent.ValueSubmitted += _viewModel.UnlockYearSubmitted;
            monthComponent.ValueSubmitted += _viewModel.UnlockMonthSubmitted;
            dayComponent.ValueSubmitted += _viewModel.UnlockDaySubmitted;
            depositComponent.ValueSubmitted += _viewModel.DepositSubmitted;

            return new Component[]
            {
                yearComponent,
                monthComponent,
                dayComponent,
                depositComponent,
                button,
            };
        }
    }
}