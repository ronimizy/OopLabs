using System.Threading;
using Shops.Console.Base.ViewControllers;
using Spectre.Console;

namespace Shops.Console.Base
{
    public class Window
    {
        private readonly ViewController _rootViewController;
        private bool _needsRedrawing = true;

        public Window(ViewController rootViewController)
        {
            _rootViewController = rootViewController;
            _rootViewController.Redraw += Redraw;
        }

        public bool Running { get; set; } = true;

        public void Run(int fps)
        {
            int delay = 1000 / fps;
            while (Running)
            {
                if (_needsRedrawing)
                {
                    _needsRedrawing = false;
                    AnsiConsole.Clear();
                    _rootViewController.DrawContent();
                }

                Thread.Sleep(delay);
            }
        }

        private void Redraw(ViewController target)
        {
            _needsRedrawing = true;
        }
    }
}