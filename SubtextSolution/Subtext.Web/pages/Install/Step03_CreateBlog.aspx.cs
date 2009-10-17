#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using log4net;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Infrastructure.Installation;
using Subtext.Framework.Logging;

namespace Subtext.Web.Install
{
    /// <summary>
    /// Page used to create an initial configuration for the blog.
    /// </summary>
    /// <remarks>
    /// This page will ONLY be displayed if there are no 
    /// blog configurations within the database.
    /// </remarks>
    public partial class Step03_CreateBlog : InstallationBase
    {
        static ILog log = new Log();

        protected override void OnLoad(EventArgs e)
        {
            btnQuickCreate.Attributes["onclick"] = "this.disabled=true;"
                                                   +
                                                   ClientScript.GetPostBackEventReference(btnQuickCreate, "").ToString();
            base.OnLoad(e);
        }

        protected void btnQuickCreate_Click(object sender, EventArgs e)
        {
            // Create the blog_config record using default values 
            // and the specified user info

            //Since the password is stored as a hash, let's not hash it again.
            const bool passwordAlreadyHashed = true;

            if(
                Config.CreateBlog("TEMPORARY BLOG NAME", HostInfo.Instance.HostUserName, HostInfo.Instance.Password,
                                  Request.Url.Host, string.Empty, passwordAlreadyHashed) > -1)
            {
                if(!String.IsNullOrEmpty(Request.QueryString["email"]))
                {
                    Blog blog = Config.GetBlog(Request.Url.Host, string.Empty);
                    blog.Email = Request.QueryString["email"];
                    Repository.UpdateConfigData(blog);
                }
                //We probably should have creating the blog authenticate the user 
                //automatically so this redirect doesn't require a login.
                var installManager = new InstallationManager(InstallationProvider.Provider);
                installManager.ResetInstallationStatusCache();
                Response.Redirect("~/Admin/Configure.aspx");
            }
            else
            {
                string errorMessage = "I'm sorry, but we had a problem creating your initial "
                                      +
                                      "configuration. Please <a href=\"http://sourceforge.net/tracker/?group_id=137896&atid=739979\">report "
                                      + "this issue</a> to the Subtext team.";

                //TODO: Pick a non-generic exception.
                throw new InvalidOperationException(errorMessage);
            }
        }
    }
}