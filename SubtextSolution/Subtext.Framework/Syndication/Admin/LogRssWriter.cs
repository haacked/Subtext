using System;
using System.Collections.Generic;
using System.Text;
using Subtext.Framework.Logging;
using System.Collections.Specialized;
using Subtext.Framework.Configuration;
using System.IO;

namespace Subtext.Framework.Syndication.Admin
{
	public class LogRssWriter : GenericRssWriter<LogEntry>
	{
		public LogRssWriter(TextWriter writer, ICollection<LogEntry> logs, bool useDeltaEncoding, ISubtextContext context)
            : base(writer, NullValue.NullDateTime, useDeltaEncoding, context)
		{
			this.Items = logs;
		}

		protected override string GetCommentRssUrl(LogEntry item)
		{
			return string.Empty;
		}

		protected override string GetGuid(LogEntry item)
		{
			return item.Message + item.Date.ToUniversalTime();
		}
		protected override string GetTrackBackUrl(LogEntry item)
		{
			return string.Empty;
		}

		protected override string GetCommentApiUrl(LogEntry item)
		{
			return "";
		}

		protected override string GetAggBugUrl(LogEntry item)
		{
			return "";
		}

		protected override ICollection<string> GetCategoriesFromItem(LogEntry item)
		{
			IList<string> collection = new List<string>();
			collection.Add(item.Level);

			string path = item.Url.PathAndQuery.Split('?')[0];
			collection.Add(path);
			return collection;
		}

		protected override string GetTitleFromItem(LogEntry item)
		{
			return item.Message;
		}

		protected override string GetLinkFromItem(LogEntry item)
		{
			return Blog.UrlFormats.AdminUrl("ErrorLog.aspx");
		}

		protected override string GetBodyFromItem(LogEntry item)
		{
			return item.Exception;
		}

		protected override string GetAuthorFromItem(LogEntry item)
		{
			return item.Logger;
		}

		protected override DateTime GetPublishedDateUtc(LogEntry item)
		{
			return item.Date;
		}

		protected override bool ItemCouldContainComments(LogEntry item)
		{
			return false;
		}

		protected override bool ItemAllowsComments(LogEntry item)
		{
			return false;
		}

		protected override bool CommentsClosedOnItem(LogEntry item)
		{
			return true;
		}

		protected override int GetFeedbackCount(LogEntry item)
		{
			return 0;
		}

		protected override DateTime GetSyndicationDate(LogEntry item)
		{
			return item.Date;
		}

        protected override EnclosureItem GetEnclosureFromItem(LogEntry item)
        {
            return null;
        }
	}
}
