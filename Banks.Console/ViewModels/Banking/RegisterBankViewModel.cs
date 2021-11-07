using Utility.Extensions;

namespace Banks.Console.ViewModels.Banking
{
    public class RegisterBankViewModel : EnteringViewModel<RegisterBankViewModel>
    {
        private string? _name;
        private decimal? _limit;

        public RegisterBankViewModel(EnteringCompletedHandler? defaultHandler = null)
            : base(defaultHandler) { }

        public string Name => _name.ThrowIfNull(nameof(_name));

        public decimal Limit => _limit.ThrowIfNull(nameof(_limit));

        public void NameSubmitted(string name)
            => _name = name.ThrowIfNull(nameof(name));

        public void LimitSubmitted(decimal limit)
            => _limit = limit.ThrowIfNull(nameof(limit));

        public void OperationSubmitted()
            => OnEnteringCompleted(this);
    }
}