using System;
using System.IO;

namespace Models.Loggers
{
    public class FileLogger : ILogger
    {
        private string Path { get; set; }

        public FileLogger(string path)
        {
            Path = path;
        }

        void ILogger.Log(Exception exception, DateTime? dateTime)
        {
            if (!dateTime.HasValue)
            {
                dateTime = DateTime.Now;
            }
            File.AppendAllText(Path, $"{dateTime.Value}: {exception.Message}{Environment.NewLine}");
        }

        void ILogger.Log(string message, DateTime? dateTime)
        {
            if (!dateTime.HasValue)
            {
                dateTime = DateTime.Now;
            }
            File.AppendAllText(Path, $"{dateTime.Value}: {message}{Environment.NewLine}");
        }
    }
}