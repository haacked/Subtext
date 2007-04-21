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
using System.Globalization;
using System.IO;
using System.Net;
using System.Xml;
using Subtext.Framework.Logging;

namespace Subtext.Plugins.Core.TextLinkAds
{
	public class FeedvertisingService : IFeedvertisingService
	{
        private static Log log = new Log();

		public virtual IList<Advertisement> ParseAdvertisementXml(string advertisementXml)
		{
			if (advertisementXml == null) throw new ArgumentNullException("advertisementXml");

			IList<Advertisement> ads = new List<Advertisement>();

			try
			{
				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.LoadXml(advertisementXml);
				advertisementXml = null;

				XmlElement links = xmlDoc.DocumentElement;
				if (links.Name == "Links" && links.HasChildNodes)
				{
					int linkCount = links.ChildNodes.Count;
					for (int i = 0; i < linkCount; i++)
					{
						XmlNode link = links.ChildNodes[i];
						if (link.Name == "Link")
						{
							Advertisement ad = new Advertisement();

							int linkElementCount = link.ChildNodes.Count;
							for (int j = 0; j < linkElementCount; j++)
							{
								XmlNode element = link.ChildNodes[j];
								string name = element.Name;
								string value = element.InnerXml.Trim();

								if (name == "URL") ad.Url = value;
								if (name == "Text") ad.Text = value;
								if (name == "BeforeText") ad.BeforeText = value;
								if (name == "AfterText") ad.AfterText = value;
								if (name == "RssText") ad.RssText = value;
								if (name == "RssBeforeText") ad.RssBeforeText = value;
								if (name == "RssAfterText") ad.RssAfterText = value;
								if (name == "RssPrefix") ad.RssPrefix = value;
							}

							// Only add valid ads
							if ( IsValid(ad) )
							{
								ads.Add(ad);
							}
						}
					}
				}

				links = null;
				xmlDoc = null;
			}
			catch(Exception exc)
			{
				if (log.IsDebugEnabled)
				{
					log.Debug(string.Format("Error occured while parsing Advertisement Xml '{0}'", advertisementXml), exc);
				}
			}

			return ads;
		}

		public virtual string DownloadAdvertisementXml(string url)
		{
			if (url == null) throw new ArgumentNullException("url");

			try
			{
				WebRequest request = WebRequest.Create(url);
				request.Timeout = 5000;

				using (WebResponse response = request.GetResponse())
				{
					using (StreamReader reader = new StreamReader(response.GetResponseStream()))
					{
						return reader.ReadToEnd();
					}
				}
			}
			catch (Exception exc)
			{
				if (log.IsDebugEnabled)
				{
					log.Debug(string.Format("Error occured while connecting to Text Link Ads '{0}'", url), exc);
				}
			}

			return string.Empty;
		}

		public virtual string GetAdvertisementHtml(Advertisement advertisement)
		{
			if (advertisement == null) throw new ArgumentNullException("advertisement");

			if (!IsValid(advertisement))
			{
				return string.Empty;
			}

			return string.Format("<p><strong><em>{0}</em></strong>: {1} <a href=\"{2}\">{3}</a><em> </em>{4}<br /></p>", advertisement.RssPrefix, advertisement.RssBeforeText, advertisement.Url, advertisement.RssText, advertisement.RssAfterText);
		}

		protected virtual bool IsValid(Advertisement advertisement)
		{
			if (advertisement == null) return false;

			return !string.IsNullOrEmpty(advertisement.RssPrefix) && !string.IsNullOrEmpty(advertisement.RssText);
		}
	}
}