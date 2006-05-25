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
using System.Globalization;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Format;
using Subtext.Framework.Tracking;

namespace Subtext.Framework.Syndication
{
	/// <summary>
	/// Abstract base class used to write RSS feeds.
	/// </summary>
	public abstract class BaseRssWriter : BaseSyndicationWriter
	{
		private bool isBuilt = false;

		/// <summary>
		/// Creates a new <see cref="BaseRssWriter"/> instance.
		/// </summary>
		/// <param name="dateLastViewedFeedItemPublished">Last viewed feed item.</param>
		protected BaseRssWriter(DateTime dateLastViewedFeedItemPublished, bool useDeltaEncoding) : base(dateLastViewedFeedItemPublished, useDeltaEncoding)
		{
		}

		/// <summary>
		/// Builds the RSS feed.
		/// </summary>
		protected override void Build()
		{
			Build(this.DateLastViewedFeedItemPublished);
		}

		/// <summary>
		/// Builds the specified last id viewed.
		/// </summary>
		/// <param name="dateLastViewedFeedItemPublished">Last id viewed.</param>
		protected override void Build(DateTime dateLastViewedFeedItemPublished)
		{
			if(!isBuilt)
			{
				StartDocument();
				SetNamespaces();
				StartChannel();
				WriteChannel();
				WriteEntries();
				EndChannel();
				EndDocument();
				isBuilt = true;
			}		
		}

		/// <summary>
		/// Sets the namespaces used within the RSS feed.
		/// </summary>
		protected virtual void SetNamespaces()
		{
			this.WriteAttributeString("xmlns:dc", "http://purl.org/dc/elements/1.1/");
			this.WriteAttributeString("xmlns:trackback", "http://madskills.com/public/xml/rss/module/trackback/");
			this.WriteAttributeString("xmlns:wfw", "http://wellformedweb.org/CommentAPI/");
			this.WriteAttributeString("xmlns:slash", "http://purl.org/rss/1.0/modules/slash/");
			
			// Copyright notice
			this.WriteAttributeString("xmlns:copyright", "http://blogs.law.harvard.edu/tech/rss");
			
			if(Config.CurrentBlog.LicenseUrl != null && Config.CurrentBlog.LicenseUrl.Length > 0)
			{
				// Used to specify a license. Does not have to be a creative commons license.
				// see http://backend.userland.com/creativeCommonsRssModule
				this.WriteAttributeString("xmlns:creativeCommons", "http://backend.userland.com/creativeCommonsRssModule");
			}
			// Similar to a favicon image.
			this.WriteAttributeString("xmlns:image", "http://purl.org/rss/1.0/modules/image/");
		}

		/// <summary>
		/// Starts the RSS document.
		/// </summary>
		protected virtual void StartDocument()
		{
			this.WriteStartElement("rss");
			this.WriteAttributeString("version","2.0");
		}

		/// <summary>
		/// Ends the document.
		/// </summary>
		protected void EndDocument()
		{
			this.WriteEndElement();
		}

		/// <summary>
		/// Writes the channel Start element.
		/// </summary>
		protected void StartChannel()
		{
			this.WriteStartElement("channel");
		}

		/// <summary>
		/// Writes the channel.
		/// </summary>
		protected virtual void WriteChannel()
		{
			BuildChannel(info.Title, info.BlogHomeUrl, info.Email, info.SubTitle, info.Language, info.Author, Config.CurrentBlog.LicenseUrl, info.Title, info.BlogHomeUrl, string.Empty);
		}
		
		/// <summary>
		/// Builds the RSS channel starting XML section.
		/// </summary>
		/// <param name="title">The title.</param>
		/// <param name="link">The link.</param>
		/// <param name="authorEmail">The author email.</param>
		/// <param name="description">The description.</param>
		/// <param name="lang">The lang.</param>
		/// <param name="copyright">The copyright.</param>
		/// <param name="cclicense">The cclicense.</param>
		protected void BuildChannel(string title, string link, string authorEmail, string description, string lang, string copyright, string cclicense)
		{
			BuildChannel(title, link, authorEmail, description, lang, copyright, cclicense, info.Title, info.BlogHomeUrl, string.Empty);
		}

		/// <summary>
		/// Builds the RSS channel starting XML section.
		/// </summary>
		/// <param name="title">The title.</param>
		/// <param name="link">The link.</param>
		/// <param name="authorEmail">The author email.</param>
		/// <param name="description">The description.</param>
		/// <param name="lang">The lang.</param>
		/// <param name="copyright">The copyright.</param>
		/// <param name="cclicense">The cclicense.</param>
		/// <param name="imageTitle">The "alt" attribute value for the RSS feed's image.</param>
		/// <param name="imageLink">The url that the image will link to. Ostensibly the url to the blog.</param>
		/// <param name="imageDescription">The "title" attribute value for the anchor tag used to surround the image and link to this blog.</param>
		protected void BuildChannel(string title, string link, string authorEmail, string description, string lang, string copyright, string cclicense, string imageTitle, string imageLink, string imageDescription)
		{
			//Required Channel Elements
			this.WriteElementString("title", title);			
			this.WriteElementString("link", link);
			this.WriteElementString("description", description);
			
			//Optional Channel Elements
			this.WriteElementString("language", lang);
			//TODO: Implement this element.
			this.WriteElementString("copyright", copyright);

			//TODO: Provide REAL email authentication.
			if(authorEmail != null && authorEmail.Length > 0 && authorEmail.IndexOf("@") > 0)
			{
				this.WriteElementString("managingEditor", authorEmail);
			}
			
			//TODO: <category>One or more categories</category>
			this.WriteElementString("generator", VersionInfo.VersionDisplayText);

			if(cclicense != null && cclicense.Length > 0)
			{
				this.WriteElementString("creativeCommons:license", cclicense);
			}

			if(link != null && link.Length > 0)
				this.AddImageElement(imageTitle, imageLink, imageDescription);
		}

