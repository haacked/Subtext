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
using System.Collections.Specialized;
using Subtext.Extensibility.Plugins;

namespace Subtext.Plugins.Examples.TestPlugin
{
	public partial class settings : STAdminGlobalSettingsBaseControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if(BlogSettings["value1"]!=null)
				txbValue1.Text = BlogSettings["value1"];
			if (BlogSettings["check"]!=null)
			{
				bool checkOn = Boolean.Parse(BlogSettings["check"]);
				chkOption.Checked = checkOn;
			}
			if (BlogSettings["value2"] != null)
				txbValue2.Text = BlogSettings["value2"];
		}
	}
}