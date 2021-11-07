using System;
using Banks.Chronometers;

namespace Banks.Console.Tools
{
    public class SettableChronometer : IChronometer
    {
        public DateTime CurrentDateTime { get; set; }
    }
}