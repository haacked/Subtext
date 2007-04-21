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
	public partial class settings : SubtextAdminGlobalSettingsBaseControl
	{

		public override void LoadSettings()
		{
            txbValue1.Text = CurrentPlugin.GetBlogSetting("value1");
            if (!String.IsNullOrEmpty(CurrentPlugin.GetBlogSetting("check")))
			{
                bool checkOn = Boolean.Parse(CurrentPlugin.GetBlogSetting("check"));
				chkOption.Checked = checkOn;
			}
            txbValue2.Text = CurrentPlugin.GetBlogSetting("value2");
		}

		public override void UpdateSettings()
		{
            CurrentPlugin.SetBlogSetting("value1", txbValue1.Text);
            CurrentPlugin.SetBlogSetting("value2", txbValue2.Text);
			if (chkOption.Checked)
                CurrentPlugin.SetBlogSetting("check", "true");
			else
                CurrentPlugin.SetBlogSetting("check", "false");
		}

	}
}