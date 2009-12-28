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
using System.Globalization;
using System.IO;
using System.Web;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Routing;
using Subtext.Framework.Text;
using Subtext.Framework.Tracking;
using Subtext.Infrastructure;

namespace Subtext.Framework.Syndication
{
    /// <summary>
    /// Generates an Atom feed.
    /// </summary>
    public abstract class BaseAtomWriter : BaseSyndicationWriter<Entry>
    {
        private static string W3Utcz(IFormattable dt)
        {
            return dt.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
        }

        private bool _isBuilt;

        /// <summary>
        /// Bases the syndication writer.
        /// </summary>
        protected BaseAtomWriter(TextWriter writer, DateTime dateLastViewedFeedItemPublished, bool useDeltaEncoding,
                                 ISubtextContext context)
            : base(writer, dateLastViewedFeedItemPublished, useDeltaEncoding, context)
        {
        }

        protected override void Build()
        {
            if(!_isBuilt)
            {
                Build(DateLastViewedFeedItemPublished);
            }
        }

        /// <summary>
        /// Builds the specified last id viewed.
        /// </summary>
        /// <param name="dateLastViewedFeedItemPublished">Last id viewed.</param>
        protected override void Build(DateTime dateLastViewedFeedItemPublished)
        {
            if(!_isBuilt)
            {
                StartDocument();
                SetNamespaces();
                WriteChannel();
                WriteEntries();
                EndDocument();
                _isBuilt = true;
            }
        }

        protected virtual void SetNamespaces()
        {
            WriteAttributeString("xmlns:dc", "http://purl.org/dc/elements/1.1/");
            WriteAttributeString("xmlns:trackback", "http://madskills.com/public/xml/rss/module/trackback/");
            WriteAttributeString("xmlns:wfw", "http://wellformedweb.org/CommentAPI/");
            WriteAttributeString("xmlns:slash", "http://purl.org/rss/1.0/modules/slash/");
            //(Duncanma 11/13/2005, changing atom namespace for 1.0 feed)
            WriteAttributeString("xmlns", "http://www.w3.org/2005/Atom");
            WriteAttributeString("xml:lang", Blog.Language);
        }

        protected virtual void StartDocument()
        {
            WriteStartElement("feed");
            //(Duncanma 11/13/2005, removing version attribute for 1.0 feed)
            //this.WriteAttributeString("version","0.3");
        }

        protected void EndDocument()
        {
            WriteEndElement();
        }

        protected virtual void WriteChannel()
        {
            var blogUrl = new Uri(UrlHelper.BlogUrl().ToFullyQualifiedUrl(Blog), "Default.aspx");
            BuildChannel(Blog.Title, blogUrl.ToString(), Blog.SubTitle);
        }

        protected void BuildChannel(string title, string link, string description)
        {
            WriteElementString("title", HtmlHelper.RemoveHtml(title));

            //(Duncanma 11/13/2005, changing link rel and href for 1.0 feed)
            WriteStartElement("link");
            //(Duncanma 12/28/2005, changing again... Atom vs atom was causing feed validation errors
            WriteAttributeString("rel", "self");
            WriteAttributeString("type", "application/atom+xml");
            string currentUrl = link + "Atom.aspx";
            if(HttpContext.Current.Request != null)
            {
                currentUrl = HttpContext.Current.Request.Url.ToString();
            }
            WriteAttributeString("href", currentUrl);

            // this.WriteAttributeString("rel","self");
            // this.WriteAttributeString("type","application/xml");
            // this.WriteAttributeString("href",info.RootUrl + "atom.aspx");
            WriteEndElement();

            //(Duncanma 11/13/2005, changing tagline to subtitle for 1.0 feed)
            WriteStartElement("subtitle");
            WriteAttributeString("type", "html");
            WriteString(HtmlHelper.RemoveHtml(description));
            WriteEndElement();

            WriteElementString("id", link);

            WriteStartElement("author");
            WriteElementString("name", Blog.Author);

            var blogUrl = new Uri(UrlHelper.BlogUrl().ToFullyQualifiedUrl(Blog), "Default.aspx");
            WriteElementString("uri", blogUrl.ToString());
            WriteEndElement();

            //(Duncanma 11/13/05) updated generator to reflect project name change to Subtext
            WriteStartElement("generator");
            //(Duncanma 11/13/2005, changing url to uri for 1.0 feed)
            WriteAttributeString("uri", "http://subtextproject.com");
            WriteAttributeString("version", VersionInfo.VersionDisplayText);
            WriteString("Subtext");
            WriteEndElement();

            //(Duncanma 11/13/2005, changing modified to updated for 1.0 feed)
            WriteElementString("updated", W3Utcz(Blog.LastUpdated));
        }

