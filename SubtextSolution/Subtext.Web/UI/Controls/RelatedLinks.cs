using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Data;
using Subtext.Framework.Providers;

namespace Subtext.Web.UI.Controls
{
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

			//fix for the blogs where only one installed
			if (CurrentBlog.Id >= 1)
				BlogId = CurrentBlog.Id;
			else
				BlogId = 0;

			Entry entry = Cacher.GetEntryFromRequest(CacheDuration.Short);

			int entryid = entry.Id;

			SqlParameter[] p =
				{
					DataHelper.MakeInParam("@EntryID", SqlDbType.Int, 4, entryid),
					DataHelper.MakeInParam("@BlogID", SqlDbType.Int, 4, BlogId)
				};

			DataTable dt = DataHelper.ExecuteDataTable(conn, CommandType.StoredProcedure, sql, p);

			int count = dt.Rows.Count;

			for (int i = 0; i < count; i++)
			{
				DataRow dr = dt.Rows[i];

				string id = dr["EntryID"].ToString();
				string title = (string) dr["Title"];
				DateTime dateAdded = (DateTime) dr["DateAdded"];

				string myURL = CurrentBlog.UrlFormats.EntryFullyQualifiedUrl(dateAdded, id);

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
