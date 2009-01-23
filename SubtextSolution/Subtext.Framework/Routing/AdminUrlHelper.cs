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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Subtext.Framework.Components;

namespace Subtext.Framework.Routing
{
    public class AdminUrlHelper
    {
        public AdminUrlHelper(UrlHelper urlHelper) {
            Url = urlHelper;
        }

        protected UrlHelper Url {
            get;
            private set;
        }

        public VirtualPath Home() {
            return Url.AdminUrl("default.aspx");
        }

        public VirtualPath Rss()
        {
            return Url.AdminUrl("adminRss.axd");
        }

        public VirtualPath ImportExport() {
            return Url.AdminUrl("ImportExport.aspx");
        }

        public VirtualPath PostsList() {
            return Url.AdminUrl("posts");
        }

        public VirtualPath PostsEdit()
        {
            return Url.AdminUrl("posts/edit.aspx");
        }

        public VirtualPath PostsEdit(int id)
        {
            return Url.AdminUrl("posts/edit.aspx", new {PostId = id });
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

        public VirtualPath FeedbackEdit()
        {
            return Url.AdminUrl("feedback/edit.aspx");
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

        public VirtualPath EditCategories(CategoryType categoryType) {
            return Url.AdminUrl("EditCategories.aspx", new {catType = categoryType });
        }

        public VirtualPath ErrorLog() {
            return Url.AdminUrl("ErrorLog.aspx");
        }
    }
}
