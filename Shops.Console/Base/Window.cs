using System.Threading;
using Shops.Console.Base.Presenters;
using Spectre.Console;

namespace Shops.Console.Base
{
    public class Window
    {
        private readonly Presenter _rootPresenter;

        public Window(Presenter rootPresenter)
        {
            _rootPresenter = rootPresenter;
        }

        public bool Running { get; set; } = true;

        public void Run(int fps)
        {
            int delay = 1000 / fps;
            while (Running)
            {
                AnsiConsole.Clear();
                _rootPresenter.View?.Render();

                Thread.Sleep(delay);
            }
        }
    }
}