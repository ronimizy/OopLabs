using Spectre.Console;

namespace Shops.Console.Base.Delegates
{
    public interface IInputFieldDelegate<T>
    {
        ValidationResult Validate(T value);
        void ProcessInput(T value);
        bool IsOptional();
    }
}