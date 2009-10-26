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

using System.Data;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;
using Subtext.Framework.Web.HttpModules;

namespace Subtext.Framework.Logging
{
    /// <summary>
    /// Summary description for DatabaseLoggingProvider.
    /// </summary>
    public class DatabaseLoggingProvider : LoggingProvider
    {
        readonly StoredProcedures _procedures = new StoredProcedures(Config.ConnectionString);

        public int BlogId
        {
            get { return BlogRequest.Current.IsHostAdminRequest ? NullValue.NullInt32 : Config.CurrentBlog.Id; }
        }

        /// <summary>
        /// Gets a pageable collection of log entries.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public override IPagedCollection<LogEntry> GetPagedLogEntries(int pageIndex, int pageSize)
        {
            using(IDataReader reader = _procedures.GetPageableLogEntries(BlogId, pageIndex, pageSize))
            {
                return reader.ReadPagedCollection(r => r.ReadObject<LogEntry>());
            }
        }

        /// <summary>
        /// Clears the log.
        /// </summary>
        public override void ClearLog()
        {
            _procedures.LogClear(BlogId.NullIfMinValue());
        }
    }
}