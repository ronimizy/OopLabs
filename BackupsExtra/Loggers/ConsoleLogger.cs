using System;
using BackupsExtra.Models;

namespace BackupsExtra.Loggers
{
    public class ConsoleLogger : ConfigurableLogger
    {
        public ConsoleLogger(LoggerConfiguration? configuration = null)
            : base(configuration) { }

        public override void OnMessage(string message, string comment = "")
        {
            string configuredMessage = FormString(message, comment);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(configuredMessage);
            Console.ResetColor();
        }

        public override void OnComment(string message)
        {
            string configuredMessage = FormString(message);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(configuredMessage);
            Console.ResetColor();
        }

        public override void OnException(Exception exception, string comment = "")
        {
            string configuredMessage = FormString(exception.Message, comment);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(configuredMessage);
            Console.ResetColor();
        }
    }
}