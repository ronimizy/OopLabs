using Utility.Extensions;

namespace Banks.Console.ViewModels
{
    public class EmailPasswordEnteringViewModel : EnteringViewModel<EmailPasswordEnteringViewModel>
    {
        private string? _email;
        private string? _password;

        public EmailPasswordEnteringViewModel(string title, EnteringCompletedHandler? defaultHandler = null)
            : base(defaultHandler)
        {
            Title = title;
        }

        public string Title { get; }
        public string Email => _email.ThrowIfNull(nameof(_email));
        public string Password => _password.ThrowIfNull(nameof(_password));

        public void SubmitEmail(string email)
            => _email = email.ThrowIfNull(nameof(email));

        public void SubmitPassword(string password)
            => _password = password.ThrowIfNull(nameof(password));

        public void ButtonPressed()
            => OnEnteringCompleted(this);
    }
}