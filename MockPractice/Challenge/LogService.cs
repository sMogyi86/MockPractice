using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		private List<ILogger> Loggers;

		public LogService()
		{
			Loggers = new List<ILogger>();
		}

		public void Log(string message, LogLevel logLevel)
		{
			foreach (var logger in Loggers.Where(l => l.SupportsLogLevel(logLevel)))
			{
				logger.Log($"{DateTime.Now}: {message}", logLevel);
			}
		}

		public void RegisterLogger(ILogger logger)
		{
			Loggers.Add(logger);
		}
	}
}
