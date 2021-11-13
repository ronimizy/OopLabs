namespace Banks.Console.ViewModels
{
    public abstract class EnteringViewModel<TViewModel>
    {
        protected EnteringViewModel(EnteringCompletedHandler? defaultHandler = null)
        {
            EnteringCompleted += defaultHandler;
        }

        public delegate void EnteringCompletedHandler(TViewModel viewModel);

        public event EnteringCompletedHandler? EnteringCompleted;

        protected virtual void OnEnteringCompleted(TViewModel viewmodel)
        {
            EnteringCompleted?.Invoke(viewmodel);
        }
    }
}