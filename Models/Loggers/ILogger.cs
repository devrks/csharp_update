using System;

namespace Models.Loggers
{
    public interface ILogger
    {
        void Log(Exception exception, DateTime? dateTime = null);

        void Log(string message, DateTime? dateTime = null);
    }
}