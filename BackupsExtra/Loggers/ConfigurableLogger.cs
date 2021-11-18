using System;
using System.Text;
using Backups.Tools;
using BackupsExtra.Models;

namespace BackupsExtra.Loggers
{
    public abstract class ConfigurableLogger : ILogger
    {
        private readonly LoggerConfiguration _configuration;

        protected ConfigurableLogger(LoggerConfiguration? configuration = null)
        {
            _configuration = configuration ?? new LoggerConfiguration();
        }

        public abstract void OnMessage(string message, string comment = "");
        public abstract void OnComment(string message);
        public abstract void OnException(Exception exception, string comment = "");

        protected string FormString(params string[] strings)
        {
            var builder = new StringBuilder();

            if (_configuration.Chronometer is not null)
            {
                builder.Append($"[{BackupConfiguration.FormatDateTime(_configuration.Chronometer.GetCurrentTime())}] ");
            }

            builder.AppendJoin(' ', strings);

            return builder.ToString();
        }
    }
}