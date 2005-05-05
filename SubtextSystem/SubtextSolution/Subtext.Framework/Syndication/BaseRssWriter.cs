using System;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Format;
using Subtext.Framework.Tracking;

namespace Subtext.Framework.Syndication
{
	/// <summary>
	/// Summary description for BaseRssWriter.
	/// </summary>
	public abstract class BaseRssWriter : BaseSyndicationWriter
	{
		public BaseRssWriter():base()
		{
		}

		private bool isBuilt = false;

		protected override void Build()
		{
			if(!isBuilt)
			{
				StartDocument();
				SetNamespaces();
				StartChannel();
				WriteChannel();
				WalkEntries();
				EndChannel();
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
			
			BuildChannel(config.Title,config.FullyQualifiedUrl,config.Author,config.SubTitle,config.Language);
		}

		protected void BuildChannel(string title, string link, string author, string description, string lang)
		{
			this.WriteElementString("title",title);			
			this.WriteElementString("link",link);
			this.WriteElementString("description",description);
			this.WriteElementString("managingEditor",author);
			this.WriteElementString("dc:language",lang);
			this.WriteElementString("generator",VersionInfo.Version);
		}

		protected void EndChannel()
		{
			this.WriteEndElement();
		}

		private void WalkEntries()
		{
			//IUrlFormat format = UrlFormat.Instance();
			BlogConfigurationSettings settings = Config.Settings;
			foreach(Entry entry in this.Entries)
			{
				this.WriteStartElement("item");

				EntryXml(entry,settings,config.UrlFormats);

				this.WriteEndElement();
			}
		}

		protected virtual void EntryXml(Entry entry, BlogConfigurationSettings settings, UrlFormats uformat)
		{
			this.WriteElementString("dc:creator",entry.Author);
			//core
			this.WriteElementString("title",entry.Title);
			//core
			this.WriteElementString("link",entry.Link);
			this.WriteElementString("pubDate",entry.DateCreated.ToString("r"));
			//core Should we set the 
			this.WriteElementString("guid",entry.Link);

			if(AllowComments && config.EnableComments && entry.AllowComments)
			{
				//optional for CommentApi Post location
				this.WriteElementString("wfw:comment",uformat.CommentApiUrl(entry.EntryID));
				//optional url for comments
				this.WriteElementString("comments",entry.Link + "#Feedback");
				//optional comment count
				this.WriteElementString("slash:comments",entry.FeedBackCount.ToString());
				//optional commentRss feed location
				this.WriteElementString("wfw:commentRss", uformat.CommentRssUrl(entry.EntryID));
				//optional trackback location
				this.WriteElementString("trackback:ping",uformat.TrackBackUrl(entry.EntryID));
				//core 
			}

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
