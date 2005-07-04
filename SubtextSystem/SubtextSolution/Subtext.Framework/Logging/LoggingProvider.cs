using System;

namespace Subtext.Framework.Logging
{
	/// <summary>
	/// Summary description for LoggingProvider.
	/// </summary>
	public class LoggingProvider
	{
		private static readonly LoggingProvider __instance = new LoggingProvider();

		public LogEntryCollection GetLogEntries(int pageIndex, int pageSize)
		{
			LogEntryCollection logEntries = new LogEntryCollection();
			LogEntry testEntry = new LogEntry();
			testEntry.Date = DateTime.Now;
			testEntry.Level = "INFO";
			testEntry.Logger = "Subtext.Framework.Logging.SomeLogger";
			testEntry.Message = "Dummy test message. Nothing else to see here.";
			testEntry.Thread = "1234";
			logEntries.Add(testEntry);
			return logEntries;
		}

		public static LoggingProvider Instance
		{
			get
			{
				return __instance;
			}
		}
	}
}
