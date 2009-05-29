using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Subtext.Framework.Routing;
using Subtext.Framework.ImportExport;
using Subtext.Framework.Syndication.Admin;
using Subtext.Web.SiteMap;
using Subtext.Framework.Services;
using Subtext.Framework.Syndication;
using Subtext.Framework.Web.Handlers;
using Subtext.Framework.Tracking;
using Subtext.Web.UI.Handlers;
using Subtext.Framework.XmlRpc;
using Subtext.Web.Controls.Captcha;

public static class Routes
{
    public static void RegisterRoutes(RouteCollection routes)
    {
        routes.Ignore("{resource}.axd/{*pathInfo}");
        routes.Ignore("skins/{*pathInfo}");
        routes.Ignore("hostadmin/{*pathinfo}");
        routes.Ignore("install/{*pathinfo}");
        routes.Ignore("SystemMessages/{*pathinfo}");

        //TODO: Consider making this a single route with a constraint of the allowed pages.
        routes.MapPage("forgotpassword");
        routes.MapPage("login");
        routes.MapPage("logout");

        routes.MapHttpHandler<SubtextBlogMlHttpHandler>("admin/handlers/BlogMLExport.ashx");
        routes.MapHttpHandler<RssAdminHandler>("admin-rss", "admin/{feedName}Rss.axd");
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
        routes.MapRoute("comments/{id}.aspx", new { controller = "CommentApi", action = "Create" }, new { id = @"\d+" });
        routes.MapRoute("comments/{action}.aspx", new { controller = "CommentApi" }, new { });
        routes.MapHttpHandler<RsdHandler>("rsd", "rsd.xml.ashx");
        routes.MapHttpHandler<AggBugHandler>("aggbug", "aggbug/{id}.aspx");
        routes.MapHttpHandler<BlogSecondaryCssHandler>("customcss", "customcss.aspx");
        routes.MapHttpHandler<RssCategoryHandler>("category/{categoryName}.aspx/rss", new { categoryName = @"[-\w\s\d]+" });
        routes.MapHttpHandler<OpmlHandler>("opml", "opml.ashx");

        routes.MapPageToControl("contact");
        routes.MapPageToControl("ArchivePostPage");
        routes.MapPageToControl("ArticleCategories");
        routes.MapControls("archives", "archives.aspx", null, new[] { "SingleColumn" });

        routes.MapControls("entry-by-id",
            "archive/{year}/{month}/{day}/{id}.aspx"
            , new { year = @"[1-9]\d{3}", month = @"(0\d)|(1[0-2])", day = @"([0-2]\d)|(3[0-1])", id = @"\d+" }
            , new[] { "viewpost", "comments", "postcomment" });

        routes.MapControls("entry-by-slug",
            "archive/{year}/{month}/{day}/{slug}.aspx"
            , new { year = @"[1-9]\d{3}", month = @"(0\d)|(1[0-2])", day = @"([0-2]\d)|(3[0-1])" }
            , new[] { "viewpost", "comments", "postcomment" });

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
            , new { /*slug = @"\w*([\w-_]+\.)*[\w-_]+"*/}
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

        routes.Add(new Route("images/IdenticonHandler.ashx", new HttpRouteHandler<IdenticonHandler>()));
        routes.Add(new Route("images/CaptchaImage.ashx", new HttpRouteHandler<CaptchaImageHandler>()));

        routes.MapRoot();
    }
}

