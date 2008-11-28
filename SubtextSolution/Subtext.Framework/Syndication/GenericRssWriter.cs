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
using System.Collections.Specialized;
using System.Globalization;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework.Configuration;
using Subtext.Framework.Format;
using Subtext.Framework.Text;
using Subtext.Framework.Tracking;

namespace Subtext.Framework.Syndication
{
	/// <summary>
	/// Abstract base class used to write RSS feeds.
	/// </summary>
	public abstract class GenericRssWriter<T> : BaseSyndicationWriter<T>
	{
		private bool isBuilt = false;

		/// <summary>
		/// Creates a new <see cref="BaseRssWriter"/> instance.
		/// </summary>
		/// <param name="dateLastViewedFeedItemPublished">Last viewed feed item.</param>
		protected GenericRssWriter(DateTime dateLastViewedFeedItemPublished, bool useDeltaEncoding)
			: base(dateLastViewedFeedItemPublished, useDeltaEncoding)
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
			RssImageElement image = 
                new RssImageElement(GetRssImage(), Blog.Title, Blog.HomeFullyQualifiedUrl, 77, 60, null);
			BuildChannel(Blog.Title, Blog.HomeFullyQualifiedUrl.ToString(), Blog.Email, Blog.SubTitle, Blog.Language, Blog.Author, Config.CurrentBlog.LicenseUrl, image);
		}
		
		/// <summary>
		/// Returns the image that will be displayed in an RSS aggregator that supports RSS images. 
		/// </summary>
		/// <returns></returns>
		public virtual Uri GetRssImage()
		{
			return new Uri(Blog.HostFullyQualifiedUrl, "images/RSS2Image.gif");
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
			
            if (authorEmail != null 
                && authorEmail.Length > 0 
                && authorEmail.IndexOf("@") > 0 
                && authorEmail.IndexOf(".") > 0 
                && (Config.CurrentBlog.ShowEmailAddressInRss))
			{
				this.WriteElementString("managingEditor", authorEmail);
			}
			
			//TODO: <category>One or more categories</category>
			this.WriteElementString("generator", VersionInfo.VersionDisplayText);

			if(cclicense != null && cclicense.Length > 0)
			{
				this.WriteElementString("creativeCommons:license", cclicense);
			}

            if (image != null)
            {
                image.WriteToXmlWriter(this);
            }
		}

		protected void EndChannel()
		{
			this.WriteEndElement();
		}

		private void WriteEntries()
		{
			BlogConfigurationSettings settings = Config.Settings;
			ClientHasAllFeedItems = true;
			LatestPublishDate = this.DateLastViewedFeedItemPublished;
			
			foreach(T entry in this.Items)
			{
				if (UseDeltaEncoding && GetSyndicationDate(entry) <= DateLastViewedFeedItemPublished)
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
				EntryXml(entry, settings, Blog.UrlFormats);
				this.WriteEndElement();
				if(GetSyndicationDate(entry) > LatestPublishDate)
				{
					LatestPublishDate = GetSyndicationDate(entry);
				}

				ClientHasAllFeedItems = false;
			}
		}

