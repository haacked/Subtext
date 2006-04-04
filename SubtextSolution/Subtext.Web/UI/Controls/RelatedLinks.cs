using System;

namespace Subtext.Web.UI.Controls
{
	using System.Collections;
	using System.Configuration;
	using System.Data;
	using System.Data.SqlClient;
	using System.Web.UI.WebControls;
	using Subtext.Common.Data;
	using Subtext.Framework;
	using Subtext.Framework.Components;
	using Subtext.Framework.Data;
	using Subtext.Framework.Providers;
	/// <summary>
	/// Summary description for RelatedLinks.
	/// </summary>
	public class RelatedLinks : BaseControl
	{
		protected Repeater urlRelatedLinks;

		private void Page_Load(object sender, EventArgs e)
		{
			string sql = "subtext_GetRelatedLinks";
			string conn = DbProvider.Instance().ConnectionString;

			ArrayList myRelLinks = new ArrayList();
			int BlogId;
			string applikasyon;

			//fix for the blogs where only one installed
			if (CurrentBlog.BlogId >= 1)
				BlogId = CurrentBlog.BlogId;
			else
				BlogId = 0;

			if (CurrentBlog.Subfolder != String.Empty)
				applikasyon = CurrentBlog.Subfolder;
			else
				applikasyon = String.Empty;

			Entry entry = Cacher.GetEntryFromRequest(CacheDuration.Short);

			//BlogId = entry.BlogId;
			int entryid = entry.EntryID;

			SqlParameter[] p =
				{
					SqlHelper.MakeInParam("@EntryID", SqlDbType.Int, 4, entryid),
					SqlHelper.MakeInParam("@BlogID", SqlDbType.Int, 4, BlogId)
				};

			DataTable dt = SqlHelper.ExecuteDataTable(conn, CommandType.StoredProcedure, sql, p);

			int count = dt.Rows.Count;

			for (int i = 0; i < count; i++)
			{
				DataRow dr = dt.Rows[i];

				string id = dr["EntryID"].ToString();
				string title = (string) dr["Title"];
				DateTime dateAdded = (DateTime) dr["DateAdded"];

				string myURL = URLFormat(applikasyon, dateAdded, id);

				myRelLinks.Add(new PositionItems(title, myURL));
			}

			urlRelatedLinks.DataSource = myRelLinks;
			urlRelatedLinks.DataBind();

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
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new EventHandler(this.Page_Load);
		}

		#endregion

		public string URLFormat(string dbApplication, DateTime dbDateAdded, string dbEntryID)
		{
			string myYear = dbDateAdded.Year.ToString();
			string myMonth = dbDateAdded.Month.ToString();
			string myDay = dbDateAdded.Day.ToString();

			int Month = int.Parse(myMonth);
			int Day = int.Parse(myDay);

			if (Month < 10)
			{
				myMonth = String.Concat("0", myMonth);
			}
			if (Day < 10)
			{
				myDay = String.Concat("0", myDay);
			}

			return CurrentBlog.BlogHomeVirtualUrl + "archive" + "/" + myYear + "/" + myMonth + "/" + myDay + "/" + dbEntryID + ".aspx";
		}
	}
	
	public class PositionItems
		{
			private string title;
			private string URL;

			public PositionItems(string title, string URL)
			{
				this.title = title;
				this.URL = URL;
			}

			public string Title
			{
				get { return title; }
			}

			public string url
			{
				get { return URL; }
			}
		}

}
