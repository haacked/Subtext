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
			return dt.ToString("yyyy-MM-ddTHH:mm:ss") + tz;
		}

		private string W3UTCZ(DateTime dt)
		{
			
			return dt.ToString("yyyy-MM-ddTHH:mm:ssZ");
		}
		#endregion

		private bool isBuilt = false;

		protected override void Build()
		{
			if(!isBuilt)
			{
				StartDocument();
				SetNamespaces();
				WriteChannel();
				WalkEntries();
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
			this.WriteAttributeString("xmlns","http://purl.org/atom/ns#");
			this.WriteAttributeString("xml:lang",config.Language);
		}

		protected virtual void StartDocument()
		{
			this.WriteStartElement("feed");
			this.WriteAttributeString("version","0.3");
		}

		protected void EndDocument()
		{
			this.WriteEndElement();
		}

		protected virtual void WriteChannel()
		{
			
			BuildChannel(config.Title,config.FullyQualifiedUrl,config.SubTitle);
		}

		protected void BuildChannel(string title, string link, string description)
		{
			this.WriteElementString("title",title);	

			this.WriteStartElement("link");
				this.WriteAttributeString("rel","alternate");
				this.WriteAttributeString("type","text/html");
				this.WriteAttributeString("href",link);
			this.WriteEndElement();

			this.WriteStartElement("tagline");
				this.WriteAttributeString("type","text/html");
				this.WriteString(description);
			this.WriteEndElement();

			this.WriteElementString("id",link);

			this.WriteStartElement("author");
				this.WriteElementString("name",config.Author);
				this.WriteElementString("url",config.FullyQualifiedUrl);
			this.WriteEndElement();

			this.WriteStartElement("generator");
				this.WriteAttributeString("url","http://scottwater.com/blog");
				this.WriteAttributeString("version",VersionInfo.Version);
				this.WriteString(".Text");
			this.WriteEndElement();

			this.WriteElementString("modified",W3UTCZ(config.LastUpdated));
		}

		private void WalkEntries()
		{
			//IUrlFormat format = UrlFormat.Instance();
			BlogConfigurationSettings settings = Config.Settings;

			string timezone = TimeZone(config.TimeZone);
			foreach(Entry entry in this.Entries)
			{
				this.WriteStartElement("entry");

				EntryXml(entry,settings,config.UrlFormats,timezone);

				this.WriteEndElement();
			}
		}

		protected virtual void EntryXml(Entry entry, BlogConfigurationSettings settings, UrlFormats uformat, string timezone)
		{
				this.WriteElementString("title",entry.Title);
						
				this.WriteStartElement("link");
					this.WriteAttributeString("rel","alternate");
					this.WriteAttributeString("type","text/html");
					this.WriteAttributeString("href",entry.Link);
				this.WriteEndElement();

				this.WriteElementString("id",entry.Link);

				this.WriteElementString("created",W3UTCZ(entry.DateCreated));
				this.WriteElementString("issued",W3UTC(entry.DateCreated.AddHours((-1) * config.TimeZone),timezone));
				this.WriteElementString("modified",W3UTCZ(entry.DateUpdated));

				if(entry.HasDescription)
				{
					this.WriteStartElement("summary");
						this.WriteAttributeString("type","text/html");
						this.WriteString(entry.Description);
					this.WriteEndElement();
				}

				this.WriteStartElement("content");
					this.WriteAttributeString("type","text/html");
					this.WriteAttributeString("mode","escaped");
							
				this.WriteString(
				string.Format
					("{0}{1}", //tag def
						entry.SyndicateDescriptionOnly ? entry.Description : entry.Body,  //use desc or full post
						(UseAggBugs && settings.Tracking.EnableAggBugs) ? TrackingUrls.AggBugImage(uformat.AggBugkUrl(entry.EntryID)) : null //use aggbugs
					)
				);		
				this.WriteEndElement();

			if(AllowComments && config.EnableComments && entry.AllowComments)
			{
				//optional for CommentApi Post location
				this.WriteElementString("wfw:comment",uformat.CommentApiUrl(entry.EntryID));
				//optional url for comments
				//this.WriteElementString("comments",entry.Link + "#Feedback");
				//optional comment count
				this.WriteElementString("slash:comments",entry.FeedBackCount.ToString());
				//optional commentRss feed location
				this.WriteElementString("wfw:commentRss", uformat.CommentRssUrl(entry.EntryID));
				//optional trackback location
				this.WriteElementString("trackback:ping",uformat.TrackBackUrl(entry.EntryID));
				//core 
			}
		}

	}
}

