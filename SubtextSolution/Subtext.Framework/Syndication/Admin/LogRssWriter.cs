using System;
using System.Collections.Generic;
using System.Text;
using Subtext.Framework.Logging;
using System.Collections.Specialized;
using Subtext.Framework.Configuration;

namespace Subtext.Framework.Syndication.Admin
{
	public class LogRssWriter:GenericRssWriter<LogEntry>
	{
		public LogRssWriter(IList<LogEntry> logs, bool useDeltaEncoding)
			: base(NullValue.NullDateTime, useDeltaEncoding)
		{
			this.Items = logs;
		}

		protected override string GetCommentRssUrl(LogEntry item, Subtext.Framework.Format.UrlFormats urlFormats)
		{
			return "";
		}

		protected override string GetGuid(LogEntry item)
		{
			return item.Message+item.Date.ToUniversalTime();
		}
		protected override string GetTrackBackUrl(LogEntry item, Subtext.Framework.Format.UrlFormats urlFormats)
		{
			return "";
		}

		protected override string GetCommentApiUrl(LogEntry item, Subtext.Framework.Format.UrlFormats urlFormats)
		{
			return "";
		}

		protected override string GetAggBugUrl(LogEntry item, Subtext.Framework.Format.UrlFormats urlFormats)
		{
			return "";
		}

		protected override System.Collections.Specialized.StringCollection GetCategoriesFromItem(LogEntry item)
		{
			StringCollection collection = new StringCollection();
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
			return Config.CurrentBlog.UrlFormats.AdminUrl("ErrorLog.aspx");
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
	}
}
