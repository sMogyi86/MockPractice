using System;
using System.Collections.Generic;
using System.Linq;

namespace MockPractice
{
    [Flags]
    public enum LogLevel
    {
        Info = 1,
        Error = 2,
        Debug = 4
    }

    public interface ILogger
    {
        void Log(string message, LogLevel logLevel);
        bool SupportsLogLevel(LogLevel logLevel);
    }

    public class LogService
    {
        private readonly List<ILogger> loggers = new List<ILogger>();
        private readonly INowProvider myNowProvider;

        public LogService(INowProvider nowProvider)
        {
            myNowProvider = nowProvider ?? throw new ArgumentNullException(nameof(nowProvider));
        }

        public void Log(string message, LogLevel logLevel)
        {
            foreach (var logger in loggers.Where(l => l.SupportsLogLevel(logLevel)))
            {
                logger.Log($"{myNowProvider.GetNow()}: {message}", logLevel);
            }
        }

        public void RegisterLogger(ILogger logger)
            => loggers.Add(logger ?? throw new ArgumentNullException(nameof(logger)));
    }
}