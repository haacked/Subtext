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
using System.Web.UI;

namespace Subtext.Web
{
    /// <summary>
    /// This page presents useful information to users connecting 
    /// to the blog via "localhost".  In otherwords, on a local 
    /// installation.
    /// </summary>
    public partial class CheckYourConnectionString : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Remote users do not get the extra information.
            if (Request.IsLocal)
            {
                plcDiagnosticInfo.Visible = true;

                Exception exception = Server.GetLastError();
                Exception baseException = null;
                if (exception != null)
                {
                    baseException = exception.GetBaseException();
                }

                if (baseException != null)
                {
                    lblErrorMessage.Text = baseException.Message;
                    lblStackTrace.Text = baseException.StackTrace;
                }
                else
                {
                    lblErrorMessage.Text = "Nothing to report. There was no error.";
                }
            }
        }
    }
}