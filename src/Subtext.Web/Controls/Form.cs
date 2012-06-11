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

// Adapted from Nikhil's blog post: http://www.nikhilk.net/Entry.aspx?id=182

using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Subtext.Web.Controls
{
    public class Form : HtmlForm
    {
        private const string FormActionScript =
    @"document.getElementById('{0}').action = window.location.href;
";

        private const string AjaxFormActionScript =
    @"Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function() {{
  document.getElementById('{0}').action = window.location.href;
}});
";

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            var sb = new StringBuilder();
            sb.AppendFormat(FormActionScript, ClientID);

            ScriptManager scriptManager = ScriptManager.GetCurrent(Page);
            if ((scriptManager != null) && scriptManager.SupportsPartialRendering)
            {
                sb.AppendFormat(AjaxFormActionScript, ClientID);
            }

            Page.ClientScript.RegisterStartupScript(typeof(Form), String.Empty, sb.ToString(), true/*addScriptTags*/);
        }
    }
}
