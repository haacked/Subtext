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
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI.WebControls;
using Subtext.Framework.Data;
using Subtext.Framework.Providers;

namespace Subtext.Web.UI.Controls
{
	/// <summary>
	///		Summary description for subTextSearch.
	/// </summary>
	public class SubtextSearch : BaseControl
	{
		protected TextBox txtSearch;
		protected Button btnSearch;

		protected Repeater SearchResults;

		private void Page_Load(object sender, EventArgs e)
		{
			txtSearch.Attributes.Add("onkeypress", "return clickButton(event,'" + btnSearch.ClientID + "')");
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
			this.btnSearch.Click += new EventHandler(this.btnSearch_Click);

		}

		#endregion

		public void btnSearch_Click(object sender, EventArgs e)
		{
			if(string.Empty != txtSearch.Text )
			{
				string storedProc = "subtext_SearchEntries";
				string connStr = DbProvider.Instance().ConnectionString;
				string searchString = txtSearch.Text.ToString();
				ArrayList mySearchItems = new ArrayList();

				//fix for the blogs where only one installed
				int blogID = 0;
				if (CurrentBlog.Id > 0)
					blogID = CurrentBlog.Id;

				SqlParameter[] p =
				{
					SqlHelper.MakeInParam("@BlogId", SqlDbType.Int, 4, blogID),
					SqlHelper.MakeInParam("@SearchStr", searchString)
				};

				DataTable dt = SqlHelper.ExecuteDataTable(connStr, CommandType.StoredProcedure, storedProc, p);

				int count = dt.Rows.Count;

				for (int i = 0; i < count; i++)
				{
					DataRow dr = dt.Rows[i];

					string entryId = ((int) dr["id"]).ToString(CultureInfo.InvariantCulture);
					string entryTitle = (string) dr["Title"];
					DateTime dateCreated = (DateTime) dr["DateAdded"];
					string entryUrl = CurrentBlog.UrlFormats.EntryFullyQualifiedUrl(dateCreated, entryId);

					mySearchItems.Add(new PositionItems(entryTitle, entryUrl));
				}

				SearchResults.DataSource = mySearchItems;
				SearchResults.DataBind();
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
}
