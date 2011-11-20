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
using System.Web.Mvc;
using Ninject;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Services;
using Subtext.Framework.Web.HttpModules;

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
        [Inject]
        public IEntryPublisher EntryPublisher
        {
            get;
            set;
        }

        protected HostInfo HostInfo
        {
            get
            {
                return Host.Value;
            }
        }

        [Inject]
        public LazyNotNull<HostInfo> Host
        {
            get;
            set;
        }

        protected override void OnLoad(EventArgs e)
        {
            btnQuickCreate.Attributes["onclick"] = "this.disabled=true;"
                                                   +
                                                   ClientScript.GetPostBackEventReference(btnQuickCreate, "");
            hostAdminlink.NavigateUrl = Url.HostAdminUrl("default.aspx");
            base.OnLoad(e);
        }

        protected void btnQuickCreate_Click(object sender, EventArgs e)
        {
            var hostInfo = HostInfo ?? HostInfo.LoadHostInfoFromDatabase(Repository, true);

            // Create the blog_config record using default values 
            // and the specified user info

            //Since the password is stored as a hash, let's not hash it again.
            const bool passwordAlreadyHashed = true;

            string subfolder = hostInfo.BlogAggregationEnabled ? "blog" : "";

            int blogId = Repository.CreateBlog("TEMPORARY BLOG NAME", hostInfo.HostUserName, hostInfo.Password,
                                           Request.Url.Host, subfolder, passwordAlreadyHashed);
            if (blogId > -1)
            {
                var blog = Repository.GetBlogById(blogId);

                BlogRequest.Current.Blog = blog;
                // Need to refresh the context now that we have a blog.
                SubtextContext = DependencyResolver.Current.GetService<ISubtextContext>();
                if (!String.IsNullOrEmpty(hostInfo.Email))
                {
                    blog.Email = hostInfo.Email;
                    Repository.UpdateConfigData(blog);
                }
                InstallationManager.CreateWelcomeContent(SubtextContext, EntryPublisher, Blog);


                //We probably should have creating the blog authenticate the user 
                //automatically so this redirect doesn't require a login.
                InstallationManager.ResetInstallationStatusCache();
                Response.Redirect(Url.BlogUrl());
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