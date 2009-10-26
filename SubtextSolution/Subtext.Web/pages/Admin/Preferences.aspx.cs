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
    public partial class EditPreferences : AdminOptionsPage
    {
        protected override void BindLocalUI()
        {
            ddlPublished.SelectedIndex = -1;
            ddlPublished.Items.FindByValue(Preferences.AlwaysCreateIsActive ? "true" : "false").Selected = true;

            ddlExpandAdvanced.SelectedIndex = -1;
            ddlExpandAdvanced.Items.FindByValue(Preferences.AlwaysExpandAdvanced ? "true" : "false").Selected = true;

            chkAutoGenerate.Checked = Config.CurrentBlog.AutoFriendlyUrlEnabled;

            base.BindLocalUI();
        }

        protected void lkbUpdate_Click(object sender, EventArgs e)
        {
            bool published = Boolean.Parse(ddlPublished.SelectedItem.Value);
            Preferences.AlwaysCreateIsActive = published;

            bool alwaysExpand = Boolean.Parse(ddlExpandAdvanced.SelectedItem.Value);
            Preferences.AlwaysExpandAdvanced = alwaysExpand;

            Blog info = Config.CurrentBlog;
            info.AutoFriendlyUrlEnabled = chkAutoGenerate.Checked;
            Repository.UpdateConfigData(info);
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
        }

        #endregion
    }
}