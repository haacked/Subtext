using System.Web;
using System.Web.Routing;
using Subtext.Framework.Components;
using System;
using System.Web.Hosting;
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

        public virtual VirtualPath EntryUrl(Entry entry) {
            if (entry == null) {
                throw new ArgumentNullException("entry");
            }

            if (NullValue.IsNull(entry.Id)) {
                return null;
            }

            RouteValueDictionary routeValues = new RouteValueDictionary(new {
                year = entry.DateCreated.ToString("yyyy"),
                month = entry.DateCreated.ToString("MM"),
                day = entry.DateCreated.ToString("dd")
            });

            string routeName;
            if (string.IsNullOrEmpty(entry.EntryName)) {
                routeValues.Add("id", entry.Id);
                routeName = "entry-by-id";
            }
            else {
                routeValues.Add("slug", entry.EntryName);
                routeName = "entry-by-slug";
            }
            
            var virtualPath = Routes.GetVirtualPath(_requestContext, routeName, routeValues);
            if (virtualPath != null) {
                return virtualPath.VirtualPath;
            }
            return null;
        }

        public virtual VirtualPath AggBugUrl(int id) {
            return GetVirtualPath("aggbug", new { id = id });
        }

        public virtual VirtualPath ResolveUrl(string virtualPath) {
            return _requestContext.HttpContext.ExpandTildePath(virtualPath);
        }

        public virtual VirtualPath BlogUrl() {
            string vp = GetVirtualPath("root", new { });
            if (!vp.EndsWith("/")) {
                vp += "/";
            }
            return vp;
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

        public VirtualPath GetVirtualPath(string routeName, object routeValues) {
            var virtualPath = Routes.GetVirtualPath(_requestContext, routeName, new RouteValueDictionary(routeValues));
            if (virtualPath == null) {
                return null;
            }
            return virtualPath.VirtualPath;
        }
    }
}