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
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace Subtext.Web.Admin.Pages
{
    /// <summary>
    /// Will cause the user to confirm navigating away from the current page. This behavior can be overriden.
    /// </summary>
    public class ConfirmationPage : AdminPage
    {
        const string scriptEnd = "';    }  }  function bypassCheck() {     g_blnCheckUnload  = false;   }</script>";

        const string scriptStart =
            "<script type=\"text/javascript\">g_blnCheckUnload = true;function RunOnBeforeUnload() {if (g_blnCheckUnload) {window.event.returnValue = '";

        public static readonly string BypassFunctionName = "bypassCheck();";

        private bool _IsInEdit = false;

        private string _message = "You will lose any non-saved text";

        public bool IsInEdit
        {
            get { return _IsInEdit; }
            set { _IsInEdit = value; }
        }

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        protected override void OnPreRender(EventArgs e)
        {
            //If we are in edit mode, register the script
            if (IsInEdit)
            {
                Type ctype = GetType();
                Page.ClientScript.RegisterClientScriptBlock(ctype
                                                            , "ConfirmationBeforeLeaving"
                                                            , string.Format(CultureInfo.InvariantCulture
                                                                            , "{0}{1}{2}"
                                                                            , scriptStart
                                                                            , Message
                                                                            , scriptEnd));
            }
            base.OnPreRender(e);
        }


        protected override void Render(HtmlTextWriter writer)
        {
            //If we are in edit mode, wire up the onbeforeunload event
            if (IsInEdit)
            {
                TextWriter tempWriter = new StringWriter();
                base.Render(new HtmlTextWriter(tempWriter));
                writer.Write(Regex.Replace(tempWriter.ToString(), "<body",
                                           "<body onbeforeunload=\"RunOnBeforeUnload()\"", RegexOptions.IgnoreCase));
            }
            else
            {
                base.Render(writer);
            }
        }
    }
}