using Spectre.Mvvm.Views;

namespace Spectre.Mvvm.Interfaces
{
    public interface INavigator
    {
        void PushView(View view);
        void PopView();
    }
}