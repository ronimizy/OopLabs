using System.Collections.Generic;
using System.Linq;
using Banks.Console.ViewModels.Banking;
using Spectre.Mvvm.Components;
using Spectre.Mvvm.Views;

namespace Banks.Console.Views.Banking
{
    public class PassportDataEnterView : View
    {
        private readonly PassportDataEnterViewModel _viewModel;

        public PassportDataEnterView(PassportDataEnterViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override string Title => "Enter passport data";

        protected override IReadOnlyCollection<Component> GetComponents()
        {
            var serialInput = new InputComponent<string>("Serial: ", s => s.All(char.IsDigit));
            var numberInput = new InputComponent<string>("Number: ", s => s.All(char.IsDigit));
            var submitButton = new ButtonComponent("Submit", _viewModel.OnSubmit);

            serialInput.ValueSubmitted += _viewModel.SerialSubmitted;
            numberInput.ValueSubmitted += _viewModel.NumberSubmitter;

            return new Component[]
            {
                serialInput,
                numberInput,
                submitButton,
            };
        }
    }
}