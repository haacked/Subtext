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
using Subtext.Web.Admin.Pages;
using Subtext.Web.Properties;

namespace Subtext.Web.Admin.Articles
{
    public partial class Default : AdminPage
    {
        public Default()
        {
            TabSectionId = "Articles";
        }

        protected override void OnLoad(EventArgs e)
        {
            entries.HeaderText = Resources.Label_Articles;
            string message = Request.QueryString["message"];
            if (!string.IsNullOrEmpty(message))
            {
                Messages.ShowMessage(message);
            }
            base.OnLoad(e);
        }
    }
}