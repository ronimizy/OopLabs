using Utility.Extensions;

namespace Banks.Console.ViewModels.Banking.RegisterAccountPlans
{
    public class EnterDepositPercentLevelViewModel : EnteringViewModel<EnterDepositPercentLevelViewModel>
    {
        private decimal? _amount;
        private decimal? _percent;

        public EnterDepositPercentLevelViewModel(EnteringCompletedHandler? defaultHandler = null)
            : base(defaultHandler) { }

        public decimal Amount => _amount.ThrowIfNull(nameof(_amount));

        public decimal Percent => _percent.ThrowIfNull(nameof(_percent));

        public void AmountSubmitted(decimal amount)
            => _amount = amount;

        public void PercentSubmitted(decimal percent)
            => _percent = percent;

        public void LevelSubmitted()
            => OnEnteringCompleted(this);
    }
}