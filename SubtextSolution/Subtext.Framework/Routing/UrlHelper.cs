using System.Web;
using System.Web.Routing;
using Subtext.Framework.Components;
using System;
using System.Web.Hosting;
using Subtext.Framework.Web;
using Subtext.Extensibility;

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
            string vp = GetVirtualPath("root", new { });
            if (!vp.EndsWith("/")) {
                vp += "/";
            }
            return vp;
        }

        public virtual VirtualPath ContactFormUrl() {
            return BlogUrl() + "Contact.aspx";
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

        public virtual VirtualPath CategoryUrl(LinkCategory category) {
            return GetVirtualPath("category", new { slug = category.Id, categoryType = "category" });
        }

        public virtual VirtualPath CategoryRssUrl(LinkCategory category)
        {
            return GetVirtualPath("rss", new { catId = category.Id });
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

        public virtual VirtualPath AdminUrl(string path, object routeValues) {
            var routeValuesDict = new RouteValueDictionary(routeValues);
            routeValuesDict.Add("pathinfo", path);
            return GetVirtualPath("admin", routeValuesDict);
        }
    }
}