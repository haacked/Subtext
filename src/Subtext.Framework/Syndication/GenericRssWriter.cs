#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Subtext.Framework.Configuration;
using Subtext.Framework.Routing;
using Subtext.Framework.Text;
using Subtext.Framework.Tracking;

namespace Subtext.Framework.Syndication
{
    /// <summary>
    /// Abstract base class used to write RSS feeds.
    /// </summary>
    public abstract class GenericRssWriter<T> : BaseSyndicationWriter<T>
    {
        private bool _isBuilt;

        protected GenericRssWriter(TextWriter writer, DateTime dateLastViewedFeedItemPublished, bool useDeltaEncoding,
                                   ISubtextContext context)
            : base(writer, dateLastViewedFeedItemPublished, useDeltaEncoding, context)
        {
        }

        /// <summary>
        /// Builds the RSS feed.
        /// </summary>
        protected override void Build()
        {
            Build(DateLastViewedFeedItemPublishedUtc);
        }

        /// <summary>
        /// Builds the specified last id viewed.
        /// </summary>
        /// <param name="dateLastViewedFeedItemPublished">Last id viewed.</param>
        protected override void Build(DateTime dateLastViewedFeedItemPublished)
        {
            if (!_isBuilt)
            {
                StartDocument();
                SetNamespaces();
                StartChannel();
                WriteChannel();
                WriteEntries();
                EndChannel();
                EndDocument();
                _isBuilt = true;
            }
        }

        /// <summary>
        /// Sets the namespaces used within the RSS feed.
        /// </summary>
        protected virtual void SetNamespaces()
        {
            WriteAttributeString("xmlns:dc", "http://purl.org/dc/elements/1.1/");
            WriteAttributeString("xmlns:trackback", "http://madskills.com/public/xml/rss/module/trackback/");
            WriteAttributeString("xmlns:wfw", "http://wellformedweb.org/CommentAPI/");
            WriteAttributeString("xmlns:slash", "http://purl.org/rss/1.0/modules/slash/");

            // Copyright notice
            WriteAttributeString("xmlns:copyright", "http://blogs.law.harvard.edu/tech/rss");

            if (!string.IsNullOrEmpty(Blog.LicenseUrl))
            {
                // Used to specify a license. Does not have to be a creative commons license.
                // see http://backend.userland.com/creativeCommonsRssModule
                WriteAttributeString("xmlns:creativeCommons", "http://backend.userland.com/creativeCommonsRssModule");
            }
            // Similar to a favicon image.
            WriteAttributeString("xmlns:image", "http://purl.org/rss/1.0/modules/image/");
        }

        /// <summary>
        /// Starts the RSS document.
        /// </summary>
        protected virtual void StartDocument()
        {
            WriteStartElement("rss");
            WriteAttributeString("version", "2.0");
        }

        /// <summary>
        /// Ends the document.
        /// </summary>
        protected void EndDocument()
        {
            WriteEndElement();
        }

        /// <summary>
        /// Writes the channel Start element.
        /// </summary>
        protected void StartChannel()
        {
            WriteStartElement("channel");
        }

        /// <summary>
        /// Writes the channel.
        /// </summary>
        protected virtual void WriteChannel()
        {
            var blogUrl = new Uri(UrlHelper.BlogUrl().ToFullyQualifiedUrl(Blog), "Default.aspx");
            var image = new RssImageElement(GetRssImage(), Blog.Title, blogUrl, 77, 60, null);
            BuildChannel(Blog.Title, blogUrl, Blog.Email, Blog.SubTitle, Blog.Language, Blog.Author, Blog.LicenseUrl,
                         image);
        }

        /// <summary>
        /// Returns the image that will be displayed in an RSS aggregator that supports RSS images. 
        /// </summary>
        /// <returns></returns>
        public virtual Uri GetRssImage()
        {
            VirtualPath url = UrlHelper.ImageUrl("RSS2Image.gif");
            return url.ToFullyQualifiedUrl(Blog);
        }

        /// <summary>
        /// Builds the RSS channel starting XML section.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="url">The url.</param>
        /// <param name="authorEmail">The author email.</param>
        /// <param name="description">The description.</param>
        /// <param name="lang">The lang.</param>
        /// <param name="copyright">The copyright.</param>
        /// <param name="cclicense">The cclicense.</param>
        protected void BuildChannel(string title, Uri url, string authorEmail, string description, string lang,
                                    string copyright, string cclicense)
        {
            BuildChannel(title, url, authorEmail, description, lang, copyright, cclicense, null);
        }

