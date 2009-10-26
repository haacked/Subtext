#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
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
            BlogId = NullValue.NullInt32;
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the blog id.
        /// </summary>
        /// <value>The blog id.</value>
        public int BlogId { get; set; }

        /// <summary>
        /// Gets or sets the date of the log entry.
        /// </summary>
        /// <value>The date.</value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the id or name of thread on which 
        /// this log message was logged.
        /// </summary>
        /// <value>The thread.</value>
        public string Thread { get; set; }

        /// <summary>
        /// Gets or sets the context of the message if any was set.
        /// </summary>
        /// <value>The context.</value>
        public string Context { get; set; }

        /// <summary>
        /// Gets or sets the level of the log message.
        /// </summary>
        /// <value>The level.</value>
        public string Level { get; set; }

        /// <summary>
        /// Gets or sets the logger that logged this message.
        /// </summary>
        /// <value>The logger.</value>
        public string Logger { get; set; }

        /// <summary>
        /// Gets or sets the log message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the exception type and stack trace 
        /// if an exception was logged.
        /// </summary>
        /// <value>The exception.</value>
        public string Exception { get; set; }

        /// <summary>
        /// Gets or sets the Url that of the request that caused this 
        /// exception.
        /// </summary>
        /// <value>The URL.</value>
        public Uri Url { get; set; }
    }
}