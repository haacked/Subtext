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
using System.Web;
using Subtext.Framework;
using Subtext.Framework.Configuration;

namespace Subtext.Web.Admin.Pages
{
	internal enum CookieSupportType
	{
		Untested,
		Testing,
		Allowed,
		NotAllowed
	}

	public class AdminPage : System.Web.UI.Page
	{		
		private const string TESTCOOKIE_NAME = "TestCookie";

		private ConfirmCommand _command;

		public AdminPage()
		{
			//
		}

		internal CookieSupportType CookieSupport
		{
			get 
			{ 
				if (HasBeenTestedForCookies())
					return (CookieSupportType)Session[Keys.SESSION_COOKIETEST]; 
				else
					return CookieSupportType.Untested;
			}
			set { Session[Keys.SESSION_COOKIETEST] = value; }
		}

		protected override void OnLoad(EventArgs e)
		{
			if(!ValidateUser)
			{
				Response.Redirect(Config.CurrentBlog().FullyQualifiedUrl + "Login.aspx?ReturnUrl=" + Request.Path);
			}

// GC: removed, if a user wants to come in with IE3 and take their lumps...
//
//			if(!IsUpLevel)
//			{
//				Page.Controls.Clear();
//				Page.Response.Write("You must be using an uplevel browser to use the Management Console.");
//				Page.Response.End();
//			}

			// REFACTOR: we really need a singleton indicator per session or run this initial 
			// dummy run in OnSessionStart. But we'll add the overhead for now. We can look at
			// putting it in the default.aspx, but that fails to work on direct url access.
			AreCookiesAllowed();
			
			base.OnLoad(e);
		}


		private bool ValidateUser
		{
			get
			{
				return Security.IsAdmin;
			}
		}

		private bool IsUpLevel
		{
			get
			{
				return Convert.ToBoolean(Page.Request.Browser["css2"]);
			}
		}

		protected bool AreCookiesAllowed()
		{
			if (!HasBeenTestedForCookies())
			{
				StartCookieTest();
				return false;
			}
			else
			{
				CookieSupportType testStatus = (CookieSupportType)Session[Keys.SESSION_COOKIETEST];

				if (CookieSupportType.Testing != testStatus)
				{
					if (CookieSupportType.Allowed == testStatus)
						return true;
					else
						return false;
				}
				else
					return FinishCookieTest();
			}
		}

		private bool HasBeenTestedForCookies()
		{	
			try
			{
				return (null != Session[Keys.SESSION_COOKIETEST]);
			}
			catch (HttpException) // if we're coming in via ErrorHandler, session is unavailable
			{
				return false;
			}
		}

		private void StartCookieTest()
		{
			try
			{			
				Session[Keys.SESSION_COOKIETEST] = CookieSupportType.Testing;
				Response.Cookies.Add(new HttpCookie(TESTCOOKIE_NAME, DateTime.Now.ToString()));
			}
			catch (HttpException) // if we're coming in via ErrorHandler, session is unavailable
			{
				return;
			}
		}

		private bool FinishCookieTest()
		{
			string testValue = Request.Cookies[TESTCOOKIE_NAME].Value;
			if (0 != testValue.Length)
			{
				Response.Cookies.Remove(TESTCOOKIE_NAME);
				Session[Keys.SESSION_COOKIETEST] = CookieSupportType.Allowed;
				return true;
			}
			else
			{
				Session[Keys.SESSION_COOKIETEST] = CookieSupportType.NotAllowed;
				return false;
			}
		}

		public ConfirmCommand Command
		{
			get { return _command; }
			set { _command = value; }
		}
	}
}

