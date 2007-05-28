using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Subtext.Framework.Components;
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

			IList<RelatedLink> topEntries = ObjectProvider.Instance().GetTopLinks(10);

			Top10Entries.DataSource = topEntries;
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
