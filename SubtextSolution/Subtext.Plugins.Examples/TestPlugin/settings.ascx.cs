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

		public override void LoadSettings()
		{
			txbValue1.Text = GetSetting("value1");
			if (!String.IsNullOrEmpty(GetSetting("check")))
			{
				bool checkOn = Boolean.Parse(GetSetting("check"));
				chkOption.Checked = checkOn;
			}
			txbValue2.Text = GetSetting("value2");
		}

		public override void UpdateSettings()
		{
			SetSetting("value1",txbValue1.Text);
			SetSetting("value2", txbValue2.Text);
			if (chkOption.Checked)
				SetSetting("check", "true");
			else
				SetSetting("check", "false");
		}

	}
}