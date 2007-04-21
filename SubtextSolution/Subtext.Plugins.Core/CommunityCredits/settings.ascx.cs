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
            if (Convert.ToBoolean(CurrentPlugin.GetDefaultSetting("AllowSingleBlogOverride")))
				overridableConfiguration.Visible = true;
			else
				notOverridableConfiguration.Visible = true;

            globalAffiliateCode.Text = CurrentPlugin.GetDefaultSetting("AffiliateCode");
            globalAffiliateKey.Text = CurrentPlugin.GetDefaultSetting("AffiliateKey");
		}

		public override void LoadSettings()
		{
            affiliateCode.Text = CurrentPlugin.GetBlogSetting("AffiliateCode");
            affiliateKey.Text = CurrentPlugin.GetBlogSetting("AffiliateKey");

			bindEffectiveCodes();
		}

		public override void UpdateSettings()
		{
            CurrentPlugin.SetBlogSetting("AffiliateCode", affiliateCode.Text);
            CurrentPlugin.SetBlogSetting("AffiliateKey", affiliateKey.Text);

			bindEffectiveCodes();
		}

		private void bindEffectiveCodes()
		{
			if (String.IsNullOrEmpty(affiliateCode.Text))
                effectiveAffiliateCode.Text = CurrentPlugin.GetDefaultSetting("AffiliateCode");
			else
				effectiveAffiliateCode.Text = affiliateCode.Text;

			if (String.IsNullOrEmpty(affiliateKey.Text))
                effectiveAffiliateKey.Text = CurrentPlugin.GetDefaultSetting("AffiliateKey");
			else
				effectiveAffiliateKey.Text = affiliateKey.Text;
		}

	}
}