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
using System.Text.RegularExpressions;
using System.Web;
using Subtext.Framework;

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


		private const string ERROR_PAGE_LOCATION = "~/error.aspx";
		private const string BLOG_INITIAL_CONFIGURATION_PAGE = "~/BlogNotConfiguredError.aspx";
		private const string BAD_CONNECTION_STRING_PAGE = "~/CheckYourConnectionString.aspx";

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
		
	
		protected void Application_BeginRequest(Object sender, EventArgs e)
		{

		}

		protected void Application_EndRequest(Object sender, EventArgs e)
		{
			#if DEBUG
				
				HttpApplication application = (HttpApplication)sender;
				HttpContext context = application.Context;

				if(!Regex.IsMatch(context.Request.Path,"rss|mainfeed|atom|services|opml",RegexOptions.IgnoreCase))
				{

					//Type t = Type.GetType("Subtext.Framework.Security, Subtext.Framework");
					Version v =  Subtext.Framework.VersionInfo.FrameworkVersion; //t.Assembly.GetName().Version;
					string machineName = System.Environment.MachineName;
					Version framework = System.Environment.Version;

					string userInfo = "No User";
					try
					{
						if(context.Request.IsAuthenticated)
						{
							userInfo = context.User.Identity.Name;
							userInfo += "<br>Is Admin: " + Subtext.Framework.Security.IsAdmin.ToString();
							userInfo += "<br>BlogID: " + Subtext.Framework.Configuration.Config.CurrentBlog.BlogID.ToString();
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
				if(exception.InnerException is BlogDoesNotExistException)
				{
					Server.Transfer(BLOG_INITIAL_CONFIGURATION_PAGE, false);
				}

				//Sql Exception and request is for "localhost"
				if(exception.InnerException is SqlException)
				{
					// Probably a bad connection string.
					Server.Transfer(BAD_CONNECTION_STRING_PAGE);
				}
			}

			// I don't know that Context can ever be null in the pipe, but we'll play it
			// extra safe. If customErrors are off, we'll just let ASP.NET default happen.
			if (Context != null && Context.IsCustomErrorEnabled)
				Server.Transfer(ERROR_PAGE_LOCATION, false);
		}

		protected void Session_End(Object sender, EventArgs e)
		{

		}

		private static string lb = "============ Debug Build ============";
		private static string message = "{0}{1}<br>Dottext Version: {2}<br>Machine Name: {3}<br>.NET Version: {4}<br>{5}<br>{6}{7}";

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


