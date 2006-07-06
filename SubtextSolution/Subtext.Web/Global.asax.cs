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
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Logging;

namespace Subtext 
{
	public class Global : System.Web.HttpApplication
	{
		private readonly static ILog log = new Subtext.Framework.Logging.Log();

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
			if(custom == "Blogger" && !Security.IsAdmin && !Security.IsHostAdmin) // Do not cache admin.
			{
				return Config.CurrentBlog.BlogId.ToString(CultureInfo.InvariantCulture);
			}

			return base.GetVaryByCustomString(context, custom);
		}

		private const string ERROR_PAGE_LOCATION = "~/SystemMessages/error.aspx";
		private const string BAD_CONNECTION_STRING_PAGE = "~/SystemMessages/CheckYourConnectionString.aspx";

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
			log4net.Repository.Hierarchy.Hierarchy h = LogManager.GetRepository() as log4net.Repository.Hierarchy.Hierarchy;
			//get the ADO appender
			h.ConfigurationChanged += new log4net.Repository.LoggerRepositoryConfigurationChangedEventHandler(log4Net_ConfigurationChanged);
			EnsureLog4NetConnectionString(h);
		    log.Info("Application_Start - This is not a malfunction.");
		}

		private static void EnsureLog4NetConnectionString(Hierarchy h)
		{
			foreach(IAppender appender in h.Root.Appenders)
			{
				AdoNetAppender adoAppender = appender as AdoNetAppender;
				if(adoAppender != null)
				{
					adoAppender.ConnectionString = Config.Settings.ConnectionString.ToString();
					adoAppender.ActivateOptions();
				}
			}
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

		protected void Application_EndRequest(Object sender, EventArgs e)
		{
			#region Debug Information
			#if DEBUG
				HttpApplication application = (HttpApplication)sender;
				HttpContext context = application.Context;

				if(!Regex.IsMatch(context.Request.Path, @"(\.css$|\.js$)|rss|mainfeed|atom|services|opml|ftbwebresource|ashx|providers", RegexOptions.IgnoreCase))
				{
					Version v =  Subtext.Framework.VersionInfo.FrameworkVersion; //t.Assembly.GetName().Version;
					string machineName = System.Environment.MachineName;
					Version framework = System.Environment.Version;

					string userInfo = "No User";
					try
					{
						if(context.Request.IsAuthenticated)
						{
							userInfo = context.User.Identity.Name;
							userInfo += "<br />Is Host Admin: " + Subtext.Framework.Security.IsHostAdmin.ToString(CultureInfo.InvariantCulture);
							if(!InstallationManager.IsInHostAdminDirectory && !InstallationManager.IsInInstallDirectory && !InstallationManager.IsInSystemMessageDirectory)
							{
								userInfo += "<br />Is Admin: " + Subtext.Framework.Security.IsAdmin.ToString(CultureInfo.InvariantCulture);
								userInfo += "<br />BlogId: " + Subtext.Framework.Configuration.Config.CurrentBlog.BlogId.ToString(CultureInfo.InvariantCulture);
							}	
						}
					}
					catch
					{}

					context.Response.Write(string.Format(message,"<center>",lb,v,machineName,framework,userInfo,lb,"</center>"));

				}
			#endif
			#endregion
		}

		/// <summary>
		/// Handles the AuthenticateRequest event of the Application control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected void Application_AuthenticateRequest(Object sender, EventArgs e)
		{
			string cookieName = FormsAuthentication.FormsCookieName;
			HttpCookie authCookie = Context.Request.Cookies[cookieName];

			if(null == authCookie)
			{
				// There is no authentication cookie.
				return;
			}
			
			FormsAuthenticationTicket authTicket;
			try
			{
				authTicket = FormsAuthentication.Decrypt(authCookie.Value);
			}
			catch(Exception ex)
			{
				log.Error("Could not decrypt the authentication cookie.", ex);
				return;
			}

			if (null == authTicket)
			{
				log.Warn("Could not decrypt the authentication cookie. No exception was thrown.");
				return; 
			}
			
			// When the ticket was created, the UserData property was assigned a
			// pipe delimited string of role names.
			string[] roles = authTicket.UserData.Split(new char[]{'|'});
			// Create an Identity object
			FormsIdentity id = new FormsIdentity( authTicket ); 

			// This principal will flow throughout the request.
			GenericPrincipal principal = new GenericPrincipal(id, roles);
			// Attach the new principal object to the current HttpContext object
			Context.User = principal;
		}

		protected void Application_Error(Object sender, EventArgs e)
		{
			Exception exception = Server.GetLastError();
			if(exception is HttpUnhandledException)
			{
				if(exception.InnerException == null)
				{
					Server.Transfer(ERROR_PAGE_LOCATION, false);
					return;
				}
				exception = exception.InnerException;
			}

			//Sql Exception and request is for "localhost"
			SqlException sqlExc = exception as SqlException;
			if(sqlExc != null &&
				(
				sqlExc.Number == (int)SqlErrorMessage.LoginFailsCannotOpenDatabase
				|| sqlExc.Number == (int)SqlErrorMessage.SqlServerDoesNotExistOrAccessDenied
				|| sqlExc.Number == (int)SqlErrorMessage.LoginFailed
				|| sqlExc.Number == (int)SqlErrorMessage.LoginFailedInvalidUserOfTrustedConnection
				|| sqlExc.Number == (int)SqlErrorMessage.LoginFailedNotAssociatedWithTrustedConnection
				|| sqlExc.Number == (int)SqlErrorMessage.LoginFailedUserNameInvalid
				|| (sqlExc.Number == (int)SqlErrorMessage.CouldNotFindStoredProcedure && sqlExc.Message.IndexOf("'blog_GetConfig'") > 0)
				)
				)
			{
				// Probably a bad connection string.
				Server.Transfer(BAD_CONNECTION_STRING_PAGE);
				return;
			}

			if(!InstallationManager.IsInInstallDirectory)
			{
				// User could be logging into the HostAdmin.
				if(exception.GetType() == typeof(BlogDoesNotExistException))
				{
					Response.Redirect("~/Install/BlogNotConfiguredError.aspx", true);
					return;
				}

				if(InstallationManager.InstallationActionRequired(exception, VersionInfo.FrameworkVersion))
				{
					Response.Redirect("~/Install/", true);
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
				Server.Transfer(BAD_CONNECTION_STRING_PAGE);
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
				Server.Transfer(BAD_CONNECTION_STRING_PAGE);
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
				Server.Transfer(ERROR_PAGE_LOCATION, false);
			}
			else
			{
				log.Error("Unhandled Exception trapped in Global.asax", exception);
			}
		}

		protected void Session_End(Object sender, EventArgs e)
		{

		}

#if DEBUG
		private static string lb = "============ Debug Build ============";
		private static string message = "{0}{1}<br />Subtext Version: {2}<br />Machine Name: {3}<br />.NET Version: {4}<br />{5}<br />{6}{7}";
#endif

		protected void Application_End(Object sender, EventArgs e)
		{
			Subtext.Framework.Stats.ClearQueue(true);
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

		private void log4Net_ConfigurationChanged(object sender, EventArgs e)
		{
			EnsureLog4NetConnectionString((Hierarchy)sender);
		}
	}
}


