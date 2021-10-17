using System;

namespace Backups.Tools
{
    public interface ILogger
    {
        void OnMessage(string message, string comment = "");
        void OnComment(string message);
        void OnException(Exception exception, string comment = "");
    }
}