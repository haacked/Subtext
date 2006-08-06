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
using Subtext.Extensibility;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Format;
using Subtext.Framework.Text;
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
			RssImageElement image = new RssImageElement(GetRssImage(), info.Title, info.HomeFullyQualifiedUrl, 77, 60, null);
			BuildChannel(info.Title, info.HomeFullyQualifiedUrl.ToString(), info.Email, info.SubTitle, info.Language, info.Author, Config.CurrentBlog.LicenseUrl, image);
		}
		
		/// <summary>
		/// Returns the image that will be displayed in an RSS aggregator that supports RSS images. 
		/// </summary>
		/// <returns></returns>
		public virtual Uri GetRssImage()
		{
			return new Uri(info.HostFullyQualifiedUrl, "images/RSS2Image.gif");
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
			BuildChannel(title, link, authorEmail, description, lang, copyright, cclicense, null);
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
		/// <param name="image">An optional sub-element of channel for rendering an image for the channel.</param>
		protected void BuildChannel(string title, string link, string authorEmail, string description, string lang, string copyright, string cclicense, RssImageElement image)
		{
			//Required Channel Elements
			this.WriteElementString("title", HtmlHelper.RemoveHtml(title));			
			this.WriteElementString("link", link);
			this.WriteElementString("description", HtmlHelper.RemoveHtml(description));
			
			//Optional Channel Elements
			this.WriteElementString("language", lang);
			//TODO: Implement this element.
			this.WriteElementString("copyright", copyright);

			//TODO: Provide REAL email authentication.
			//TODO: Allow blog owner to omit this field on a per-blog basis without having to remove email address. Or we might consider a separate field for Syndicated email address.
			if (authorEmail != null && authorEmail.Length > 0 && authorEmail.IndexOf("@") > 0 && authorEmail.IndexOf(".") > 0)
			{
				this.WriteElementString("managingEditor", authorEmail);
			}
			
			//TODO: <category>One or more categories</category>
			this.WriteElementString("generator", VersionInfo.VersionDisplayText);

			if(cclicense != null && cclicense.Length > 0)
			{
				this.WriteElementString("creativeCommons:license", cclicense);
			}

			if(image != null)
				image.WriteToXmlWriter(this);
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
		    
	        foreach (string category in entry.Categories)
            {
                this.WriteElementString("category", category);
            }
		    
			this.WriteElementString("link", entry.FullyQualifiedUrl.ToString());
			this.WriteElementString
			(
				"description", //Tag
				string.Format
				(
					"{0}{1}", //tag def
					entry.SyndicateDescriptionOnly ? entry.Description : entry.Body,  //use desc or full post
					(UseAggBugs && settings.Tracking.EnableAggBugs) ? TrackingUrls.AggBugImage(urlFormats.AggBugkUrl(entry.Id)) : null //use aggbugs
				)
			);

			if (!String.IsNullOrEmpty(entry.Author))
            {
				this.WriteElementString("dc:creator", entry.Author);
            }
		    
			this.WriteElementString("guid", entry.FullyQualifiedUrl.ToString());
			this.WriteElementString("pubDate", entry.DateCreated.ToString("r"));			

			if (entry.PostType == PostType.BlogPost || entry.PostType == PostType.Story)
			{
				if (AllowComments && info.CommentsEnabled && entry.AllowComments && !entry.CommentingClosed)
				{
					// Comment API (http://wellformedweb.org/story/9)
					this.WriteElementString("wfw:comment", urlFormats.CommentApiUrl(entry.Id));
				}
				
				this.WriteElementString("comments", entry.FullyQualifiedUrl + "#feedback");

				if (entry.FeedBackCount > 0)
					this.WriteElementString("slash:comments", entry.FeedBackCount.ToString(CultureInfo.InvariantCulture));
				
				this.WriteElementString("wfw:commentRss", urlFormats.CommentRssUrl(entry.Id));

				if (info.TrackbacksEnabled)
					this.WriteElementString("trackback:ping", urlFormats.TrackBackUrl(entry.Id));
			}
		}
	}
}
