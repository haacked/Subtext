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
using System.Linq;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework.Configuration;
using Subtext.Framework.Providers;
using Subtext.Framework.Web.HttpModules;

namespace Subtext.Framework.Services
{
    public class BlogLookupService : IBlogLookupService
    {
        HostInfo _host;

        public BlogLookupService(ObjectProvider repository, HostInfo host)
        {
            Repository = repository;
            _host = host;
        }

        protected ObjectProvider Repository { get; private set; }

        protected HostInfo Host
        {
            get
            {
                if(_host == null)
                {
                    _host = HostInfo.LoadHost(true);
                }
                return _host;
            }
        }

        public BlogLookupResult Lookup(BlogRequest blogRequest)
        {
            if(Host == null)
            {
                return new BlogLookupResult(null, null);
            }

            string host = blogRequest.Host;
            Blog blog = Repository.GetBlog(host, blogRequest.Subfolder);
            if(blog != null)
            {
                if(!String.Equals(host, blog.Host, StringComparison.OrdinalIgnoreCase)
                   || !String.Equals(blogRequest.Subfolder, blog.Subfolder, StringComparison.OrdinalIgnoreCase))
                {
                    UriBuilder alternateUrl = ReplaceHost(blogRequest.RawUrl, blog.Host);
                    alternateUrl = ReplaceSubfolder(alternateUrl, blogRequest, blog.Subfolder);
                    return new BlogLookupResult(null, alternateUrl.Uri);
                }
                return new BlogLookupResult(blog, null);
            }

            IPagedCollection<Blog> pagedBlogs = Repository.GetPagedBlogs(null, 0, 10, ConfigurationFlags.None);
            int totalBlogCount = pagedBlogs.MaxItems;
            if(Host.BlogAggregationEnabled && totalBlogCount > 0)
            {
                if(!String.IsNullOrEmpty(blogRequest.Subfolder))
                {
                    return null;
                }
                return new BlogLookupResult(Host.AggregateBlog, null);
            }

            if(totalBlogCount == 1)
            {
                Blog onlyBlog = pagedBlogs.First();
                if(onlyBlog.Host == blogRequest.Host)
                {
                    Uri onlyBlogUrl =
                        ReplaceSubfolder(new UriBuilder(blogRequest.RawUrl), blogRequest, onlyBlog.Subfolder).Uri;
                    return new BlogLookupResult(null, onlyBlogUrl);
                }

                //Extra special case to deal with a common deployment problem where dev uses "localhost" on 
                //dev machine. But deploys to real domain.
                if(OnlyBlogIsLocalHostNotCurrentHost(host, onlyBlog))
                {
                    onlyBlog.Host = host;
                    Repository.UpdateBlog(onlyBlog);

                    if(onlyBlog.Subfolder != blogRequest.Subfolder)
                    {
                        Uri onlyBlogUrl =
                            ReplaceSubfolder(new UriBuilder(blogRequest.RawUrl), blogRequest, onlyBlog.Subfolder).Uri;
                        return new BlogLookupResult(null, onlyBlogUrl);
                    }
                    return new BlogLookupResult(onlyBlog, null);
                }

                //return new BlogLookupResult(blog, null);
            }

            return null;
        }

        private static bool OnlyBlogIsLocalHostNotCurrentHost(string host, Blog onlyBlog)
        {
            return (
                       !String.Equals("localhost", host, StringComparison.OrdinalIgnoreCase)
                       && String.Equals("localhost", onlyBlog.Host, StringComparison.OrdinalIgnoreCase)
                   )
                   || (
                          !String.Equals("127.0.0.1", host, StringComparison.OrdinalIgnoreCase)
                          && String.Equals("127.0.0.1", onlyBlog.Host, StringComparison.OrdinalIgnoreCase)
                      );
        }

        private static UriBuilder ReplaceHost(Uri originalUrl, string newHost)
        {
            var builder = new UriBuilder(originalUrl) {Host = newHost};
            return builder;
        }

        private static UriBuilder ReplaceSubfolder(UriBuilder originalUrl, BlogRequest blogRequest, string newSubfolder)
        {
            if(!String.Equals(blogRequest.Subfolder, newSubfolder, StringComparison.OrdinalIgnoreCase))
            {
                string appPath = blogRequest.ApplicationPath;
                if(!appPath.EndsWith("/"))
                {
                    appPath += "/";
                }

                int indexAfterAppPath = appPath.Length;
                if(!String.IsNullOrEmpty(blogRequest.Subfolder))
                {
                    originalUrl.Path = originalUrl.Path.Remove(indexAfterAppPath, blogRequest.Subfolder.Length + 1);
                }
                if(!String.IsNullOrEmpty(newSubfolder))
                {
                    originalUrl.Path = originalUrl.Path.Substring(0, indexAfterAppPath) + newSubfolder + "/" +
                                       originalUrl.Path.Substring(indexAfterAppPath);
                }
            }
            return originalUrl;
        }
    }
}