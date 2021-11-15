using System;
using System.IO;
using BackupsExtra.Models;
using Utility.Extensions;

namespace BackupsExtra.Loggers
{
    public class FileLogger : ConfigurableLogger
    {
        private readonly string _filePath;

        public FileLogger(string filePath, LoggerConfiguration? configuration = null)
            : base(configuration)
        {
            _filePath = filePath.ThrowIfNull(nameof(filePath));
        }

        public override void OnMessage(string message, string comment = "")
            => Write(FormString(message, comment));

        public override void OnComment(string message)
            => Write(FormString(message));

        public override void OnException(Exception exception, string comment = "")
            => Write(FormString(exception.Message, comment));

        private void Write(string message)
        {
            using var stream = new FileStream(_filePath, FileMode.Append, FileAccess.Write);
            using var writer = new StreamWriter(stream);
            writer.WriteLine(message);
        }
    }
}