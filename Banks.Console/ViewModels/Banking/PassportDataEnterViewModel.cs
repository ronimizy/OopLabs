using Banks.Models;
using Spectre.Mvvm.Interfaces;
using Utility.Extensions;

namespace Banks.Console.ViewModels.Banking
{
    public class PassportDataEnterViewModel
    {
        private readonly RegisterClientViewModel _registerClientViewModel;
        private readonly INavigator _navigator;
        private string? _serial;
        private string? _number;

        public PassportDataEnterViewModel(RegisterClientViewModel registerClientViewModel, INavigator navigator)
        {
            _registerClientViewModel = registerClientViewModel;
            _navigator = navigator;
        }

        public void SerialSubmitted(string serial)
            => _serial = serial.ThrowIfNull(nameof(serial));

        public void NumberSubmitter(string number)
            => _number = number.ThrowIfNull(nameof(number));

        public void OnSubmit()
        {
            var data = new PassportData(_serial.ThrowIfNull(nameof(_serial)), _number.ThrowIfNull(nameof(_number)));
            _registerClientViewModel.PassportDataSubmitted(data);
            _navigator.PopView();
        }
    }
}