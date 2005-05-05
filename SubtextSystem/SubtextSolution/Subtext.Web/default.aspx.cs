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
using System.Web.UI;
using System.Web.UI.WebControls;
using Subtext.Framework.Data;
using Subtext.Framework.Providers;

namespace Subtext.Web
{
	/// <summary>
	/// Summary description for _default.
	/// </summary>
	public class _default : Page
	{

		protected Repeater Bloggers;
		protected Literal BlogCount;
		protected Literal PostCount;
		protected Literal StoryCount;
		protected Literal CommentCount;
		protected Literal PingtrackCount;
		protected Repeater RecentPosts;
		protected HyperLink HyperLink1;
		protected HyperLink OpmlLink;
		protected HyperLink RssLink;
		protected HyperLink TitleLink;
		protected Literal Style;
		protected HyperLink Hyperlink6;
		protected HyperLink Hyperlink7;
		protected HyperLink Hyperlink4;
		protected HyperLink Hyperlink5;
		protected Literal TitleTag;

		private void Page_Load(object sender, EventArgs e)
		{
			
			TitleTag.Text = TitleLink.Text = ConfigurationSettings.AppSettings["AggregateTitle"] as string;
			TitleLink.NavigateUrl = ConfigurationSettings.AppSettings["AggregateUrl"] as string;


			//No postbacks on this page. It is output cached.
			BindData();
			SetStyle();
		}

		protected string GetEntryUrl(string host, string app, string entryName, DateTime dt)
		{
			
			return string.Format("{0}archive/{1}/{2}.aspx",GetFullUrl(host,app),dt.ToString("yyyy/MM/dd"),entryName);
		}

		private void SetStyle()
		{
			const string style = "<link href=\"{0}{1}\" type=\"text/css\" rel=\"stylesheet\">";
			string apppath = Request.ApplicationPath.EndsWith("/") ? Request.ApplicationPath : Request.ApplicationPath + "/";
			Style.Text = string.Format(style,apppath,"Style.css") + "\n" + string.Format(style,apppath,"blue.css");

		}

		private string appPath = null;
		const string fullUrl = "http://{0}{1}{2}/";
		protected string GetFullUrl(string host, string app)
		{
			if(appPath == null)
			{
				appPath = Request.ApplicationPath;
				if(!appPath.ToLower().EndsWith("/"))
				{
					appPath += "/";
				}
			}
			return string.Format(fullUrl,host,appPath,app);

		}

		private void BindData()
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
			string sql = "DNW_HomePageData";
			
			string conn = DbProvider.Instance().ConnectionString;


			SqlParameter[] p = 
				{
					SqlHelper.MakeInParam("@Host",SqlDbType.NVarChar,100,ConfigurationSettings.AppSettings["AggregateHost"] as string),
					SqlHelper.MakeInParam("@GroupID",SqlDbType.Int,4,GroupID)
				};

					DataSet ds = SqlHelper.ExecuteDataset(conn,CommandType.StoredProcedure,sql,p);

			Bloggers.DataSource = ds.Tables[0];
			RecentPosts.DataSource = ds.Tables[1];

			DataTable dtCounts = ds.Tables[2];
			if(dtCounts != null)
			{
				DataRow dr = dtCounts.Rows[0];
				BlogCount.Text = dr["BlogCount"].ToString();
				PostCount.Text = dr["PostCount"].ToString();
				StoryCount.Text = dr["StoryCount"].ToString();
				CommentCount.Text = dr["CommentCount"].ToString();
				PingtrackCount.Text = dr["PingtrackCount"].ToString();

			}

			Bloggers.DataBind();
			RecentPosts.DataBind();

			ds.Clear();
			ds.Dispose();

		}

		protected string FormatDate(string date)
		{
			DateTime dt = DateTime.Parse(date);
			return dt.ToString("MMddyyyy");
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
			this.Load += new EventHandler(this.Page_Load);

		}
		#endregion
	}
}

