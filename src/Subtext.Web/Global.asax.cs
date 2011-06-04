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
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using log4net;
using Ninject.Activation.Caching;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Infrastructure.Installation;
using Subtext.Framework.Logging;
using Subtext.Framework.Routing;
using Subtext.Framework.Security;
using Subtext.Framework.Services.SearchEngine;
using Subtext.Framework.Web.HttpModules;

namespace Subtext.Web
{
    public class SubtextApplication : HttpApplication
    {
        //This call is to kickstart log4net.
        //log4net Configuration Attribute is in AssemblyInfo
        private const string BadConnectionStringPage = "~/aspx/SystemMessages/CheckYourConnectionString.aspx";
        private const string DatabaseLoginFailedPage = "~/aspx/SystemMessages/DatabaseLoginFailed.aspx";
        private const string DeprecatedPhysicalPathsPage = "~/aspx/SystemMessages/DeprecatedPhysicalPaths.aspx";
        private const string ErrorPageLocation = "~/aspx/SystemMessages/error.aspx";
        private readonly static ILog Log = new Log(LogManager.GetLogger(typeof(SubtextApplication)));

        public bool LogInitialized { get; private set; }
        public ReadOnlyCollection<string> DeprecatedPhysicalPaths { get; private set; }

        /// <summary>
        /// Method called by the application on startup.  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Start(object sender, EventArgs e)
        {
            var routes = new SubtextRouteMapper(RouteTable.Routes, DependencyResolver.Current);
            StartApplication(routes, new HttpServerUtilityWrapper(Server));
            Application["DeprecatedPhysicalPaths"] = DeprecatedPhysicalPaths;
        }

        public virtual void StartApplication(SubtextRouteMapper routes, HttpServerUtilityBase server)
        {
            Routes.RegisterRoutes(routes);

            var deprecatedPaths = new[]
            {
                "~/Admin", "~/HostAdmin", "~/Install",
                "~/SystemMessages", "~/AggDefault.aspx", "~/DTP.aspx",
                "~/ForgotPassword.aspx", "~/login.aspx", "~/logout.aspx", "~/MainFeed.aspx"
            };
            var invalidPaths =
                from path in deprecatedPaths
                where Directory.Exists(server.MapPath(path)) || File.Exists(server.MapPath(path))
                select path;
            DeprecatedPhysicalPaths = new ReadOnlyCollection<string>(invalidPaths.ToList());
        }

        public override void Init()
        {
            if (DeprecatedPhysicalPaths == null)
            {
                DeprecatedPhysicalPaths = Application["DeprecatedPhysicalPaths"] as ReadOnlyCollection<string>;
            }
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
            if (custom == "Blogger")
            {
                return string.Format("{0}:{1}", Config.CurrentBlog.Id.ToString(CultureInfo.InvariantCulture), User.IsAdministrator());
            }

            return base.GetVaryByCustomString(context, custom);
        }

        /// <summary>
        /// Method called during at the beginning of each request.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (BlogRequest.Current.RequestLocation != RequestLocation.StaticFile)
            {
                BeginApplicationRequest(Log);
            }
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            var cache = DependencyResolver.Current.GetService<Ninject.Activation.Caching.ICache>() as Cache;
            if (cache != null)
            {
                cache.Clear(HttpContext.Current);
                //cache.DisposeRequestScoped(httpContext);
            }
        }

        public void BeginApplicationRequest(ILog log)
        {
            if (!LogInitialized)
            {
                //This line will trigger the configuration.
                log.Info("Subtext Application Started");
                LogInitialized = true;
            }

            if (DeprecatedPhysicalPaths != null && DeprecatedPhysicalPaths.Count > 0)
            {
                throw new DeprecatedPhysicalPathsException(DeprecatedPhysicalPaths);
            }
        }

