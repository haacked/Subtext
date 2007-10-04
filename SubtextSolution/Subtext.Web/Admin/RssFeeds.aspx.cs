using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Subtext.Web.Admin.WebUI;

namespace Subtext.Web.Admin.Pages
{
	public partial class RssFeeds : StatsPage
	{
		public RssFeeds()
		{
			this.TabSectionId = "Stats";
		}

		protected void Page_Load(object sender, EventArgs e)
		{
		}
	}
}
