using System;
using System.Collections.Generic;
using System.Text;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Extensibility;
using System.Text.RegularExpressions;
using System.Collections;
using Subtext.Framework.Logging;

namespace Subtext.Framework.Syndication.Admin
{
	public class RssAdminHandler:EntryCollectionHandler<object>
	{
		string title = "";
		string rssType = "";
		string[] filters;
		int count;
		Enum filterFlags;

		protected override bool IsLocalCacheOK()
		{
			string dt = LastModifiedHeader;

			if (dt != null)
			{
				IList ec = GetFeedEntriesSimple();

				if (ec != null && ec.Count > 0)
				{
					//Get the first entry.
					object entry = default(object);
					//TODO: Probably change GetFeedEntries to return IList<Entry>
					foreach (object en in ec)
					{
						entry = en;
						break;
					}
					return DateTime.Compare(DateTime.Parse(dt), ConvertLastUpdatedDate(GetItemCreatedDate(entry))) == 0;
				}
			}
			return false;
		}

		protected override bool RequiresAdminRole
		{
			get
			{
				return true;
			}
		}
		protected void SetOptions()
		{
			if (!Int32.TryParse(Context.Request.QueryString["Count"], out count))
			{
				count = Config.Settings.ItemCount;
			}
			if (Regex.IsMatch(Context.Request.Url.PathAndQuery, "ModeratedCommentRss", RegexOptions.IgnoreCase))
			{
				title = "Comments requiring your approval.";
				filters = new string[] { "NeedsModeration" };
				rssType = "Comment";
				return;
			}

			if (Regex.IsMatch(Context.Request.Url.PathAndQuery, "ReferrersRss", RegexOptions.IgnoreCase))
			{
				title = "Referrals";
				rssType = "Referral";
				return;
			}

			if (Regex.IsMatch(Context.Request.Url.PathAndQuery, "ErrorsRss", RegexOptions.IgnoreCase))
			{
				title = "Errors";
				rssType = "Log";
				return;
			}
			
			title = this.Context.Request["Title"];
			rssType = this.Context.Request.QueryString["Type"];

			string qryFilters = Context.Request.QueryString["Filter"];
			if (String.IsNullOrEmpty(qryFilters))
			{
				filters = new string[] { };
			}
			else
			{
				filters = qryFilters.Split('+');
			}
		}

		protected override void ProcessFeed()
		{

			SetOptions();
			base.ProcessFeed();


		}
		protected override IList<object> GetFeedEntries()
		{
			throw new Exception("The method or operation is not implemented.");
		}
		
		protected IList GetFeedEntriesSimple()
		{
			if(String.IsNullOrEmpty(rssType))
				throw new Exception("Rss Type must be specified.");


			switch(rssType)
			{
				case "Comment":
					FeedbackStatusFlags flags = FeedbackStatusFlags.None;

					foreach (string filter in filters)
					{
						if (Enum.IsDefined(typeof(FeedbackStatusFlags), filter))
						{
							flags |= (FeedbackStatusFlags)Enum.Parse(typeof(FeedbackStatusFlags), filter, true);
						}
					}

					filterFlags = flags;
					IList<FeedbackItem> moderatedFeedback = FeedbackItem.GetPagedFeedback(0, count, flags, FeedbackType.None);
					return (IList)moderatedFeedback;
				case "Referral":
					IList<Referrer> referrers = Stats.GetPagedReferrers(0, count);
					return (IList)referrers;
				case "Log":
					IList<LogEntry> entries = LoggingProvider.Instance().GetPagedLogEntries(0, count);
					return (IList)entries;
			}


			return null;
			
		}

		protected override DateTime GetItemCreatedDate(object item)
		{
			if (item is FeedbackItem)
				return ((FeedbackItem)item).DateCreated;
			if (item is Referrer)
				return ((Referrer)item).LastReferDate;
			if (item is LogEntry)
				return ((LogEntry)item).Date;
			return DateTime.Now;
		}

		protected override BaseSyndicationWriter SyndicationWriter
		{
			get
			{
				IList feed = GetFeedEntriesSimple();
				if (feed is IList<FeedbackItem>)
				{

					Entry entry = new Entry(PostType.None);
					entry.Title = title;

					if (((FeedbackStatusFlags)filterFlags) == FeedbackStatusFlags.NeedsModeration)
					{
						entry.Url = CurrentBlog.UrlFormats.AdminUrl("Feedback.aspx?status=2");
					}
					else
					{
						entry.Url = CurrentBlog.UrlFormats.AdminUrl("Feedback.aspx");

					}

					entry.Body = "";

					IList<FeedbackItem> feedback = (IList<FeedbackItem>)feed;

					return new CommentRssWriter(feedback, entry);

				}
				if (feed is IList<Referrer>)
				{
					IList<Referrer> referrers = (IList<Referrer>)feed;
					DateTime lastReferrer = NullValue.NullDateTime;
					if (referrers.Count > 0)
						lastReferrer = referrers[0].LastReferDate;
					return new ReferrerRssWriter(referrers, lastReferrer, this.UseDeltaEncoding);
				}
				if (feed is IList<LogEntry>)
				{
					IList<LogEntry> entries = (IList<LogEntry>)feed;
					return new LogRssWriter(entries, this.UseDeltaEncoding);
				}
				return null;
			}
		}

		protected override bool IsMainfeed
		{
			get { return false; }
		}
	}
}