        /// <summary>
        /// Builds the RSS channel starting XML section.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="url">The url.</param>
        /// <param name="authorEmail">The author email.</param>
        /// <param name="description">The description.</param>
        /// <param name="lang">The lang.</param>
        /// <param name="copyright">The copyright.</param>
        /// <param name="cclicense">The cclicense.</param>
        /// <param name="image">An optional sub-element of channel for rendering an image for the channel.</param>
        protected void BuildChannel(string title, Uri url, string authorEmail, string description, string lang,
                                    string copyright, string cclicense, RssImageElement image)
        {
            //Required Channel Elements
            WriteElementString("title", HtmlHelper.RemoveHtml(title));
            WriteElementString("link", url.ToString());
            WriteElementString("description", HtmlHelper.RemoveHtml(description));

            //Optional Channel Elements
            WriteElementString("language", lang);
            //TODO: Implement this element.
            WriteElementString("copyright", copyright);

            if (!string.IsNullOrEmpty(authorEmail)
               && authorEmail.IndexOf("@") > 0
               && authorEmail.IndexOf(".") > 0
               && (Blog.ShowEmailAddressInRss))
            {
                WriteElementString("managingEditor", authorEmail);
            }

            //TODO: <category>One or more categories</category>
            WriteElementString("generator", VersionInfo.VersionDisplayText);

            if (!string.IsNullOrEmpty(cclicense))
            {
                WriteElementString("creativeCommons:license", cclicense);
            }

            if (image != null)
            {
                image.WriteToXmlWriter(this);
            }
        }

        protected void EndChannel()
        {
            WriteEndElement();
        }

        private void WriteEntries()
        {
            BlogConfigurationSettings settings = Config.Settings;
            ClientHasAllFeedItems = true;
            LatestPublishDateUtc = DateLastViewedFeedItemPublishedUtc;

            foreach (T entry in Items)
            {
                if (UseDeltaEncoding && GetSyndicationDate(entry) <= DateLastViewedFeedItemPublishedUtc)
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
                WriteStartElement("item");
                EntryXml(entry, settings);
                WriteEndElement();
                if (GetSyndicationDate(entry) > LatestPublishDateUtc)
                {
                    LatestPublishDateUtc = GetSyndicationDate(entry);
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
        protected virtual void EntryXml(T item, BlogConfigurationSettings settings)
        {
            //core
            WriteElementString("title", GetTitleFromItem(item));

            ICollection<string> categories = GetCategoriesFromItem(item);
            if (categories != null)
            {
                foreach (string category in categories)
                {
                    WriteElementString("category", category);
                }
            }

            string fullUrl = GetLinkFromItem(item);

            WriteElementString("link", fullUrl);
            WriteElementString
                (
                "description", //Tag
                string.Format
                    (
                    CultureInfo.InvariantCulture,
                    "{0}{1}", //tag def
                    GetBodyFromItem(item),
                    (UseAggBugs && settings.Tracking.EnableAggBugs)
                        ? TrackingUrls.AggBugImage(GetAggBugUrl(item))
                        : null //use aggbugs
                    )
                );

            string author = GetAuthorFromItem(item);
            if (!String.IsNullOrEmpty(author))
            {
                WriteElementString("dc:creator", author);
            }

            WriteElementString("guid", GetGuid(item));
            WriteElementString("pubDate", GetPublishedDateUtc(item).ToString("r", CultureInfo.InvariantCulture));

            if (ItemCouldContainComments(item))
            {
                if (AllowComments && Blog.CommentsEnabled && ItemAllowsComments(item) && !CommentsClosedOnItem(item))
                {
                    // Comment API (http://wellformedweb.org/story/9)
                    WriteElementString("wfw:comment", GetCommentApiUrl(item));
                }

                WriteElementString("comments", fullUrl + "#feedback");

                if (GetFeedbackCount(item) > 0)
                {
                    WriteElementString("slash:comments", GetFeedbackCount(item).ToString(CultureInfo.InvariantCulture));
                }

                WriteElementString("wfw:commentRss", GetCommentRssUrl(item));

                if (Blog.TrackbacksEnabled)
                {
                    WriteElementString("trackback:ping", GetTrackBackUrl(item));
                }
            }

            EnclosureItem encItem = GetEnclosureFromItem(item);
            if (encItem != null)
            {
                WriteStartElement("enclosure");
                WriteAttributeString("url", encItem.Url);
                WriteAttributeString("length", encItem.Size.ToString(CultureInfo.InvariantCulture));
                WriteAttributeString("type", encItem.MimeType);
                WriteEndElement();
            }
        }


        protected abstract string GetCommentRssUrl(T item);
        protected abstract string GetTrackBackUrl(T item);
        protected abstract string GetCommentApiUrl(T item);
        protected abstract string GetAggBugUrl(T item);

        /// <summary>
        /// Gets the categories from entry.
        /// </summary>
        /// <param name="item">The entry.</param>
        /// <returns></returns>
        protected abstract ICollection<string> GetCategoriesFromItem(T item);

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

        #region Nested type: EnclosureItem

        protected class EnclosureItem
        {
            public string MimeType { get; set; }

            public long Size { get; set; }

            public string Url { get; set; }
        }

        #endregion
    }
}