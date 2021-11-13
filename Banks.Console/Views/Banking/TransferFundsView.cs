using System;
using System.Collections.Generic;
using Banks.Console.ViewModels.Banking;
using Spectre.Mvvm.Components;
using Spectre.Mvvm.Views;

namespace Banks.Console.Views.Banking
{
    public class TransferFundsView : View
    {
        private readonly TransferFundsViewModel _viewModel;

        public TransferFundsView(TransferFundsViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override string Title => "Transfer Funds";

        protected override IReadOnlyCollection<Component> GetComponents()
        {
            var amountComponent = new InputComponent<decimal>("Amount: ", d => d >= 0, defaultHandler: _viewModel.SubmitAmount);
            var idComponent = new InputComponent<Guid>("Receiver Id: ", defaultHandler: _viewModel.SubmitReceiverId);
            var button = new ButtonComponent("Transfer", _viewModel.ButtonSubmitted);

            return new Component[]
            {
                amountComponent,
                idComponent,
                button,
            };
        }
    }
}