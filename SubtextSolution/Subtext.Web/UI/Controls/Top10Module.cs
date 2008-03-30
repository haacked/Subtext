using System;

namespace Subtext.Web.UI.Controls
{
	using System.Collections;
	using System.Configuration;
	using System.Data;
	using System.Data.SqlClient;
	using System.Web.UI.WebControls;
	using Subtext.Framework.Data;
	using Subtext.Framework.Providers;

	/// <summary>
	/// Summary description for Top10Module.
	/// </summary>
	public class Top10Module : BaseControl
	{
		protected Repeater Top10Entries;

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			string sql = "subtext_GetTop10byBlogId";
			string conn = DbProvider.Instance().ConnectionString;
			int BlogId;

			ArrayList myLastItems = new ArrayList();

			//fix for the blogs where only one installed
			if (CurrentBlog.Id >= 1)
				BlogId = CurrentBlog.Id;
			else
				BlogId = 0;

			SqlParameter[] p =
				{
					DataHelper.MakeInParam("@BlogID", SqlDbType.Int, 4, BlogId)
				};

			DataTable dt = DataHelper.ExecuteDataTable(conn, CommandType.StoredProcedure, sql, p);

			int count = dt.Rows.Count;

			for (int i = 0; i < count; i++)
			{
				DataRow dr = dt.Rows[i];

				string title = (string) dr["title"];

				DateTime dateAdded = (DateTime) dr["DateAdded"];
				string id = dr["EntryId"].ToString();

				string myURL = CurrentBlog.UrlFormats.EntryFullyQualifiedUrl(dateAdded, id);

				myLastItems.Add(new PositionItems(title, myURL));

			}

			Top10Entries.DataSource = myLastItems;
			Top10Entries.DataBind();
		}

	}

	public class PositionTopItems
	{
		private string title;
		private string URL;

		public PositionTopItems(string title, string URL)
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
