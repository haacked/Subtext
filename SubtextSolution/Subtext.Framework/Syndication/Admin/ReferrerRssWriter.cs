using System;
using System.Collections.Generic;
using System.Text;
using Subtext.Framework.Components;
using System.Collections.Specialized;
using Subtext.Framework.Configuration;
using Subtext.Framework.Format;
using Subtext.Extensibility;

namespace Subtext.Framework.Syndication.Admin
{
	public class ReferrerRssWriter:GenericRssWriter<Referrer>
	{
		BlogInfo currentBlog = null;
		public ReferrerRssWriter(IList<Referrer> referrers, DateTime dateLastViewedFeedItemPublished, bool useDeltaEncoding)
			: base(dateLastViewedFeedItemPublished, useDeltaEncoding)
		{
			this.Items = referrers;
			currentBlog = Config.CurrentBlog;
		}

		protected override System.Collections.Specialized.StringCollection GetCategoriesFromItem(Referrer item)
		{
			StringCollection strings = new StringCollection();
			strings.Add(item.PostTitle);
			strings.Add(new Uri(item.ReferrerURL).Host);
			return strings;
		}
		protected override string GetGuid(Referrer item)
		{
			return item.BlogId.ToString() + item.EntryID.ToString() + item.ReferrerURL;
		}

		protected override string GetTitleFromItem(Referrer item)
		{
			return item.PostTitle + " - " + UrlFormats.ShortenUrl(item.ReferrerURL,20) ;
		}

		protected override string GetLinkFromItem(Referrer item)
		{
			return currentBlog.UrlFormats.AdminUrl("Referrers.aspx");
		}

		protected override string GetBodyFromItem(Referrer item)
		{
			return String.Format("{1} referrals from <a href=\"{0}\">{0}</a> ", item.ReferrerURL, item.Count);
		}

		protected override string GetAuthorFromItem(Referrer item)
		{
			return "";
		}

		protected override DateTime GetPublishedDateUtc(Referrer item)
		{
			return item.LastReferDate;
		}

		protected override bool ItemCouldContainComments(Referrer item)
		{
			return false;
		}

		protected override bool ItemAllowsComments(Referrer item)
		{
			return false;
		}

		protected override bool CommentsClosedOnItem(Referrer item)
		{
			return true;
		}

		protected override int GetFeedbackCount(Referrer item)
		{
			return item.Count;
		}

		protected override DateTime GetSyndicationDate(Referrer item)
		{
			return item.LastReferDate;
		}
		protected override string GetAggBugUrl(Referrer item, UrlFormats urlFormats)
		{
			return "";
		}
		protected override string GetCommentApiUrl(Referrer item, UrlFormats urlFormats)
		{
			return "";
		}
		protected override string GetCommentRssUrl(Referrer item, UrlFormats urlFormats)
		{
			return "";
		}
		protected override string GetTrackBackUrl(Referrer item, UrlFormats urlFormats)
		{
			return "";
		}
	}
}
