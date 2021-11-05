using System.Threading;
using Spectre.Console;
using Spectre.Mvvm.Views;

namespace Spectre.Mvvm
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