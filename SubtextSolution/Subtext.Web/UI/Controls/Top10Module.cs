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
			if (CurrentBlog.BlogId >= 1)
				BlogId = CurrentBlog.BlogId;
			else
				BlogId = 0;

			string application = CurrentBlog.Subfolder;

			SqlParameter[] p =
				{
					SqlHelper.MakeInParam("@BlogID", SqlDbType.Int, 4, BlogId)
				};

			DataTable dt = SqlHelper.ExecuteDataTable(conn, CommandType.StoredProcedure, sql, p);

			int count = dt.Rows.Count;

			for (int i = 0; i < count; i++)
			{
				DataRow dr = dt.Rows[i];

				string title = (string) dr["title"];

				DateTime dateAdded = (DateTime) dr["DateAdded"];
				string id = dr["EntryId"].ToString();

				string myURL = URLFormat(application, dateAdded, id);

				myLastItems.Add(new PositionItems(title, myURL));

			}

			Top10Entries.DataSource = myLastItems;
			Top10Entries.DataBind();
		}

		public string URLFormat(string dbApplication, DateTime dbDateAdded, string dbEntryID)
		{
			string myURL = ConfigurationSettings.AppSettings["AggregateURL"];

			myURL = myURL + "/" + dbApplication + "/";

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

			myURL = myURL + "archive" + "/" + myYear + "/" + myMonth + "/" + myDay + "/" + dbEntryID + ".aspx";

			return myURL;
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