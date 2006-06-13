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
using System.Collections.Specialized;
using Subtext.Extensibility.Providers;
using Subtext.Framework.Components;
using Subtext.Framework.Data;
using Subtext.Framework.Providers;

namespace Subtext.Framework.Logging
{
	/// <summary>
	/// Provider for retrieving log entries.
	/// </summary>
	public abstract class LoggingProvider : ProviderBase
	{
		string _name;

		/// <summary>
		/// Returns the configured concrete instance of a <see cref="ObjectProvider"/>.
		/// </summary>
		/// <returns></returns>
		public static LoggingProvider Instance()
		{
			//TODO: Make this a real provider.
			return new DatabaseLoggingProvider();
		}

		/// <summary>
		/// Gets a pageable collection of log entries.
		/// </summary>
		/// <param name="pageIndex">Index of the page.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="sortDirection">The sort direction.</param>
		/// <returns></returns>
        public abstract IPagedCollection<LogEntry> GetPagedLogEntries(int pageIndex, int pageSize, SortDirection sortDirection);

		/// <summary>
		/// Clears the log.
		/// </summary>
		public abstract void ClearLog();

		/// <summary>
		/// Initializes the specified provider.
		/// </summary>
		/// <param name="name">Friendly Name of the provider.</param>
		/// <param name="configValue">Config value.</param>
		public override void Initialize(string name, NameValueCollection configValue)
		{
			_name = name;
		}

		/// <summary>
		/// Returns the friendly name of the provider when the provider is initialized.
		/// </summary>
		/// <value></value>
		public override string Name
		{
			get
			{
				return _name;
			}
		}
	}
}
