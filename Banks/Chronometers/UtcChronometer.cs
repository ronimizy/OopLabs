using System;

namespace Banks.Chronometers
{
    public class UtcChronometer : IChronometer
    {
        public DateTime CurrentDateTime => DateTime.UtcNow;
    }
}