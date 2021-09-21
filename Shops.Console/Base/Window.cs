using System.Threading;
using Shops.Console.Base.Views;
using Spectre.Console;

namespace Shops.Console.Base
{
    public class Window
    {
        private readonly View _rootView;

        public Window(View rootView)
        {
            _rootView = rootView;
        }

        public bool Running { get; set; } = true;

        public void Run(int fps)
        {
            int delay = 1000 / fps;
            while (Running)
            {
                AnsiConsole.Clear();
                _rootView.Draw();

                Thread.Sleep(delay);
            }
        }
    }
}