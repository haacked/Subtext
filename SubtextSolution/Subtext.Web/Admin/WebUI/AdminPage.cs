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
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Web.Controls;

namespace Subtext.Web.Admin.Pages
{
	internal enum CookieSupportType
	{
		Untested,
		Testing,
		Allowed,
		NotAllowed
	}

	/// <summary>
	/// Base Page class for all pages in the admin tool.
	/// </summary>
	public class AdminPage : System.Web.UI.Page
	{
		private const string TESTCOOKIE_NAME = "TestCookie";

		private ConfirmCommand _command;
		
		protected override void OnLoad(EventArgs e)
		{		
			if(!ValidateUser)
			{
				Response.Redirect(Config.CurrentBlog.VirtualUrl + "Login.aspx?ReturnUrl=" + Request.Path);
			}

			// REFACTOR: we really need a singleton indicator per session or run this initial 
			// dummy run in OnSessionStart. But we'll add the overhead for now. We can look at
			// putting it in the default.aspx, but that fails to work on direct url access.
			AreCookiesAllowed();

			ControlHelper.ApplyRecursively(new ControlAction(SetTextBoxStyle), this);
			base.OnLoad(e);
		}

		void SetTextBoxStyle(Control control)
		{
			TextBox textBox = control as TextBox;
			if(textBox != null)
			{
				if(textBox.TextMode == TextBoxMode.SingleLine || textBox.TextMode == TextBoxMode.Password)
					textBox.CssClass = "textinput";
				if(textBox.TextMode == TextBoxMode.MultiLine)
					textBox.CssClass = "textarea";
			}
		}

		private bool ValidateUser
		{
			get
			{
				return Security.IsAdmin || Security.IsHostAdmin;
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
			catch (HttpException)
			{
				return false;
			}
		}

		private void StartCookieTest()
		{
			try
			{			
				Session[Keys.SESSION_COOKIETEST] = CookieSupportType.Testing;
				Response.Cookies.Add(new HttpCookie(TESTCOOKIE_NAME, DateTime.Now.ToString(CultureInfo.InvariantCulture)));
			}
			catch (HttpException)
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

