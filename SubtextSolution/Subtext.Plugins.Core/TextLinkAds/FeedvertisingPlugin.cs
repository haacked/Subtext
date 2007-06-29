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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Net;
using Subtext.Extensibility.Attributes;
using Subtext.Extensibility.Plugins;
using Subtext.Framework.Components;

namespace Subtext.Plugins.Core.TextLinkAds
{
	[Identifier("{DEA07F66-11AC-42fa-B5AF-AB1437517B34}")]
	[Description("Feedvertising",
		Author = "Bill Pierce",
		Company = "",
		HomePageUrl = "http://blogs.meetandplay.com/wpierce/",
		Version = "0.0.1",
		Description = "Add text ads to syndicated feeds")]
	public class FeedvertisingPlugin : PluginBase
	{
		private const int AdRefreshSeconds = 900;

		private bool initialized;
		private readonly IFeedvertisingService service = new FeedvertisingService();
		private IList<Advertisement> ads = new List<Advertisement>();

		#region PluginBase Members

		public override void Init(SubtextApplication application)
		{
			application.EntryUpdated += new EventHandler<EntryEventArgs>(EntryUpdated);
			application.EntrySyndicating += new EventHandler<EntryEventArgs>(EntryRendering);
			//application.SingleEntryRendering += new EventHandler<SubtextEventArgs>(EntryRendering);
		}

		protected virtual void EntryUpdated(object sender, EntryEventArgs e)
		{
			if (e.State != ObjectState.Create) return;

			if (!initialized) Initialize();

			Entry entry = e.Entry;
			UpdateAdsIfExpired();
			SetEntryAdvertisement(entry, GetAdvertisementHtml());
		}

		protected virtual void EntryRendering(object sender, EntryEventArgs e)
		{
    		Entry entry = e.Entry;

			string advertisement = GetEntryAdvertisement(entry);

			if (!string.IsNullOrEmpty(advertisement))
			{
				if (entry.SyndicateDescriptionOnly && !string.IsNullOrEmpty(entry.Description) )
				{
					entry.Description += advertisement;
				}
				entry.Body += advertisement;
			}
		}

		#endregion

		#region Properties

		public DateTime AdsExpireAt
		{
			get
			{
				DateTime dtAdsExpireAt;
				string sAdsExpireAt = GetBlogSetting("AdsExpireAt");
				if (!DateTime.TryParse(sAdsExpireAt, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtAdsExpireAt))
				{
					return DateTime.MinValue;
				}
				return dtAdsExpireAt;
			}
			set
			{
				SetBlogSetting("AdsExpireAt", value.ToString(CultureInfo.InvariantCulture));
			}
		}

		public string AdXml
		{
			get
			{
                return GetBlogSetting("AdXml");
			}
			set
			{
                SetBlogSetting("AdXml", value);
			}
		}

		public string WebsiteKey
		{
			get
			{
				return GetBlogSetting("WebsiteKey");
			}
		}

		public int LastUsedAdIndex
		{
			get
			{
				int iLastUsedAdIndex;
                string sLastUsedAdIndex = GetBlogSetting("LastUsedAdIndex");
				if (!Int32.TryParse(sLastUsedAdIndex, NumberStyles.Integer, CultureInfo.InvariantCulture, out iLastUsedAdIndex))
				{
					return  0;
				}
				return iLastUsedAdIndex;
			}
			set
			{
				SetBlogSetting("LastUsedAdIndex", value.ToString(CultureInfo.InvariantCulture));
			}
		}

		public string AdUrl
		{
			get
			{
				string textLinkAdsUrlTemplate = GetDefaultSetting("TextLinkAdsUrlTemplate");
				if( string.IsNullOrEmpty(textLinkAdsUrlTemplate))
				{
					throw new ConfigurationErrorsException("Must have TextLinkAdsUrlTemplate in web.config file");
				}
				return string.Format(textLinkAdsUrlTemplate, WebsiteKey);
			}
		}

		#endregion

		#region Protected Members

		protected virtual void Initialize()
		{
			string adXml = AdXml;
			if (!string.IsNullOrEmpty(adXml))
			{
				ads = service.ParseAdvertisementXml(adXml);
			}
			initialized = true;
		}

		protected virtual void UpdateAdsIfExpired()
		{
			if (AdsExpireAt < DateTime.Now)
			{
				string adXml = service.DownloadAdvertisementXml(AdUrl);
				if (!string.IsNullOrEmpty(adXml))
				{
					IList<Advertisement> newAds = service.ParseAdvertisementXml(adXml);
					if (newAds.Count > 0)
					{
						ads = newAds;
						AdsExpireAt = DateTime.Now.AddSeconds(AdRefreshSeconds);
						AdXml = adXml;
					}
				}
			}
		}

		protected virtual string GetAdvertisementHtml()
		{
			if (ads.Count == 0) return string.Empty;

			int lastUsedAdIndex = LastUsedAdIndex + 1;
			if (lastUsedAdIndex >= ads.Count)
			{
				lastUsedAdIndex = 0;
			}
			LastUsedAdIndex = lastUsedAdIndex;

			Advertisement ad = ads[lastUsedAdIndex];
			return service.GetAdvertisementHtml(ad);
		}

		protected virtual string GetEntryAdvertisement(Entry entry)
		{
			return GetEntrySetting(entry, "Advertisement");
		}

		protected virtual void SetEntryAdvertisement(Entry entry, string advertisement)
		{
			SetEntrySetting(entry, "Advertisement", advertisement);
		}

		#endregion
	}
}