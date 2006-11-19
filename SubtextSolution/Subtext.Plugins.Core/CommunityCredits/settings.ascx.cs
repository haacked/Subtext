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

namespace Subtext.Plugins.Core.CommunityCredits
{
	public partial class settings : SubtextAdminGlobalSettingsBaseControl
	{

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if (Convert.ToBoolean(CurrentPlugin.DefaultSettings["AllowSingleBlogOverride"]))
				overridableConfiguration.Visible = true;
			else
				notOverridableConfiguration.Visible = true;

			globalAffiliateCode.Text = CurrentPlugin.DefaultSettings["AffiliateCode"];
			globalAffiliateKey.Text = CurrentPlugin.DefaultSettings["AffiliateKey"];
		}

		public override void LoadSettings()
		{
			affiliateCode.Text = GetSetting("AffiliateCode");
			affiliateKey.Text = GetSetting("AffiliateKey");

			bindEffectiveCodes();
		}

		public override void UpdateSettings()
		{
			SetSetting("AffiliateCode", affiliateCode.Text);
			SetSetting("AffiliateKey", affiliateKey.Text);

			bindEffectiveCodes();
		}

		private void bindEffectiveCodes()
		{
			if (String.IsNullOrEmpty(affiliateCode.Text))
				effectiveAffiliateCode.Text = CurrentPlugin.DefaultSettings["AffiliateCode"];
			else
				effectiveAffiliateCode.Text = affiliateCode.Text;

			if (String.IsNullOrEmpty(affiliateKey.Text))
				effectiveAffiliateKey.Text = CurrentPlugin.DefaultSettings["AffiliateKey"];
			else
				effectiveAffiliateKey.Text = affiliateKey.Text;
		}

	}
}