        /// <summary>
        /// Handles the Error event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Application_Error(Object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            if (BlogRequest.Current == null || BlogRequest.Current.RequestLocation != RequestLocation.StaticFile)
            {
                var installationManager = DependencyResolver.Current.GetService<IInstallationManager>();
                OnApplicationError(exception, new HttpServerUtilityWrapper(Server), Log, installationManager);
            }
        }

        public void OnApplicationError(Exception exception, HttpServerUtilityBase server, ILog log, IInstallationManager installationManager)
        {
            exception = UnwrapHttpUnhandledException(exception);
            if (exception == null)
            {
                server.Transfer(ErrorPageLocation);
                return;
            }

            if (HandleDeprecatedFilePathsException(exception, server, this))
            {
                return;
            }

            LogIfCommentException(exception, log);

            if (HandleSqlException(exception, server))
            {
                return;
            }

            BlogRequest blogRequest = BlogRequest.Current;
            if (HandleRequestLocationException(exception, blogRequest, installationManager, new HttpResponseWrapper(Response)))
            {
                return;
            }

            if (HandleBadConnectionStringException(exception, server))
            {
                return;
            }

            if (exception is HttpException)
            {
                if (((HttpException)exception).GetHttpCode() == 404)
                {
                    return;
                }
            }

            bool isCustomErrorEnabled = Context == null ? false : Context.IsCustomErrorEnabled;

            HandleUnhandledException(exception, server, isCustomErrorEnabled, log);
        }

        public static Exception UnwrapHttpUnhandledException(Exception exception)
        {
            if (exception is HttpUnhandledException)
            {
                if (exception.InnerException == null)
                {
                    return null;
                }
                exception = exception.InnerException;
            }
            return exception;
        }

        public static bool HandleDeprecatedFilePathsException(Exception exception, HttpServerUtilityBase server,
                                                              SubtextApplication application)
        {
            var depecratedException = exception as DeprecatedPhysicalPathsException;
            if (depecratedException != null)
            {
                server.Execute(DeprecatedPhysicalPathsPage, false);
                server.ClearError();
                application.FinishRequest();
                return true;
            }

            return false;
        }

        public virtual void FinishRequest()
        {
            CompleteRequest();
        }

        public static void LogIfCommentException(Exception exception, ILog log)
        {
            var commentException = exception as BaseCommentException;
            if (commentException != null)
            {
                string message = "Comment exception thrown and handled in Global.asax.";
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    message += string.Format("-- User Agent: {0}", HttpContext.Current.Request.UserAgent);
                }
                log.Info(message, commentException);
            }
        }

        public static bool HandleSqlException(Exception exception, HttpServerUtilityBase server)
        {
            //Sql Exception and request is for "localhost"
            var sqlException = exception as SqlException;
            if (sqlException != null)
            {
                int exceptionNumber = sqlException.Number;
                string message = sqlException.Message;

                return HandleSqlExceptionNumber(exceptionNumber, message, server);
            }
            return false;
        }

        public static bool HandleSqlExceptionNumber(int exceptionNumber, string exceptionMessage,
                                                    HttpServerUtilityBase server)
        {
            if (exceptionNumber == (int)SqlErrorMessage.SqlServerDoesNotExistOrAccessDenied
               ||
               (exceptionNumber == (int)SqlErrorMessage.CouldNotFindStoredProcedure &&
                exceptionMessage.Contains("'blog_GetConfig'"))
                )
            {
                // Probably a bad connection string.
                server.Transfer(BadConnectionStringPage);
                return true;
            }

            if (exceptionNumber == (int)SqlErrorMessage.LoginFailsCannotOpenDatabase
               || exceptionNumber == (int)SqlErrorMessage.LoginFailed
               || exceptionNumber == (int)SqlErrorMessage.LoginFailedInvalidUserOfTrustedConnection
               || exceptionNumber == (int)SqlErrorMessage.LoginFailedNotAssociatedWithTrustedConnection
               || exceptionNumber == (int)SqlErrorMessage.LoginFailedUserNameInvalid
                )
            {
                // Probably a bad connection string.
                server.Transfer(DatabaseLoginFailedPage);
                return true;
            }
            return false;
        }

        public static bool HandleRequestLocationException(Exception exception, BlogRequest blogRequest, IInstallationManager installManager, HttpResponseBase response)
        {
            if (blogRequest == null || (blogRequest.RequestLocation != RequestLocation.Installation &&
               blogRequest.RequestLocation != RequestLocation.Upgrade))
            {
                if (installManager.InstallationActionRequired(VersionInfo.CurrentAssemblyVersion, exception))
                {
                    response.Redirect("~/install/default.aspx", true);
                    return true;
                }
            }

            if (blogRequest.RequestLocation != RequestLocation.SystemMessages)
            {
                if (exception.GetType() == typeof(BlogInactiveException))
                {
                    response.Redirect("~/SystemMessages/BlogNotActive.aspx", true);
                    return true;
                }
            }
            return false;
        }

        public static bool HandleBadConnectionStringException(Exception exception, HttpServerUtilityBase server)
        {
            if (exception is InvalidOperationException && exception.Message.Contains("ConnectionString"))
            {
                // Probably a missing connection string.
                server.Transfer(BadConnectionStringPage);
                return true;
            }

            if (exception is ArgumentException
               && (
                      exception.Message.Contains("Keyword not supported")
                      || exception.Message.Contains("Invalid value for key")
                  )
                )
            {
                // Probably a malformed connection string.
                server.Transfer(BadConnectionStringPage);
                return true;
            }

            return false;
        }

        public static void HandleUnhandledException(Exception exception, HttpServerUtilityBase server,
                                                    bool isCustomErrorEnabled, ILog log)
        {
            if (isCustomErrorEnabled)
            {
                server.Transfer(ErrorPageLocation);
            }
            else
            {
                log.Error("Unhandled Exception trapped in Global.asax", exception);
            }
        }

        /// <summary>
        /// Handles the End event of the Application control.
        /// </summary>
        protected void Application_End()
        {
            EndApplication();
        }

        public void EndApplication()
        {
            var searchEngine = DependencyResolver.Current.GetService<ISearchEngineService>();
            if (searchEngine != null)
                searchEngine.Dispose();
            LogInitialized = false;
        }
    }
}