		/// <summary>
		/// Adds the image element to the rss feed.  This image is often displayed by 
		/// RSS aggregators with the feed contents.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="link"></param>
		/// <param name="description"></param>
		protected void AddImageElement(string title, string link,
			string description)
		{
			// Image Example
			// <image>
			//		<title>Joel On Software</title>
			//		<url>http://www.joelonsoftware.com/RssJoelOnSoftware.jpg</url>
			//		<link>http://www.joelonsoftware.com</link>
			//		<width>144</width>
			//		<height>25</height>
			//		<description>Painless Software Management</description>
			//	</image>
			this.WriteStartElement("image");
			this.WriteElementString("title",title);
			this.WriteElementString("url", info.RootUrl + "RSS2Image.gif");
			this.WriteElementString("link", link);
			this.WriteElementString("width", "77");
			this.WriteElementString("height", "60");
			this.WriteElementString("description", description); //Used in the alt tag.
			this.WriteEndElement();
		}

		protected void EndChannel()
		{
			this.WriteEndElement();
		}

		private void WriteEntries()
		{
			BlogConfigurationSettings settings = Config.Settings;
			this.clientHasAllFeedItems = true;
			this.latestPublishDate = this.DateLastViewedFeedItemPublished;
			
			foreach(Entry entry in this.Entries)
			{
				if(this.useDeltaEncoding && entry.DateSyndicated <= DateLastViewedFeedItemPublished)
				{
					// Since Entries are ordered by DatePublished descending, as soon 
					// as we encounter one that is smaller than or equal to 
					// one the client has already seen, we're done as we 
					// know the client already has the rest of the items in 
					// the collection.
					return;
				}

				// If we're here, we know that entry.EntryId is larger than 
				// the LastViewedFeedItemId.  Thus we can send it.
				this.WriteStartElement("item");
				EntryXml(entry, settings, info.UrlFormats);
				this.WriteEndElement();
				if(entry.DateSyndicated > latestPublishDate)
				{
					latestPublishDate = entry.DateSyndicated;
				}

				this.clientHasAllFeedItems = false;
			}
		}

		/// <summary>
		/// Writes the XML for a single entry.
		/// </summary>
		/// <param name="entry">Entry.</param>
		/// <param name="settings">Settings.</param>
		/// <param name="urlFormats">Uformat.</param>
		protected virtual void EntryXml(Entry entry, BlogConfigurationSettings settings, UrlFormats urlFormats)
		{
			//core
			this.WriteElementString("title", entry.Title);
			this.WriteElementString("link", entry.FullyQualifiedUrl);
			this.WriteElementString
			(
				"description", //Tag
				string.Format
				(
					"{0}{1}", //tag def
					entry.SyndicateDescriptionOnly ? entry.Description : entry.Body,  //use desc or full post
					(UseAggBugs && settings.Tracking.EnableAggBugs) ? TrackingUrls.AggBugImage(urlFormats.AggBugkUrl(entry.EntryID)) : null //use aggbugs
				)
			);
			//TODO: Perform real email auth.
			if(entry.Email != null && entry.Email.Length > 0 && entry.Email.IndexOf('@') > 0)
				this.WriteElementString("author", entry.Email);
			this.WriteElementString("guid", entry.FullyQualifiedUrl);
			this.WriteElementString("pubDate", entry.DateCreated.ToString("r"));			
			

			if(AllowComments && info.CommentsEnabled && entry.AllowComments && !entry.CommentingClosed)
			{
				// Comment API (http://wellformedweb.org/story/9)
				this.WriteElementString("wfw:comment", urlFormats.CommentApiUrl(entry.EntryID));
			}

			this.WriteElementString("comments", entry.FullyQualifiedUrl + "#feedback");
			
			if(entry.FeedBackCount > 0)
				this.WriteElementString("slash:comments", entry.FeedBackCount.ToString(CultureInfo.InvariantCulture));
			
			this.WriteElementString("wfw:commentRss", urlFormats.CommentRssUrl(entry.EntryID));
			
			if(info.TrackbacksEnabled)
				this.WriteElementString("trackback:ping", urlFormats.TrackBackUrl(entry.EntryID));

			//optional
			if(settings.UseXHTML && entry.IsXHMTL)
			{
				this.WriteStartElement("body");
				this.WriteAttributeString("xmlns", "http://www.w3.org/1999/xhtml");
				//If Syndicate Description Only was checked, write out the description to the
				//body tag rather than the full text of the post
				// - Robb Allen
				if (entry.SyndicateDescriptionOnly)
				{
					this.WriteRaw(entry.Description + ((UseAggBugs && settings.Tracking.EnableAggBugs)  ? TrackingUrls.AggBugImage(urlFormats.AggBugkUrl(entry.EntryID)) : null));
				}
				else
				{
					this.WriteRaw(entry.Body + ((UseAggBugs && settings.Tracking.EnableAggBugs)  ? TrackingUrls.AggBugImage(urlFormats.AggBugkUrl(entry.EntryID)) : null));
				}
				this.WriteEndElement();
			}			
		}
	}
}
