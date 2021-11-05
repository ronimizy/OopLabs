using System;
using Banks.Chronometers;

namespace Banks.Tests.Mocks
{
    public class ChronometerMock : IChronometer
    {
        public DateTime CurrentDateTime { get; set; }
    }
}