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
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;
using Subtext.Framework.Syndication.Compression;

namespace Subtext.Web
{
	/// <summary>
	/// This class writes out an rss feed.
	/// </summary>
	public class RSSPage : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			int GroupID = 1;

			if(Request.QueryString["GroupID"] !=null)
			{
				try
				{
					GroupID = Int32.Parse(Request.QueryString["GroupID"]);
				}
				catch{}

			}			
			string sql = "DNW_GetRecentPosts";
			string conn = Subtext.Framework.Providers.DbProvider.Instance().ConnectionString;

			SqlParameter[] p = 
				{
					SqlHelper.MakeInParam("@Host",SqlDbType.NVarChar,100,ConfigurationSettings.AppSettings["AggregateHost"] as string),
					SqlHelper.MakeInParam("@GroupID",SqlDbType.Int,4,GroupID)
				};

			DataTable feedData = SqlHelper.ExecuteDataTable(conn,CommandType.StoredProcedure,sql,p);

			
			if(feedData != null && feedData.Rows.Count > 0)
			{
				string rssXml = GetRSS(feedData,Request.ApplicationPath);
				string encoding = null;
	
				if(Config.Settings.UseSyndicationCompression && this.AcceptEncoding != null)
				{
					SyndicationCompressionFilter filter = null;
		
					filter = SyndicationCompressionHelper.GetFilterForScheme(this.AcceptEncoding, Response.Filter);

					if(filter != null)
					{
						encoding = filter.ContentEncoding;
						Response.Filter = filter.Filter;
						Response.AppendHeader("Content-Encoding", encoding);
					}
				}

				if(encoding == null)
				{
					Response.ContentEncoding = System.Text.Encoding.UTF8;
				}

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

				//writer.WriteStartDocument();

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
				writer.WriteElementString("title",ConfigurationSettings.AppSettings["AggregateTitle"] as string);
				writer.WriteElementString("link",Context.Request.Url.ToString());
				writer.WriteElementString("description",ConfigurationSettings.AppSettings["AggregateDescription"] as string);
				writer.WriteElementString("generator",Subtext.Framework.VersionInfo.Version);

				int count = dt.Rows.Count;
				int servertz = Config.Settings.ServerTimeZone;
				string baseUrl = "http://{0}" + appPath + "{1}/";

				bool useAggBugs = Config.Settings.Tracking.EnableAggBugs;

				for(int i = 0; i< count; i++)
				{
					DataRow dr = dt.Rows[i];

					writer.WriteStartElement("item");
					writer.WriteElementString("title",(string)dr["Title"]);

					string baselink = string.Format(baseUrl,(string)dr["Host"],(string)dr["Application"]);
					string link = string.Format(baselink + "archive/{0}/{1}.aspx",((DateTime)dr["DateAdded"]).ToString("yyyy/MM/dd"),dr["EntryName"]);
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
					writer.WriteElementString("slash:comments",dr["FeedBackCount"].ToString());
					writer.WriteElementString("trackback:ping",string.Format(baselink + "services/trackbacks/{0}.aspx",dr["ID"]));


					writer.WriteStartElement("source");
					writer.WriteAttributeString("url",baselink + "rss.aspx");
					writer.WriteString((string)dr["BlogTitle"]);
					writer.WriteEndElement();

					string desc = (string)dr["Description"];

					string aggText = useAggBugs ? Subtext.Framework.Tracking.TrackingUrls.AggBugImage(string.Format(baselink + "aggbug/{0}.aspx",dr["ID"])) : string.Empty;

					writer.WriteElementString("description",string.Format("{0}{1}",desc,aggText));
			
					if(dr["IsXHTML"] != DBNull.Value && (bool)dr["IsXHTML"])
					{

						writer.WriteStartElement("body");
						writer.WriteAttributeString("xmlns","http://www.w3.org/1999/xhtml");
						writer.WriteRaw((string)dr["Text"]);
						writer.WriteEndElement();

					}
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
				throw e;
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

