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
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Xml;
using Subtext.Framework.Configuration;
using Subtext.Framework.Providers;

namespace Subtext.Web
{
	/// <summary>
	/// This class writes out a consolidated rss feed for every blog in the system. 
	/// This is used by hosted solutions that contain an aggregate blog.
	/// </summary>
	public class RSSPage : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			int groupId = 1;

			if(Request.QueryString["GroupID"] !=null)
			{
				try
				{
					groupId = Int32.Parse(Request.QueryString["GroupID"]);
				}
				catch{}

			}

            DataTable feedData = DbProvider.Instance().GetAggregateRecentPosts(groupId);
			
		    //TODO: Use our other feed generation code.
			if(feedData != null && feedData.Rows.Count > 0)
			{
				string rssXml = GetRSS(feedData, Request.ApplicationPath);		
				Response.ContentEncoding = System.Text.Encoding.UTF8;
				Response.ContentType = "text/xml";
				Response.Write(rssXml);
			}
		}

		private string GetRSS(DataTable dt, string appPath)
		{
			if(!appPath.EndsWith("/"))
			{
				appPath += "/";
			}
			try
			{
				StringWriter sw = new StringWriter();
				XmlTextWriter writer = new XmlTextWriter(sw);

				//RSS ROOT
				writer.WriteStartElement("rss");
				writer.WriteAttributeString("version","2.0");
				writer.WriteAttributeString("xmlns:dc","http://purl.org/dc/elements/1.1/");
				writer.WriteAttributeString("xmlns:trackback","http://madskills.com/public/xml/rss/module/trackback/");
				writer.WriteAttributeString("xmlns:wfw","http://wellformedweb.org/CommentAPI/");
				writer.WriteAttributeString("xmlns:slash","http://purl.org/rss/1.0/modules/slash/");

				//Channel
				writer.WriteStartElement("channel");
				//Channel Description
                writer.WriteElementString("title", ConfigurationManager.AppSettings["AggregateTitle"]);
				writer.WriteElementString("link",Context.Request.Url.ToString());
                writer.WriteElementString("description", ConfigurationManager.AppSettings["AggregateDescription"]);
				
				//CHANGE: FrameworkVersion used insted of Version which is not exist.
				writer.WriteElementString("generator",Subtext.Framework.VersionInfo.FrameworkVersion.ToString());

				int count = dt.Rows.Count;
				int servertz = Config.Settings.ServerTimeZone;
				string baseUrl = "http://{0}" + appPath + "{1}/";

				bool useAggBugs = Config.Settings.Tracking.EnableAggBugs;

				for(int i = 0; i< count; i++)
				{
					DataRow dr = dt.Rows[i];

					writer.WriteStartElement("item");
					writer.WriteElementString("title", (string)dr["Title"]);

					string baselink = string.Format(baseUrl, dr["Host"], dr["Application"]);
					string link = string.Format(CultureInfo.InvariantCulture, baselink + "archive/{0:yyyy/MM/dd}/{1}.aspx", ((DateTime)dr["DateAdded"]), dr["EntryName"]);
					writer.WriteElementString("link",link);

					DateTime time = (DateTime)dr["DateAdded"];
					int tz = (int)dr["TimeZone"];
					int offset = (servertz - tz);
					
					
					writer.WriteElementString("pubDate",(time.AddHours(offset)).ToUniversalTime().ToString("r"));
					//writer.WriteElementString("guid",link);
					writer.WriteStartElement("guid");
					writer.WriteAttributeString("isPermaLink","true");
					writer.WriteString(link);
					writer.WriteEndElement();

					writer.WriteElementString("wfw:comment",string.Format(baselink + "comments/{0}.aspx",dr["ID"]));
					writer.WriteElementString("wfw:commentRss", string.Format(baselink + "comments/commentRss/{0}.aspx",dr["ID"]));
					writer.WriteElementString("comments",link + "#comment");
					int feedbackCount = 0;
					if(dr["FeedBackCount"] != DBNull.Value)
					{
						feedbackCount = (int)dr["FeedBackCount"];
					}
					writer.WriteElementString("slash:comments", feedbackCount.ToString(CultureInfo.InvariantCulture));
					writer.WriteElementString("trackback:ping",string.Format(baselink + "services/trackbacks/{0}.aspx",dr["ID"]));


					writer.WriteStartElement("source");
					writer.WriteAttributeString("url",baselink + "rss.aspx");
					writer.WriteString((string)dr["BlogTitle"]);
					writer.WriteEndElement();

					string desc = (string)dr["Description"];

					string aggText = useAggBugs ? Subtext.Framework.Tracking.TrackingUrls.AggBugImage(string.Format(baselink + "aggbug/{0}.aspx",dr["ID"])) : string.Empty;

					writer.WriteElementString("description", string.Format("{0}{1}", desc, aggText));
					writer.WriteElementString("dc:creator",(string)dr["Author"]);	
					writer.WriteEndElement();
				
				}
				writer.WriteEndElement();
				
				writer.WriteEndElement();
				writer.Flush();
				writer.Close();
				sw.Close();
				return sw.ToString();

			}
			catch(Exception e)
			{
				throw;
			}
		}

		/// <summary>
		/// Returns the "Accept-Encoding" value from the HTTP Request header. 
		/// This is a list of encodings that may be sent to the browser.
		/// </summary>
		/// <remarks>
		/// Specifically we're looking for gzip.
		/// </remarks>
		/// <value></value>
		protected string AcceptEncoding
		{
			get
			{
				return Request.Headers["Accept-Encoding"];
			}
		}
		
		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}

