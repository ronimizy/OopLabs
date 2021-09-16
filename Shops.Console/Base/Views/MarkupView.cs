using Spectre.Console;

namespace Shops.Console.Base.Views
{
    public class MarkupView : View
    {
        private readonly Markup _markup;

        public MarkupView(Markup markup)
        {
            _markup = markup;
        }

        protected override void RenderBody()
            => AnsiConsole.Render(_markup);
    }
}