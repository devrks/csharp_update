using System;

namespace Models.Loggers
{
    public class ConsoleLogger : ILogger
    {
        public void Log(Exception exception, DateTime? dateTime = null)
        {
            if (!dateTime.HasValue)
            {
                dateTime = DateTime.Now;
            }
            Console.WriteLine($"{dateTime.Value}: {exception.Message}");
        }

        public void Log(string message, DateTime? dateTime = null)
        {
            if (!dateTime.HasValue)
            {
                dateTime = DateTime.Now;
            }
            Console.WriteLine($"{dateTime.Value}: {message}");
        }
    }
}