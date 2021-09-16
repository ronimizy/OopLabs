using System;
using Shops.Console.Base.Delegates;
using Spectre.Console;

namespace Shops.Console.Delegates
{
    public class StrategyInputFieldDelegate<T> : IInputFieldDelegate<T>
    {
        private readonly Action<T> _processor;
        private readonly bool _isOptional;
        private readonly Func<T, bool>? _validator;

        public StrategyInputFieldDelegate(Action<T> processor, bool isOptional = false, Func<T, bool>? validator = null)
        {
            _processor = processor;
            _isOptional = isOptional;
            _validator = validator;
        }

        public ValidationResult Validate(T value)
            => _validator?.Invoke(value) ?? true
                   ? ValidationResult.Success()
                   : ValidationResult.Error();

        public void ProcessInput(T value)
            => _processor(value);

        public bool IsOptional()
            => _isOptional;
    }
}