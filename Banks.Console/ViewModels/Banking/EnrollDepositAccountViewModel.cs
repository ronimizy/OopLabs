using Banks.Chronometers;
using Banks.Plans;
using Utility.Extensions;

namespace Banks.Console.ViewModels.Banking
{
    public class EnrollDepositAccountViewModel : EnteringViewModel<EnrollDepositAccountViewModel>
    {
        private readonly IChronometer _chronometer;
        private int? _unlockYear;
        private int? _unlockMonth;
        private int? _unlockDay;
        private decimal? _deposit;

        public EnrollDepositAccountViewModel(IChronometer chronometer, DepositAccountPlan plan)
        {
            _chronometer = chronometer;
            Plan = plan;
        }

        public DepositAccountPlan Plan { get; }
        public int CurrentYear => _chronometer.CurrentDateTime.Year;

        public int UnlockYear => _unlockYear.ThrowIfNull(nameof(_unlockYear));
        public int UnlockMonth => _unlockMonth.ThrowIfNull(nameof(_unlockMonth));
        public int UnlockDay => _unlockDay.ThrowIfNull(nameof(_unlockDay));
        public decimal Deposit => _deposit.ThrowIfNull(nameof(_deposit));

        public void UnlockYearSubmitted(int year)
            => _unlockYear = year;

        public void UnlockMonthSubmitted(int month)
            => _unlockMonth = month;

        public void UnlockDaySubmitted(int day)
            => _unlockDay = day;

        public void DepositSubmitted(decimal deposit)
            => _deposit = deposit;

        public void ButtonPressed()
            => OnEnteringCompleted(this);
    }
}