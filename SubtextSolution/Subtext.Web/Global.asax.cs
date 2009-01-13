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
using System.Data.SqlClient;
using System.Globalization;
using System.Web;
using System.Web.Routing;
using log4net;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;
using Subtext.Framework.Exceptions;
using Subtext.Framework.ImportExport;
using Subtext.Framework.Logging;
using Subtext.Framework.Routing;
using Subtext.Framework.Services;
using Subtext.Framework.Syndication;
using Subtext.Framework.Syndication.Admin;
using Subtext.Framework.Tracking;
using Subtext.Framework.XmlRpc;
using Subtext.Web.Controls.Captcha;
using Subtext.Web.SiteMap;
using Subtext.Web.UI.Handlers;

namespace Subtext.Web
{
    public class Global : HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes) {
            routes.Ignore("hostadmin/{*pathinfo}");
            routes.Ignore("install/{*pathinfo}");
            routes.Ignore("SystemMessages/{*pathinfo}");

            routes.MapPage("login.aspx", "~/login.aspx");
            routes.MapPage("logout.aspx", "~/logout.aspx");
            
            routes.MapHttpHandler<SubtextBlogMlHttpHandler>("admin/handlers/BlogMLExport.ashx");
            routes.MapHttpHandler<RssAdminHandler>("admin/{adminRss}Rss.axd");
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
            routes.MapHttpHandler<CommentHandler>("comment-api", "comments/{id}.aspx", new { id = @"\d+" });

            routes.MapHttpHandler<AggBugHandler>("aggbug", "aggbug/{id}.aspx");
            routes.MapHttpHandler<BlogSecondaryCssHandler>("customcss.aspx");
            routes.MapHttpHandler<RssCategoryHandler>("category/{categoryName}.aspx/rss", new {categoryName=@"[-\w\s\d]+"});
            routes.MapPageToControl("ArchivePostPage");
            routes.MapPageToControl("ArticleCategories");
            routes.MapControls("archives.aspx", new[] {"SingleColumn"});

            routes.MapControls("entry-by-id", 
                "archive/{year}/{month}/{day}/{id}.aspx"
                , new { year = @"[1-9]\d{3}", month = @"(0\d)|(1[0-2])", day = @"([0-2]\d)|(3[0-1])", id=@"\d+" }
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

            routes.MapPageToControl("contact");
            
            routes.MapControls("gallery", "gallery/{id}.aspx"
                , new { id = @"\d+"}
                , new[] { "GalleryThumbNailViewer" });

            routes.MapControls("gallery-image", "gallery/image/{id}.aspx"
                , new { id = @"\d+" }
                , new[] { "ViewPicture" });

            routes.MapControls("category", "{categoryType}/{slug}.aspx"
                , new { categoryType = @"category|stories" }
                , new[] { "CategoryEntryList" });

            routes.MapControls("tags/{tag}/default.aspx", new[] { "TagEntryList" });
            routes.MapControls("tags/default.aspx", new[] { "FullTagCloud" });
            routes.MapControls("tags/default.aspx", new[] { "FullTagCloud" });
            routes.MapHttpHandler<RssTagHandler>("tags/{tag}/rss.aspx");

            routes.MapHttpHandler<TrackBackHandler>("trackbacks", "services/trackbacks/{id}.aspx", new { id = @"\d+" });
            routes.MapXmlRpcHandler<PingBackService>("services/pingback/{id}.aspx", new { id = @"\d+" });
            routes.MapXmlRpcHandler<MetaWeblog>("services/metablogapi.aspx");
            routes.MapControls("Default.aspx", new[] { "homepage" });
            routes.MapControls("root", string.Empty, null, new[] { "homepage" });

            routes.Add(new Route("images/IdenticonHandler.ashx", new HttpRouteHandler<IdenticonHandler>()));
            routes.Add(new Route("images/CaptchaImage.ashx", new HttpRouteHandler<CaptchaImageHandler>()));
        }

        //This call is to kickstart log4net.
        //log4net Configuration Attribute is in AssemblyInfo
        private readonly static ILog log = LogManager.GetLogger(typeof(Global));

        static Global()
        {
            //Wrap the logger with our own.
            log = new Log(log);
        }
	
        /// <summary>
        /// <para>
        /// This is used to vary partial caching of ASCX controls and ASPX pages on a per blog basis.  
        /// You can see this in action via the [PartialCaching] attribute.
        /// </para>
        /// <para>
        /// Provides an application-wide implementation of the <see cref="P:System.Web.UI.PartialCachingAttribute.VaryByCustom"/> property.
        /// </para>
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"/> that contains information about the current Web request</param>
        /// <param name="custom"></param>
        /// <returns>
        /// If the value of the <paramref name="custom"/> parameter is "browser", the browser's
        /// <see cref="System.Web.HttpBrowserCapabilities.Type"/> ; otherwise,
        /// <see langword="null"/> .
        /// </returns>
        public override string GetVaryByCustomString(HttpContext context, string custom)
        {
            if(custom == "Blogger")
            {
                return Config.CurrentBlog.Id.ToString(CultureInfo.InvariantCulture);
            }

            return base.GetVaryByCustomString(context, custom);
        }

        private const string ErrorPageLocation = "~/SystemMessages/error.aspx";
        private const string BadConnectionStringPage = "~/SystemMessages/CheckYourConnectionString.aspx";
        private const string DatabaseLoginFailedPage = "~/SystemMessages/DatabaseLoginFailed.aspx";

        /// <summary>
        /// Initializes a new instance of the <see cref="Global"/> class.
        /// </summary>
        public Global()
        {
            InitializeComponent();
        }	
		
        /// <summary>
        /// Method called by the application on startup.  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Start(Object sender, EventArgs e)
        {
            //This line will trigger the configuration.
            log.Info("Application_Start - This is not a malfunction.");
            RegisterRoutes(RouteTable.Routes);
        }

        /// <summary>
        /// Method called when a session starts.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Session_Start(Object sender, EventArgs e)
        {
			
        }
		
        /// <summary>
        /// Method called during at the beginning of each request.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_BeginRequest(Object sender, EventArgs e)
        {		
            //KLUDGE: This is required due to a bug in Log4Net 1.2.9.
            // This should be fixed in the next release.
            Log.SetBlogIdContext(NullValue.NullInt32);
        }

        /// <summary>
        /// Handles the EndRequest event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Application_EndRequest(Object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the AuthenticateRequest event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            //Handled by Subtext.Web.HttpModules.AuthenticationModule in Subtext.Framework.
        }

        /// <summary>
        /// Handles the Error event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Application_Error(Object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            if(exception is HttpUnhandledException)
            {
                if(exception.InnerException == null)
                {
                    Server.Transfer(ErrorPageLocation, false);
                    return;
                }
                exception = exception.InnerException;
            }

            BaseCommentException commentException = exception as BaseCommentException;
            if (commentException != null)
            {
                string message = "Comment exception thrown and handled in Global.asax.";
                if(HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    message += "-- User Agent: " + HttpContext.Current.Request.UserAgent;
                }
                log.Info(message, commentException);
            }
			
            //Sql Exception and request is for "localhost"
            SqlException sqlExc = exception as SqlException;
            if (sqlExc != null)
            {
                if (sqlExc.Number == (int)SqlErrorMessage.SqlServerDoesNotExistOrAccessDenied
                    || (sqlExc.Number == (int)SqlErrorMessage.CouldNotFindStoredProcedure && sqlExc.Message.IndexOf("'blog_GetConfig'") > 0)
                    )
                {
                    // Probably a bad connection string.
                    Server.Transfer(BadConnectionStringPage);
                    return;
                }

                if (sqlExc.Number == (int)SqlErrorMessage.LoginFailsCannotOpenDatabase
                    || sqlExc.Number == (int)SqlErrorMessage.LoginFailed
                    || sqlExc.Number == (int)SqlErrorMessage.LoginFailedInvalidUserOfTrustedConnection
                    || sqlExc.Number == (int)SqlErrorMessage.LoginFailedNotAssociatedWithTrustedConnection
                    || sqlExc.Number == (int)SqlErrorMessage.LoginFailedUserNameInvalid
                    )
                {
                    // Probably a bad connection string.
                    Server.Transfer(DatabaseLoginFailedPage);
                    return;
                }
            }

            if (!InstallationManager.IsInInstallDirectory && !InstallationManager.IsInUpgradeDirectory)
            {
                if (InstallationManager.InstallationActionRequired(exception, VersionInfo.FrameworkVersion))
                {
                    Response.Redirect("~/Install/", true);
                    return;
                }

                // User could be logging into the HostAdmin.
                if(exception.GetType() == typeof(BlogDoesNotExistException))
                {
                    Response.Redirect("~/Install/BlogNotConfiguredError.aspx", true);
                    return;
                }
            }

            if(!InstallationManager.IsInSystemMessageDirectory)
            {
                if(exception.GetType() == typeof(BlogInactiveException))
                {
                    HttpContext.Current.Response.Redirect("~/SystemMessages/BlogNotActive.aspx");
                }
            }

            if(exception is InvalidOperationException && exception.Message.IndexOf("ConnectionString") >= 0)
            {
                // Probably a missing connection string.
                Server.Transfer(BadConnectionStringPage);
                return;
            }

            if(exception is ArgumentException 
               && (
                      exception.Message.IndexOf("Keyword not supported") >= 0
                      ||	exception.Message.IndexOf("Invalid value for key") >= 0
                  )
                )
            {
                // Probably a malformed connection string.
                Server.Transfer(BadConnectionStringPage);
                return;
            }

            if(exception is HttpException)
            {
                if(((HttpException)exception).GetHttpCode() == 404)
                {
                    return;
                }
            }

            // I don't know that Context can ever be null in the pipe, but we'll play it
            // extra safe. If customErrors are off, we'll just let ASP.NET default happen.
            if (Context != null && Context.IsCustomErrorEnabled)
            {
                Server.Transfer(ErrorPageLocation, false);
            }
            else
            {
                log.Error("Unhandled Exception trapped in Global.asax", exception);
            }
        }

        /// <summary>
        /// Handles the End event of the Session control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Session_End(Object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the End event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Application_End(Object sender, EventArgs e)
        {
            Stats.ClearQueue(true);
        }
			
        #region Web Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {    
        }
        #endregion
    }
}