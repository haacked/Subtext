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

            ddlUsePlainHtmlEditor.SelectedIndex = -1;
            ddlUsePlainHtmlEditor.Items.FindByValue(Preferences.UsePlainHtmlEditor ? "true" : "false").Selected = true;

            base.BindLocalUI();
        }

        protected void lkbUpdate_Click(object sender, EventArgs e)
        {
            bool published = Boolean.Parse(ddlPublished.SelectedItem.Value);
            Preferences.AlwaysCreateIsActive = published;

            bool alwaysExpand = Boolean.Parse(ddlExpandAdvanced.SelectedItem.Value);
            Preferences.AlwaysExpandAdvanced = alwaysExpand;

            bool usePlainHtmlEditor = Boolean.Parse(ddlUsePlainHtmlEditor.SelectedItem.Value);
            Preferences.UsePlainHtmlEditor = usePlainHtmlEditor;
        }
    }
}