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
using Subtext.Extensibility.Plugins;

namespace Subtext.Web.HostAdmin
{
	public partial class PluginList : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			pluginList.DataSource = SubtextApplication.Current.Plugins.Values;
			loadingErrorList.DataSource = SubtextApplication.Current.PluginLoadingErrors;
			DataBind();
		}
	}
}
