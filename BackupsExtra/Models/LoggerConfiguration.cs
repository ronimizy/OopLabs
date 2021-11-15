using Backups.Chronometers;

namespace BackupsExtra.Models
{
    public class LoggerConfiguration
    {
        public LoggerConfiguration(IChronometer? chronometer = null)
        {
            Chronometer = chronometer;
        }

        public IChronometer? Chronometer { get; }
    }
}