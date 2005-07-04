using System;

namespace Subtext.Framework.Logging
{
	/// <summary>
	/// Contains log entry information. This class used only for retrieving and viewing logging information.
	/// </summary>
	public class LogEntry
	{
		/// <summary>
		/// Date of the entry
		/// </summary>
		private DateTime _date;
		public DateTime Date
		{
			get
			{
				return _date;
			}
			set
			{
				_date = value;
			}
		}

		/// <summary>
		/// ID of the thread which logged the entry
		/// </summary>
		private string _thread;
		public string Thread
		{
			get
			{
				return _thread;
			}
			set
			{
				_thread = value;
			}
		}

		/// <summary>
		/// Full name of the type (class) that logged the entry
		/// </summary>
		private string _logger;
		public string Logger
		{
			get
			{
				return _logger;
			}
			set
			{
				_logger = value;
			}
		}

		/// <summary>
		/// Level of the entry
		/// </summary>
		private string _level;
		public string Level
		{
			get
			{
				return _level;
			}
			set
			{
				_level = value;
			}
		}

		/// <summary>
		/// Message of the entry
		/// </summary>
		private string _message;
		public string Message
		{
			get
			{
				return _message;
			}
			set
			{
				_message = value;
			}
		}

		/// <summary>
		/// Exception (if any) that was the cause of generating the entry
		/// </summary>
		private string _exception;
		public string Exception
		{
			get
			{
				return _exception;
			}
			set
			{
				_exception = value;
			}
		}
	}
}
