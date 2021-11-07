using Utility.Extensions;

namespace Banks.Console.ViewModels.Banking.RegisterAccountPlans
{
    public class RegisterCreditAccountPlanViewModel : EnteringViewModel<RegisterCreditAccountPlanViewModel>
    {
        private decimal? _percent;
        private decimal? _limit;

        public RegisterCreditAccountPlanViewModel(EnteringCompletedHandler? defaultHandler = null)
            : base(defaultHandler) { }

        public decimal Percent => _percent.ThrowIfNull(nameof(_percent));
        public decimal Limit => _limit.ThrowIfNull(nameof(_limit));

        public void PercentSubmitted(decimal percent)
            => _percent = percent;

        public void LimitSubmitted(decimal limit)
            => _limit = limit;

        public void ButtonPressed()
            => OnEnteringCompleted(this);
    }
}