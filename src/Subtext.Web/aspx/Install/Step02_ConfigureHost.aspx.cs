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
using System.Web;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Security;

namespace Subtext.Web.Install
{
    /// <summary>
    /// Page used to create an initial configuration for the blog.
    /// </summary>
    /// <remarks>
    /// This page will ONLY be displayed if there are no 
    /// blog configurations within the database.
    /// </remarks>
    public partial class Step02_ConfigureHost : InstallationBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //We need to make sure that the form is ONLY displayed 
            //if there really is no Host record.
            hostForm.Visible = true;
            if (Repository.GetBlogCount() == 0)
            {
                ltlMessage.Text =
                    "<p>"
                    + "At this point, you may <strong>remove the database owner rights</strong> (dbo) from "
                    + "the database user account used to connect to the database.</p>";
            }
            else
            {
                ltlMessage.Text =
                    "<p>"
                    + "Welcome!  It appears that you have existing blogs, but no Host Admin account set up. "
                    + "We can remedy that situation quickly."
                    + "</p>"
                    + "<p>"
                    + "Just specify a username and password "
                    + "for the special Host Administrator account. "
                    + "This account can create blogs in this system. "
                    + "</p>";
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string userName = txtUserName.Text;
                string password = txtPassword.Text;
                string email = txtEmail.Text;

                // Create the HostInfo record.
                if (HostInfo.CreateHost(Repository, userName, password, email))
                {
                    if (Repository.GetBlogCount() == 0)
                    {
                        //Changed the following method to public so all authentication tickets are handled by the same code.
                        SubtextContext.HttpContext.SetAuthenticationTicket(Blog, "HostAdmin", false, "HostAdmin");
                        string queryString = !String.IsNullOrEmpty(txtEmail.Text)
                                                 ? "?email=" + HttpUtility.UrlEncode(txtEmail.Text)
                                                 : string.Empty;
                        Response.Redirect(NextStepUrl + queryString);
                    }
                    else
                    {
                        InstallationManager.ResetInstallationStatusCache();
                        Response.Redirect("InstallationComplete.aspx");
                    }
                }
                else
                {
                    const string errorMessage = "I'm sorry, but we had a problem creating your initial "
                                                +
                                                "configuration. Please <a href=\"http://code.google.com/p/subtext/issues/\" title=\"Subtext at Google Code\">report "
                                                + "this issue</a> to the Subtext team.";

                    throw new InvalidOperationException(errorMessage);
                }
            }
        }
    }
}