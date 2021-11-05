using System;

namespace Banks.Chronometers
{
    public interface IChronometer
    {
        DateTime CurrentDateTime { get; }
    }
}