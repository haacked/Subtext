#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Web;
using Subtext.Framework.Configuration;
using Subtext.Framework.Format;
using Subtext.Framework.Web.HttpModules;
using Subtext.Framework.Providers;
using Subtext.Framework.Services;

namespace Subtext.Framework.Web.HttpModules
{
    /// <summary>
    /// Examines incoming http requests and adds Subtext specific as well as blog 
    /// specific information to the context.
    /// </summary>
    public class BlogRequestModule : IHttpModule
    {
        public BlogRequestModule() : this(new BlogLookupService(ObjectProvider.Instance(), HostInfo.Instance)) { 
        }

        public BlogRequestModule(IBlogLookupService blogLookup) {
            BlogLookup = blogLookup;
        }

        protected IBlogLookupService BlogLookup {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a module and prepares it to handle
        /// requests.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application</param>
        public void Init(HttpApplication context) {
            context.BeginRequest += MapUrlToBlogStatus;
        }
        
        /// <summary>
        /// Maps the incoming URL to the corresponding blog. If no blog matches, then 
        /// makes sure any host application settings are still set.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void MapUrlToBlogStatus(object sender, EventArgs e)
        {
            BlogRequest.Current = ConvertRequestToBlogRequest(new HttpContextWrapper(HttpContext.Current));
        }

        public BlogRequest ConvertRequestToBlogRequest(HttpContextBase httpContext) {
            // REVIEW: Maybe the BlogLookup.Lookup should take in an HttpContextBase 
            // and return the BlogRequest as part of the result.
            var blogRequest = new BlogRequest(httpContext.Request);
            if (blogRequest.RequestLocation == RequestLocation.Installation) {
                return blogRequest;
            }
            
            BlogLookupResult result = BlogLookup.Lookup(blogRequest);

            if (result == null) {
                if (blogRequest.RequestLocation != RequestLocation.LoginPage) {
                    httpContext.Response.Redirect("~/Install/BlogNotConfiguredError.aspx", true);
                }
                return null;
            }

            if (result.Blog == null && result.AlternateUrl != null) {
                var httpResponse = httpContext.Response;
                httpResponse.StatusCode = 301;
                httpResponse.Status = "301 Moved Permanently";
                httpResponse.RedirectLocation = result.AlternateUrl.ToString();
                httpResponse.End();
                return null;
            }

            //TODO: Check to see if bog is inactive.
            if (result.Blog != null && !result.Blog.IsActive && blogRequest.RequestLocation == RequestLocation.Blog) {
                httpContext.Response.Redirect("~/SystemMessages/BlogNotActive.aspx", true);
                return null;
            }

            blogRequest.Blog = result.Blog;
            return blogRequest;
        }

        /// <summary>
        /// Disposes of the resources (other than memory) used by the
        /// module that implements <see langword="IHttpModule."/>
        /// </summary>
        public void Dispose()
        {
            //Do Nothing.
        }
    }
}