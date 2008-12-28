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
using Subtext.Framework.Text;
using Subtext.Framework.Tracking;
using System.IO;

namespace Subtext.Framework.Syndication
{
	/// <summary>
	/// Generates an Atom feed.
	/// </summary>
	public abstract class BaseAtomWriter : BaseSyndicationWriter<Entry>
	{
		#region TimeHelpers
		
		private static string W3UTC(DateTime dt, TimeZone tz)
		{
			TimeSpan timeZone = tz.GetUtcOffset(dt);
			if (timeZone.TotalHours >= 0) {
				return dt.ToString("yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture) + ":00+" + timeZone.TotalHours +
					":" + ((timeZone.Minutes > 0) ? timeZone.Minutes.ToString() : "00");
			}
			return dt.ToString("yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture) + ":00" + timeZone.TotalHours +
				":" + ((timeZone.Minutes > 0) ? timeZone.Minutes.ToString() : "00");
		}

		private static string W3UTCZ(IFormattable dt)
		{
			return dt.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
		}
		#endregion

		private bool isBuilt = false;

		/// <summary>
		/// Bases the syndication writer.
		/// </summary>
		/// <param name="dateLastViewedFeedItemPublished">Last viewed feed item.</param>
		/// <param name="useDeltaEncoding">if set to <c>true</c> [use delta encoding].</param>
		protected BaseAtomWriter(TextWriter writer, DateTime dateLastViewedFeedItemPublished, bool useDeltaEncoding, ISubtextContext context) : base(writer, dateLastViewedFeedItemPublished, useDeltaEncoding, context)
		{
		}

		protected override void Build()
		{
			if(!isBuilt)
			{
				Build(this.DateLastViewedFeedItemPublished);
			}
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
				WriteChannel();
				WriteEntries();
				EndDocument();
				isBuilt = true;
			}
		}

		protected virtual void SetNamespaces()
		{
			this.WriteAttributeString("xmlns:dc","http://purl.org/dc/elements/1.1/");
			this.WriteAttributeString("xmlns:trackback","http://madskills.com/public/xml/rss/module/trackback/");
			this.WriteAttributeString("xmlns:wfw","http://wellformedweb.org/CommentAPI/");
			this.WriteAttributeString("xmlns:slash","http://purl.org/rss/1.0/modules/slash/");
			//(Duncanma 11/13/2005, changing atom namespace for 1.0 feed)
			this.WriteAttributeString("xmlns","http://www.w3.org/2005/Atom");
			this.WriteAttributeString("xml:lang",Blog.Language);
		}

		protected virtual void StartDocument()
		{
			this.WriteStartElement("feed");
			//(Duncanma 11/13/2005, removing version attribute for 1.0 feed)
			//this.WriteAttributeString("version","0.3");
		}

		protected void EndDocument()
		{
			this.WriteEndElement();
		}

		protected virtual void WriteChannel()
		{
            Uri blogUrl = new Uri(UrlHelper.BlogUrl().ToFullyQualifiedUrl(Blog), "Default.aspx");
			BuildChannel(Blog.Title, blogUrl.ToString(), Blog.SubTitle);
		}

		protected void BuildChannel(string title, string link, string description)
		{
            this.WriteElementString("title", HtmlHelper.RemoveHtml(title));

            //(Duncanma 11/13/2005, changing link rel and href for 1.0 feed)
            this.WriteStartElement("link");
            //(Duncanma 12/28/2005, changing again... Atom vs atom was causing feed validation errors
            this.WriteAttributeString("rel", "self");
            this.WriteAttributeString("type", "application/atom+xml");
            string currentURL = link + "Atom.aspx";
            if (System.Web.HttpContext.Current.Request != null)
                currentURL = System.Web.HttpContext.Current.Request.Url.ToString();
            this.WriteAttributeString("href", currentURL);

            // this.WriteAttributeString("rel","self");
            // this.WriteAttributeString("type","application/xml");
            // this.WriteAttributeString("href",info.RootUrl + "atom.aspx");
            this.WriteEndElement();

            //(Duncanma 11/13/2005, changing tagline to subtitle for 1.0 feed)
            this.WriteStartElement("subtitle");
            this.WriteAttributeString("type", "html");
            this.WriteString(HtmlHelper.RemoveHtml(description));
            this.WriteEndElement();

            this.WriteElementString("id", link);

            this.WriteStartElement("author");
            this.WriteElementString("name", Blog.Author);

            Uri blogUrl = new Uri(UrlHelper.BlogUrl().ToFullyQualifiedUrl(Blog), "Default.aspx");
            this.WriteElementString("uri", blogUrl.ToString());
            this.WriteEndElement();

            //(Duncanma 11/13/05) updated generator to reflect project name change to Subtext
            this.WriteStartElement("generator");
            //(Duncanma 11/13/2005, changing url to uri for 1.0 feed)
            this.WriteAttributeString("uri", "http://subtextproject.com");
            this.WriteAttributeString("version", VersionInfo.VersionDisplayText);
            this.WriteString("Subtext");
            this.WriteEndElement();

            //(Duncanma 11/13/2005, changing modified to updated for 1.0 feed)
            this.WriteElementString("updated", W3UTCZ(Blog.LastUpdated));
		}

		private void WriteEntries()
		{
			BlogConfigurationSettings settings = Config.Settings;

			ClientHasAllFeedItems = true;
			LatestPublishDate = this.DateLastViewedFeedItemPublished;

			foreach(Entry entry in this.Items)
			{
				// We'll show every entry if RFC3229 is not enabled.
				//TODO: This is wrong.  What if a post is not published 
				// and then gets published later. It will not be displayed.
				if(!UseDeltaEncoding || entry.DateSyndicated > this.DateLastViewedFeedItemPublished)
				{
					this.WriteStartElement("entry");
					EntryXml(entry, settings, Blog.UrlFormats, Blog.TimeZone);
					this.WriteEndElement();
					ClientHasAllFeedItems = false;
					
					//Update the latest publish date.
					if(entry.DateSyndicated > LatestPublishDate)
					{
						LatestPublishDate = entry.DateSyndicated;
					}
				}
			}
		}

		protected virtual void EntryXml(Entry entry, BlogConfigurationSettings settings, UrlFormats urlFormats, TimeZone timezone)
		{
				this.WriteElementString("title",entry.Title);
						
				this.WriteStartElement("link");
				//(Duncanma 11/13/2005, changing alternate to self for 1.0 feed)
				this.WriteAttributeString("rel", "alternate");
				this.WriteAttributeString("type", "text/html");
				this.WriteAttributeString("href", UrlHelper.EntryUrl(entry).ToFullyQualifiedUrl(Blog).ToString());
				this.WriteEndElement();

				this.WriteElementString("id", UrlHelper.EntryUrl(entry).ToFullyQualifiedUrl(Blog).ToString());

				//(Duncanma 11/13/2005, hiding created, change issued to
			    //published and modified to updated for 1.0 feed)
				//this.WriteElementString("created",W3UTCZ(entry.DateCreated));
				this.WriteElementString("published", W3UTCZ(entry.DateCreated));
				this.WriteElementString("updated", W3UTCZ(entry.DateModified));

				if(entry.HasDescription)
				{
					this.WriteStartElement("summary");
					//(Duncanma 11/13/2005, changing text/html to html for 1.0 feed)
					this.WriteAttributeString("type", "html");
					this.WriteString(entry.Description);
					this.WriteEndElement();
				}

				this.WriteStartElement("content");
				//(Duncanma 11/13/2005, changing text/html to html for 1.0 feed)
				this.WriteAttributeString("type","html");
				//(Duncanma 11/13/2005, hiding mode for 1.0 feed)
				//this.WriteAttributeString("mode","escaped");
							
				this.WriteString
				(
					string.Format
					("{0}{1}", //tag def
						entry.SyndicateDescriptionOnly ? entry.Description : entry.Body,  //use desc or full post
						(UseAggBugs && settings.Tracking.EnableAggBugs) ? TrackingUrls.AggBugImage(urlFormats.AggBugUrl(entry.Id)) : null //use aggbugs
					)
				);		
				this.WriteEndElement();

			if(AllowComments && Blog.CommentsEnabled && entry.AllowComments && !entry.CommentingClosed)
			{
				//optional for CommentApi Post location
				this.WriteElementString("wfw:comment", UrlHelper.CommentApiUrl(entry.Id));
				//optional url for comments
				//this.WriteElementString("comments",entry.Link + "#feedback");
				//optional comment count
				this.WriteElementString("slash:comments", entry.FeedBackCount.ToString(CultureInfo.InvariantCulture));
				//optional commentRss feed location
				this.WriteElementString("wfw:commentRss", urlFormats.CommentRssUrl(entry.Id));
				//optional trackback location
				this.WriteElementString("trackback:ping", urlFormats.TrackBackUrl(entry.Id));
				//core 
			}
		}
	}
}



