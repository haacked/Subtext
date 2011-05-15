#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using Subtext.Framework;
using Subtext.Framework.Configuration;

namespace Subtext.Web.Admin.Pages
{
    /// <summary>
    /// Admin Page used to set syndication settings.
    /// </summary>
    public partial class Syndication : AdminOptionsPage
    {
        // abstract out at a future point for i18n
        private const string RES_FAILURE = "Syndication settings update failed.";
        private const string RES_SUCCESS = "Your syndication settings were successfully updated.";

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (!IsPostBack)
            {
                PopulateForm();
            }
        }

        private void PopulateForm()
        {
            Blog info = Config.CurrentBlog;

            chkEnableSyndication.Checked = info.IsAggregated;
            chkUseDeltaEncoding.Checked = info.RFC3229DeltaEncodingEnabled;
            chkUseSyndicationCompression.Checked = info.UseSyndicationCompression;
            txtFeedBurnerName.Text = info.RssProxyUrl;
            txtLicenseUrl.Text = info.LicenseUrl;
        }

        private void SaveSettings()
        {
            try
            {
                UpdateConfiguration();
                Messages.ShowMessage(RES_SUCCESS);
            }
            catch (Exception ex)
            {
                Messages.ShowError(String.Format(Constants.RES_EXCEPTION, RES_FAILURE, ex.Message));
            }
        }

        private void UpdateConfiguration()
        {
            Blog info = Config.CurrentBlog;

            info.IsAggregated = chkEnableSyndication.Checked;
            info.UseSyndicationCompression = chkUseSyndicationCompression.Checked;
            info.RFC3229DeltaEncodingEnabled = chkUseDeltaEncoding.Checked;
            info.RssProxyUrl = txtFeedBurnerName.Text;
            info.LicenseUrl = txtLicenseUrl.Text;

            Repository.UpdateConfigData(info);
        }

        protected void lkbPost_Click(object sender, EventArgs e)
        {
            SaveSettings();
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Page.Load += new EventHandler(Page_Load);
        }

        #endregion
    }
}