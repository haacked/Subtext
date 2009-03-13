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
using System.Configuration;
using System.Web;
using System.Web.Routing;
using Subtext.Extensibility;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework.Components;
using Subtext.Framework.Web;

namespace Subtext.Framework.Routing
{
    public class UrlHelper {
        protected UrlHelper() { 
        }

        public UrlHelper(RequestContext context, RouteCollection routes) {
            _requestContext = context ?? new RequestContext(new HttpContextWrapper(HttpContext.Current), new RouteData());
            Routes = routes ?? RouteTable.Routes;
        }

        RequestContext _requestContext;

        public RouteCollection Routes {
            get;
            private set;
        }

        public virtual VirtualPath AppRoot() {
            string appRoot = _requestContext.HttpContext.Request.ApplicationPath;
            if (!appRoot.EndsWith("/")) {
                appRoot += "/";
            }

            if (!appRoot.StartsWith("/")) {
                throw new InvalidOperationException("Whoa! An assumption was broken.");
            }
            return appRoot;
        }
        
        public virtual VirtualPath FeedbackUrl(FeedbackItem comment) {
            if (comment == null) {
                throw new ArgumentNullException("comment");
            }
            string entryUrl = EntryUrl(comment.Entry);
            if (string.IsNullOrEmpty(entryUrl)) {
                return null;
            }
            return entryUrl + "#" + comment.Id;
        }

        public virtual VirtualPath EntryUrl(IEntryIdentity entry)
        {
            if (entry == null) {
                throw new ArgumentNullException("entry");
            }
            if (entry.PostType == PostType.None) {
                throw new ArgumentException("Entry must have a valid PostType", "entry");
            }

            if (NullValue.IsNull(entry.Id)) {
                return null;
            }

            string routeName;
            RouteValueDictionary routeValues = new RouteValueDictionary();

            if (entry.PostType == PostType.BlogPost) {
                routeValues.Add("year", entry.DateCreated.ToString("yyyy"));
                routeValues.Add("month", entry.DateCreated.ToString("MM"));
                routeValues.Add("day", entry.DateCreated.ToString("dd"));
                routeName = "entry-";
            }
            else {
                routeName = "article-";
            }
            
            if (string.IsNullOrEmpty(entry.EntryName)) {
                routeValues.Add("id", entry.Id);
                routeName += "by-id";
            }
            else {
                routeValues.Add("slug", entry.EntryName);
                routeName += "by-slug";
            }
            
            var virtualPath = Routes.GetVirtualPath(_requestContext, routeName, routeValues);
            if (virtualPath != null) {
                return virtualPath.VirtualPath;
            }
            return null;
        }

        public virtual VirtualPath ImageUrl(Image image) {
            return GetVirtualPath("gallery-image", new { id = image.ImageID });
        }

        public virtual VirtualPath GalleryUrl(int id) {
            return GetVirtualPath("gallery", new { id = id });
        }

        public virtual VirtualPath AggBugUrl(int id) {
            return GetVirtualPath("aggbug", new { id = id });
        }

        public virtual VirtualPath ResolveUrl(string virtualPath) {
            return _requestContext.HttpContext.ExpandTildePath(virtualPath);
        }

        public virtual VirtualPath BlogUrl() {
            string vp = GetVirtualPath("root", new {});
            if (!(vp ?? string.Empty).EndsWith("/")) {
                vp += "/";
            }
            return vp;
        }

        public virtual VirtualPath BlogUrl(Blog blog)
        {
            string vp = GetVirtualPath("root", new { subfolder = blog.Subfolder });
            if (!(vp ?? string.Empty).EndsWith("/")) {
                vp += "/";
            }
            return vp;
        }

        public virtual VirtualPath ContactFormUrl() {
            return GetVirtualPath("contact", null);
        }

        public virtual VirtualPath MonthUrl(DateTime dateTime) {
            return GetVirtualPath("entries-by-month", new { year = dateTime.ToString("yyyy"), month = dateTime.ToString("MM") });
        }

        public virtual VirtualPath CommentApiUrl(int entryId) {
            return GetVirtualPath("comment-api", new { id = entryId });
        }

