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

using System.Configuration.Provider;
using Subtext.Extensibility.Interfaces;

namespace Subtext.Framework.Logging
{
    /// <summary>
    /// Provider for retrieving log entries.
    /// </summary>
    public abstract class LoggingProvider : ProviderBase
    {
        /// <summary>
        /// Returns the configured concrete instance of a <see cref="LoggingProvider"/>.
        /// </summary>
        /// <returns></returns>
        public static LoggingProvider Instance()
        {
            //TODO: Make this a service.
            return new DatabaseLoggingProvider();
        }

        /// <summary>
        /// Gets a pageable collection of log entries.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public abstract IPagedCollection<LogEntry> GetPagedLogEntries(int pageIndex, int pageSize);

        /// <summary>
        /// Clears the log.
        /// </summary>
        public abstract void ClearLog();
    }
}