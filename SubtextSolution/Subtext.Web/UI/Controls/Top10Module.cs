using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using Subtext.Framework.Data;
using Subtext.Framework.Providers;

namespace Subtext.Web.UI.Controls
{
	/// <summary>
	/// Summary description for Top10Module.
	/// </summary>
	public class Top10Module : BaseControl
	{
		protected Repeater Top10Entries;

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

            int blogId = Blog.Id >= 1 ? Blog.Id : 0;
			ArrayList myLastItems = new ArrayList();

			var entrySummaries = ObjectProvider.Instance().GetTopEntrySummaries(blogId, 10);

			foreach(var entrySummary in entrySummaries)
			{
				string title = entrySummary.Title;
                string myURL = Url.EntryUrl(entrySummary);

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
