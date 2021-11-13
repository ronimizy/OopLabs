using System;
using Utility.Extensions;

namespace Banks.Console.ViewModels.Banking
{
    public class TransferFundsViewModel : EnteringViewModel<TransferFundsViewModel>
    {
        private decimal? _amount;
        private Guid? _receiverId;

        public TransferFundsViewModel(EnteringCompletedHandler? defaultHandler = null)
            : base(defaultHandler) { }

        public decimal Amount => _amount.ThrowIfNull(nameof(_amount));
        public Guid ReceiverId => _receiverId.ThrowIfNull(nameof(_receiverId));

        public void SubmitAmount(decimal amount)
            => _amount = amount;

        public void SubmitReceiverId(Guid receiverId)
            => _receiverId = receiverId;

        public void ButtonSubmitted()
            => OnEnteringCompleted(this);
    }
}