using System;
using System.Collections.Generic;
using System.Text;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

namespace Subtext.Framework.Syndication.Admin
{
	public class RssReferrerHandler : EntryCollectionHandler<Referrer>
	{
		private DateTime lastReferrer = DateTime.MinValue;
		IList<Referrer> referrers = null;
		protected override bool RequiresAdminRole
		{
			get
			{
				return true;
			}
		}
		protected override IList<Referrer> GetFeedEntries()
		{
			referrers = Stats.GetPagedReferrers(0, 50);
			if (referrers.Count > 0)
				lastReferrer = referrers[0].LastReferDate;
			return referrers;
		}


		protected override DateTime GetItemCreatedDate(Referrer item)
		{
			return item.LastReferDate;
		}

		protected override BaseSyndicationWriter<Referrer> SyndicationWriter
		{
			get { return new ReferrerRssWriter(GetFeedEntries(), lastReferrer, this.UseDeltaEncoding); }
		}

		protected override bool IsMainfeed
		{
			get { return false; }
		}
	}
}
