using Spectre.Console;

namespace Spectre.Mvvm.Components
{
    public class MarkupComponent : Component
    {
        private readonly Markup _value;

        public MarkupComponent(Markup value)
        {
            _value = value;
        }

        public override void Draw()
            => AnsiConsole.Write(_value);
    }
}