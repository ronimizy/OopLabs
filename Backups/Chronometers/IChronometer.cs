using System;

namespace Backups.Chronometers
{
    public interface IChronometer
    {
        DateTime GetCurrentTime();
    }
}