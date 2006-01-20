#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// .Text WebLog
// 
// .Text is an open source weblog system started by Scott Watermasysk. 
// Blog: http://ScottWater.com/blog 
// RSS: http://scottwater.com/blog/rss.aspx
// Email: Dottext@ScottWater.com
//
// For updated news and information please visit http://scottwater.com/dottext and subscribe to 
// the Rss feed @ http://scottwater.com/dottext/rss.aspx
//
// On its release (on or about August 1, 2003) this application is licensed under the BSD. However, I reserve the 
// right to change or modify this at any time. The most recent and up to date license can always be fount at:
// http://ScottWater.com/License.txt
// 
// Please direct all code related questions to:
// GotDotNet Workspace: http://www.gotdotnet.com/Community/Workspaces/workspace.aspx?id=e99fccb3-1a8c-42b5-90ee-348f6b77c407
// Yahoo Group http://groups.yahoo.com/group/DotText/
// 
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
	/// Generates RSS
	/// </summary>
	public class BaseAtomWriter : BaseSyndicationWriter
	{

		#region TimeHelpers
		//Maybe move to globals/util?
		private string TimeZone(int tz)
		{
			if(tz < 0)
			{
				return tz.ToString("00") + ":00";
			}
			else
			{
				return "+" + tz.ToString("00") + ":00";
			}

		}

		private string W3UTC(DateTime dt, string tz)
		{
			return dt.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture) + tz;
		}

		private string W3UTCZ(DateTime dt)
		{
			return dt.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
		}
		#endregion

		private bool isBuilt = false;

		/// <summary>
		/// Bases the syndication writer.
		/// </summary>
		/// <param name="dateLastViewedFeedItemPublished">Last viewed feed item.</param>
		protected BaseAtomWriter(DateTime dateLastViewedFeedItemPublished, bool useDeltaEncoding) : base(dateLastViewedFeedItemPublished, useDeltaEncoding)
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
			this.WriteAttributeString("xml:lang",info.Language);
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
			BuildChannel(info.Title, info.BlogHomeUrl, info.SubTitle);
			
		}

		protected void BuildChannel(string title, string link, string description)
		{
			this.WriteElementString("title",title);	

			//(Duncanma 11/13/2005, changing link rel and href for 1.0 feed)
			this.WriteStartElement("link");
			//(Duncanma 12/28/2005, changing again... Atom vs atom was causing feed validation errors
				this.WriteAttributeString("rel","self");
				this.WriteAttributeString("type","application/xml");
				string currentURL = link + "Atom.aspx";
				if (System.Web.HttpContext.Current.Request != null)
					currentURL = System.Web.HttpContext.Current.Request.Url.ToString();
				this.WriteAttributeString("href",currentURL);

//				this.WriteAttributeString("rel","self");
//				this.WriteAttributeString("type","application/xml");
//				this.WriteAttributeString("href",info.RootUrl + "atom.aspx");
			this.WriteEndElement();

			//(Duncanma 11/13/2005, changing tagline to subtitle for 1.0 feed)
			this.WriteStartElement("subtitle");
				this.WriteAttributeString("type","html");
				this.WriteString(description);
			this.WriteEndElement();

			this.WriteElementString("id",link);

			this.WriteStartElement("author");
				this.WriteElementString("name", info.Author);
				//(Duncanma 11/13/2005, changing url to uri for 1.0 feed)
				this.WriteElementString("uri", info.BlogHomeUrl);
			this.WriteEndElement();
			
			//(Duncanma 11/13/05) updated generator to reflect project name change to Subtext
			this.WriteStartElement("generator");
				//(Duncanma 11/13/2005, changing url to uri for 1.0 feed)
				this.WriteAttributeString("uri","http://subtextproject.com");
				this.WriteAttributeString("version", VersionInfo.VersionDisplayText);
				this.WriteString("Subtext");
			this.WriteEndElement();

			//(Duncanma 11/13/2005, changing modified to updated for 1.0 feed)
			this.WriteElementString("updated",W3UTCZ(info.LastUpdated));
		}

		private void WriteEntries()
		{
			BlogConfigurationSettings settings = Config.Settings;

			string timezone = TimeZone(info.TimeZone);
			this.clientHasAllFeedItems = true;
			this.latestPublishDate = this.DateLastViewedFeedItemPublished;

			foreach(Entry entry in this.Entries)
			{
				// We'll show every entry if RFC3229 is not enabled.
				//TODO: This is wrong.  What if a post is not published 
				// and then gets published later. It will not be displayed.
				if(!useDeltaEncoding || entry.DateSyndicated > this.DateLastViewedFeedItemPublished)
				{
					this.WriteStartElement("entry");
					EntryXml(entry, settings, info.UrlFormats, timezone);
					this.WriteEndElement();
					this.clientHasAllFeedItems = false;
					
					//Update the latest publish date.
					if(entry.DateSyndicated > base.latestPublishDate)
					{
						base.latestPublishDate = entry.DateSyndicated;
					}
				}
			}
		}

		protected virtual void EntryXml(Entry entry, BlogConfigurationSettings settings, UrlFormats uformat, string timezone)
		{
				this.WriteElementString("title",entry.Title);
						
				this.WriteStartElement("link");
					//(Duncanma 11/13/2005, changing alternate to self for 1.0 feed)
					this.WriteAttributeString("rel","self");
					this.WriteAttributeString("type","text/html");
					this.WriteAttributeString("href",entry.Link);
				this.WriteEndElement();

				this.WriteElementString("id",entry.Link);

				//(Duncanma 11/13/2005, hiding created, change issued to
			    //published and modified to updated for 1.0 feed)
				//this.WriteElementString("created",W3UTCZ(entry.DateCreated));
				this.WriteElementString("published",W3UTC(entry.DateCreated.AddHours((-1) * info.TimeZone),timezone));
				this.WriteElementString("updated",W3UTCZ(entry.DateUpdated));

				if(entry.HasDescription)
				{
					this.WriteStartElement("summary");
						//(Duncanma 11/13/2005, changing text/html to html for 1.0 feed)
						this.WriteAttributeString("type","html");
						this.WriteString(entry.Description);
					this.WriteEndElement();
				}

				this.WriteStartElement("content");
					//(Duncanma 11/13/2005, changing text/html to html for 1.0 feed)
					this.WriteAttributeString("type","html");
					//(Duncanma 11/13/2005, hiding mode for 1.0 feed)
					//this.WriteAttributeString("mode","escaped");
							
				this.WriteString(
				string.Format
					("{0}{1}", //tag def
						entry.SyndicateDescriptionOnly ? entry.Description : entry.Body,  //use desc or full post
						(UseAggBugs && settings.Tracking.EnableAggBugs) ? TrackingUrls.AggBugImage(uformat.AggBugkUrl(entry.EntryID)) : null //use aggbugs
					)
				);		
				this.WriteEndElement();

			if(AllowComments && info.CommentsEnabled && entry.AllowComments && !entry.CommentingClosed)
			{
				//optional for CommentApi Post location
				this.WriteElementString("wfw:comment",uformat.CommentApiUrl(entry.EntryID));
				//optional url for comments
				//this.WriteElementString("comments",entry.Link + "#feedback");
				//optional comment count
				this.WriteElementString("slash:comments",entry.FeedBackCount.ToString(CultureInfo.InvariantCulture));
				//optional commentRss feed location
				this.WriteElementString("wfw:commentRss", uformat.CommentRssUrl(entry.EntryID));
				//optional trackback location
				this.WriteElementString("trackback:ping",uformat.TrackBackUrl(entry.EntryID));
				//core 
			}
		}

	}
}

