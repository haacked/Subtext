using System;
using System.Collections.Generic;
using System.Text;
using Subtext.Framework.Logging;

namespace Subtext.Framework.Syndication.Admin
{
	class RssErrorHandler : EntryCollectionHandler<LogEntry>
	{
		IList<LogEntry> logEntries = null;
		protected override bool RequiresAdminRole
		{
			get
			{
				return true;
			}
		}

		protected override IList<LogEntry> GetFeedEntries()
		{
			logEntries = LoggingProvider.Instance().GetPagedLogEntries(0, 50);

			return logEntries;
		}

		protected override DateTime GetItemCreatedDate(LogEntry item)
		{
			return item.Date;
		}

		protected override BaseSyndicationWriter<LogEntry> SyndicationWriter
		{
			get { return new LogRssWriter(GetFeedEntries(),UseDeltaEncoding); }
		}

		protected override bool IsMainfeed
		{
			get { return false; }
		}
	}
}
