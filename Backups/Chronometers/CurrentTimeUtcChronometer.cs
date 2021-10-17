using System;

namespace Backups.Chronometers
{
    public class CurrentTimeUtcChronometer : IChronometer
    {
        public DateTime GetCurrentTime()
            => DateTime.UtcNow;
    }
}