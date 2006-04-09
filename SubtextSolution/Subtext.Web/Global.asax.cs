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
using System.Text.RegularExpressions;
using System.Web;
using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;
using Subtext.Extensibility.Providers;
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

		public override string GetVaryByCustomString(HttpContext context, string custom)
		{
			if(custom == "Blogger")
			{
				return Subtext.Framework.Configuration.Config.CurrentBlog.Subfolder;
			}

			return base.GetVaryByCustomString(context,custom);
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


			if(InstallationManager.IsInHostAdminDirectory)
				return;

			// Want to redirect to install if installation is required, 
			// or if we're missing a HostInfo record.
			if((InstallationManager.IsInstallationActionRequired(VersionInfo.FrameworkVersion) || InstallationManager.HostInfoRecordNeeded))
			{
				InstallationState state = InstallationManager.GetCurrentInstallationState(VersionInfo.FrameworkVersion);
				if(state == InstallationState.NeedsInstallation)
				{
					Response.Redirect("~/Install/", true);
					return;
				}

				if(state == InstallationState.NeedsUpgrade || state == InstallationState.NeedsRepair)
				{
					if(!InstallationManager.IsInHostAdminDirectory && !InstallationManager.IsOnLoginPage && !InstallationManager.IsInSystemMessageDirectory)
					{
						Response.Redirect("~/SystemMessages/UpgradeInProgress.aspx", true);
						return;
					}
				}
			}
		}

		protected void Application_EndRequest(Object sender, EventArgs e)
		{
			#if DEBUG
				HttpApplication application = (HttpApplication)sender;
				HttpContext context = application.Context;

				if(!Regex.IsMatch(context.Request.Path,"rss|mainfeed|atom|services|opml|ftbwebresource|ashx",RegexOptions.IgnoreCase))
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
		}

		protected void Application_AuthenticateRequest(Object sender, EventArgs e)
		{
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
				sqlExc.Number == (int)SqlErrorMessages.LoginFailsCannotOpenDatabase
				|| sqlExc.Number == (int)SqlErrorMessages.LoginFailed
				|| sqlExc.Number == (int)SqlErrorMessages.LoginFailedInvalidUserOfTrustedConnection
				|| sqlExc.Number == (int)SqlErrorMessages.LoginFailedNotAssociatedWithTrustedConnection
				|| sqlExc.Number == (int)SqlErrorMessages.LoginFailedUserNameInvalid
				|| (sqlExc.Number == (int)SqlErrorMessages.CouldNotFindStoredProcedure && sqlExc.Message.IndexOf("'blog_GetConfig'") > 0)
				)
				)
			{
				// Probably a bad connection string.
				Server.Transfer(BAD_CONNECTION_STRING_PAGE);
				return;
			}

			if(!InstallationManager.IsInInstallDirectory)
			{
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


