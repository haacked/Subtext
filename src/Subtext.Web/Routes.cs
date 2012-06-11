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

using System.Web.Mvc;
using System.Web.Routing;
using Subtext.Framework.Routing;
using Subtext.Framework.Services;
using Subtext.Framework.Syndication;
using Subtext.Framework.Syndication.Admin;
using Subtext.Framework.Tracking;
using Subtext.Framework.Web.Handlers;
using Subtext.Framework.XmlRpc;
using Subtext.Web.Admin.Services.Ajax;
using Subtext.Web.Controls.Captcha;
using Subtext.Web.SiteMap;
using Subtext.Web.UI.Handlers;

public static class Routes
{
    public static void RegisterRoutes(SubtextRouteMapper routes)
    {
        routes.MapControls("entry-by-id",
                   "archive/{year}/{month}/{day}/{id}.aspx"
                   ,
                   new
                   {
                       year = @"[1-9]\d{3}",
                       month = @"(0\d)|(1[0-2])",
                       day = @"([0-2]\d)|(3[0-1])",
                       id = @"\d+"
                   }
                   , new[] { "viewpost", "comments", "postcomment" });

        routes.MapControls("entry-by-slug",
                           "archive/{year}/{month}/{day}/{slug}.aspx"
                           , new { year = @"[1-9]\d{3}", month = @"(0\d)|(1[0-2])", day = @"([0-2]\d)|(3[0-1])" }
                           , new[] { "viewpost", "comments", "postcomment" });

        routes.MapRoot();
        routes.MapRoute("skins-admin", "admin/skins/{action}/{id}", new { controller = "skins", id = UrlParameter.Optional });
        routes.MapRoute("comments-admin", "admin/comments/{action}.ashx", new { controller = "comment" });
        routes.Ignore("{resource}.axd/{*pathInfo}");
        routes.Ignore("skins/{*pathInfo}");
        routes.MapSystemPage("MainFeed");
        routes.MapSystemDirectories("hostadmin", "install", "SystemMessages");

        //TODO: Consider making this a single route with a constraint of the allowed pages.
        routes.MapPage("forgotpassword");
        routes.MapPage("login");

        routes.MapHttpHandler<AjaxServices>("ajax-services", "admin/Services/Ajax/AjaxServices.ashx");
        routes.MapHttpHandler<RssAdminHandler>("admin-rss", "admin/{feedName}Rss.axd");
        routes.MapRoute("export", "admin/export.ashx", new { controller = "export", action = "blogml" });
        routes.MapRoute("wlwmanifest", "wlwmanifest.xml.ashx", new { controller = "manifest", action = "index" });
        routes.MapRoute("opensearchdesc", "opensearchdesc.xml.ashx", new { controller = "opensearch", action = "index" });
        routes.MapDirectory("admin");
        routes.MapDirectory("providers");

        routes.MapHttpHandler<SiteMapHttpHandler>("sitemap.ashx");
        routes.MapHttpHandler<BrowserDetectionService>("BrowserServices.ashx");

        //Todo: Add a data token to indicate feed title.
        // By default, the main feed is RSS. To chang it to atom, just 
        // swap the route names.
        routes.MapHttpHandler<RssHandler>("rss", "rss.aspx");
        routes.MapHttpHandler<AtomHandler>("atom", "atom.aspx");
        routes.MapHttpHandler<RssCommentHandler>("comment-rss", "comments/commentRss/{id}.aspx");
        routes.MapRoute("comment-api", "comments/{id}.aspx", new { controller = "CommentApi", action = "Create" },
                        new { id = @"\d+" });
        routes.MapRoute("aggbug", "aggbug/{id}.aspx", new { controller = "Statistics", action = "RecordAggregatorView" },
                        new { id = @"\d+" });
        routes.MapHttpHandler<RsdHandler>("rsd", "rsd.xml.ashx");
        routes.MapHttpHandler<BlogSecondaryCssHandler>("customcss", "customcss.aspx");
        //TODO: routes.MapHttpHandler<CategoryRedirectHandler>("category-redirect", "category/{category}.aspx/rss", new { category = @"\d+" });
        routes.MapHttpHandler<RssCategoryHandler>("category-rss", "category/{slug}/rss",
                                                  new { slug = @"[-\w\s\d\.]+" });
        routes.MapHttpHandler<OpmlHandler>("opml", "opml.xml.ashx");

        routes.MapPagesToControlOfSameName("contact", "ArchivePostPage", "ArticleCategories", "search");

        routes.MapControls("archives", "archives.aspx", null, new[] { "ArchivesPage" });

        routes.MapControls("entries-by-day", "archive/{year}/{month}/{day}.aspx"
                           , new { year = @"[1-9]\d{3}", month = @"(0\d)|(1[0-2])", day = @"([0-2]\d)|(3[0-1])" }
                           , new[] { "ArchiveDay" });

        routes.MapControls("entries-by-month",
                           "archive/{year}/{month}.aspx"
                           , new { year = @"[1-9]\d{3}", month = @"(0\d)|(1[0-2])" }
                           , new[] { "ArchiveMonth" });

        routes.MapControls("article-by-id", "articles/{id}.aspx"
                           , new { id = @"\d+" }
                           , new[] { "viewpost", "comments", "postcomment" });

        routes.MapControls("article-by-slug", "articles/{slug}.aspx"
                           , null
                           , new[] { "viewpost", "comments", "postcomment" });

        routes.MapControls("gallery", "gallery/{id}.aspx"
                           , new { id = @"\d+" }
                           , new[] { "GalleryThumbNailViewer" });

        routes.MapControls("gallery-image", "gallery/image/{id}.aspx"
                           , new { id = @"\d+" }
                           , new[] { "ViewPicture" });

        routes.MapControls("category", "{categoryType}/{slug}.aspx"
                           , new { categoryType = @"category|stories" }
                           , new[] { "CategoryEntryList" });


        routes.MapControls("tag", "tags/{tag}/default.aspx", null, new[] { "TagEntryList" });
        routes.MapControls("tag-cloud", "tags/default.aspx", null, new[] { "FullTagCloud" });
        routes.MapHttpHandler<RssTagHandler>("tag-rss", "tags/{tag}/rss.aspx");

        routes.MapHttpHandler<TrackBackHandler>("trackbacks", "services/trackbacks/{id}.aspx", new { id = @"\d+" });
        routes.MapXmlRpcHandler<PingBackService>("services/pingback/{id}.aspx", new { id = @"\d+" });
        routes.MapXmlRpcHandler<MetaWeblog>("metaweblogapi", "services/metablogapi.aspx", null);

        routes.Add("captcha", new Route("images/services/CaptchaImage.ashx", new HttpRouteHandler<CaptchaImageHandler>(routes.ServiceLocator)));
        routes.Add("logout", new SubtextRoute("account/logout.ashx", new MvcRouteHandler()) { Defaults = new RouteValueDictionary(new { controller = "account", action = "logout" }) });

    }
}