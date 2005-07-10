#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// .Text WebLog
// 
// .Text is an open source weblog system started by Scott Watermasysk. 
// Blog: http://ScottWater.com/blog 
// RSS: http://scottwater.com/blog/rss.aspx
// Email: Dottext@ScottWater.com
//
// For updated news and information please visit http://scottwater.com/dottext and subscribe to 
// the Rss feed @ http://scottwater.com/dottext/rss.aspx
//
// On its release (on or about August 1, 2003) this application is licensed under the BSD. However, I reserve the 
// right to change or modify this at any time. The most recent and up to date license can always be fount at:
// http://ScottWater.com/License.txt
// 
// Please direct all code related questions to:
// GotDotNet Workspace: http://www.gotdotnet.com/Community/Workspaces/workspace.aspx?id=e99fccb3-1a8c-42b5-90ee-348f6b77c407
// Yahoo Group http://groups.yahoo.com/group/DotText/
// 
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using Subtext.Framework;
using Subtext.Framework.Data;
using Subtext.Framework.Exceptions;

namespace Subtext 
{
	public class Global : System.Web.HttpApplication
	{
		public override string GetVaryByCustomString(HttpContext context, string custom)
		{
			if(custom == "Blogger")
			{
				return Subtext.Framework.Configuration.Config.CurrentBlog.Application;
			}

			return base.GetVaryByCustomString(context,custom);
		}

		private const string ERROR_PAGE_LOCATION = "~/SystemMessages/error.aspx";
		private const string BAD_CONNECTION_STRING_PAGE = "~/SystemMessages/CheckYourConnectionString.aspx";

		public Global()
		{
			InitializeComponent();
		}	
		
		protected void Application_Start(Object sender, EventArgs e)
		{
		}
 
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
			// Want to redirect to install if installation is required, 
			// or if we're missing a HostInfo record.
			if((InstallationManager.IsInstallationActionRequired(VersionInfo.FrameworkVersion) || InstallationManager.HostInfoRecordNeeded) && !InstallationManager.IsInInstallDirectory)
			{
				Response.Redirect("~/Install/");
			}
		}

		protected void Application_EndRequest(Object sender, EventArgs e)
		{
			#if DEBUG
				HttpApplication application = (HttpApplication)sender;
				HttpContext context = application.Context;

				if(!Regex.IsMatch(context.Request.Path,"rss|mainfeed|atom|services|opml",RegexOptions.IgnoreCase))
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
							userInfo += "<br>Is Admin: " + Subtext.Framework.Security.IsAdmin.ToString(CultureInfo.InvariantCulture);
							userInfo += "<br>BlogID: " + Subtext.Framework.Configuration.Config.CurrentBlog.BlogID.ToString(CultureInfo.InvariantCulture);
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

				if(!InstallationManager.IsInInstallDirectory)
				{
					if(exception.InnerException.GetType() == typeof(BlogDoesNotExistException))
					{
						Response.Redirect("~/Install/BlogNotConfiguredError.aspx");
					}

					if(InstallationManager.GetIsInstallationActionRequired(exception.InnerException, VersionInfo.FrameworkVersion))
					{
						Response.Redirect("~/Install/");
					}	
				}

				//Sql Exception and request is for "localhost"
				SqlException sqlExc = exception.InnerException as SqlException;
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

				if(exception.InnerException is InvalidOperationException && exception.InnerException.Message.IndexOf("ConnectionString") >= 0)
				{
					// Probably a missing connection string.
					Server.Transfer(BAD_CONNECTION_STRING_PAGE);
					return;
				}

				if(exception.InnerException is ArgumentException 
					&& (
					exception.InnerException.Message.IndexOf("Keyword not supported") >= 0
					||	exception.InnerException.Message.IndexOf("Invalid value for key") >= 0
					)
					)
				{
					// Probably a malformed connection string.
					Server.Transfer(BAD_CONNECTION_STRING_PAGE);
					return;
				}
			}

			if(exception is HttpException)
			{
				if(((HttpException)exception).GetHttpCode() == 404)
				{
					return;
				}
				// I don't know that Context can ever be null in the pipe, but we'll play it
				// extra safe. If customErrors are off, we'll just let ASP.NET default happen.
				if (Context != null && Context.IsCustomErrorEnabled)
					Server.Transfer(ERROR_PAGE_LOCATION, false);
			}
		}

		protected void Session_End(Object sender, EventArgs e)
		{

		}

		private static string lb = "============ Debug Build ============";
		private static string message = "{0}{1}<br>Subtext Version: {2}<br>Machine Name: {3}<br>.NET Version: {4}<br>{5}<br>{6}{7}";

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
	}
}