        public virtual VirtualPath CommentRssUrl(int entryId) {
            return GetVirtualPath("comment-rss", new { id = entryId });
        }

        public virtual VirtualPath TrackbacksUrl(int entryId) {
            return GetVirtualPath("trackbacks", new { id = entryId });
        }

        public virtual VirtualPath CategoryUrl(Category category) {
            return GetVirtualPath("category", new { slug = category.Id, categoryType = "category" });
        }

        public virtual VirtualPath CategoryRssUrl(Category category)
        {
            return GetVirtualPath("rss", new { catId = category.Id });
        }

        /// <summary>
        /// Returns the url for all posts on the day specified by the date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public virtual VirtualPath DayUrl(DateTime date) {
            return GetVirtualPath("entries-by-day", 
                new { 
                    year = date.ToString("yyyy"), 
                    month = date.ToString("MM"), 
                    day = date.ToString("dd") 
                });
        }

        /// <summary>
        /// Returns the url for all posts on the day specified by the date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public virtual Uri RssUrl(Blog blog) {
            if (blog.RssProxyEnabled) {
                return RssProxyUrl(blog);
            }

            return GetVirtualPath("rss", null).ToFullyQualifiedUrl(blog);
        }

        /// <summary>
        /// Returns the url for all posts on the day specified by the date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public virtual Uri AtomUrl(Blog blog) {
            if (blog.RssProxyEnabled) {
                return RssProxyUrl(blog);
            }

            return GetVirtualPath("atom", null).ToFullyQualifiedUrl(blog);
        }

        public virtual Uri RssProxyUrl(Blog blog) {
            //TODO: Store this in db.
            string feedburnerUrl = ConfigurationManager.AppSettings["FeedBurnerUrl"];
            feedburnerUrl = String.IsNullOrEmpty(feedburnerUrl) ? "http://feedproxy.google.com/" : feedburnerUrl;
            return new Uri(new Uri(feedburnerUrl), blog.RssProxyUrl);
        }

        public virtual VirtualPath GetVirtualPath(string routeName, object routeValues) {
            RouteValueDictionary routeValueDictionary;

            if (routeValues is RouteValueDictionary) {
                routeValueDictionary = (RouteValueDictionary)routeValues;
            }
            else {
                routeValueDictionary = new RouteValueDictionary(routeValues);    
            }

            var virtualPath = Routes.GetVirtualPath(_requestContext, routeName, routeValueDictionary);
            if (virtualPath == null) {
                return null;
            }
            return virtualPath.VirtualPath;
        }

        public virtual VirtualPath LoginUrl() {
            return GetVirtualPath("login", new { });
        }

        public virtual VirtualPath LogoutUrl() {
            return GetVirtualPath("logout", new { });
        }

        public virtual VirtualPath ArchivesUrl() {
            return GetVirtualPath("archives", new { });
        }
        
        public virtual VirtualPath AdminUrl(string path) {
            return AdminUrl(path, null);
        }
                
        public virtual VirtualPath AdminUrl(string path, object routeValues) {
            var routeValuesDict = new RouteValueDictionary(routeValues);
            routeValuesDict.Add("pathinfo", path);
            return GetVirtualPath("admin", routeValuesDict);
        }

        public virtual VirtualPath AdminRssUrl(string feedName)
        {
            return GetVirtualPath("admin-rss", new { feedName = feedName });
        }

        public virtual Uri MetaWeblogApiUrl(Blog blog) {
            var vp = GetVirtualPath("metaweblogapi", null);
            return vp.ToFullyQualifiedUrl(blog);
        }

        public virtual Uri RsdUrl(Blog blog) {
            var vp = GetVirtualPath("rsd", null);
            return vp.ToFullyQualifiedUrl(blog);
        }

        public virtual VirtualPath CustomCssUrl() {
            return GetVirtualPath("customcss", null);
        }

        public virtual VirtualPath EditIconUrl() {
            return AppRoot() + "images/edit.gif";
        }

        public virtual VirtualPath TagUrl(string tagName) {
            return GetVirtualPath("tag", new { tag = tagName });
        }

        public virtual VirtualPath TagCloudUrl() {
            return GetVirtualPath("tag-cloud", null);
        }
    }
}