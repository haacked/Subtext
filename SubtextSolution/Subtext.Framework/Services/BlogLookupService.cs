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
using Subtext.Framework.Providers;
using Subtext.Framework.Web.HttpModules;
using Subtext.Framework.Configuration;
using Subtext.Extensibility.Interfaces;

namespace Subtext.Framework.Services
{
    public class BlogLookupService : IBlogLookupService
    {
        public BlogLookupService(ObjectProvider repository, HostInfo host) {
            Repository = repository;
            Host = host;
        }

        protected ObjectProvider Repository {
            get;
            private set;
        }

        protected HostInfo Host {
            get;
            private set;
        }

        public BlogLookupResult Lookup(BlogRequest blogRequest)
        {
            string host = blogRequest.Host;
            Blog blog = Repository.GetBlog(host, blogRequest.Subfolder, true /* strict */);
            if (blog != null) {
                return new BlogLookupResult(blog, null);
            }
                
            string alternateHost = GetAlternateHostAlias(host);
            blog = Repository.GetBlog(alternateHost, blogRequest.Subfolder, true /* strict */);
            if (blog != null) {
                Uri alternateUrl = ReplaceHost(blogRequest.RawUrl, alternateHost).Uri;
                return new BlogLookupResult(null, alternateUrl);
            }
            
            blog = Repository.GetBlogByDomainAlias(host, blogRequest.Subfolder, true /* strict */);
            if (blog != null) {
                UriBuilder alternateUrl = ReplaceHost(blogRequest.RawUrl, blog.Host);
                alternateUrl = ReplaceSubfolder(alternateUrl, blogRequest, blog.Subfolder);
                return new BlogLookupResult(null, alternateUrl.Uri);
            }

            int totalBlogCount = GetBlogCount();
            if (Host.BlogAggregationEnabled && totalBlogCount > 0) {
                return new BlogLookupResult(Host.AggregateBlog, null);
            }

            // Special case. If there's only one blog in the system, we'll return it.
            // Makes it easier for development.
            if (totalBlogCount == 1) {
                blog = Repository.GetBlog(host, blogRequest.Subfolder, false /* strict */);
                //Extra special case to deal with a common deployment problem where dev uses "localhost" on 
                //dev machine. But deploys to real domain.
                if (!String.Equals("localhost", host, StringComparison.OrdinalIgnoreCase) 
                    && String.Equals("localhost", blog.Host, StringComparison.OrdinalIgnoreCase)) {
                    blog.Host = host;
                    Repository.UpdateBlog(blog);
                }

                return new BlogLookupResult(blog, null);
            }

            return null;
        }

        private int GetBlogCount() {
            IPagedCollection pagedBlogs = Repository.GetPagedBlogs(null, 0, 10, ConfigurationFlags.None);
            return pagedBlogs.MaxItems;
        }

        private UriBuilder ReplaceHost(Uri originalUrl, string newHost) {
            UriBuilder builder = new UriBuilder(originalUrl);
            builder.Host = newHost;
            return builder;
        }

        private UriBuilder ReplaceSubfolder(UriBuilder originalUrl, BlogRequest blogRequest, string newSubfolder) {
            if (!String.Equals(blogRequest.Subfolder, newSubfolder, StringComparison.OrdinalIgnoreCase)) {
                if (!String.IsNullOrEmpty(blogRequest.Subfolder)) {
                    originalUrl.Path = originalUrl.Path.Remove(0, blogRequest.Subfolder.Length + 1);
                }
                if (!String.IsNullOrEmpty(newSubfolder)) {
                    originalUrl.Path = "/" + newSubfolder + originalUrl.Path;
                }
            }
            return originalUrl;
        }

        /// <summary>
        /// If the host starts with www., gets the host without the www. If it 
        /// doesn't start with www., returns the host with www.
        /// </summary>
        /// <param name="host">Host.</param>
        /// <returns></returns>
        private string GetAlternateHostAlias(string host)
        {
            if (String.IsNullOrEmpty(host))
                throw new ArgumentException("Cannot get an alternative alias to a null host", "host");

            if (host.StartsWith("www.", StringComparison.OrdinalIgnoreCase))
                return host.Substring(4);
            else
                return "www." + host;
        }
    }
}