        private void WriteEntries()
        {
            BlogConfigurationSettings settings = Config.Settings;

            ClientHasAllFeedItems = true;
            LatestPublishDate = DateLastViewedFeedItemPublished;

            foreach(Entry entry in Items)
            {
                // We'll show every entry if RFC3229 is not enabled.
                //TODO: This is wrong.  What if a post is not published 
                // and then gets published later. It will not be displayed.
                if(!UseDeltaEncoding || entry.DateSyndicated > DateLastViewedFeedItemPublished)
                {
                    WriteStartElement("entry");
                    EntryXml(entry, settings, Blog.TimeZone);
                    WriteEndElement();
                    ClientHasAllFeedItems = false;

                    //Update the latest publish date.
                    if(entry.DateSyndicated > LatestPublishDate)
                    {
                        LatestPublishDate = entry.DateSyndicated;
                    }
                }
            }
        }

        protected virtual void EntryXml(Entry entry, BlogConfigurationSettings settings, ITimeZone timezone)
        {
            WriteElementString("title", entry.Title);

            WriteStartElement("link");
            //(Duncanma 11/13/2005, changing alternate to self for 1.0 feed)
            WriteAttributeString("rel", "alternate");
            WriteAttributeString("type", "text/html");
            WriteAttributeString("href", UrlHelper.EntryUrl(entry).ToFullyQualifiedUrl(Blog).ToString());
            WriteEndElement();

            WriteElementString("id", UrlHelper.EntryUrl(entry).ToFullyQualifiedUrl(Blog).ToString());

            //(Duncanma 11/13/2005, hiding created, change issued to
            //published and modified to updated for 1.0 feed)
            //this.WriteElementString("created",W3Utcz(entry.DateCreated));
            WriteElementString("published", W3Utcz(entry.DateCreated));
            WriteElementString("updated", W3Utcz(entry.DateModified));

            if(entry.HasDescription)
            {
                WriteStartElement("summary");
                //(Duncanma 11/13/2005, changing text/html to html for 1.0 feed)
                WriteAttributeString("type", "html");
                WriteString(entry.Description);
                WriteEndElement();
            }

            WriteStartElement("content");
            //(Duncanma 11/13/2005, changing text/html to html for 1.0 feed)
            WriteAttributeString("type", "html");
            //(Duncanma 11/13/2005, hiding mode for 1.0 feed)
            //this.WriteAttributeString("mode","escaped");

            WriteString
                (
                string.Format
                    (CultureInfo.InvariantCulture, "{0}{1}", //tag def
                     entry.SyndicateDescriptionOnly ? entry.Description : entry.Body, //use desc or full post
                     (UseAggBugs && settings.Tracking.EnableAggBugs)
                         ? TrackingUrls.AggBugImage(UrlHelper.AggBugUrl(entry.Id))
                         : null //use aggbugs
                    )
                );
            WriteEndElement();

            if(AllowComments && Blog.CommentsEnabled && entry.AllowComments && !entry.CommentingClosed)
            {
                //optional for CommentApi Post location
                WriteElementString("wfw:comment", UrlHelper.CommentApiUrl(entry.Id));
                //optional url for comments
                //this.WriteElementString("comments",entry.Link + "#feedback");
                //optional comment count
                WriteElementString("slash:comments", entry.FeedBackCount.ToString(CultureInfo.InvariantCulture));
                //optional commentRss feed location
                WriteElementString("wfw:commentRss", UrlHelper.CommentRssUrl(entry.Id));
                //optional trackback location
                WriteElementString("trackback:ping", UrlHelper.TrackbacksUrl(entry.Id));
                //core 
            }
        }
    }
}