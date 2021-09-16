using System.Threading;
using Shops.Console.Base.ViewControllers;
using Spectre.Console;

namespace Shops.Console.Base
{
    public class Window
    {
        private readonly Controller _rootController;

        public Window(Controller rootController)
        {
            _rootController = rootController;
        }

        public bool Running { get; set; } = true;

        public void Run(int fps)
        {
            int delay = 1000 / fps;
            while (Running)
            {
                AnsiConsole.Clear();
                _rootController.View?.Render();

                Thread.Sleep(delay);
            }
        }
    }
}