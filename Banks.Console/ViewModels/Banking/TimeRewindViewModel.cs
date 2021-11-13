using System;
using Banks.Console.Tools;
using Banks.Console.Views;
using Spectre.Mvvm.Interfaces;
using Spectre.Mvvm.Models;

namespace Banks.Console.ViewModels.Banking
{
    public class TimeRewindViewModel
    {
        private readonly SettableChronometer _chronometer;

        public TimeRewindViewModel(SettableChronometer chronometer, INavigator navigator)
        {
            _chronometer = chronometer;
            Navigator = navigator;
        }

        public INavigator Navigator { get; }
        public DateTime CurrentDateTime => _chronometer.CurrentDateTime;

        public NavigationElement IncreaseYearElement => new NavigationElement("Increase Year", n =>
        {
            n.PushView(new PromptView<int>("Value: ", validator: i => i > 0, callback: i =>
            {
                DateTime time = _chronometer.CurrentDateTime;
                _chronometer.CurrentDateTime = new DateTime(time.Year + i, time.Month, time.Day);
                n.PopView();
            }));
        });

        public NavigationElement IncreaseMonthElement => new NavigationElement("Increase Month", n =>
        {
            n.PushView(new PromptView<int>("Value: ", validator: i => i > 0, callback: i =>
            {
                DateTime time = _chronometer.CurrentDateTime;
                _chronometer.CurrentDateTime = new DateTime(time.Year, time.Month + i, time.Day);
                n.PopView();
            }));
        });

        public NavigationElement IncreaseDayElement => new NavigationElement("Increase Day", n =>
        {
            n.PushView(new PromptView<int>("Value: ", validator: i => i > 0, callback: i =>
            {
                DateTime time = _chronometer.CurrentDateTime;
                _chronometer.CurrentDateTime = new DateTime(time.Year, time.Month, time.Day + i);
                n.PopView();
            }));
        });
    }
}