#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;

namespace Subtext.Framework.Logging
{
	/// <summary>
	/// Represents an entry within the Subtext log.
	/// </summary>
	/// <remarks>
	/// These entries as of now are added by Log4Net.
	/// </remarks>
	[Serializable]
	public class LogEntry
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LogEntry"/> class.
		/// </summary>
		public LogEntry()
		{
		}

		/// <summary>
		/// Gets or sets the id.
		/// </summary>
		/// <value>The id.</value>
		public int Id
		{
			get { return _id; }
			set { _id = value; }
		}

		int _id;

		/// <summary>
		/// Gets or sets the blog id.
		/// </summary>
		/// <value>The blog id.</value>
		public int BlogId
		{
			get { return _blogId; }
			set { _blogId = value; }
		}

		int _blogId = NullValue.NullInt32;

		/// <summary>
		/// Gets or sets the date of the log entry.
		/// </summary>
		/// <value>The date.</value>
		public DateTime Date
		{
			get { return _date; }
			set { _date = value; }
		}

		DateTime _date;

		/// <summary>
		/// Gets or sets the id or name of thread on which 
		/// this log message was logged.
		/// </summary>
		/// <value>The thread.</value>
		public string Thread
		{
			get { return _thread; }
			set { _thread = value; }
		}

		string _thread;

		/// <summary>
		/// Gets or sets the context of the message if any was set.
		/// </summary>
		/// <value>The context.</value>
		public string Context
		{
			get { return _context; }
			set { _context = value; }
		}

		string _context;

		/// <summary>
		/// Gets or sets the level of the log message.
		/// </summary>
		/// <value>The level.</value>
		public string Level
		{
			get { return _level; }
			set { _level = value; }
		}

		string _level;

		/// <summary>
		/// Gets or sets the logger that logged this message.
		/// </summary>
		/// <value>The logger.</value>
		public string Logger
		{
			get { return _logger; }
			set { _logger = value; }
		}

		string _logger;

		/// <summary>
		/// Gets or sets the log message.
		/// </summary>
		/// <value>The message.</value>
		public string Message
		{
			get { return _message; }
			set { _message = value; }
		}

		string _message;

		/// <summary>
		/// Gets or sets the exception type and stack trace 
		/// if an exception was logged.
		/// </summary>
		/// <value>The exception.</value>
		public string Exception
		{
			get { return _exception; }
			set { _exception = value; }
		}

		string _exception;

		/// <summary>
		/// Gets or sets the Url that of the request that caused this 
		/// exception.
		/// </summary>
		/// <value>The URL.</value>
		public Uri Url
		{
			get
			{
				return this.url;
			}
			set
			{
				this.url = value;
			}
		}
		Uri url;
	}
}