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

		protected virtual void StartDocument()
		{
			this.WriteStartElement("rss");
			this.WriteAttributeString("version","2.0");
		}

		protected void EndDocument()
		{
			this.WriteEndElement();
		}

		protected void StartChannel()
		{
			this.WriteStartElement("channel");
		}

		protected virtual void WriteChannel()
		{
			BuildChannel(info.Title, info.RootUrl, info.Email, info.SubTitle, info.Language, info.Author, Config.CurrentBlog.LicenseUrl);
		}

		protected void BuildChannel(string title, string link, string authorEmail, string description, string lang, string copyright, string cclicense)
		{
			//Required Channel Elements
			this.WriteElementString("title", title);			
			this.WriteElementString("link", link);
			this.WriteElementString("description", description);
			
			//Optional Channel Elements
			this.WriteElementString("language", lang);
			this.WriteElementString("dc:language", lang);
			//TODO: Implement this element.
			this.WriteElementString("copyright", copyright);

			//TODO: Provide REAL email authentication.
			if(authorEmail != null && authorEmail.Length > 0 &&authorEmail.IndexOf("@") > 0)
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
				this.AddImageElement(title, link, description);
		}

		// <summary>
		// Adds the image element to the rss feed.
		// </summary>
		// <param name="link">Link.</param>
		// <param name="description">Description.</param>
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
			this.WriteElementString("description", description);
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
				if(entry.DateSyndicated > base.latestPublishDate)
				{
					base.latestPublishDate = entry.DateSyndicated;
				}

				this.clientHasAllFeedItems = false;
			}
		}

		/// <summary>
		/// Writes the XML for a single entry.
		/// </summary>
		/// <param name="entry">Entry.</param>
		/// <param name="settings">Settings.</param>
		/// <param name="uformat">Uformat.</param>
		protected virtual void EntryXml(Entry entry, BlogConfigurationSettings settings, UrlFormats uformat)
		{
			//core
			this.WriteElementString("title", entry.Title);
			this.WriteElementString("link", entry.Link);
			this.WriteElementString
			(
				"description", //Tag
				string.Format
				(
					"{0}{1}", //tag def
					entry.SyndicateDescriptionOnly ? entry.Description : entry.Body,  //use desc or full post
					(UseAggBugs && settings.Tracking.EnableAggBugs) ? TrackingUrls.AggBugImage(uformat.AggBugkUrl(entry.EntryID)) : null //use aggbugs
				)
			);
			//TODO: Perform real email auth.
			if(entry.Email != null && entry.Email.Length > 0 && entry.Email.IndexOf('@') > 0)
				this.WriteElementString("author", entry.Email);
			this.WriteElementString("dc:creator", entry.Author);
			this.WriteElementString("guid", entry.Link);
			this.WriteElementString("pubDate", entry.DateCreated.ToString("r"));			
			

			if(AllowComments && info.CommentsEnabled && entry.AllowComments && !entry.CommentingClosed)
			{
				// Comment API (http://wellformedweb.org/story/9)
				this.WriteElementString("wfw:comment", uformat.CommentApiUrl(entry.EntryID));
			}

			this.WriteElementString("comments", entry.Link + "#Feedback");
			
			if(entry.FeedBackCount > 0)
				this.WriteElementString("slash:comments", entry.FeedBackCount.ToString(CultureInfo.InvariantCulture));
			
			this.WriteElementString("wfw:commentRss", uformat.CommentRssUrl(entry.EntryID));
			
			if(info.TrackbacksEnabled)
				this.WriteElementString("trackback:ping", uformat.TrackBackUrl(entry.EntryID));

			//optional
			if(settings.UseXHTML && entry.IsXHMTL)
			{
				this.WriteStartElement("body");
				this.WriteAttributeString("xmlns", "http://www.w3.org/1999/xhtml");
				this.WriteRaw(entry.Body + ((UseAggBugs && settings.Tracking.EnableAggBugs)  ? TrackingUrls.AggBugImage(uformat.AggBugkUrl(entry.EntryID)) : null));
				this.WriteEndElement();
			}			
		}
	}
}
