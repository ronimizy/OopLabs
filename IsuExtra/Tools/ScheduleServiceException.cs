using System;

namespace IsuExtra.Tools
{
    public class ScheduleServiceException : Exception
    {
        public ScheduleServiceException() { }

        public ScheduleServiceException(string? message)
            : base(message) { }

        public ScheduleServiceException(string? message, Exception? innerException)
            : base(message, innerException) { }
    }
}