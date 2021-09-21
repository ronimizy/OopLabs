using Spectre.Console;

namespace Shops.Console.Base.Components
{
    public class MarkupComponent : Component
    {
        private readonly Markup _value;

        public MarkupComponent(Markup value)
        {
            _value = value;
        }

        public override void Draw()
            => AnsiConsole.Render(_value);
    }
}