		protected virtual string GetGuid(T item)
		{
			return GetLinkFromItem(item);
		}
		/// <summary>
		/// Writes the XML for a single entry.
		/// </summary>
		/// <param name="item">Entry.</param>
		/// <param name="settings">Settings.</param>
		/// <param name="urlFormats">Uformat.</param>
		protected virtual void EntryXml(T item, BlogConfigurationSettings settings, UrlFormats urlFormats)
		{
			//core
			this.WriteElementString("title", GetTitleFromItem(item));

			StringCollection categories = GetCategoriesFromItem(item);
			if (categories != null)
			{
				foreach (string category in categories)
				{
					this.WriteElementString("category", category);
				}
			}

			string fullUrl = GetLinkFromItem(item);

			this.WriteElementString("link", fullUrl);
			this.WriteElementString
			(
				"description", //Tag
				string.Format
				(
					"{0}{1}", //tag def
					GetBodyFromItem(item), (UseAggBugs && settings.Tracking.EnableAggBugs) ? TrackingUrls.AggBugImage(GetAggBugUrl(item,urlFormats)) : null //use aggbugs
				)
			);

			string author = GetAuthorFromItem(item);
			if (!String.IsNullOrEmpty(author))
            {
				this.WriteElementString("dc:creator", author);
            }

			this.WriteElementString("guid", GetGuid(item));
			this.WriteElementString("pubDate", GetPublishedDateUtc(item).ToString("r"));

			if (ItemCouldContainComments(item))
			{
				if (AllowComments && Blog.CommentsEnabled && ItemAllowsComments(item) && !CommentsClosedOnItem(item))
				{
					// Comment API (http://wellformedweb.org/story/9)
					this.WriteElementString("wfw:comment", GetCommentApiUrl(item, urlFormats));
				}

				this.WriteElementString("comments", fullUrl + "#feedback");

				if (GetFeedbackCount(item) > 0)
					this.WriteElementString("slash:comments", GetFeedbackCount(item).ToString(CultureInfo.InvariantCulture));

				this.WriteElementString("wfw:commentRss", GetCommentRssUrl(item, urlFormats));

				if (Blog.TrackbacksEnabled)
					this.WriteElementString("trackback:ping", GetTrackBackUrl(item, urlFormats));
			}

		    EnclosureItem encItem = GetEnclosureFromItem(item);
            if(encItem!=null)
            {
                this.WriteStartElement("enclosure");
                this.WriteAttributeString("url",encItem.Url);
                this.WriteAttributeString("length", encItem.Size.ToString());
                this.WriteAttributeString("type", encItem.MimeType);
                this.WriteEndElement();
            }
		}


		protected abstract string GetCommentRssUrl(T item, UrlFormats urlFormats);
		protected abstract string GetTrackBackUrl(T item, UrlFormats urlFormats);
		protected abstract string GetCommentApiUrl(T item, UrlFormats urlFormats);
		protected abstract string GetAggBugUrl(T item, UrlFormats urlFormats);

		/// <summary>
		/// Gets the categories from entry.
		/// </summary>
		/// <param name="item">The entry.</param>
		/// <returns></returns>
		protected abstract StringCollection GetCategoriesFromItem(T item);

		/// <summary>
		/// Gets the title from item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		protected abstract string GetTitleFromItem(T item);

		/// <summary>
		/// Gets the link from item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		protected abstract string GetLinkFromItem(T item);

		/// <summary>
		/// Gets the body from item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		protected abstract string GetBodyFromItem(T item);

		/// <summary>
		/// Gets the author from item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		protected abstract string GetAuthorFromItem(T item);

		/// <summary>
		/// Gets the publish date from item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		protected abstract DateTime GetPublishedDateUtc(T item);

		/// <summary>
		/// Returns true if the Item could contain comments.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		protected abstract bool ItemCouldContainComments(T item);

		/// <summary>
		/// Returns true if the item allows comments, otherwise false.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		protected abstract bool ItemAllowsComments(T item);

		/// <summary>
		/// Returns true if comments are closed, otherwise false.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		protected abstract bool CommentsClosedOnItem(T item);

		/// <summary>
		/// Gets the feedback count for the item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		protected abstract int GetFeedbackCount(T item);

		/// <summary>
		/// Obtains the syndication date for the specified entry, since 
		/// we don't necessarily know if the type has that field, we 
		/// can delegate this to the inheriting class.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		protected abstract DateTime GetSyndicationDate(T item);

        /// <summary>
        /// Gets the enclosure for the item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        protected abstract EnclosureItem GetEnclosureFromItem(T item);

        protected class EnclosureItem
        {
            public string MimeType
            {
                get;
                set;
            }

            public long Size
            {
                get;
                set;
            }

            public string Url
            {
                get;
                set;
            }
        }
	}
}
