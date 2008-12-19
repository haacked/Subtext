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
using System.Data;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework.Components;
using Subtext.Framework.Providers;
using Subtext.Framework.Data;
using Subtext.Framework.Configuration;

namespace Subtext.Framework.Logging
{
	/// <summary>
	/// Summary description for DatabaseLoggingProvider.
	/// </summary>
	public class DatabaseLoggingProvider : LoggingProvider
	{
        StoredProcedures _procedures = new StoredProcedures(Config.ConnectionString);
        
        public int BlogId
        {
            get
            {
                if (InstallationManager.IsInHostAdminDirectory)
                    return NullValue.NullInt32;
                else
                    return Config.CurrentBlog.Id;
            }
        }
        
		/// <summary>
		/// Gets a pageable collection of log entries.
		/// </summary>
		/// <param name="pageIndex">Index of the page.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <returns></returns>
        public override IPagedCollection<LogEntry> GetPagedLogEntries(int pageIndex, int pageSize)
		{
            using (IDataReader reader = _procedures.GetPageableLogEntries(BlogId, pageIndex, pageSize))
            {
                return reader.GetPagedCollection(r => r.LoadObject<LogEntry>());
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
