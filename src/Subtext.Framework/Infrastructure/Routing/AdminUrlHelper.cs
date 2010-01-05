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

using System.Web.Routing;
using Subtext.Framework.Components;

namespace Subtext.Framework.Routing
{
    public class AdminUrlHelper
    {
        public AdminUrlHelper(UrlHelper urlHelper)
        {
            Url = urlHelper;
        }

        public UrlHelper Url { get; private set; }

        public VirtualPath Home()
        {
            return Url.AdminUrl("");
        }

        public VirtualPath Rss()
        {
            return Url.AdminUrl("adminRss.axd");
        }

        public VirtualPath ImportExport()
        {
            return Url.AdminUrl("ImportExport.aspx");
        }

        public VirtualPath Export(bool embedAttachments)
        {
            return Url.GetVirtualPath("export", new RouteValueDictionary{ {"embed", embedAttachments }});
        }

        public VirtualPath PostsList()
        {
            return Url.AdminUrl("posts");
        }

        public VirtualPath PostsEdit()
        {
            return Url.AdminUrl("posts/edit.aspx");
        }

        //TODO: Unit test
        public VirtualPath PostsEdit(int id)
        {
            return Url.AdminUrl("posts/edit.aspx", new {PostId = id});
        }

        public VirtualPath ArticlesList()
        {
            return Url.AdminUrl("articles");
        }

        public VirtualPath ArticlesEdit()
        {
            return Url.AdminUrl("articles/edit.aspx");
        }

        public VirtualPath FeedbackList()
        {
            return Url.AdminUrl("feedback");
        }

        //TODO: Unit test
        public VirtualPath FeedbackEdit(int id)
        {
            var routeValues = new RouteValueDictionary {{"return-to-post", "true"}, {"FeedbackID", id}};
            return Url.AdminUrl("feedback/edit.aspx", routeValues);
        }

        public VirtualPath LinksEdit()
        {
            return Url.AdminUrl("EditLinks.aspx");
        }

        public VirtualPath GalleriesEdit()
        {
            return Url.AdminUrl("EditGalleries.aspx");
        }

        public VirtualPath Referrers(int id)
        {
            return Url.AdminUrl("Referrers.aspx");
        }

        public VirtualPath Statistics()
        {
            return Url.AdminUrl("Statistics.aspx");
        }

        public VirtualPath Options()
        {
            return Url.AdminUrl("Options.aspx");
        }

        public VirtualPath Credits()
        {
            return Url.AdminUrl("Credits.aspx");
        }

        public VirtualPath EditCategories()
        {
            return Url.AdminUrl("EditCategories.aspx");
        }

        public VirtualPath EditCategories(CategoryType categoryType)
        {
            return Url.AdminUrl("EditCategories.aspx", new {catType = categoryType});
        }

        public VirtualPath EditGalleries()
        {
            return Url.AdminUrl("EditGalleries.aspx");
        }

        public VirtualPath EditLinks()
        {
            return Url.AdminUrl("EditLinks.aspx");
        }

        public VirtualPath ErrorLog()
        {
            return Url.AdminUrl("ErrorLog.aspx");
        }

        public VirtualPath FullTextSearch()
        {
            return Url.AdminUrl("FullTextSearch.aspx");
        }

        public VirtualPath AjaxServices()
        {
            return Url.GetVirtualPath("ajax-services", null);
        }
    }
}