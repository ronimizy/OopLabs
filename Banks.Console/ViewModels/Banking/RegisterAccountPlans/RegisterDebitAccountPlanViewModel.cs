using Utility.Extensions;

namespace Banks.Console.ViewModels.Banking.RegisterAccountPlans
{
    public class RegisterDebitAccountPlanViewModel : EnteringViewModel<RegisterDebitAccountPlanViewModel>
    {
        private decimal? _percentage;

        public RegisterDebitAccountPlanViewModel(EnteringCompletedHandler? defaultHandler = null)
            : base(defaultHandler) { }

        public decimal Percentage => _percentage.ThrowIfNull(nameof(_percentage));

        public void PercentageSubmitted(decimal value)
            => _percentage = value;

        public void RegistrationSubmitted()
            => OnEnteringCompleted(this);
    }
}